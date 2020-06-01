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

            return $"Servo Output - ActivityPercent : {ActivityPercent}, WheelSpeed : {WheelSpeed}, ";
        }
        public class Encoding : AncestorPacketEncoding<ServoOutput>
        {
            private static readonly double ActivityPercentBitResolution = 0.0015259021896696d;
            private static readonly double WheelSpeedBitResolution = 0.000244140625 / 60;
            private static readonly double ActivityPercentBias = 0.0d;
            private static readonly double WheelSpeedBias = 0.0d;
            private static byte ByteCount = 5;

            public new const byte Id = 50;
            public Encoding(PacketEncoding encoding) : base(encoding, Id)
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void EncodeCore(IPacket packet, BinaryWriter writer)
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

            public override IPacket DecodeCore(BinaryReader reader)
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
