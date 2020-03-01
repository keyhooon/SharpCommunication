using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;

namespace SharpCommunication.Base.Transport.SerialPort
{
    public class SerialPortDataTransport<TPacket> : DataTransport<TPacket> where TPacket : IPacket
    {
        private readonly SerialPortDataTransportOption _option;
        private System.IO.Ports.SerialPort _serialPort;

        public SerialPortDataTransport(IChannelFactory<TPacket> channelFactory, SerialPortDataTransportOption option) : base(channelFactory)
        {
            _option = option;
        }

        protected override bool IsOpenCore => _serialPort?.IsOpen?? false;

        protected override void CloseCore()
        {

            _serialPort.Close();
            _serialPort.Dispose();
        }

        protected override void OpenCore()
        {
            _serialPort = new System.IO.Ports.SerialPort(_option.PortName, _option.BaudRate);
            _serialPort.ReadTimeout = 1000;

            _serialPort.Open();
            _channels.Add(ChannelFactory.Create(_serialPort.BaseStream));
        }
    }
}
