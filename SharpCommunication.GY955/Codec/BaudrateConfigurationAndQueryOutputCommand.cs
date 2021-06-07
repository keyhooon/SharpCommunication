using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpCommunication.GY955.Codec
{
    public class BaudrateConfigurationAndQueryOutput : IPacket
    {
        private byte Value { get; set; }
        public void SetRequestAccelerometer() => Value = 0x15;
        public void SetRequestGyroScope() => Value = 0x25;
        public void SetRequestMagnetometer() => Value = 0x35;
        public void SetRequestEuler() => Value = 0x45;
        public void SetRequestQuaternion() => Value = 0x55;
        public void SetBaudRate9600() => Value = 0xAE;
        public void SetBaudRate115200() => Value = 0xAF;

        public override string ToString()
        {
            if (Value == 0x15)
                return $"Query Output {{ Accelerometer }} ";
            else if (Value == 0x25)
                return $"Query Output {{ Gyroscope }} ";
            else if (Value == 0x35)
                return $"Query Output {{ Magnetometer }} ";
            else if (Value == 0x45)
                return $"Query Output {{ Euler }} ";
            else if (Value == 0x55)
                return $"Query Output {{ Quaternion }} ";
            else if (Value == 0xAE)
                return $"Baudrate Configuration {{ 9600 }} ";
            else if (Value == 0xAF)
                return $"Baudrate Configuration {{ 115200 }} ";
            else
                return string.Empty;
        }

        public class Encoding : AncestorPacketEncoding
        {


            public Encoding(EncodingDecorator encoding) : base(encoding, 0xA5, typeof(BaudrateConfigurationAndQueryOutput))
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var pac = (OutputConfigurationRegister)packet;
                writer.Write((byte)pac.Value);
                writer.Write((byte)(0xA5 + pac.Value));
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var data = reader.ReadByte();
                var crc8 = (byte)(data + 0xA5);
                return crc8 != reader.ReadByte() ? null : new BaudrateConfigurationAndQueryOutput() { Value = data };
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }
}
