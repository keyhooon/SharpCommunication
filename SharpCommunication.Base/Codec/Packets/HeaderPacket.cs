using System.IO;

namespace SharpCommunication.Base.Codec.Packets
{
    public interface IHeaderPacket : IPacket
    {

    }

    public class HeaderPacketEncoding<T> : PacketEncoding<T> where T : IHeaderPacket
    {
        public byte[] Header { get; }
        public HeaderPacketEncoding(IEncoding<IPacket> encoding, byte[] header) : base(encoding)
        {
            Header = header;
        }

        public override void EncodeCore(T packet, BinaryWriter writer)
        {
            writer.Write(Header);
            Encoding.EncodeCore(packet, writer);
        }

        public override T DecodeCore( BinaryReader reader)
        {
            var found = 0;
            while (found < Header.Length)
            {
                var header = reader.ReadByte();
                if (header == Header[found])
                    found++;
                else if (header == Header[0])
                    found = 1;
                else
                    found = 0;
            }
            return (T) Encoding.DecodeCore(reader);
        }
    }
    public static class HasHeaderPacketHelper
    {
        public static PacketEncodingBuilder WithHeader(this PacketEncodingBuilder mapItemBuilder, byte[] header)
        {
            mapItemBuilder.SetupActions.Add(item => (IEncoding<IPacket>)new HeaderPacketEncoding<IHeaderPacket>(item, header));
            return mapItemBuilder;
        }

    }
}
