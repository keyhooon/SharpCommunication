namespace SharpCommunication.Transport.Network
{
    public class TcpDataTransportOption : DataTransportOption
    {

        public int ListenPort
        {
            get;
            set;
        }
        public int BackLog
        {
            get;
            set;
        }
    }
}
