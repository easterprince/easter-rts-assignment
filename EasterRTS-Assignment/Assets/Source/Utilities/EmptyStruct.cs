using System.IO;

namespace EasterRts.Utilities {

    public struct EmptyStruct : IBinarySerializable {
        
        public void ReadFrom(BinaryReader reader) {}
        public void WriteTo(BinaryWriter writer) {}
    }
}
