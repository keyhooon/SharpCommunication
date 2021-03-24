using System.IO;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    public class LightState : IAncestorPacket
    {

        public bool Light1 { get; set; }
        public bool Light2 { get; set; }
        public bool Light3 { get; set; }
        public bool Light4 { get; set; }

        public override string ToString()
        {

            return $"Light State {{ Light1 : {Light1}" +
                $", Light2 : {Light2}" +
                $", Light3 : {Light3}" +
                $", Light4 : {Light4} }}";
        }
        public class Encoding : AncestorPacketEncoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding, 6, typeof(LightState))
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (LightSetting)packet;
                byte crc8 = 0;
                var value = (byte)(o.Light1 | o.Light2 << 1 | o.Light3 << 2 | o.Light4 << 3);
                crc8 += value;
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadByte();
                byte crc8 = 0;
                crc8 += value;
                if (crc8 == reader.ReadByte())
                    return new LightState
                    {
                        Light1 = (value & 1) == 1,
                        Light2 = (value >> 1 & 1) == 1,
                        Light3 = (value >> 2 & 1) == 1,
                        Light4 = (value >> 3 & 1) == 1
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));
        }
    }
}
