using System;

namespace SharpCommunication.Base.Channels
{
    public class ChannelDestructEventArg : EventArgs
    {
        public ChannelDestructEventArg(IChannel channel)
        {
            Channel = channel;
        }

        public IChannel Channel
        {
            get;
        }
    }
}
