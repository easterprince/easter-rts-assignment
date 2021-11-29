using System.Reflection;

namespace EasterRts.Editor.Reflection {
    
    public static class ObjectExtensions {

        /// <summary>
        /// Get field value using reflection.
        /// </summary>
        public static object GetFieldValue<TReflected>(this TReflected reflected, string fieldName) {
            return reflected.GetField(fieldName).GetValue(reflected);
        }


        /// <summary>
        /// Set field value using reflection.
        /// </summary>
        public static void SetFieldValue<TReflected>(this TReflected reflected, string fieldName, object value) {
            reflected.GetField(fieldName).SetValue(reflected, value);
        }

        private static FieldInfo GetField<TReflected>(this TReflected reflected, string fieldName) {
            return typeof(TReflected).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}
