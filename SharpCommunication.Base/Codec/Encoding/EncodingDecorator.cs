using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{

    public abstract class EncodingDecorator 
    {
        public EncodingDecorator Encoding { get; }
        public EncodingDecorator(EncodingDecorator encoding)
        {
            Encoding = encoding;
        }

        public abstract void EncodeCore(IPacket packet, BinaryWriter writer);

        public abstract IPacket DecodeCore(BinaryReader reader);

 
    }
}
