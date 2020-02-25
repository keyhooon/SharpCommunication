using SharpCommunication.Base.Codec.Packets;

namespace SharpCommunication.Base.Channels
{
    public class DataReceivedEventArg<T> where T : IPacket, new()
    {

        public new T Data { get; }
        public DataReceivedEventArg(T data)
        {
            Data = data;
        }
    }
}
