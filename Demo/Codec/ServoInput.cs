using SharpCommunication.Codec.Encoding;
using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    class ServoInput : IPacket, IAncestorPacket
    {

        public double Throttle { get; set; }
        public double Pedal { get; set; }
        public double Cruise { get; set; }
        public bool IsBreak { get; set; }


        public ServoInput()
        {

        }
        public override string ToString()
        {

            return $"Servo Input - Throttle : {Throttle}, Pedal : {Pedal}, " +
                $"Cruise : {Cruise}, IsBreak : {IsBreak}, ";
        }
        public class Encoding : AncestorPacketEncoding<ServoInput>
        {
            private static readonly byte _byteCount = 7;
            private static readonly double _throttleBitResolution = 0.001953125;
            private static readonly double _pedalBitResolution = 0.001953125;
            private static readonly double _cruiseBitResolution = 0.001953125;
            private static readonly double _throttleBias = 0.0d;
            private static readonly double _pedalBias = 0.0d;
            private static readonly double _cruiseBias = 0.0d;

            public new const byte Id = 9;
            public Encoding(EncodingDecorator encoding) : base(encoding, Id)
            {

            }
            public Encoding() : this(null)
            {

            }

            public override void EncodeCore(IPacket packet, BinaryWriter writer)
            {
                var o = (ServoInput)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.Throttle - _throttleBias) / _throttleBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Pedal - _pedalBias) / _pedalBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Cruise - _cruiseBias) / _cruiseBitResolution));
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
                var value = reader.ReadBytes(_byteCount);
                byte crc8 = 0;
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                if (crc8 == reader.ReadByte())
                    return new ServoInput()
                    {
                        Throttle = BitConverter.ToUInt16(value.Take(2).ToArray()) * _throttleBitResolution + _throttleBias,
                        Pedal = BitConverter.ToUInt16(value.Skip(2).Take(2).ToArray()) * _pedalBitResolution + _pedalBias,
                        Cruise = BitConverter.ToUInt16(value.Skip(4).Take(2).ToArray()) * _cruiseBitResolution + _cruiseBias,
                        IsBreak = (value.Skip(6).First() == 1)? true: false
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }

}
