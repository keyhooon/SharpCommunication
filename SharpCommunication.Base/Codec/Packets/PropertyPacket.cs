using System.IO;

namespace SharpCommunication.Base.Codec.Packets
{
    public interface IPropertyPacket : IPacket
    {
        byte[] PropertyBinary { get; set; }

    }
    public class HasPropertyPacketEncoding<T> : PacketEncoding<T> where T : IPropertyPacket
    {
        public byte PropertySize { get; }
        public HasPropertyPacketEncoding(IEncoding<T> encoding, byte propertySize) : base(encoding)
        {
            PropertySize = propertySize;
        }

        public override void EncodeCore(T packet, BinaryWriter writer)
        {
            writer.Write(packet.PropertyBinary, 0, PropertySize);
            Encoding.EncodeCore(packet, writer);
        }

        public override T DecodeCore( BinaryReader reader)
        {
            var binary = reader.ReadBytes(PropertySize);
            var obj = (IPropertyPacket)Encoding.DecodeCore(reader);
            obj.PropertyBinary = binary;
            
            return (T)obj;
         }
    }
    public static class HasPropertyPacketHelper
    {
        public static PacketEncodingBuilder WithProperty(this PacketEncodingBuilder mapItemBuilder, byte propertySize)
        {
            mapItemBuilder.SetupActions.Add(item => (IEncoding<IPacket>)new HasPropertyPacketEncoding<IPropertyPacket>((IEncoding<IPropertyPacket>)item, propertySize));
            return mapItemBuilder;
        }

    }
}

