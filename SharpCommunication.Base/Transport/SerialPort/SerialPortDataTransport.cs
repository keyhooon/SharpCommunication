using SharpCommunication.Base.Channels;


namespace SharpCommunication.Base.Transport.SerialPort
{
    public class SerialPortDataTransport : DataTransport
    {
        private readonly SerialPortDataTransportOption _option;
        private System.IO.Ports.SerialPort _serialPort;

        public SerialPortDataTransport(ChannelFactory channelFactory, SerialPortDataTransportOption option) : base(channelFactory)
        {
            _option = option;
        }

        protected override bool IsOpenCore => _serialPort!.IsOpen;

        protected override void CloseCore()
        {
            _serialPort.Dispose();
        }

        protected override void OpenCore()
        {
            _serialPort = new System.IO.Ports.SerialPort(_option.PortName, _option.BaudRate, _option.Parity, _option.DataBits, _option.StopBits);
            _channels.Add(ChannelFactory.Create(_serialPort.BaseStream));
        }
    }
}
