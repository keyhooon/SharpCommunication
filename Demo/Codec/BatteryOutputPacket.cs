using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System.IO;

namespace Demo.Codec
{
    class BatteryOutputPacket : IPacket, IAncestorPacket
    {
        private const int id = 2;
        public int Id => id;
        public double Current { get; set; }
        public double Voltage { get; set; }
        public double Temprature { get; set; }

        private static double CurrentBitResolution = 0.125d;
        private static double VoltageBitResolution = 0.25d;
        private static double TempratureBitResolution = 0.125d;

        private static double CurrentBias = 0.0d;
        private static double VoltageBias = 20.0d;
        private static double TempratureBias = 0.0d;

        public BatteryOutputPacket()
        {

        }
        public class Encoding : AncestorPacketEncoding<BatteryOutputPacket>
        {

            public Encoding(IEncoding<IPacket> encoding) : base(encoding, id)
            {

            }

            public override void EncodeCore(BatteryOutputPacket packet, BinaryWriter writer)
            {
                writer.Write((byte)((packet.Current - CurrentBias) / CurrentBitResolution));
                writer.Write((byte)((packet.Voltage - VoltageBias) / VoltageBitResolution));
                writer.Write((short)((packet.Temprature - TempratureBias) / TempratureBitResolution));
            }

            public override BatteryOutputPacket DecodeCore(BinaryReader reader)
            {
                return new BatteryOutputPacket()
                {
                    Current = reader.ReadByte() * CurrentBitResolution + CurrentBias,
                    Voltage = reader.ReadByte() * VoltageBitResolution + VoltageBias,
                    Temprature = reader.ReadByte() * TempratureBitResolution + TempratureBias,
                };
            }
        }

    }

    public static class BatteryOutputPacketHelper
    {
        public static PacketEncodingBuilder WithBatteryConfigurationPacket(this PacketEncodingBuilder mapItemBuilder)
        {
            mapItemBuilder.SetupActions.Add(item => (IEncoding<IPacket>)new BatteryOutputPacket.Encoding(item));
            return mapItemBuilder;
        }

    }
}
