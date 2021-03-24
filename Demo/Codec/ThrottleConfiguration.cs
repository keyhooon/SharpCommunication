
using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    class ThrottleConfiguration : IAncestorPacket
    {

        public double FaultThreshold { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }


        public override string ToString()
        {

            return $"Throttle Configuration {{ FaultThreshold : {FaultThreshold}v, Min : {Min}v, " +
                $"Max : {Max}v }}";
        }
        public class Encoding : AncestorPacketEncoding
        {
            public static readonly byte ByteCount = 6;
            private const double FaultThresholdBitResolution = 5.035477225909819e-5;
            private const double MinBitResolution = 5.035477225909819e-5;
            private const double MaxBitResolution = 5.035477225909819e-5;
            private const double FaultThresholdBias = 0.0d;
            private const double MinBias = 0.0d;
            private const double MaxBias = 0.0d;

            public Encoding(EncodingDecorator encoding) : base(encoding, 10, typeof(ThrottleConfiguration))
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (ThrottleConfiguration)packet;
                var value = BitConverter.GetBytes((ushort)((o.FaultThreshold - FaultThresholdBias) / FaultThresholdBitResolution));
                byte crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Min - MinBias) / MinBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Max - MaxBitResolution) / MaxBias));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(ByteCount);
                byte crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                if (crc8 == reader.ReadByte())
                    return new ThrottleConfiguration
                    {
                        FaultThreshold = BitConverter.ToUInt16(value.Take(2).ToArray()) * FaultThresholdBitResolution + FaultThresholdBias,
                        Min = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * MinBitResolution + MinBias,
                        Max = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * MaxBias + MaxBitResolution,
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));

        }

    }
}