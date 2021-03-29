using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpCommunication.Channels;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Transport.SerialPort
{
    public class SerialPortDataTransport<TPacket> : DataTransport<TPacket> where TPacket : IPacket
    {
        private System.IO.Ports.SerialPort _serialPort;

        public SerialPortDataTransport(IChannelFactory<TPacket> channelFactory, IOptionsMonitor<SerialPortDataTransportOption> option) : base(channelFactory, option)
        {

        }

        public SerialPortDataTransport(IChannelFactory<TPacket> channelFactory, IOptionsMonitor<SerialPortDataTransportOption> option, ILogger log) : base(channelFactory, option, log)
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
            var option = ((IOptionsMonitor<SerialPortDataTransportOption>)Option);
            var serialPortDataTransportOption = option.CurrentValue;
            _serialPort = new System.IO.Ports.SerialPort(serialPortDataTransportOption.PortName, serialPortDataTransportOption.BaudRate, serialPortDataTransportOption.Parity, serialPortDataTransportOption.DataBits, serialPortDataTransportOption.StopBits)
            {
                ReadTimeout = serialPortDataTransportOption.ReadTimeout
            };

            _serialPort.Open();
            InnerChannels.Add(ChannelFactory.Create(_serialPort.BaseStream));
        }
    }
}
