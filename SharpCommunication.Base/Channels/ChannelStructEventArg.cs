using SharpCommunication.Base.Codec.Packets;
using System;

namespace SharpCommunication.Base.Channels
{
    public class ChannelStructEventArg<T> : EventArgs where T: IPacket, new()
    {
        public ChannelStructEventArg(IChannel<T> channel)
        {
            Channel = channel;
        }

        public IChannel<T> Channel
        {
            get;
        }
    }
}
