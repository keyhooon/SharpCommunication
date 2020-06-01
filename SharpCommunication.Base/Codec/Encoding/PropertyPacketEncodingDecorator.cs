using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
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

}

