using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
    public class HeaderPacketEncoding : EncodingDecorator, IHeaderPacketEncoding
    {
        public byte[] Header { get; }
        public HeaderPacketEncoding(EncodingDecorator encoding, byte[] header) : base(encoding)
        {
            Header = header;
        }

        public override void EncodeCore(IPacket packet, BinaryWriter writer)
        {
            writer.Write(Header);
            Encoding.EncodeCore(packet, writer);
        }

        public override IPacket DecodeCore( BinaryReader reader)
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
            return Encoding.DecodeCore(reader);
        }
    }
    public interface IHeaderPacketEncoding
    {
        byte[] Header { get; }
    }

}
