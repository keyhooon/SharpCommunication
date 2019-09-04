using System;
using System.IO;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec;

namespace SharpCommunication.Base.Channels.Decorator
{
    public class ChannelDecorator : IChannel
    {
        protected Channel innerChannel;

        public ChannelDecorator(ICodec codec, Stream stream, IDisposable streamingObject) : this(new Channel(codec, stream, streamingObject))
        {

        }
        public ChannelDecorator(Channel innerChannel)
        {
            this.innerChannel = innerChannel;
            innerChannel.DataReceived += DataReceived;
        }

        public ICodec Codec => innerChannel.Codec;
        public BinaryReader Reader => innerChannel.Reader;
        public BinaryWriter Writer => innerChannel.Writer;
        public void Dispose() { innerChannel.Dispose(); }
        public event EventHandler<DataReceivedEventArg> DataReceived;
    }
}
