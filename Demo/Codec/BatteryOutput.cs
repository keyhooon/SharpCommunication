using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    public class BatteryOutput : IAncestorPacket
    {
        public double Current { get; set; }
        public double Voltage { get; set; }
        public double Temperature { get; set; }

        public override string ToString()
        {

            return $"Battery Output {{ Current : {Current}, Voltage : {Voltage}, " +
                $"Temperature : {Temperature} }} ";
        }


        public class Encoding : AncestorPacketEncoding
        {
            private const byte ByteCount = 6;
            private const double CurrentBitResolution = 0.125d;
            private const double VoltageBitResolution = 0.25d;
            private const double TemperatureBitResolution = 0.125d;
            private const double CurrentBias = 0.0d;
            private const double VoltageBias = 20.0d;
            private const double TemperatureBias = 0.0d;

            public Encoding(EncodingDecorator encoding) : base(encoding, 2, typeof(BatteryOutput))
            {

            }
            public Encoding(): this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (BatteryOutput)packet;
                var value = BitConverter.GetBytes((ushort)((o.Current - CurrentBias) / CurrentBitResolution));
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Voltage - VoltageBias) / VoltageBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Temperature - TemperatureBias) / TemperatureBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(ByteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                if (crc8 == reader.ReadByte())
                    return new BatteryOutput
                    {
                        Current = BitConverter.ToUInt16(value.Take(2).ToArray()) * CurrentBitResolution + CurrentBias,
                        Voltage = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * VoltageBitResolution + VoltageBias,
                        Temperature = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * TemperatureBitResolution + TemperatureBias,
                    };
                return null;
            }

            public static PacketEncodingBuilder CreateBuilder() => 
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }
}
