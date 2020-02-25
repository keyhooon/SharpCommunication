using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System.IO;

namespace Demo.Codec
{
    class CoreSituationPacket : IPacket, IAncestorPacket
    {
        private const int id = 3;
        public int Id => id;
        public double Temprature { get; set; }
        public double Voltage { get; set; }


        private static double tempratureBitResolution = 0.0625d;
        private static double voltageBitResolution = 0.0625d;


        private static double tempratureBias = 0.0d;
        private static double voltageBias = 0.0d;

        public CoreSituationPacket()
        {

        }
        public class Encoding : AncestorPacketEncoding<CoreSituationPacket>
        {

            public Encoding(IEncoding<IPacket> encoding) : base(encoding, id)
            {

            }

            public override void EncodeCore(CoreSituationPacket packet, BinaryWriter writer)
            {
                writer.Write((short)((packet.Temprature - tempratureBias) / tempratureBitResolution));
                writer.Write((ushort)((packet.Voltage - voltageBias) / voltageBitResolution));


            }

            public override CoreSituationPacket DecodeCore(BinaryReader reader)
            {
                return new CoreSituationPacket()
                {
                    Temprature = reader.ReadByte() * tempratureBitResolution + tempratureBias,
                    Voltage = reader.ReadByte() * voltageBitResolution + voltageBias,
 
                };
            }
        }

    }

    public static class CoreSituationPacketHelper
    {
        public static PacketEncodingBuilder WithBatteryConfigurationPacket(this PacketEncodingBuilder mapItemBuilder)
        {
            mapItemBuilder.SetupActions.Add(item => (IEncoding<IPacket>)new CoreSituationPacket.Encoding(item));
            return mapItemBuilder;
        }

    }
}
