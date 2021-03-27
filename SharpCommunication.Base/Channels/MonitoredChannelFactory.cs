using SharpCommunication.Channels.Decorator;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Channels
{
    public class MonitoredChannelFactory<TPacket> : ChannelFactory<TPacket> where TPacket : IPacket
    {
        public override IChannel<TPacket> Create(Stream stream) => 
            new MonitoredChannel<TPacket>((Channel<TPacket>)base.Create(stream));

        public override IChannel<TPacket> Create(Stream inputStream, Stream outputStream) => 
            new MonitoredChannel<TPacket>((Channel<TPacket>) base.Create(inputStream, outputStream));

        public MonitoredChannelFactory(ICodec<TPacket> codec) : base(codec) { }
    }
}
