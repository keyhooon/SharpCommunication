using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
    public class HasUnixTimeEpochPacketEncoding : PacketEncoding
    {
        public HasUnixTimeEpochPacketEncoding(PacketEncoding encoding) : base(encoding)
        {
        }

        public override void EncodeCore(IPacket packet, BinaryWriter writer)
        {
            var unixTimeEpochPacket = (IUnixTimeEpochPacket)packet;
            writer.Write(unixTimeEpochPacket.DateTime.ToUnixEpoch());
            Encoding.EncodeCore(unixTimeEpochPacket, writer);
        }

        public override IPacket DecodeCore(BinaryReader reader)
        {
            var datetime = reader.ReadUInt32().ToUnixTime();
            var unixTimeEpochPacket = (IUnixTimeEpochPacket) Encoding.DecodeCore(reader);
            unixTimeEpochPacket.DateTime = datetime;
            return unixTimeEpochPacket;
        }

    }

}

