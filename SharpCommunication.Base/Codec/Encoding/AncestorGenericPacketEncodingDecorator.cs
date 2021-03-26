using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

    public interface IAncestorPacketEncoding<in T, out TG> : IEncoding<IPacket> where T : IAncestorPacket
    {
        TG Id { get; }
        Type PacketType { get; }
    }

}
