using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{

    public abstract class EncodingDecorator : IEncoding<IPacket>
    {
        public EncodingDecorator Encoding { get; }
        public EncodingDecorator(EncodingDecorator encoding)
        {
            Encoding = encoding;
        }

        public abstract void EncodeCore(IPacket packet, BinaryWriter writer);

        public abstract IPacket DecodeCore(BinaryReader reader);

        void IEncoding<IPacket>.Encode(IPacket packet, BinaryWriter writer)
        {
            EncodeCore(packet, writer);
        }

        IPacket IEncoding<IPacket>.Decode(BinaryReader reader)
        {
            return DecodeCore(reader);
        }
    }
}
