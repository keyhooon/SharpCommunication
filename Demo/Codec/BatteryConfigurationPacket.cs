
using System;
using System.IO;
using System.Linq;
using System.Text;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
namespace Demo.Codec
{
    public class BatteryConfigurationPacket : IPacket, IAncestorPacket
    {
        public readonly static byte id = 1;
        public const byte byteCount = 10;
        public int Id => id;
        public double OverCurrent { get; set; }
        public double OverVoltage { get; set; }
        public double UnderVoltage { get; set; }
        public double NominalVoltage { get; set; }
        public double OverTemprature { get; set; }

        private static double overCurrentBitResolution = 0.125d;
        private static double overVoltageBitResolution = 0.25d;
        private static double underVoltageBitResolution = 0.125d;
        private static double nominalVoltageBitResolution = 1.0d;
        private static double overTempratureBitResolution = 0.25d;

        private static double overCurrentBias = 0.125d;
        private static double overVoltageBias = 0.25d;
        private static double underVoltageBias = 0.125d;
        private static double nominalVoltageBias = 1.0d;
        private static double overTempratureBias = 0.25d;

        public BatteryConfigurationPacket()
        {

        }
        public override string ToString()
        {

            return $"Battery Configuration - OverCurrent : {OverCurrent}, OverVoltage : {OverVoltage}, " +
                $"UnderVoltage : {UnderVoltage}, NominalVoltage : {NominalVoltage}, " +
                $"OverTemprature : {OverTemprature}, ";
        }
        public class Encoding : AncestorPacketEncoding
        {

            public Encoding(PacketEncoding encoding) : base(encoding, id)
            {

            }
            public Encoding() : base(null, id)
            {

            }

            public override void EncodeCore(IPacket packet, BinaryWriter writer)
            {
                var o = (BatteryConfigurationPacket)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.OverCurrent - overCurrentBias) / overCurrentBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.OverVoltage - overVoltageBias) / overVoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.UnderVoltage - underVoltageBias) / underVoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.NominalVoltage - nominalVoltageBias) / nominalVoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.OverTemprature - overTempratureBias) / overTempratureBitResolution));
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
                    return new BatteryConfigurationPacket()
                    {
                        OverCurrent = BitConverter.ToUInt16(value.Take(2).ToArray()) * overCurrentBitResolution + overCurrentBias,
                        OverVoltage = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * overVoltageBitResolution + overVoltageBias,
                        UnderVoltage = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * underVoltageBitResolution + underVoltageBias,
                        NominalVoltage = BitConverter.ToUInt16(value.Skip(6).Take(2).ToArray()) * nominalVoltageBitResolution + nominalVoltageBias,
                        OverTemprature = BitConverter.ToUInt16(value.Skip(8).Take(2).ToArray()) * overTempratureBitResolution + overTempratureBias,
                    };
                return null;
            }
        }

    }

    public static class BatteryConfigurationPacketHelper
    {
        public static PacketEncodingBuilder CreateBatteryConfigurationEncodingBuilder(this PacketEncodingBuilder packetEncodingBuilder)
        {

            packetEncodingBuilder.SetupActions.Add(item => new BatteryConfigurationPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}
