using System;
using System.Globalization;
using System.Runtime.Serialization;
using UnityEngine;

namespace EasterRts.Utilities.FixedFloats {

    [Serializable]
    public struct FixedSingle : IEquatable<FixedSingle>, IComparable<FixedSingle>, ISerializable {

        // Constants.

        private const int _power = 3;
        private const int _scale = 1000;
        private const int _scaleHalf = _scale / 2;

        private const int _positiveOverflowValue = int.MaxValue;
        private const int _negativeOverflowValue = int.MinValue + 1;
        private const int _nanValue = int.MinValue;

        private const int _intPositiveLimit = (int.MaxValue - 1) / _scale;
        private const int _intNegativeLimit = (int.MinValue + 1) / _scale;

        private const string _positiveOverflowString = "+overflow";
        private const string _negativeOverflowString = "-overflow";
        private const string _nanString = "nan";


        // Static fields.

        private static readonly string _format;
        private static readonly NumberStyles _style;

        private static readonly FixedSingle _positiveOverflow;
        public static FixedSingle PositiveOverflow => _positiveOverflow;

        private static readonly FixedSingle _negativeOverflow;
        public static FixedSingle NegativeOverflow => _negativeOverflow;

        private static readonly FixedSingle _nan;
        public static FixedSingle NaN => _nan;

        private static readonly FixedSingle _quantum;
        public static FixedSingle Quantum => _quantum;

        static FixedSingle() {
            _format = "{0}{1}.{2:D" + _power.ToString() + "}";
            _style = NumberStyles.Number;
            _positiveOverflow = new FixedSingle(_positiveOverflowValue);
            _negativeOverflow = new FixedSingle(_negativeOverflowValue);
            _nan = new FixedSingle(_nanValue);
            _quantum = new FixedSingle(1);
        }


        // Fields.

        [SerializeField]
        private int _value;


        // Constructors.

        private FixedSingle(int value) {
            _value = value;
        }


        // Serialization.

