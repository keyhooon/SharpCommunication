
using System.IO;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
namespace Demo.Codec
{
    public class BatteryConfigurationPacket : IPacket, IAncestorPacket
    {
        private const int id = 1;
        public int Id => id;
        public double OverCurrent { get; set; }
        public double OverVoltage { get; set; }
        public double UnderVoltage { get; set; }
        public double NominalVoltage { get; set; }
        public double OverTemprature { get; set; }

        private static double overCurrentBitResolution = 0.125d;
        private static double overVoltageBitResolution = 0.25d;
        private static double underVoltageBitResolution = 0.125d;
        private static double nominalVoltageBitResolution = 1.0d;
        private static double overTempratureBitResolution = 0.25d;

        private static double overCurrentBias = 0.125d;
        private static double overVoltageBias = 0.25d;
        private static double underVoltageBias = 0.125d;
        private static double nominalVoltageBias = 1.0d;
        private static double overTempratureBias = 0.25d;

        public BatteryConfigurationPacket()
        {

        }
        public class Encoding : AncestorPacketEncoding<BatteryConfigurationPacket>
        {

            public Encoding(IEncoding<BatteryConfigurationPacket> encoding) : base(encoding,id)
            {

            }
            public Encoding() : base(null, id)
            {

            }

            public override void EncodeCore(BatteryConfigurationPacket packet, BinaryWriter writer)
            {
                writer.Write((byte)((packet.OverCurrent - overCurrentBias) / overCurrentBitResolution));
                writer.Write((byte)((packet.OverCurrent - overVoltageBias) / overVoltageBitResolution));
                writer.Write((byte)((packet.OverCurrent - underVoltageBias) / underVoltageBitResolution));
                writer.Write((byte)((packet.OverCurrent - nominalVoltageBias) / nominalVoltageBitResolution));
                writer.Write((byte)((packet.OverCurrent - overTempratureBias) / overTempratureBitResolution));

            }

            public override BatteryConfigurationPacket DecodeCore(BinaryReader reader)
            {
                return new BatteryConfigurationPacket()
                {
                    OverCurrent = (reader.ReadByte() * overCurrentBitResolution) + overCurrentBias,
                    OverVoltage = (reader.ReadByte() * overVoltageBitResolution) + overVoltageBias,
                    UnderVoltage = reader.ReadByte() * underVoltageBitResolution + underVoltageBias,
                    NominalVoltage = reader.ReadByte() * nominalVoltageBitResolution + nominalVoltageBias,
                    OverTemprature = reader.ReadByte() * overTempratureBitResolution + overTempratureBias,
                };
            }
        }

    }

    public static class BatteryConfigurationPacketHelper
    {
        public static PacketEncodingBuilder WithBatteryConfigurationPacket(this PacketEncodingBuilder mapItemBuilder)
        {
            mapItemBuilder.SetupActions.Add(item => (IEncoding<IPacket>)new BatteryConfigurationPacket.Encoding(item));
            return mapItemBuilder;
        }

    }
}
