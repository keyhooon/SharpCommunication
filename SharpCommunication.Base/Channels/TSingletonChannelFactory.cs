using System;
using System.IO;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec;

namespace SharpCommunication.Base.Channels
{
    class TSingletonChannelFactory<T> : ChannelFactory where T : IPacket
    {
        public override Channel Create(Stream stream, IDisposable streamingObject)
        {
            return new Channel<T>(Codec, stream, streamingObject);
        }

        public TSingletonChannelFactory(ICodec codec) : base(codec) { }
        public TSingletonChannelFactory() : base() { }
    }
}
