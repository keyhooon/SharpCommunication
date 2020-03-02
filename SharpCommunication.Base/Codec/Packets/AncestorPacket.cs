using System.Collections.Generic;
using System.IO;

namespace SharpCommunication.Base.Codec.Packets
{
    public interface IAncestorPacket : IPacket
    {
        public byte Id { get; }
    }

    public class AncestorPacketEncoding : PacketEncoding 
    {
        public byte Id { get; protected set; }
        public AncestorPacketEncoding(PacketEncoding encoding, byte id) : base(encoding)
        {
            Id = id;
        }

        public override void EncodeCore(IPacket packet, BinaryWriter writer)
        {
            Encoding.EncodeCore(packet, writer);
        }

        public override IPacket DecodeCore(BinaryReader reader)
        {
            return Encoding.DecodeCore(reader);
        }
    }
    public static class HasAnsectorPacketHelper
    {
        public static PacketEncodingBuilder WithAncestor(this PacketEncodingBuilder packetEncodingBuilder, byte id)
        {
            packetEncodingBuilder.SetupActions.Add(item => new AncestorPacketEncoding(item, id));
            return packetEncodingBuilder;
        }

    }
}
