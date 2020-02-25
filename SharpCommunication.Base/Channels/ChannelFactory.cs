using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System.IO;

namespace SharpCommunication.Base.Channels
{
    public class ChannelFactory<T> : IChannelFactory<T> where T : IPacket, new()
    {
        public ICodec<T> Codec { get; protected set; }

        public ChannelFactory(ICodec<T> codec)
        {
            Codec = codec;
        }

        public virtual Channel<T> Create(Stream stream)
        {
            return new Channel<T>(Codec, stream);
        }
    }
}
