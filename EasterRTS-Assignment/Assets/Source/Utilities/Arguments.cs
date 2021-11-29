using System;

namespace EasterRts.Utilities {

    public static class Arguments {

        public static T ValidateNotNull<T>(T argument, string argumentName)
            where T : class {

            if (argument == null) {
                throw new ArgumentNullException(argumentName);
            }
            return argument;
        }

        public static int ValidateNonNegative(int argument, string argumentName) {
            if (argument < 0) {
                throw new ArgumentOutOfRangeException(argumentName, argument, $"{argumentName} must be greater than or equal to zero.");
            }
            return argument;
        }
    }
}
