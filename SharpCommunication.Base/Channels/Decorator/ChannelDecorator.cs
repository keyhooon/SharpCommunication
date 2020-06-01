using SharpCommunication.Codec;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Channels.Decorator
{
    public class ChannelDecorator<TPacket> : Channel<TPacket>, IDisposable where TPacket : IPacket
    {
        protected Channel< TPacket> innerChannel;

        public ChannelDecorator(Channel<TPacket> innerChannel) : base(innerChannel)
        {
            this.innerChannel = innerChannel;
        }

        public new ICodec<TPacket> Codec => innerChannel.Codec;
        public new BinaryReader Reader => innerChannel.Reader;
        public new BinaryWriter Writer => innerChannel.Writer;

        public override void Dispose() 
        { 
            base.Dispose(); 
            innerChannel.Dispose(); 
        }

    }
}
