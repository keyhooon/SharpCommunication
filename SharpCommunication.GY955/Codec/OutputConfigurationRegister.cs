using System;
using System.IO;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Codec
{
    public class OutputConfigurationRegister : IAncestorPacket
    {
        public OutputConfiguration Value { get; set; }

        public override string ToString()
        {

            return $"Output Configuration Register {{ Accelerometer : {Value.HasFlag(OutputConfiguration.ACC)}, " +
                $"Magnetometer : {Value.HasFlag(OutputConfiguration.MAG)}, " +
                $"Gyroscope : { Value.HasFlag(OutputConfiguration.GYRO)}, " +
                $"Euler : {Value.HasFlag(OutputConfiguration.EULER)}, " +
                $"Quaternion : { Value.HasFlag(OutputConfiguration.Q4)}, " +
                $"FiftyHertz : {Value.HasFlag(OutputConfiguration.FiftyHertz)}, " +
                $"HundredHertz : { Value.HasFlag(OutputConfiguration.HundredHertz)}, " +
                $"Auto : {Value.HasFlag(OutputConfiguration.Auto)}}} ";
        }
        [Flags]
        public enum OutputConfiguration : byte
        {
            ACC, MAG, GYRO, EULER, Q4, FiftyHertz, HundredHertz, Auto
        }

        public class Encoding : AncestorPacketEncoding
        {


            public Encoding(EncodingDecorator encoding) : base(encoding, 0xAA, typeof(OutputConfigurationRegister))
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var pac = (OutputConfigurationRegister)packet;
                writer.Write((byte)pac.Value);
                writer.Write((byte)(0xAA + pac.Value));
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var data = reader.ReadByte();
                var crc8 = (byte)(data + 0xAA);
                if (crc8 != reader.ReadByte())
                    return null;
                return new OutputConfigurationRegister() { Value = (OutputConfiguration)data };
                
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }
}
