using EasterRts.Utilities;
using Sirenix.OdinInspector;
using System;
using System.IO;
using UnityEngine;

namespace EasterRts.Common.Cores {
    
    [Serializable]
    [InlineProperty]
    public struct CoreId : IEquatable<CoreId>, IBinarySerializable {

        private const int _undefinedValue = 0;

        private static CoreId _undefined = new CoreId(_undefinedValue);
        public static CoreId Undefined => _undefined;

        private static CoreId _first = new CoreId(_undefinedValue + 1);
        public static CoreId First => _first;

        [SerializeField]
        [HideLabel]
        private int _value;

        public CoreId(int id) {
            _value = id;
        }

        public bool IsUndefined => _value == _undefinedValue;

        public bool Equals(CoreId other) => _value == other._value;

        public override bool Equals(object obj) {
            if (obj is CoreId other) {
                return Equals(other);
            }
            return false;
        }

        public static bool operator ==(CoreId first, CoreId second) => first.Equals(second);

        public static bool operator !=(CoreId first, CoreId second) => !first.Equals(second);

        public override int GetHashCode() => _value.GetHashCode();

        public CoreId GetNext() => new CoreId(_value + 1);

        public void ReadFrom(BinaryReader reader) => _value = reader.ReadInt32();
        public void WriteTo(BinaryWriter writer) => writer.Write(_value);

        public override string ToString() {
            return $"{_value}";
        }

        public static CoreId GetLatter(CoreId first, CoreId second) {
            var firstIsPositive = (first._value >= _undefinedValue);
            var secondIsPositive = (second._value >= _undefinedValue);
            if (firstIsPositive != secondIsPositive) {
                return secondIsPositive ? first : second;
            }
            return first._value > second._value ? first : second;
        }
    }
}
