namespace SharpCommunication.Transport.Network
{
    public class TcpDataTransportOption : DataTransportOption
    {


        private int _listenPort;
        public int ListenPort
        {
            get => _listenPort;
            set
            {
                if (_listenPort == value)
                    return;
                _listenPort = value;
                OnPropertyChanged();
            }
        }

        private int _backLog;
        public int BackLog
        {
            get => _backLog;
            set
            {
                if (_backLog == value)
                    return;
                _backLog = value;
                OnPropertyChanged();
            }
        }
    }
}
