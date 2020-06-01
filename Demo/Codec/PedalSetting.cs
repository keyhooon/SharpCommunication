using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;
using System.Linq;

namespace Communication.Codec
{
    public class PedalSetting : IPacket, IAncestorPacket
    {
        public byte AssistLevel { get; set; }
        public byte ActivationTime { get; set; }
        public int LowLimit { get; set; }
        public int HighLimit { get; set; }

        public override string ToString()
        {

            return $"AssistLevel : {AssistLevel}, ActivationTime : {ActivationTime}, " +
                $"LowLimit : {LowLimit}, HighLimit : {HighLimit}";
        }
        public class Encoding : AncestorPacketEncoding<PedalSetting>
        {
            public new const byte Id = 8;
            private const byte ByteCount = 5;
            public Encoding(EncodingDecorator encoding) : base(encoding, Id)
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void EncodeCore(IPacket packet, BinaryWriter writer)
            {
                var o = (PedalSetting)packet;
                byte crc8 = 0;
                var value = new byte[] { (byte)(o.AssistLevel | o.ActivationTime << 3) };
                crc8 += value[0];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)o.LowLimit);
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)o.HighLimit);
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket DecodeCore(BinaryReader reader)
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

