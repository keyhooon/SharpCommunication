using SharpCommunication.Channels.Decorator;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Channels
{
    public class MonitoredCachedChannelFactory<TPacket> : ChannelFactory<TPacket> where TPacket : IPacket
    {
        public override IChannel<TPacket> Create(Stream stream) =>
            new MonitoredChannel<TPacket>(new CachedChannel<TPacket>((Channel<TPacket>)base.Create(stream)));

        public override IChannel<TPacket> Create(Stream inputStream, Stream outputStream) =>
            new MonitoredChannel<TPacket>(new CachedChannel<TPacket>((Channel<TPacket>)base.Create(inputStream, outputStream)));

        public MonitoredCachedChannelFactory(ICodec<TPacket> codec) : base(codec) { }
    }
}