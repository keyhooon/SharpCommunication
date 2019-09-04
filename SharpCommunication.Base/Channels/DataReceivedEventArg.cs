using System;

namespace SharpCommunication.Base.Channels
{
    public class DataReceivedEventArg : EventArgs
    {
        public object Data { get; private set; }
        public DataReceivedEventArg(object data)
        {
            Data = data;
        }
    }
}
