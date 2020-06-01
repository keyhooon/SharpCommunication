using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;
using System.Linq;

namespace Demo.Codec
{
    class CoreSituation : IPacket, IAncestorPacket
    {

        public double Temprature { get; set; }
        public double Voltage { get; set; }

        public override string ToString()
        {
            return $"Battery Situation - Temprature : {Temprature}, Voltage : {Voltage} ";
        }
        public CoreSituation()
        {

        }
        public class Encoding : AncestorPacketEncoding<CoreSituation>
        {
            private static byte byteCount = 4;
            private static double _tempratureBitResolution = 0.0625d;
            private static double _voltageBitResolution = 0.0625d;
            private static double _tempratureBias = 0.0d;
            private static double _voltageBias = 0.0d;

            public new const byte Id = 3;
            public Encoding(PacketEncoding encoding) : base(encoding, Id)
            {

            }
            public Encoding() : base(null, Id)
            {

            }
            public override void EncodeCore(IPacket packet, BinaryWriter writer)
            {
                var o = (CoreSituation)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.Temprature - _tempratureBias) / _tempratureBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Voltage - _voltageBias) / _voltageBitResolution));
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
                    return new CoreSituation()
                    {
                        Temprature = BitConverter.ToUInt16(value.Take(2).ToArray()) * _tempratureBitResolution + _tempratureBias,
                        Voltage = BitConverter.ToUInt16( value.Skip(2).Take(2).ToArray()) * _voltageBitResolution + _voltageBias,
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() => 
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }
}
