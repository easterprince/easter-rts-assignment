using EasterRts.Common.Cores;
using EasterRts.Utilities;
using EasterRts.Utilities.FixedFloats;
using Sirenix.OdinInspector;
using System;
using System.IO;
using UnityEngine;

namespace EasterRts.Battle.Data.Units {

    [Serializable]
    public struct UnitIntentionData : IBinarySerializable {

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
        private CoreId _targetedUnit;
        public CoreId TargetedUnit => _targetedUnit;


        // ToString().

        public override string ToString() {
            return _type switch {
                UnitIntentionType.Idle => "idle",
                UnitIntentionType.Move => $"move to {_targetedPosition}",
                UnitIntentionType.Follow => $"follow unit with id {_targetedUnit}",
                _ => "unknown",
            };
        }


        // Constructing methods.

        public static UnitIntentionData CreateIdle() =>
            new UnitIntentionData {
                _type = UnitIntentionType.Idle,
            };

        public static UnitIntentionData CreateMotion(FixedVector2 targetedPosition) =>
            new UnitIntentionData {
                _type = UnitIntentionType.Move,
                _targetedPosition = targetedPosition,
            };

        public static UnitIntentionData CreateFollowing(CoreId targetedUnit) =>
            new UnitIntentionData {
                _type = UnitIntentionType.Follow,
                _targetedUnit = targetedUnit,
            };


        // Serialization methods.

        public void ReadFrom(BinaryReader reader) {
            _type = (UnitIntentionType) reader.ReadByte();
            if (_type == UnitIntentionType.Move) {
                _targetedPosition = reader.ReadFixedVector2();
            }
            if (_type == UnitIntentionType.Follow) {
                _targetedUnit.ReadFrom(reader);
            }
        }

        public void WriteTo(BinaryWriter writer) {
            writer.Write((byte) _type);
            if (_type == UnitIntentionType.Move) {
                writer.Write(_targetedPosition);
            }
            if (_type == UnitIntentionType.Follow) {
                _targetedUnit.WriteTo(writer);
            }
        }
    }
}
