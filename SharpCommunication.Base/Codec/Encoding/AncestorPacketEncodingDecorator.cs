using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{

    public class AncestorPacketEncoding : EncodingDecorator, IAncestorPacketEncoding<IAncestorPacket> 
    {
        public event EventHandler<EncodingOperationFinishedEventArgs> EncodeFinished;
        public event EventHandler<EncodingOperationFinishedEventArgs> DecodeFinished;


        public virtual byte Id { get; }
        public virtual Type PacketType { get; }
        public AncestorPacketEncoding(EncodingDecorator encoding, byte id, Type packetType) : base(encoding)
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

}
