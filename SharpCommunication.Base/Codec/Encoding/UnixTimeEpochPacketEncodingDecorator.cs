using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
    public class UnixTimeEpochPacketEncoding<T> : EncodingDecorator, IUnixTimeEpochPacketEncoding<T> where T : IUnixTimeEpochPacket
    {
        public UnixTimeEpochPacketEncoding(EncodingDecorator encoding) : base(encoding)
        {
        }

        public override void EncodeCore(IPacket packet, BinaryWriter writer)
        {
            Encode((T)packet, writer);
        }

        public override IPacket DecodeCore(BinaryReader reader)
        {
            return Decode(reader);
        }

        public void Encode(T packet, BinaryWriter writer)
        {
            var unixTimeEpochPacket = (IUnixTimeEpochPacket)packet;
            writer.Write(unixTimeEpochPacket.DateTime.ToUnixEpoch());
            Encoding.EncodeCore(unixTimeEpochPacket, writer);
        }

        public T Decode(BinaryReader reader)
        {
            var datetime = reader.ReadUInt32().ToUnixTime();
            var unixTimeEpochPacket = (IUnixTimeEpochPacket)Encoding.DecodeCore(reader);
            unixTimeEpochPacket.DateTime = datetime;
            return (T)unixTimeEpochPacket;
        }
    }
    public interface IUnixTimeEpochPacketEncoding<T> : IEncoding<T> where T : IUnixTimeEpochPacket
    {

    }
}

