using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Channels
{
    public class DataReceivedEventArg<T> where T : IPacket
    {

        public T Data { get; }
        public DataReceivedEventArg(T data)
        {
            Data = data;
        }
    }
}
