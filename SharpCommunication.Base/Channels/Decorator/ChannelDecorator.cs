using SharpCommunication.Codec;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Channels.Decorator
{
    public abstract class ChannelDecorator<TPacket> : Channel<TPacket>, IDisposable where TPacket : IPacket
    {
         internal Channel< TPacket> innerChannel;

        public ChannelDecorator(Channel<TPacket> innerChannel) 
        {
            this.innerChannel = innerChannel;
            innerChannel.DataReceived += (sender, e) => { 
                OnDataReceived(e);
            };
        }
        public override ICodec<TPacket> Codec => innerChannel.Codec;
        public override BinaryReader Reader => innerChannel.Reader;
        public override BinaryWriter Writer => innerChannel.Writer;
        public override void Transmit(TPacket packet) => innerChannel.Transmit(packet);
        public override void Dispose() => innerChannel.Dispose();
    }
}
