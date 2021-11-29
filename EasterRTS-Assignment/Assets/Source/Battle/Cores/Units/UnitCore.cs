using EasterRts.Battle.Data.Units;
using EasterRts.Common.Cores;
using EasterRts.Utilities.FixedFloats;
using Sirenix.OdinInspector;

namespace EasterRts.Battle.Cores.Units {

    public class UnitCore : DependentCoreBase<UnitCore, UnitSystem> {

        // Action fields.

        private static readonly IntentionSetting _intentionSetting = new IntentionSetting();
        private class IntentionSetting : ActionBase<UnitCore, UnitIntention> {
            public override void Call(UnitCore core, UnitIntention intention) => core.SetIntention(intention);
        }


        // Static fields.

        private static readonly FixedSingle _unitFollowingDistance = FixedSingle.FromThousandths(1600);


        // Static constructor.

        static UnitCore() {
            RegisterAction(_intentionSetting);
        }


        // Data fields.

        [ShowInInspector]
        [ReadOnly]
        private UnitIntention _intention;
        public UnitIntention Intention => _intention;


        // Module fields.

        [ShowInInspector]
        [ReadOnly]
        private readonly UnitMovement _movement;
        public UnitMovement Movement => _movement;


        // Life cycle.

        internal UnitCore(UnitData data, UnitSystem context) : base(context, data.Id) {
            _movement = new UnitMovement(this);
        }

        internal void Initialize(UnitData data) {
            _intention = UnitIntention.FromData(data.Intention, IdToCore);
            _movement.Initialize(data.Movement);
        }

        internal void Disable() {
            _movement.Disable();
            DisableSelf();
        }


        // Progression methods.

        protected override void Act() {
            ActOnMovement();
        }

        private void ActOnMovement() {
            if (!ValidateIntention()) {
                PlanIntentionSetting(UnitIntention.CreateIdle(), Token);
                return;
            }
            var destination = CalculateDestination();
            if (destination != null) {
                var destinationLocation = Context.Context.Map.GetLocation(destination.Value);
                _movement.PlanDestinationSetting(destinationLocation, Token);
            }
        }


        // Support methods.

        private bool ValidateIntention() {
            return _intention.IsValid && (_intention.Type != UnitIntentionType.Follow || _intention.TargetedUnit != this);
        }

        private FixedVector2? CalculateDestination() {
            switch (_intention.Type) {
                case UnitIntentionType.Move:
                    return _intention.TargetedPosition;
                case UnitIntentionType.Follow:
                    var targetedUnit = _intention.TargetedUnit;
                    if (targetedUnit != null && targetedUnit != this) {
                        var nextPosition = Movement.GetNextLocationPosition();
                        var targetNextPosition = targetedUnit.Movement.GetNextLocationPosition();
                        var offset = nextPosition - targetNextPosition;
                        var minimalAllowedDistance = _unitFollowingDistance + 2 * (Movement.SpeedLimit + targetedUnit.Movement.SpeedLimit) * Context.Context.TimeDelta;
                        var maximalAllowedDistance = _unitFollowingDistance + 3 * (Movement.SpeedLimit + targetedUnit.Movement.SpeedLimit) * Context.Context.TimeDelta;
                        var currentDistance = offset.Magnitude;
                        if (currentDistance >= minimalAllowedDistance && currentDistance <= maximalAllowedDistance) {
                            return null;
                        }
                        var targetedPosition = targetNextPosition + offset.Normalized * (minimalAllowedDistance + maximalAllowedDistance) / 2;
                        return targetedPosition;
                    }
                    break;
            }
            return null;
        }


        // Action methods.

        private void SetIntention(UnitIntention intention) {
            if (!intention.IsValid) {
                intention = UnitIntention.CreateIdle();
            }
            _intention = intention;
            ActOnMovement();
        }

        public void PlanIntentionSetting(UnitIntention intention, TrustToken callerToken) =>
            PlanAction(_intentionSetting, intention, callerToken);


        // ToString().

        public override string ToString() => $"Unit (id = {Id})";
    }
}
