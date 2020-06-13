using SharpCommunication.Channels.Decorator;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Channels
{
    public class MonitoredChannelFactory<TPacket> : IChannelFactory<TPacket> where TPacket : IPacket
    {
        public ICodec<TPacket> Codec { get; protected set; }

        public MonitoredChannelFactory(ICodec<TPacket> codec)
        {
            Codec = codec;
        }

        public virtual IChannel<TPacket> Create(Stream stream)
        {
            return Create(stream, stream);
        }

        public IChannel<TPacket> Create(Stream inputStream, Stream outputStream)
        {
            return new MonitoredChannel<TPacket>(new Channel<TPacket>(Codec, inputStream, outputStream));

        }
    }
}
