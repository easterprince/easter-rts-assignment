using EasterRts.Battle.Cores.Map;
using EasterRts.Battle.Data.Map;
using EasterRts.Battle.Data.Units;
using EasterRts.Common.Cores;
using EasterRts.Utilities.FixedFloats;
using Sirenix.OdinInspector;

namespace EasterRts.Battle.Cores.Units {

    public class UnitMovement : DependentCoreBase<UnitMovement, UnitCore> {

        // Nested types.

        private struct MovementState {

            public MapLocation location;
            public FixedVector2 velocity;
            public MapLocation immediateDestination;
            public MapLocation followedFinalDestination;
        }


        // Action fields.

        private static readonly DestinationSetting _destinationSetting;
        private class DestinationSetting : ActionBase<UnitMovement, MapLocation> {
            public override void Call(UnitMovement core, MapLocation location) => core.SetDestination(location);
        }

        private static readonly MovementUpdate _movementUpdate;
        private class MovementUpdate : ActionBase<UnitMovement, MovementState> {
            public override void Call(UnitMovement core, MovementState argument) => core.UpdateState(argument);
        }

        static UnitMovement() {
            RegisterAction(_destinationSetting = new DestinationSetting());
            RegisterAction(_movementUpdate = new MovementUpdate());
        }


        // Static fields.

        private static readonly FixedSingle _slowdownRadius = FixedSingle.FromThousandths(1500);
        private static readonly FixedSingle _slowdownFactor = FixedSingle.FromThousandths(500);
        private static readonly FixedSingle _destinationAchievedDistance = FixedSingle.FromThousandths(1000);


        // Data fields.

        [ShowInInspector]
        [ReadOnly]
        private FixedSingle _speedLimit;
        public FixedSingle SpeedLimit => _speedLimit;

        [ShowInInspector]
        [ReadOnly]
        private MapLocation _location;
        public MapLocation Location => _location;

        [ShowInInspector]
        [ReadOnly]
        private FixedVector2 _velocity;
        public FixedVector2 Velocity => _velocity;

        [ShowInInspector]
        [ReadOnly]
        private MapLocation _immediateDestination;

        [ShowInInspector]
        [ReadOnly]
        private MapLocation _followedFinalDestination;

        [ShowInInspector]
        [ReadOnly]
        private MapLocation _finalDestination;
        public MapLocation Destination => _finalDestination;


        // Life cycle.

        internal UnitMovement(UnitCore context) : base(context, CoreId.Undefined) {}

        internal void Initialize(MovementData data) {
            _speedLimit = data.SpeedLimit;
            _location = new MapLocation(data.Location, IdToCore);
            _velocity = data.Velocity;
            _immediateDestination = new MapLocation(data.ImmediateDestination, IdToCore);
            _followedFinalDestination = new MapLocation(data.FollowedFinalDestination, IdToCore);
            _finalDestination = new MapLocation(data.FinalDestination, IdToCore);
        }

        internal void Disable() {
            DisableSelf();
        }


        // Properties.

        private BattleCore Battle => UnitSystem.Context;
        private UnitSystem UnitSystem => Unit.Context;
        private UnitCore Unit => Context;
        private MapSystem Map => Battle.Map;


        // Reading methods.

        public FixedVector2 GetNextLocationPosition() {
            return _location.Position + _velocity * Battle.TimeDelta;
        }


        // Progression methods.

        protected override void Act() {

            // Apply velocity.
            var location = _location;
            if (location.Site == null) {
                location = Map.GetLocation(location.Position);
            }
            bool updateImmediateDestination = false;
            if (_velocity.IsValid) {
                var newPosition = GetNextLocationPosition();
                var changeSite = (location.Site != _immediateDestination.Site) && ((newPosition - _immediateDestination.Position).MagnitudeOverestimate <= _destinationAchievedDistance);
                if (changeSite) {
                    location = new MapLocation(newPosition, _immediateDestination.Site);
                    updateImmediateDestination = true;
                } else {
                    location = new MapLocation(newPosition, location.Site);
                }
            }

            // Update immediate destination.
            updateImmediateDestination |= (_followedFinalDestination != _finalDestination);
            var immediateDestination = _immediateDestination;
            var finalDestination = _finalDestination;
            if (updateImmediateDestination) {
                var macroPath = location.Site.FindMacroPath(finalDestination.Site);
                if (macroPath == null) {

                    immediateDestination = Location;
                    finalDestination = Location;

                } else if (macroPath.Count == 0) {

                    immediateDestination = _finalDestination;

                } else {

                    FixedVector2 guidingPosition;
                    if (macroPath.Count <= 1 || macroPath[1].Neigbour == _finalDestination.Site) {
                        guidingPosition = _finalDestination.Position;
                    } else {
                        guidingPosition = macroPath[1].Neigbour.GenerativePoint;
                    }
                    immediateDestination = new MapLocation(
                        position: macroPath[0].Edge.GetNearestPoint(location.Position, guidingPosition),
                        site: macroPath[0].Neigbour
                    );
                }
            }

            // Calculate velocity.
            var direction = DetermineIntentedDirection(location, immediateDestination);
            var slowdownFactor = CalculateSlowdown(location);
            var velocity = _speedLimit * slowdownFactor * direction;
            if (!velocity.IsValid) {
                velocity = FixedVector2.Zero;
            }

            // Plan movement.
            var newState = new MovementState {
                location = location,
                velocity = velocity,
                immediateDestination = immediateDestination,
                followedFinalDestination = finalDestination,
            };
            PlanStateUpdate(newState, Token);
            if (finalDestination != _finalDestination) {
                PlanDestinationSetting(finalDestination, Token);
            }
        }


        // Action methods.

        private void SetDestination(MapLocation destination) {
            _finalDestination = destination;
        }

        public void PlanDestinationSetting(MapLocation destination, TrustToken callerToken) =>
            PlanAction(_destinationSetting, destination, callerToken);

        private void UpdateState(MovementState newState) {
            _location = newState.location;
            _velocity = newState.velocity;
            _immediateDestination = newState.immediateDestination;
            _followedFinalDestination = newState.followedFinalDestination;
        }

        private void PlanStateUpdate(MovementState newState, TrustToken callerToken) =>
            PlanAction(_movementUpdate, newState, callerToken);


        // Support methods.

        private FixedVector2 DetermineIntentedDirection(MapLocation location, MapLocation immediateDestination) {
            var movement = immediateDestination.Position - location.Position;
            var movementDistance = movement.Magnitude;
            var movementLimit = _speedLimit * Battle.TimeDelta;
            FixedVector2 direction;
            if (movementDistance < movementLimit) {
                direction = movement / movementLimit;
            } else {
                direction = movement / movementDistance;
            }
            return direction;
        }

        private FixedSingle CalculateSlowdown(MapLocation location) {
            foreach (var unit in UnitSystem.Units) {
                if (unit == Unit) {
                    continue;
                }
                var distance = (location.Position - unit.Movement.GetNextLocationPosition()).Magnitude;
                if (distance < _slowdownRadius) {
                    return _slowdownFactor;
                }
            }
            return 1;
        }
    }
}
