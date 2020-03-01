using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Base.Channels.Decorator
{
    public class ChannelDecorator<TPacket> : Channel<TPacket>, IDisposable where TPacket : IPacket, new()
    {
        protected Channel< TPacket> innerChannel;

        public ChannelDecorator(Channel<TPacket> innerChannel) : base(innerChannel)
        {
            this.innerChannel = innerChannel;
        }

        public ICodec<TPacket> Codec => innerChannel.Codec;
        public BinaryReader Reader => innerChannel.Reader;
        public BinaryWriter Writer => innerChannel.Writer;

        public void Dispose() { innerChannel.Dispose(); }
        public event EventHandler<DataReceivedEventArg<TPacket>> DataReceived;
    }
}
