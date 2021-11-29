using EasterRts.Utilities.FixedFloats;
using System.IO;
using UnityEngine;

namespace EasterRts.Utilities {
    
    public static class Serialization {

        public static Vector2 ReadVector2(this BinaryReader reader) {
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            var result = new Vector2(x, y);
            return result;
        }

        public static void Write(this BinaryWriter writer, Vector2 value) {
            writer.Write(value.x);
            writer.Write(value.y);
        }

        public static Vector2Int ReadVector2Int(this BinaryReader reader) {
            var x = reader.ReadInt32();
            var y = reader.ReadInt32();
            var result = new Vector2Int(x, y);
            return result;
        }

        public static void Write(this BinaryWriter writer, Vector2Int value) {
            writer.Write(value.x);
            writer.Write(value.y);
        }

        public static FixedSingle ReadFixedSingle(this BinaryReader reader) {
            var value = reader.ReadInt32();
            return FixedSingle.FromRawValue(value);
        }

        public static void Write(this BinaryWriter writer, FixedSingle value) {
            writer.Write(value.RawValue);
        }

        public static FixedVector2 ReadFixedVector2(this BinaryReader reader) {
            var x = reader.ReadFixedSingle();
            var y = reader.ReadFixedSingle();
            var result = new FixedVector2(x, y);
            return result;
        }

        public static void Write(this BinaryWriter writer, FixedVector2 value) {
            writer.Write(value.X);
            writer.Write(value.Y);
        }
    }
}
