using SharpCommunication.Codec.Packets;
using System;

namespace SharpCommunication.Channels
{
    public class ChannelStructEventArg<TPacket> : EventArgs where TPacket : IPacket
    {
        public ChannelStructEventArg(IChannel<TPacket> channel)
        {
            Channel = channel;
        }

        public IChannel<TPacket> Channel
        {
            get;
        }
    }
}
