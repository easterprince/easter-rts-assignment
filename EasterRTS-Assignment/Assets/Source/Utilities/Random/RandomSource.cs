namespace EasterRts.Utilities.Random {
    
    public struct RandomSource {

        private const uint _multiplier = 0xFAAE1A75;
        private const uint _addend = 0x27BC867B;

        private uint _current;

        public RandomSource(RandomData data) {
            _current = data.Current;
        }

        public RandomData ToData() => new RandomData(_current);

        public uint NextUInt() {
            _current = _current * _multiplier + _addend;
            return _current;
        }

        public uint NextUInt(uint count) {
            
            var value = NextUInt();
            uint result;
            
            if (count == 0) {
                result = 0;
            
            // Alternative to just using less significant bits which not that random.
            } else if (count <= 1024) {
                result = value / (uint.MaxValue / count);
                if (result >= count) {
                    result = count - 1;
                }

            } else {
                result = value % count;
            }

            return result;
        }

        public int NextInt(int count) => NextInt(0, count);

        public int NextInt(int from, int to) {
            uint length = (uint) (to - from);
            uint value = NextUInt(length);
            if (to <= from) {
                return from;
            }
            int result = (int) ((uint) from + value);
            return result;
        }
    }
}
