using System;
using System.IO;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Codec.Encoding
{
    public class AncestorGenericPacketEncodingDecorator<T> : EncodingDecorator, IAncestorPacketEncoding<IAncestorPacket, T>
    {
        public event EventHandler<EncodingOperationFinishedEventArgs> EncodeFinished;
        public event EventHandler<EncodingOperationFinishedEventArgs> DecodeFinished;


        public virtual T Id { get; }
        public virtual Type PacketType { get; }
        public AncestorGenericPacketEncodingDecorator(EncodingDecorator encoding, T id, Type packetType) : base(encoding)
        {
            Id = id;
            PacketType = packetType;
        }

        public override void Encode(IPacket packet, BinaryWriter writer)
        {
            Encoding?.Encode(packet, writer);
            EncodeFinished?.Invoke(this, new EncodingOperationFinishedEventArgs(packet));
        }

        public override IPacket Decode(BinaryReader reader)
        {
            var packet = Encoding?.Decode(reader);
            if (packet != null)
                DecodeFinished?.Invoke(this, new EncodingOperationFinishedEventArgs(packet));
            return packet;
        }
    }
    public class EncodingOperationFinishedEventArgs : EventArgs
    {
        public IPacket Packet { get; }
        public EncodingOperationFinishedEventArgs(IPacket packet)
        {
            Packet = packet;
        }
    }
    public interface IAncestorPacketEncoding<in T> : IEncoding<IPacket> where T : IAncestorPacket
    {
        byte Id { get; }
        Type PacketType { get; }
    }
    public interface IAncestorPacketEncoding<in T, out TG> : IEncoding<IPacket> where T : IAncestorPacket
    {
        TG Id { get; }
        Type PacketType { get; }
    }

}
