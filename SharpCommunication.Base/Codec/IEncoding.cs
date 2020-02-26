using System.IO;

namespace SharpCommunication.Base.Codec
{
    public interface IEncoding<TCodableData>
    {
        public IEncoding<TCodableData> Encoding { get; }
        void EncodeCore(TCodableData obj, BinaryWriter writer);
        TCodableData DecodeCore( BinaryReader reader);
    }
}
