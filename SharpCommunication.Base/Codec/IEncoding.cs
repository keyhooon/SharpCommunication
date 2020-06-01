using System.IO;

namespace SharpCommunication.Codec
{
    public interface IEncoding<TCodableData>
    {
        void EncodeCore(TCodableData obj, BinaryWriter writer);
        TCodableData DecodeCore( BinaryReader reader);
    }
}
