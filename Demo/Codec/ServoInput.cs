using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    public class ServoInput : IAncestorPacket
    {

        public double Throttle { get; set; }
        public double Pedal { get; set; }
        public double Cruise { get; set; }
        public bool IsBreak { get; set; }


        public override string ToString()
        {

            return $"Servo Input {{ Throttle : {Throttle}, Pedal : {Pedal}, " +
                $"Cruise : {Cruise}, IsBreak : {IsBreak} }} ";
        }
        public class Encoding : AncestorPacketEncoding
        {
            private const byte ByteCount = 7;
            private const double ThrottleBitResolution = 0.001953125;
            private const double PedalBitResolution = 0.001953125;
            private const double CruiseBitResolution = 0.001953125;
            private const double ThrottleBias = 0.0d;
            private const double PedalBias = 0.0d;
            private const double CruiseBias = 0.0d;

            public Encoding(EncodingDecorator encoding) : base(encoding, 10, typeof(ServoInput))
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (ServoInput)packet;
                var value = BitConverter.GetBytes((ushort)((o.Throttle - ThrottleBias) / ThrottleBitResolution));
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Pedal - PedalBias) / PedalBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Cruise - CruiseBias) / CruiseBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = new[] { (o.IsBreak?(byte)1:(byte)0)};
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(ByteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                if (crc8 == reader.ReadByte())
                    return new ServoInput
                    {
                        Throttle = BitConverter.ToUInt16(value.Take(2).ToArray()) * ThrottleBitResolution + ThrottleBias,
                        Pedal = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * PedalBitResolution + PedalBias,
                        Cruise = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * CruiseBitResolution + CruiseBias,
                        IsBreak = (value.Skip(6).First() == 1)
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }

}
