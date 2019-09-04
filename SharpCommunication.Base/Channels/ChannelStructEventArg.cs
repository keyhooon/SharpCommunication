using System;

namespace SharpCommunication.Base.Channels
{
    public class ChannelStructEventArg : EventArgs
    {
        public ChannelStructEventArg(IChannel channel)
        {
            Channel = channel;
        }

        public IChannel Channel
        {
            get;
        }
    }
}
