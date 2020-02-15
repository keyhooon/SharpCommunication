namespace SharpCommunication.Base.Channels
{
    public class DataReceivedEventArg<T> : DataReceivedEventArg
    {
        public new T Data => (T)base.Data;

        public DataReceivedEventArg(T data) : base(data)
        {

        }
    }
}
