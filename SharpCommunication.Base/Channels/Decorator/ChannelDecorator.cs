using SharpCommunication.Codec;
using SharpCommunication.Codec.Packets;
using System;
using System.ComponentModel;
using System.IO;

namespace SharpCommunication.Channels.Decorator
{
    public abstract class ChannelDecorator<TPacket> : Channel<TPacket>, IDisposable where TPacket : IPacket
    {
        internal Channel< TPacket> InnerChannel;

        protected ChannelDecorator(Channel<TPacket> innerChannel) 
        {
             InnerChannel = innerChannel;
             innerChannel.DataReceived += (sender, e) => OnDataReceived(e);
             innerChannel.ErrorReceived += (sender, e) => OnErrorReceived(e);
        }
        public override ICodec<TPacket> Codec => InnerChannel.Codec;
        public override BinaryReader Reader => InnerChannel.Reader;
        public override BinaryWriter Writer => InnerChannel.Writer;
        public override void Transmit(TPacket packet) => InnerChannel.Transmit(packet);
        public override void Dispose() => InnerChannel.Dispose();

    }
}
