using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;
using System.IO;
using System.Linq;

namespace Demo.Codec
{
    class CoreSituationPacket : IPacket, IAncestorPacket
    {
        public static byte id = 3;
        public const byte byteCount = 4;
        public int Id => id;
        public double Temprature { get; set; }
        public double Voltage { get; set; }


        private static double tempratureBitResolution = 0.0625d;
        private static double voltageBitResolution = 0.0625d;


        private static double tempratureBias = 0.0d;
        private static double voltageBias = 0.0d;
        public override string ToString()
        {
            return $"Battery Situation - Temprature : {Temprature}, Voltage : {Voltage} ";
        }
        public CoreSituationPacket()
        {

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
                var o = (CoreSituationPacket)packet;
                byte crc8 = 0;
                byte[] value;
                value = BitConverter.GetBytes((ushort)((o.Temprature - tempratureBias) / tempratureBitResolution));
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = BitConverter.GetBytes((ushort)((o.Voltage - voltageBias) / voltageBitResolution));
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
                    return new CoreSituationPacket()
                    {
                        Temprature = BitConverter.ToUInt16(value.Take(2).ToArray()) * tempratureBitResolution + tempratureBias,
                        Voltage = BitConverter.ToUInt16( value.Skip(2).Take(2).ToArray()) * voltageBitResolution + voltageBias,
                    };
                return null;
            }

        }

    }

    public static class CoreSituationPacketHelper
    {
        public static PacketEncodingBuilder CreateCoreSituationEncodingBuilder(this PacketEncodingBuilder packetEncodingBuilder)
        {
            packetEncodingBuilder.SetupActions.Add(item => new CoreSituationPacket.Encoding(item));
            return packetEncodingBuilder;
        }

    }
}
