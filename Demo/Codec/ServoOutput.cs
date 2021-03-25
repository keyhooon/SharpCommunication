using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    public class ServoOutput : IAncestorPacket
    {

        public double ActivityPercent { get; set; }
        public double WheelSpeed { get; set; }




        public override string ToString()
        {

            return $"Servo Output {{ ActivityPercent : {ActivityPercent}, WheelSpeed : {WheelSpeed} }} ";
        }
        public class Encoding : AncestorPacketEncoding
        {
            private const double ActivityPercentBitResolution = 0.0015259021896696d;
            private const double WheelSpeedBitResolution = 0.000244140625 / 60;
            private const double ActivityPercentBias = 0.0d;
            private const double WheelSpeedBias = 0.0d;
            private const byte ByteCount = 5;

            public Encoding(EncodingDecorator encoding) : base(encoding, 11, typeof(ServoOutput))
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (ServoOutput)packet;
                var value = BitConverter.GetBytes((ushort)((o.ActivityPercent - ActivityPercentBias) / ActivityPercentBitResolution));
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.WheelSpeed - WheelSpeedBias) / WheelSpeedBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte)(current + t));
                value = new byte[] { 0 };
                crc8 = value.Aggregate(crc8, (current, t) => (byte)(current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(ByteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                if (crc8 == reader.ReadByte())
                    return new ServoOutput
                    {
                        ActivityPercent = BitConverter.ToUInt16(value, 0) * ActivityPercentBitResolution + ActivityPercentBias,
                        WheelSpeed = 1 / (BitConverter.ToUInt16(value, 2) * WheelSpeedBitResolution + WheelSpeedBias)
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }

    }

}
