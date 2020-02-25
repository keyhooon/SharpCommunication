using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Base.Channels.Decorator
{
    public class ChannelDecorator<T> : IChannel<T> where T : IPacket, new()
    {
        protected Channel<T> innerChannel;

        public ChannelDecorator(ICodec<T> codec, Stream stream) : this(new Channel<T>(codec, stream))
        {

        }
        public ChannelDecorator(Channel<T> innerChannel)
        {
            this.innerChannel = innerChannel;
            innerChannel.DataReceived += DataReceived;
        }

        public ICodec<T> Codec => innerChannel.Codec;
        public BinaryReader Reader => innerChannel.Reader;
        public BinaryWriter Writer => innerChannel.Writer;
        public void Dispose() { innerChannel.Dispose(); }
        public event EventHandler<DataReceivedEventArg<T>> DataReceived;
    }
}
