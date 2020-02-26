using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System.IO;

namespace Demo.Codec
{
    class CoreConfigurationPacket : IPacket, IAncestorPacket
    {
        private const int id = 4;
        public int Id => id;
        public string UniqueId { get; set; }
        public string FirmwareVersion{ get; set; }
        public string ModelVersion { get; set; }

        public CoreConfigurationPacket()
        {

        }
        public class Encoding : AncestorPacketEncoding<CoreConfigurationPacket>
        {

            public Encoding(IEncoding<CoreConfigurationPacket> encoding) : base(encoding, id)
            {

            }
            public Encoding() : base(null, id)
            {

            }

            public override void EncodeCore(CoreConfigurationPacket packet, BinaryWriter writer)
            {
                writer.Write(packet.UniqueId.ToByteArray());
                writer.Write(packet.FirmwareVersion.ToByteArray());
                writer.Write(packet.ModelVersion.ToByteArray());
            }

            public override CoreConfigurationPacket DecodeCore(BinaryReader reader)
            {
                return new CoreConfigurationPacket()
                {
                    UniqueId = reader.ReadBytes(12).ToHexString(),
                    FirmwareVersion = reader.ReadBytes(2).ToHexString(),
                    ModelVersion = reader.ReadBytes(2).ToHexString(),
                };
            }
        }

    }

    public static class CoreConfigurationPacketHelper
    {
        public static PacketEncodingBuilder WithBatteryConfigurationPacket(this PacketEncodingBuilder mapItemBuilder)
        {
            mapItemBuilder.SetupActions.Add(item => (IEncoding<IPacket>)new CoreConfigurationPacket.Encoding(item));
            return mapItemBuilder;
        }

    }
}