        public FixedSingle(SerializationInfo info, StreamingContext context) {
            _value = (int) info.GetValue(nameof(_value), typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(nameof(_value), _value);
        }


        // Properties.

        public bool IsValid => _value != _positiveOverflowValue && _value > _negativeOverflowValue;
        public bool IsOverflow => _value == _positiveOverflowValue || _value == _negativeOverflowValue;
        public bool IsPositiveOverflow => _value == _positiveOverflowValue;
        public bool IsNegativeOverflow => _value == _negativeOverflowValue;
        public bool IsNan => _value == _nanValue;

        public int RawValue => _value;


        // String conversion.

        public override string ToString() {
            if (_value == _positiveOverflowValue) {
                return _positiveOverflowString;
            }
            if (_value == _negativeOverflowValue) {
                return _negativeOverflowString;
            }
            if (_value == _nanValue) {
                return _nanString;
            }
            return string.Format(_format, (_value < 0 ? "-" : ""), Mathf.Abs(_value / _scale), Mathf.Abs(_value % _scale));
        }

        public static bool TryParse(string s, out FixedSingle result) {
            if (!decimal.TryParse(s, _style, CultureInfo.InvariantCulture, out decimal decimalResult)) {
                result = new FixedSingle(_nanValue);
                return false;
            }
            result = (FixedSingle) decimalResult;
            return true;
        }


        // Comparisons.

        public override int GetHashCode() => _value.GetHashCode();

        public override bool Equals(object obj) {
            if (obj is FixedSingle other) {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(FixedSingle other) => _value == other._value;

        public int CompareTo(FixedSingle other) {
            if (IsValid != other.IsValid) {
                return IsValid ? 1 : -1;
            }
            return _value < other._value ? -1 : 1;
        }

        public static bool operator ==(FixedSingle first, FixedSingle second) {
            return first.IsValid && second.IsValid && first._value == second._value;
        }

        public static bool operator !=(FixedSingle first, FixedSingle second) {
            return first.IsValid && second.IsValid && first._value != second._value;
        }

        public static bool operator <(FixedSingle first, FixedSingle second) {
            return first.IsValid && second.IsValid && first._value < second._value;
        }

        public static bool operator >(FixedSingle first, FixedSingle second) {
            return first.IsValid && second.IsValid && first._value > second._value;
        }

        public static bool operator <=(FixedSingle first, FixedSingle second) {
            return first.IsValid && second.IsValid && first._value <= second._value;
        }

        public static bool operator >=(FixedSingle first, FixedSingle second) {
            return first.IsValid && second.IsValid && first._value >= second._value;
        }


        // Arithmetics.

        public static FixedSingle operator +(FixedSingle first, FixedSingle second) {
            if (!first.IsValid || !second.IsValid) {
                return _nan;
            }
            if (first._value < 0) {
                // first._value + second._value <= _negativeOverflowValue
                if (second._value <= _negativeOverflowValue - first._value) {
                    return _negativeOverflow;
                }
            } else if (first._value > 0) {
                // first._value + second._value >= _positiveOverflowValue
                if (second._value >= _positiveOverflowValue - first._value) {
                    return _positiveOverflow;
                }
            }
            return new FixedSingle(first._value + second._value);
        }

        public static FixedSingle operator -(FixedSingle first, FixedSingle second) {
            if (!first.IsValid || !second.IsValid) {
                return _nan;
            }
            if (first._value < 0) {
                // first._value - second._value <= _negativeOverflowValue
                if (second._value >= first._value - _negativeOverflowValue) {
                    return _negativeOverflow;
                }
            } else if (first._value > 0) {
                // first._value - second._value >= _positiveOverflowValue
                if (second._value <= first._value - _positiveOverflowValue) {
                    return _positiveOverflow;
                }
            }
            return new FixedSingle(first._value - second._value);
        }

        public static FixedSingle operator *(FixedSingle first, FixedSingle second) {
            if (!first.IsValid || !second.IsValid) {
                return _nan;
            }
            if (first._value == 0 || second._value == 0) {
                return new FixedSingle(0);
            }
            var valuesProduct = (long) first._value * second._value / _scale;
            if (valuesProduct > 0 && valuesProduct >= _positiveOverflowValue) {
                // first._value * second._value / _scale >= _positiveOverflowValue
                return PositiveOverflow;
            } else if (valuesProduct < 0 && valuesProduct <= _negativeOverflowValue) {
                // first._value * second._value / _scale <= _negativeOverflowValue
                return NegativeOverflow;
            }
            return new FixedSingle((int) valuesProduct);
        }

        public static FixedSingle operator /(FixedSingle first, FixedSingle second) {
            if (!first.IsValid || !second.IsValid || second._value == 0) {
                return _nan;
            }
            return FromThousandths((long) first._value * _scale / second._value);
        }


        // Roundings.

        public int Floor() {
            if (!IsValid) {
                return 0;
            }
            var result = _value / _scale;
            if (_value < 0 && _value % _scale < 0) {
                --result;
            }
            return result;
        }

        public int Ceil() {
            if (!IsValid) {
                return 0;
            }
            var result = _value / _scale;
            if (_value > 0 && _value % _scale > 0) {
                ++result;
            }
            return result;
        }

        public int Round() {
            if (!IsValid) {
                return 0;
            }
            int result = Floor();
            int modulo = _value % _scale;
            if (modulo < 0) {
                modulo += _scale;
            }
            if (modulo < _scaleHalf || modulo == _scaleHalf && result % 2 == 0) {
                return result;
            }
            return result + 1;
        }


        // Number conversions.

        public static implicit operator FixedSingle(int original) {
            if (original > _intPositiveLimit) {
                return _positiveOverflow;
            }
            if (original < _intNegativeLimit) {
                return _negativeOverflow;
            }
            return new FixedSingle(original * _scale);
        }

        public static explicit operator FixedSingle(float original) {
            if (float.IsNaN(original)) {
                return _nan;
            }
            var value = Mathf.Round(original * _scale);
            if (value >= _positiveOverflowValue) {
                return _positiveOverflow;
            }
            if (value <= _negativeOverflowValue) {
                return _negativeOverflow;
            }
            return new FixedSingle((int) value);
        }

        public static explicit operator float(FixedSingle original) {
            if (!original.IsValid) {
                return float.NaN;
            }
            return (float) original._value / _scale;
        }

        public static explicit operator FixedSingle(double original) {
            if (double.IsNaN(original)) {
                return _nan;
            }
            var value = Math.Round(original * _scale);
            if (value >= _positiveOverflowValue) {
                return _positiveOverflow;
            }
            if (value <= _negativeOverflowValue) {
                return _negativeOverflow;
            }
            return new FixedSingle((int) value);
        }

        public static explicit operator double(FixedSingle original) {
            if (!original.IsValid) {
                return double.NaN;
            }
            return (double) original._value / _scale;
        }

        public static explicit operator FixedSingle(decimal original) {
            if (original >= _intPositiveLimit + 1) {
                return _positiveOverflow;
            }
            if (original <= _intNegativeLimit - 1) {
                return _negativeOverflow;
            }
            int thousands = (int) Math.Round(original * _scale);
            return FromThousandths(thousands);
        }

        public static FixedSingle FromThousandths(int thousandths) {
            if (thousandths >= _positiveOverflowValue) {
                return _positiveOverflow;
            }
            if (thousandths <= _negativeOverflowValue) {
                return _negativeOverflow;
            }
            return new FixedSingle(thousandths);
        }

        public static FixedSingle FromThousandths(long thousandths) {
            if (thousandths >= _positiveOverflowValue) {
                return _positiveOverflow;
            }
            if (thousandths <= _negativeOverflowValue) {
                return _negativeOverflow;
            }
            return new FixedSingle((int) thousandths);
        }

        public static FixedSingle FromRawValue(int rawValue) {
            return new FixedSingle(rawValue);
        }


        // Math methods.

        public static FixedSingle Abs(FixedSingle value) {
            if (!value.IsValid) {
                return _nan;
            }
            if (value._value < 0) {
                return new FixedSingle(-value._value);
            }
            return value;
        }

        public static FixedSingle Min(FixedSingle first, FixedSingle second) {
            if (!first.IsValid || !second.IsValid) {
                return _nan;
            }
            return first._value < second._value ? first : second;
        }

        public static FixedSingle Max(FixedSingle first, FixedSingle second) {
            if (!first.IsValid || !second.IsValid) {
                return _nan;
            }
            return first._value > second._value ? first : second;
        }

        public static FixedSingle Clamp(FixedSingle value, FixedSingle min, FixedSingle max) {
            if (!value.IsValid || !min.IsValid || !max.IsValid) {
                return _nan;
            }
            return value < min ? min : value > max ? max : value;
        }


#if UNITY_EDITOR
        // Reflection utilities.

        public static string ValueFieldName => nameof(_value);
#endif
    }
}

