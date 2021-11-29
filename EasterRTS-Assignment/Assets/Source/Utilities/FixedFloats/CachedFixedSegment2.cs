using System;

namespace EasterRts.Utilities.FixedFloats {
    
    public class CachedFixedSegment2 {

        private readonly FixedSegment2 _segment;
        public FixedSegment2 Segment => _segment;

        private readonly FixedVector2 _difference;
        private readonly FixedSingle _differenceSquaredMagnitude;
        private readonly FixedSingle _nearestPointAddend;
        private readonly FixedVector2 _middle;

        public CachedFixedSegment2(FixedSegment2 segment) {
            if (!segment.IsValid) {
                throw new ArgumentException(nameof(segment), "Segment must be valid!");
            }
            _segment = segment;
            _difference = segment.End - segment.Start;
            _differenceSquaredMagnitude = _difference.SquaredMagnitude;
            _nearestPointAddend = (_segment.Start * _difference).Sum;
            _middle = (segment.End + segment.Start) / 2;
        }

        public FixedSingle GetDistance(FixedVector2 from) =>
            (GetNearestPoint(from) - from).Magnitude;

        public FixedVector2 GetNearestPoint(FixedVector2 from) {

            if (_differenceSquaredMagnitude == 0) {
                return _segment.Start;
            }

            var t = ((_difference * from).Sum - _nearestPointAddend) / _differenceSquaredMagnitude;
            
            if (!t.IsValid) {
                return _middle;
            } else if (t.IsPositiveOverflow || t > 1) {
                return _segment.End;
            } else if (t.IsNegativeOverflow || t < 0) {
                return _segment.Start;
            }
            var result = _segment.Start + _difference * t;
            return result;
        }

        public FixedVector2 GetNearestPoint(FixedVector2 firstFrom, FixedVector2 secondFrom) {

            if (_differenceSquaredMagnitude == 0) {
                return _segment.Start;
            }

            var firstDifference = _difference;
            var secondDifference = secondFrom - firstFrom;
            var originsDifference = firstFrom - _segment.Start;

            var denominator = secondDifference.X * firstDifference.Y - secondDifference.Y * firstDifference.X;
            var numerator = secondDifference.X * originsDifference.Y - secondDifference.Y * originsDifference.X;
            var t = numerator / denominator;

            if (!t.IsValid) {
                return _middle;
            } else if (t.IsPositiveOverflow || t > 1) {
                return _segment.End;
            } else if (t.IsNegativeOverflow || t < 0) {
                return _segment.Start;
            }
            var result = _segment.Start + _difference * t;
            return result;
        }
    }
}
