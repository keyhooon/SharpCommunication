using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
    public class PropertyPacketEncoding : EncodingDecorator, IPropertyPacketEncoding
    {
        public byte PropertySize { get; }
        public PropertyPacketEncoding(EncodingDecorator encoding, byte propertySize) : base(encoding)
        {
            PropertySize = propertySize;
        }


        public override void Encode(IPacket packet, BinaryWriter writer)
        {
            var propertyPacket = (IPropertyPacket)packet;
            writer.Write(propertyPacket.PropertyBinary, 0, PropertySize);
            Encoding.Encode(packet, writer);
        }

        public override IPacket Decode(BinaryReader reader)
        {
            var binary = reader.ReadBytes(PropertySize);
            var propertyPacket = (IPropertyPacket)Encoding.Decode(reader);
            propertyPacket.PropertyBinary = binary;
            return propertyPacket;
        }
    }
    public interface IPropertyPacketEncoding : IEncoding<IPacket>
    {
        byte PropertySize { get; }
    }
}

