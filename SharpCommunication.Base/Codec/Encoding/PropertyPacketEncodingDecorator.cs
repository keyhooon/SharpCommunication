using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
    public class PropertyPacketEncoding<T> : EncodingDecorator, IPropertyPacketEncoding<T> where T : IPropertyPacket
    {
        public byte PropertySize { get; }
        public PropertyPacketEncoding(EncodingDecorator encoding, byte propertySize) : base(encoding)
        {
            PropertySize = propertySize;
        }

        public override void EncodeCore(IPacket packet, BinaryWriter writer)
        {
            Encode((T)packet, writer);
        }

        public override IPacket DecodeCore(BinaryReader reader)
        {
            return Decode(reader);
        }

        public void Encode(T packet, BinaryWriter writer)
        {
            var propertyPacket = (IPropertyPacket)packet;
            writer.Write(propertyPacket.PropertyBinary, 0, PropertySize);
            Encoding.EncodeCore(packet, writer);
        }

        public T Decode(BinaryReader reader)
        {
            var binary = reader.ReadBytes(PropertySize);
            var propertyPacket = (IPropertyPacket)Encoding.DecodeCore(reader);
            propertyPacket.PropertyBinary = binary;
            return (T)propertyPacket;
        }
    }
    public interface IPropertyPacketEncoding<T> : IEncoding<T> where T : IPropertyPacket
    {
        byte PropertySize { get; }
    }
}

