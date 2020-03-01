using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;

namespace SharpCommunication.Base.Channels
{
    public class ChannelDestructEventArg<TPacket> : EventArgs where TPacket : IPacket
    {
        public ChannelDestructEventArg(IChannel<TPacket> channel)
        {
            Channel = channel;
        }

        public IChannel<TPacket> Channel
        {
            get;
        }
    }
}
