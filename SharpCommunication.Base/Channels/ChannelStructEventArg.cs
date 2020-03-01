using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;

namespace SharpCommunication.Base.Channels
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
