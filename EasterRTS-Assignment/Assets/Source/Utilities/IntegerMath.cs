using System;

namespace EasterRts.Utilities {
    
    public static class IntegerMath {

        public static long Sqrt(long value) {
            if (value < 0) {
                throw new ArithmeticException("Value can't be negative!");
            }
            if (value == 0) {
                return 0;
            }
            if (value == 1) {
                return 1;
            }
            long previous2 = -2;
            long previous1 = -1;
            long current = 1000000;
            while (previous2 != current) {
                previous2 = previous1;
                previous1 = current;
                current = (current + value / current) / 2;
            }
            return current <= previous1 ? current : previous1;
        }
    }
}
