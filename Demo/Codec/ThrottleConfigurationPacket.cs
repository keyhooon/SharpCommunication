
using System;
using System.IO;
using System.Linq;
using System.Text;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;

namespace Demo.Codec
{
    class ThrottleConfigurationPacket : IPacket, IAncestorPacket
    {
        public readonly static byte id = 10;
        public const byte byteCount = 6;
        public int Id => id;
        public double FaultThreshold { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }

        private static double FaultThresholdBitResolution = 5.035477225909819e-5;
        private static double MinBitResolution = 5.035477225909819e-5;
        private static double MaxBitResolution = 5.035477225909819e-5;


        private static double FaultThresholdBias = 0.0d;
        private static double MinBias = 0.0d;
        private static double MaxBias = 0.0d;

        public ThrottleConfigurationPacket()
        {

        }
        public override string ToString()
        {

            return $"Throttle Configuration - FaultThreshold : {FaultThreshold}v, Min : {Min}v, " +
                $"Max : {Max}v";
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
                var o = (ThrottleConfigurationPacket)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.FaultThreshold - FaultThresholdBias) / FaultThresholdBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Min - MinBias) / MinBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Max - MaxBitResolution) / MaxBias));
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
                    return new ThrottleConfigurationPacket()
                    {
                        FaultThreshold = BitConverter.ToUInt16(value.Take(2).ToArray()) * FaultThresholdBitResolution + FaultThresholdBias,
                        Min = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * MinBitResolution + MinBias,
                        Max = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * MaxBias + MaxBitResolution,
                    };
                return null;
            }
        }

    }

    public static class ThrottleConfigurationPacketHelper
    {
        public static PacketEncodingBuilder CreateThrottleConfigurationEncodingBuilder(this PacketEncodingBuilder packetEncodingBuilder)
        {

            packetEncodingBuilder.SetupActions.Add(item => new ThrottleConfigurationPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}