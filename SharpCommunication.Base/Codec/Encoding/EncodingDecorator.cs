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

        public abstract void Encode(IPacket packet, BinaryWriter writer);

        public abstract IPacket Decode(BinaryReader reader);


    }
}
