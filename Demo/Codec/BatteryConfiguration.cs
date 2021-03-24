
using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    public class BatteryConfiguration: IAncestorPacket
    {
        public double OverCurrent { get; set; }
        public double OverVoltage { get; set; }
        public double UnderVoltage { get; set; }
        public double NominalVoltage { get; set; }
        public double OverTemperature { get; set; }

        public override string ToString()
        {

            return $"Battery Configuration {{ OverCurrent : {OverCurrent}, OverVoltage : {OverVoltage}, " +
                $"UnderVoltage : {UnderVoltage}, NominalVoltage : {NominalVoltage}, " +
                $"OverTemperature : {OverTemperature} }} ";
        }
        public class Encoding : AncestorPacketEncoding
        {
            private const double OverCurrentBitResolution = 0.125d;
            private const double OverVoltageBitResolution = 0.25d;
            private const double UnderVoltageBitResolution = 0.125d;
            private const double NominalVoltageBitResolution = 1.0d;
            private const double OverTemperatureBitResolution = 0.25d;
            private const double OverCurrentBias = 0.125d;
            private const double OverVoltageBias = 0.25d;
            private const double UnderVoltageBias = 0.125d;
            private const double NominalVoltageBias = 1.0d;
            private const double OverTemperatureBias = 0.25d;
            private const byte ByteCount = 10;

            public Encoding(EncodingDecorator encoding) : base(encoding,1, typeof(BatteryConfiguration))
            {
  
            }
            public Encoding() : this(null)
            {

            }
            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (BatteryConfiguration)packet;
                var value = BitConverter.GetBytes((ushort)((o.OverCurrent - OverCurrentBias) / OverCurrentBitResolution));
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.OverVoltage - OverVoltageBias) / OverVoltageBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.UnderVoltage - UnderVoltageBias) / UnderVoltageBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.NominalVoltage - NominalVoltageBias) / NominalVoltageBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.OverTemperature - OverTemperatureBias) / OverTemperatureBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                writer.Write(crc8);
            }
            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(ByteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                if (crc8 == reader.ReadByte())
                    return new BatteryConfiguration
                    {
                        OverCurrent = BitConverter.ToUInt16(value.Take(2).ToArray()) * OverCurrentBitResolution + OverCurrentBias,
                        OverVoltage = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * OverVoltageBitResolution + OverVoltageBias,
                        UnderVoltage = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * UnderVoltageBitResolution + UnderVoltageBias,
                        NominalVoltage = BitConverter.ToUInt16(value.Skip(6).Take(2).ToArray()) * NominalVoltageBitResolution + NominalVoltageBias,
                        OverTemperature = BitConverter.ToUInt16(value.Skip(8).Take(2).ToArray()) * OverTemperatureBitResolution + OverTemperatureBias,
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }
}
