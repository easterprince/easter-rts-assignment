using System;

namespace EasterRts.Utilities.Random {
    
    [Serializable]
    public struct RandomData {

        private uint _current;
        public uint Current => _current;

        public RandomData(uint current) {
            _current = current;
        }
    }
}
