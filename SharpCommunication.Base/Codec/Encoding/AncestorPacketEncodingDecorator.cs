using SharpCommunication.Base.Codec.Packets;
using System.Collections.Generic;
using System.IO;

namespace SharpCommunication.Base.Codec.Encoding
{

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

}
