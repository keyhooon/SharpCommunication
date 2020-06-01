using SharpCommunication.Codec.Packets;
using System;

namespace SharpCommunication.Channels
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
