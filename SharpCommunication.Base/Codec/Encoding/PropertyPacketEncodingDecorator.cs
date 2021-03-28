using System;
using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
    public class PropertyPacketEncoding<T> : EncodingDecorator, IPropertyPacketEncoding
    {
        private readonly Func<IPropertyPacket<T>, byte[]> _encode;
        private readonly Action<byte[], IPropertyPacket<T>> _decode;
        private byte PropertySize { get; }
        public PropertyPacketEncoding(EncodingDecorator encoding, byte propertySize, Func<IPropertyPacket<T>, byte[]> Encode, Action<byte[], IPropertyPacket<T>> Decode) : base(encoding)
        {
            _encode = Encode;
            _decode = Decode;
            PropertySize = propertySize;
        }


        public override void Encode(IPacket packet, BinaryWriter writer)
        {

            var propertyPacket = (IPropertyPacket<T>)packet;
            var binary = _encode(propertyPacket);
            writer.Write(binary);
            Encoding.Encode(packet, writer);
        }

        public override IPacket Decode(BinaryReader reader)
        {
            var binary = reader.ReadBytes(PropertySize);
            var propertyPacket = (IPropertyPacket<T>)Encoding.Decode(reader);
            _decode(binary, propertyPacket);
            return propertyPacket;
        }
    }
    public interface IPropertyPacketEncoding : IEncoding<IPacket>
    {

    }
}

