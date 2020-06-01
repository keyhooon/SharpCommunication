
using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
namespace Demo.Codec
{
    public class BatteryConfiguration: IPacket, IAncestorPacket
    {
        public double OverCurrent { get; set; }
        public double OverVoltage { get; set; }
        public double UnderVoltage { get; set; }
        public double NominalVoltage { get; set; }
        public double OverTemprature { get; set; }

        public BatteryConfiguration()
        {

        }
        public override string ToString()
        {

            return $"Battery Configuration - OverCurrent : {OverCurrent}, OverVoltage : {OverVoltage}, " +
                $"UnderVoltage : {UnderVoltage}, NominalVoltage : {NominalVoltage}, " +
                $"OverTemprature : {OverTemprature}, ";
        }
        public class Encoding : PacketEncoding
        {
            private static readonly double _overCurrentBitResolution = 0.125d;
            private static readonly double _overVoltageBitResolution = 0.25d;
            private static readonly double _underVoltageBitResolution = 0.125d;
            private static readonly double _nominalVoltageBitResolution = 1.0d;
            private static readonly double _overTempratureBitResolution = 0.25d;
            private static readonly double _overCurrentBias = 0.125d;
            private static readonly double _overVoltageBias = 0.25d;
            private static readonly double _underVoltageBias = 0.125d;
            private static readonly double _nominalVoltageBias = 1.0d;
            private static readonly double _overTempratureBias = 0.25d;
            public static byte byteCount = 10;

            public const byte Id = 1;
            public Encoding(PacketEncoding encoding) : base(encoding)
            {
  
            }
            public Encoding() : this(null)
            {

            }
            public override void EncodeCore(IPacket packet, BinaryWriter writer)
            {
                var o = (BatteryConfiguration)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.OverCurrent - _overCurrentBias) / _overCurrentBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.OverVoltage - _overVoltageBias) / _overVoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.UnderVoltage - _underVoltageBias) / _underVoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.NominalVoltage - _nominalVoltageBias) / _nominalVoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.OverTemprature - _overTempratureBias) / _overTempratureBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                writer.Write(crc8);
            }
            public override IPacket DecodeCore(BinaryReader reader)
            {
                var value = reader.ReadBytes(byteCount);
                byte crc8 = 0;
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                if (crc8 == reader.ReadByte())
                    return new BatteryConfiguration()
                    {
                        OverCurrent = BitConverter.ToUInt16(value.Take(2).ToArray()) * _overCurrentBitResolution + _overCurrentBias,
                        OverVoltage = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * _overVoltageBitResolution + _overVoltageBias,
                        UnderVoltage = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * _underVoltageBitResolution + _underVoltageBias,
                        NominalVoltage = BitConverter.ToUInt16(value.Skip(6).Take(2).ToArray()) * _nominalVoltageBitResolution + _nominalVoltageBias,
                        OverTemprature = BitConverter.ToUInt16(value.Skip(8).Take(2).ToArray()) * _overTempratureBitResolution + _overTempratureBias,
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item)).AddDecorate(item=> new AncestorPacketEncoding<BatteryConfiguration>(item, Id));
        }
    }
}
