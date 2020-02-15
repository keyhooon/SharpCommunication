using System;
using System.IO;
using SharpCommunication.Base.Codec;

namespace SharpCommunication.Base.Channels
{
    public class ChannelFactory<T> : ChannelFactory where T : IPacket
    {
        public override Channel Create(Stream stream, IDisposable streamingObject)
        {
            return new Channel<T>(Codec, stream, streamingObject);
        }

        public ChannelFactory(ICodec codec) : base(codec) { }
        public ChannelFactory() : base() { }
    }
}
