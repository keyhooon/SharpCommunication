
using System;
using System.IO;
using System.Linq;
using System.Text;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;

namespace Demo.Codec
{
    class ServoInputPacket : IPacket, IAncestorPacket
    {
        public readonly static byte id = 9;
        public const byte byteCount = 7;
        public int Id => id;
        public double Throttle { get; set; }
        public double Pedal { get; set; }
        public double Cruise { get; set; }
        public bool IsBreak { get; set; }

        private static double ThrottleBitResolution = 0.001953125;
        private static double PedalBitResolution = 0.001953125;
        private static double CruiseBitResolution = 0.001953125;


        private static double ThrottleBias = 0.0d;
        private static double PedalBias = 0.0d;
        private static double CruiseBias = 0.0d;

        public ServoInputPacket()
        {

        }
        public override string ToString()
        {

            return $"Servo Input - Throttle : {Throttle}, Pedal : {Pedal}, " +
                $"Cruise : {Cruise}, IsBreak : {IsBreak}, ";
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
                var o = (ServoInputPacket)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.Throttle - ThrottleBias) / ThrottleBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Pedal - PedalBias) / PedalBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Cruise - CruiseBias) / CruiseBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = new byte[] { (o.IsBreak?(byte)1:(byte)0)};
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
                    return new ServoInputPacket()
                    {
                        Throttle = BitConverter.ToUInt16(value.Take(2).ToArray()) * ThrottleBitResolution + ThrottleBias,
                        Pedal = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * PedalBitResolution + PedalBias,
                        Cruise = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * CruiseBitResolution + CruiseBias,
                        IsBreak = (value.Skip(6).First() == 1)? true: false
                    };
                return null;
            }
        }

    }

    public static class ServoInputPacketHelper
    {
        public static PacketEncodingBuilder CreateServoInputEncodingBuilder(this PacketEncodingBuilder packetEncodingBuilder)
        {

            packetEncodingBuilder.SetupActions.Add(item => new ServoInputPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}
