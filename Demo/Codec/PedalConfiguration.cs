using System.IO;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Communication.Codec
{
    public class PedalConfiguration : IPacket, IAncestorPacket
    {

        public byte MagnetCount { get; set; }

        public override string ToString()
        {

            return $"MagnetCount : {MagnetCount}";
        }
        public class Encoding : AncestorPacketEncoding<PedalConfiguration>
        {
            private const byte byteCount = 1;
            public new const byte Id = 7;
            public Encoding(PacketEncoding encoding) : base(encoding, Id)
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void EncodeCore(IPacket packet, BinaryWriter writer)
            {
                var o = (PedalConfiguration)packet;
                byte crc8 = 0;
                var value = o.MagnetCount;
                crc8 += value;
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket DecodeCore(BinaryReader reader)
            {
                var value = reader.ReadByte();
                byte crc8 = 0;
                crc8 += value;
                if (crc8 == reader.ReadByte())
                    return new PedalConfiguration
                    {
                        MagnetCount = value
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));

        }

    }

}

