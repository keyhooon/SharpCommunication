using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
    public class UnixTimeEpochPacketEncoding : EncodingDecorator, IUnixTimeEpochPacketEncoding
    {
        public UnixTimeEpochPacketEncoding(EncodingDecorator encoding) : base(encoding)
        {
        }

        public override void Encode(IPacket packet, BinaryWriter writer)
        {
            var unixTimeEpochPacket = (IUnixTimeEpochPacket)packet;
            writer.Write(unixTimeEpochPacket.DateTime.ToUnixEpoch());
            Encoding.Encode(unixTimeEpochPacket, writer);
        }

        public override IPacket Decode(BinaryReader reader)
        {
            var datetime = reader.ReadUInt32().ToUnixTime();
            var unixTimeEpochPacket = (IUnixTimeEpochPacket)Encoding.Decode(reader);
            unixTimeEpochPacket.DateTime = datetime;
            return unixTimeEpochPacket;
        }
    }
    public interface IUnixTimeEpochPacketEncoding : IEncoding<IPacket>
    {

    }
}

