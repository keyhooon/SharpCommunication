using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    public class PedalSetting : IAncestorPacket
    {
        public byte AssistLevel { get; set; }
        public byte ActivationTime { get; set; }
        public int LowLimit { get; set; }
        public int HighLimit { get; set; }

        public override string ToString()
        {

            return $"Pedal Setting {{ AssistLevel : {AssistLevel}, ActivationTime : {ActivationTime}, " +
                $"LowLimit : {LowLimit}, HighLimit : {HighLimit} }}";
        }
        public class Encoding : AncestorPacketEncoding
        {
            private const byte ByteCount = 5;

            public override byte Id => 8;

            public override Type PacketType => typeof(PedalSetting);

            public Encoding(EncodingDecorator encoding) : base(encoding, 8, typeof(PedalSetting))
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (PedalSetting)packet;
                byte crc8 = 0;
                var value = new[] { (byte)(o.AssistLevel | o.ActivationTime << 3) };
                crc8 += value[0];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)o.LowLimit);
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)o.HighLimit);
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(ByteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));

                if (crc8 == reader.ReadByte())
                    return new PedalSetting
                    {
                        AssistLevel = (byte)(value.First() & 0b111),
                        ActivationTime = (byte)(value.First() >> 3 & 0b11),
                        LowLimit = BitConverter.ToUInt16(value, 1),
                        HighLimit = BitConverter.ToUInt16(value, 3)
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));


        }
    }

}

