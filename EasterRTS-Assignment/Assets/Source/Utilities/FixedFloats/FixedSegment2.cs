using System;
using UnityEngine;

namespace EasterRts.Utilities.FixedFloats {
    
    [Serializable]
    public struct FixedSegment2 {

        [SerializeField]
        private FixedVector2 _start;
        public FixedVector2 Start => _start;

        [SerializeField]
        private FixedVector2 _end;
        public FixedVector2 End => _end;

        public FixedSegment2(FixedVector2 start, FixedVector2 end) {
            if (!start.IsValid || !end.IsValid) {
                _start = FixedVector2.Invalid;
                _end = FixedVector2.Invalid;
            } else {
                _start = start;
                _end = end;
            }
        }

        public bool IsValid => _start.IsValid && _end.IsValid;
    }
}
