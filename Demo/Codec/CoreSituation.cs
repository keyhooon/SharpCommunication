using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    class CoreSituation : IAncestorPacket
    {

        public double Temperature { get; set; }
        public double Voltage { get; set; }

        public override string ToString()
        {
            return $"Battery Situation {{ Temperature : {Temperature}, Voltage : {Voltage} }} ";
        }

        public class Encoding : AncestorPacketEncoding
        {
            private const byte ByteCount = 4;
            private const double TemperatureBitResolution = 0.0625d;
            private const double VoltageBitResolution = 0.0625d;
            private const double TemperatureBias = 0.0d;
            private const double VoltageBias = 0.0d;


            public Encoding(EncodingDecorator encoding) : base(encoding, 3, typeof(CoreSituation))
            {

            }
            public Encoding() : this(null)
            {

            }
            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (CoreSituation)packet;
                var value = BitConverter.GetBytes((ushort)((o.Temperature - TemperatureBias) / TemperatureBitResolution));
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Voltage - VoltageBias) / VoltageBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(ByteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                if (crc8 == reader.ReadByte())
                    return new CoreSituation
                    {
                        Temperature = BitConverter.ToUInt16(value.Take(2).ToArray()) * TemperatureBitResolution + TemperatureBias,
                        Voltage = BitConverter.ToUInt16( value.Skip(2).Take(2).ToArray()) * VoltageBitResolution + VoltageBias,
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() => 
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }
}
