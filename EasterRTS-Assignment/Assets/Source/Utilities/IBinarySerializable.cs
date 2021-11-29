using System.IO;

namespace EasterRts.Utilities {

    public interface IBinarySerializable {

        void ReadFrom(BinaryReader reader);
        void WriteTo(BinaryWriter writer);
    }
}
