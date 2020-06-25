using SharpCommunication.Codec;
using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Channels
{
    public class ChannelFactory<TPacket> : IChannelFactory<TPacket> where TPacket : IPacket
    {
        public ICodec<TPacket> Codec { get; protected set; }

        public ChannelFactory(ICodec<TPacket> codec)
        {
            Codec = codec;
        }

        public IChannel<TPacket> Create(Stream stream)
        {
            return Create(stream,stream);
        }
        public virtual IChannel<TPacket> Create(Stream inputStream, Stream outputStream)
        {
            return new Channel<TPacket>(Codec, inputStream, outputStream);
        }
    }
}
