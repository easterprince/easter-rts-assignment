namespace EasterRts.Utilities {

    public static class Floats {

        public static float ClampPeriodically(float value, float period) {
            if (period == 0) {
                return 0;
            }
            value %= period;
            if (value < 0) {
                value += period;
            }
            return value;
        }
    }
}
