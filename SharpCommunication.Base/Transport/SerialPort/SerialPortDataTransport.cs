using Microsoft.Extensions.Logging;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;

namespace SharpCommunication.Base.Transport.SerialPort
{
    public class SerialPortDataTransport<TPacket> : DataTransport<TPacket> where TPacket : IPacket
    {
        private System.IO.Ports.SerialPort _serialPort;

        public SerialPortDataTransport(IChannelFactory<TPacket> channelFactory, SerialPortDataTransportOption option) : base(channelFactory, option)
        {

        }

        public SerialPortDataTransport(IChannelFactory<TPacket> channelFactory, SerialPortDataTransportOption option, ILogger log) : base(channelFactory, option, log)
        {

        }

        protected override bool IsOpenCore => _serialPort?.IsOpen?? false;

        protected override void CloseCore()
        {
            _serialPort.Close();
            _serialPort.Dispose();
        }

        protected override void OpenCore()
        {
            var option = ((SerialPortDataTransportOption)Option);
            _serialPort = new System.IO.Ports.SerialPort(option.PortName, option.BaudRate, option.Parity, option.DataBits, option.StopBits)
            {
                ReadTimeout = option.ReadTimeout
            };

            _serialPort.Open();
            _channels.Add(ChannelFactory.Create(_serialPort.BaseStream));
        }
    }
}
