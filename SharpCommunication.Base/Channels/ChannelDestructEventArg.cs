using SharpCommunication.Base.Codec.Packets;
using System;

namespace SharpCommunication.Base.Channels
{
    public class ChannelDestructEventArg<T> : EventArgs where T: IPacket,new()
    {
        public ChannelDestructEventArg(IChannel<T> channel)
        {
            Channel = channel;
        }

        public IChannel<T> Channel
        {
            get;
        }
    }
}
