using System.IO;

namespace SharpCommunication.Base.Codec
{
    public interface IEncoding<TCodableData>
    {
        void EncodeCore(TCodableData obj, BinaryWriter writer);
        TCodableData DecodeCore( BinaryReader reader);
    }
}
