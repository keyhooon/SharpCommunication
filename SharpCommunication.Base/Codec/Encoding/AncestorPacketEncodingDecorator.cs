using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{

    public class AncestorPacketEncoding<T> : EncodingDecorator, IAncestorPacketEncoding<T> where T : IAncestorPacket
    {
        public byte Id { get; }
        public Type PacketType => typeof(T);
        public AncestorPacketEncoding(EncodingDecorator encoding, byte id) : base(encoding)
        {
            Id = id;
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
            Encoding.EncodeCore(packet, writer);

        }

        public T Decode(BinaryReader reader)
        {
            return (T)Encoding.DecodeCore(reader);
        }
    }
    public interface IAncestorPacketEncoding<T> : IEncoding<T> where T : IAncestorPacket
    {
        byte Id { get; }
        Type PacketType =>typeof(T);
    }

}
