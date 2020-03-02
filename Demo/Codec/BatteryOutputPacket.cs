using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;
using System.IO;
using System.Linq;

namespace Demo.Codec
{
    class BatteryOutputPacket : IPacket, IAncestorPacket
    {
        public static byte id = 2;
        public static byte byteCount = 6;
        public byte Id => id;
        public double Current { get; set; }
        public double Voltage { get; set; }
        public double Temprature { get; set; }

        private static double CurrentBitResolution = 0.125d;
        private static double VoltageBitResolution = 0.25d;
        private static double TempratureBitResolution = 0.125d;

        private static double CurrentBias = 0.0d;
        private static double VoltageBias = 20.0d;
        private static double TempratureBias = 0.0d;

        public override string ToString()
        {

            return $"Battery Output - Current : {Current}, Voltage : {Voltage}, " +
                $"Temprature : {Temprature} ";
        }

        public BatteryOutputPacket()
        {

        }
        public class Encoding : AncestorPacketEncoding
        {

            public Encoding(PacketEncoding encoding) : base(encoding, id)
            {

            }
            public Encoding():base (null, id)
            {

            }

            public override void EncodeCore(IPacket packet, BinaryWriter writer)
            {
                var o = (BatteryOutputPacket)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.Current - CurrentBias) / CurrentBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Voltage - VoltageBias) / VoltageBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Temprature - TempratureBias) / TempratureBitResolution));
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
                    return new BatteryOutputPacket()
                    {
                        Current = BitConverter.ToUInt16(value.Take(2).ToArray()) * CurrentBitResolution + CurrentBias,
                        Voltage = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * VoltageBitResolution + VoltageBias,
                        Temprature = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * TempratureBitResolution + TempratureBias,
                    };
                return null;
            }

        }

    }

    public static class BatteryOutputPacketHelper
    {
        public static PacketEncodingBuilder CreateBatteryOutputEncodingBuilder(this PacketEncodingBuilder PacketEncodingBuilder)
        {
            PacketEncodingBuilder.SetupActions.Add(item => new BatteryOutputPacket.Encoding(item));
            return PacketEncodingBuilder;
        }

    }
}
