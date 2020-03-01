using System.IO;

namespace SharpCommunication.Base.Codec.Packets
{
    public interface IPropertyPacket : IPacket
    {
        byte[] PropertyBinary { get; set; }

    }
    public class HasPropertyPacketEncoding : PacketEncoding
    {
        public byte PropertySize { get; }
        public HasPropertyPacketEncoding(PacketEncoding encoding, byte propertySize) : base(encoding)
        {
            PropertySize = propertySize;
        }

        public override void EncodeCore(IPacket packet, BinaryWriter writer)
        {
            var propertyPacket = (IPropertyPacket)packet;
            writer.Write(propertyPacket.PropertyBinary, 0, PropertySize);
            Encoding.EncodeCore(packet, writer);
        }

        public override IPacket DecodeCore( BinaryReader reader)
        {
            var binary = reader.ReadBytes(PropertySize);
            var propertyPacket = (IPropertyPacket) Encoding.DecodeCore(reader);
            propertyPacket.PropertyBinary = binary;
            return propertyPacket;
         }
    }
    public static class HasPropertyPacketHelper
    {
        public static PacketEncodingBuilder WithProperty(this PacketEncodingBuilder mapItemBuilder, byte propertySize)
        {
            mapItemBuilder.SetupActions.Add(item => new HasPropertyPacketEncoding(item, propertySize));
            return mapItemBuilder;
        }

    }
}

