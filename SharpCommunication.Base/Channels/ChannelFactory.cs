using System;
using System.IO;
using SharpCommunication.Base.Codec;

namespace SharpCommunication.Base.Channels
{
    public class ChannelFactory : IChannelFactory
    {
        public ICodec Codec { get; protected set; }
        public ChannelFactory(ICodec codec)
        {
            Codec = codec;
        }

        public ChannelFactory()
        {

        }

        public virtual Channel Create(Stream stream, IDisposable streamingObject)
        {
            return new Channel(Codec, stream, streamingObject);
        }
    }
}
