using System;
using System.IO;
using UnityEngine;

namespace EasterRts.Common.Cores {

    [Serializable]
    public struct ActionId : IEquatable<ActionId>, IComparable<ActionId> {

        private const int _undefinedValue = -1;

        private static ActionId _undefined = new ActionId(_undefinedValue);
        public static ActionId Undefined => _undefined;

        [SerializeField]
        private int _value;
        public int Value => _value;

        public ActionId(int value) {
            _value = value;
        }

        public bool IsUndefined => _value == _undefinedValue;

        public override int GetHashCode() => _value.GetHashCode();

        public bool Equals(ActionId other) => _value == other._value;

        public override bool Equals(object obj) {
            if (obj is ActionId other) {
                return Equals(other);
            }
            return false;
        }

        public ActionId GetNext() => new ActionId(_value + 1);

        public static ActionId ReadFrom(BinaryReader reader) => new ActionId(reader.ReadInt32());

        public void WriteTo(BinaryWriter writer) => writer.Write(_value);

        public int CompareTo(ActionId other) {
            if (_value == other._value) {
                return 0;
            }
            return _value < other._value ? -1 : 1;
        }

        public override string ToString() {
            return $"{_value}";
        }
    }
}
