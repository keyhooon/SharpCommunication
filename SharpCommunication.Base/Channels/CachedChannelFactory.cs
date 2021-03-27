using SharpCommunication.Channels.Decorator;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Channels
{
    public class CachedChannelFactory<TPacket> : ChannelFactory<TPacket> where TPacket : IPacket
    {
        public override IChannel<TPacket> Create(Stream stream) =>
            new CachedChannel<TPacket>((Channel<TPacket>)base.Create(stream));

        public override IChannel<TPacket> Create(Stream inputStream, Stream outputStream) =>
            new CachedChannel<TPacket>((Channel<TPacket>)base.Create(inputStream, outputStream));

        public CachedChannelFactory(ICodec<TPacket> codec) : base(codec) { }
    }
}