using Sirenix.OdinInspector;
using System;
using System.IO;
using UnityEngine;

namespace EasterRts.Utilities.FixedFloats {

    [Serializable]
    [InlineProperty]
    public struct FixedVector2 : IEquatable<FixedVector2>, IBinarySerializable {

        // Static fields.

        private static readonly FixedVector2 _invalid =
            new FixedVector2 {
                _x = FixedSingle.NaN,
                _y = FixedSingle.NaN,
            };
        public static FixedVector2 Invalid => _invalid;


        private static readonly FixedVector2 _zero =
            new FixedVector2 {
                _x = 0,
                _y = 0,
            };
        public static FixedVector2 Zero => _zero;

        private static FixedSingle _sqrt2Ceil = FixedSingle.FromThousandths(1415);
        private static FixedSingle _sqrt5Ceil = FixedSingle.FromThousandths(2237);


        // Fields.

        [SerializeField]
        [LabelWidth(12)]
        [HorizontalGroup("group")]
        private FixedSingle _x;
        public FixedSingle X => _x;

        [SerializeField]
        [LabelWidth(12)]
        [HorizontalGroup("group")]
        private FixedSingle _y;
        public FixedSingle Y => _y;


        // Constructors.

        public FixedVector2(FixedSingle x, FixedSingle y) {
            if (!x.IsValid || !y.IsValid) {
                this = _invalid;
            } else {
                _x = x;
                _y = y;
            }
        }


        // Properties.

        public bool IsValid => _x.IsValid && _y.IsValid;

        public FixedSingle Sum => _x + _y;
        public FixedSingle Product => _x * _y;

        public FixedSingle SquaredMagnitude {
            get {
                if (!IsValid) {
                    return FixedSingle.NaN;
                }
                var squared = _x * _x + _y * _y;
                return squared;
            }
        }

        public FixedSingle Magnitude {
            get {
                if (!IsValid) {
                    return FixedSingle.NaN;
                }
                var squaredValue = (long) _x.RawValue * _x.RawValue + (long) _y.RawValue * _y.RawValue;
                var value = IntegerMath.Sqrt(squaredValue);
                return FixedSingle.FromThousandths(value);
            }
        }

        public FixedSingle MagnitudeOverestimate {
            get {
                if (!IsValid) {
                    return FixedSingle.NaN;
                }
                var absoluteX = FixedSingle.Abs(_x);
                var absoluteY = FixedSingle.Abs(_y);
                var common = FixedSingle.Min(absoluteX, absoluteY);
                var difference = FixedSingle.Max(absoluteX, absoluteY) - common;
                FixedSingle estimate;
                if (difference < common) {
                    estimate = _sqrt2Ceil * common + difference;
                } else {
                    estimate = _sqrt5Ceil * common + (difference - common);
                }
                return estimate;
            }
        }

        public FixedVector2 Normalized {
            get {
                if (!IsValid) {
                    return _invalid;
                }
                if (this == Zero) {
                    return Zero;
                }
                return this / Magnitude;
            }
        }


        // ToString().

        public override string ToString() {
            return $"({_x}, {_y})";
        }


        // Hash code calculation.

        public override int GetHashCode() {
            int hashCode = 979593255;
            hashCode = hashCode * -1521134295 + _x.GetHashCode();
            hashCode = hashCode * -1521134295 + _y.GetHashCode();
            return hashCode;
        }


        // Comparisons.

        public override bool Equals(object obj) {
            if (obj is FixedVector2 other) {
                return this == other;
            }
            return false;
        }

        public bool Equals(FixedVector2 other) {
            if (!IsValid || !other.IsValid) {
                return false;
            }
            return _x == other._x && _y == other._y;
        }

        public static bool operator ==(FixedVector2 first, FixedVector2 second) {
            return first.Equals(second);
        }

        public static bool operator !=(FixedVector2 first, FixedVector2 second) {
            return !first.Equals(second);
        }


        // Conversions.

        public static explicit operator Vector2Int(FixedVector2 vector) =>
            new Vector2Int((int) vector._x, (int) vector._y);

        public static implicit operator FixedVector2(Vector2Int vector) =>
            new FixedVector2(vector.x, vector.y);

        public static explicit operator FixedVector2(Vector2 vector) =>
            new FixedVector2((FixedSingle) vector.x, (FixedSingle) vector.y);

        public static implicit operator Vector2(FixedVector2 vector) =>
            new Vector2((float) vector._x, (float) vector._y);


        // Operators.

        public static FixedVector2 operator +(FixedVector2 first, FixedVector2 second) =>
            new FixedVector2(first._x + second._x, first._y + second._y);

        public static FixedVector2 operator -(FixedVector2 first, FixedVector2 second) =>
            new FixedVector2(first._x - second._x, first._y - second._y);

        public static FixedVector2 operator *(FixedVector2 vector, FixedSingle scalar) =>
            new FixedVector2(vector._x * scalar, vector._y * scalar);

        public static FixedVector2 operator *(FixedSingle scalar, FixedVector2 vector) =>
            new FixedVector2(scalar * vector._x, scalar * vector._y);

        public static FixedVector2 operator *(FixedVector2 first, FixedVector2 second) =>
            new FixedVector2(first._x * second._x, first._y * second._y);

        public static FixedVector2 operator /(FixedVector2 vector, FixedSingle scalar) =>
            new FixedVector2(vector._x / scalar, vector._y / scalar);

        public static FixedVector2 operator /(FixedVector2 first, FixedVector2 second) =>
            new FixedVector2(first._x / second._x, first._y / second._y);


        // Math methods.

        public static FixedVector2 Min(FixedVector2 first, FixedVector2 second) =>
            new FixedVector2(FixedSingle.Min(first._x, second._x), FixedSingle.Min(first._y, second._y));

        public static FixedVector2 Max(FixedVector2 first, FixedVector2 second) =>
            new FixedVector2(FixedSingle.Max(first._x, second._x), FixedSingle.Max(first._y, second._y));

        public static FixedVector2 Clamp(FixedVector2 value, FixedVector2 min, FixedVector2 max) =>
            new FixedVector2(FixedSingle.Clamp(value._x, min._x, max._x), FixedSingle.Clamp(value._y, min._y, max._y));


        // Rounding.

        public Vector2Int Floor() => new Vector2Int(_x.Floor(), _y.Floor());


        // Serialization.

        public void ReadFrom(BinaryReader reader) => this = reader.ReadFixedVector2();
        public void WriteTo(BinaryWriter writer) => writer.Write(this);
    }
}