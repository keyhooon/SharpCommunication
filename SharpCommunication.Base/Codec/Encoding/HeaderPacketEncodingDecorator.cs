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


        public override IPacket Decode( BinaryReader reader)
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
            return Encoding.Decode(reader);
        }

        public override void Encode(IPacket packet, BinaryWriter writer)
        {
            writer.Write(Header);
            Encoding.Encode(packet, writer);
        }
    }
    public interface IHeaderPacketEncoding : IEncoding<IPacket>
    {
        byte[] Header { get; }
    }

}
