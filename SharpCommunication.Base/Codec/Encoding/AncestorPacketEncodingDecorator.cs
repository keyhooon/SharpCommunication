using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{

    public abstract class AncestorPacketEncoding : EncodingDecorator, IAncestorPacketEncoding<IAncestorPacket> 
    {
        public abstract byte Id { get; }
        public abstract Type PacketType { get; }
        public AncestorPacketEncoding(EncodingDecorator encoding) : base(encoding)
        {

        }

        public override void Encode(IPacket packet, BinaryWriter writer)
        {
            Encoding?.Encode(packet, writer);
        }

        public override IPacket Decode(BinaryReader reader)
        {
            return Encoding?.Decode(reader);
        }
    }
    public interface IAncestorPacketEncoding<in T> : IEncoding<IPacket> where T : IAncestorPacket
    {
        byte Id { get; }
        Type PacketType { get; }
    }

}
