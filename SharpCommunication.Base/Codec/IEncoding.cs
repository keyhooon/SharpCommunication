using System.IO;

namespace SharpCommunication.Codec
{
    public interface IEncoding<TCodableData>
    {
        void Encode(TCodableData obj, BinaryWriter writer);
        TCodableData Decode( BinaryReader reader);
    }
}
