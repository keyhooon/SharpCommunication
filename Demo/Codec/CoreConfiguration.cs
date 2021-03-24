using System.IO;
using System.Linq;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    class CoreConfiguration : IAncestorPacket
    {
        public string UniqueId { get; set; }
        public string FirmwareVersion{ get; set; }
        public string ModelVersion { get; set; }

        public override string ToString()
        {

            return $"Core Configuration {{ UniqueId : {UniqueId}, FirmwareVersion : {FirmwareVersion}, " +
                $"ModelVersion : {ModelVersion} }}";
        }
        public class Encoding : AncestorPacketEncoding
        {

            public Encoding(EncodingDecorator encoding) : base(encoding ,4, typeof(CoreConfiguration))
            {

            }
            public Encoding() : this(null)
            {

            }
            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var o = (CoreConfiguration)packet;
                var value = o.UniqueId.ToByteArray();
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = o.FirmwareVersion.ToByteArray();
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                value = o.ModelVersion.ToByteArray();
                crc8 = value.Aggregate(crc8, (current, t) => (byte) (current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var value = reader.ReadBytes(16);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte) (current + t));
                if (crc8 == reader.ReadByte())
                    return new CoreConfiguration
                    {
                        UniqueId = value.Take(12).ToHexString(),
                        FirmwareVersion = value.Skip(12).Take(2).ToHexString(),
                        ModelVersion = value.Skip(14).Take(2).ToHexString(),
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
            
        }

    }
}