using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Communication.Codec
{
    public class ServoOutput : IPacket, IAncestorPacket
    {

        public double ActivityPercent { get; set; }
        public double WheelSpeed { get; set; }




        public override string ToString()
        {

            return $"Servo Output {{ ActivityPercent : {ActivityPercent}, WheelSpeed : {WheelSpeed} }} ";
        }
        public class Encoding : AncestorPacketEncoding
        {
            private static readonly double _activityPercentBitResolution = 0.0015259021896696d;
            private static readonly double _wheelSpeedBitResolution = 0.000244140625 / 60;
            private static readonly double _activityPercentBias = 0.0d;
            private static readonly double _wheelSpeedBias = 0.0d;
            private static readonly byte _byteCount = 5;

            public override byte Id => 50;

            public override Type PacketType => typeof(ServoOutput);

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (ServoOutput)packet;
                var value = BitConverter.GetBytes((ushort)((o.ActivityPercent - _activityPercentBias) / _activityPercentBitResolution));
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.WheelSpeed - _wheelSpeedBias) / _wheelSpeedBitResolution));
                crc8 = value.Aggregate(crc8, (current, t) => (byte)(current + t));
                value = new byte[] { 0 };
                crc8 = value.Aggregate(crc8, (current, t) => (byte)(current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(_byteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                if (crc8 == reader.ReadByte())
                    return new ServoOutput
                    {
                        ActivityPercent = BitConverter.ToUInt16(value, 0) * _activityPercentBitResolution + _activityPercentBias,
                        WheelSpeed = 1 / (BitConverter.ToUInt16(value, 2) * _wheelSpeedBitResolution + _wheelSpeedBias)
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }

    }

}
