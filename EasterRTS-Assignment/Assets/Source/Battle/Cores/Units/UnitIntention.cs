using EasterRts.Battle.Data.Units;
using EasterRts.Common.Cores;
using EasterRts.Utilities.FixedFloats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasterRts.Battle.Cores.Units {

    public struct UnitIntention {

        // Fields.

        [SerializeField]
        private UnitIntentionType _type;
        public UnitIntentionType Type => _type;

        [SerializeField]
        [ShowIf(nameof(_type), UnitIntentionType.Move)]
        private FixedVector2 _targetedPosition;
        public FixedVector2 TargetedPosition => _targetedPosition;

        [SerializeField]
        [ShowIf(nameof(_type), UnitIntentionType.Follow)]
        private UnitCore _targetedUnit;
        public UnitCore TargetedUnit => _targetedUnit;


        // Properties.

        public bool IsValid => _type switch {
            UnitIntentionType.Idle => true,
            UnitIntentionType.Move => _targetedPosition.IsValid,
            UnitIntentionType.Follow => _targetedUnit != null && _targetedUnit.Enabled,
            _ => false,
        };


        // ToString().

        public override string ToString() {
            return _type switch {
                UnitIntentionType.Idle => "idle",
                UnitIntentionType.Move => $"move to {_targetedPosition}",
                UnitIntentionType.Follow => $"follow unit {_targetedUnit}",
                _ => "unknown",
            };
        }


        // Constructing methods.

        public static UnitIntention FromData(UnitIntentionData data, IdToCore idToCore) =>
            data.Type switch {
                UnitIntentionType.Move => CreateMotion(data.TargetedPosition),
                UnitIntentionType.Follow => CreateFollowing(idToCore.Get<UnitCore>(data.TargetedUnit)),
                _ => CreateIdle(),
            };

        public static UnitIntention CreateIdle() =>
            new UnitIntention {
                _type = UnitIntentionType.Idle,
            };

        public static UnitIntention CreateMotion(FixedVector2 targetedPosition) =>
            new UnitIntention {
                _type = UnitIntentionType.Move,
                _targetedPosition = targetedPosition,
            };

        public static UnitIntention CreateFollowing(UnitCore targetedUnit) =>
            new UnitIntention {
                _type = UnitIntentionType.Follow,
                _targetedUnit = targetedUnit,
            };
    }
}
