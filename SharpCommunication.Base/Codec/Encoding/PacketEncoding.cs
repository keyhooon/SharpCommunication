using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{

    public abstract class PacketEncoding : IEncoding<IPacket>
    {
        public PacketEncoding Encoding { get; }
        public PacketEncoding(PacketEncoding encoding)
        {
            Encoding = encoding;
        }

        public abstract void EncodeCore(IPacket packet, BinaryWriter writer);

        public abstract IPacket DecodeCore(BinaryReader reader);

    }
}
