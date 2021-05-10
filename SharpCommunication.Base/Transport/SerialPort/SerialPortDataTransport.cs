using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpCommunication.Channels;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Transport.SerialPort
{
    public class SerialPortDataTransport<TPacket> : DataTransport<TPacket> where TPacket : IPacket
    {
        private System.IO.Ports.SerialPort _serialPort;

        public SerialPortDataTransport(IChannelFactory<TPacket> channelFactory, SerialPortDataTransportSettings settings) : base(channelFactory, settings)
        {

        }


        public SerialPortDataTransport(IChannelFactory<TPacket> channelFactory, SerialPortDataTransportSettings settings, ILogger log) : base(channelFactory, settings, log)
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
            var settings = ((SerialPortDataTransportSettings)Settings);
            var serialPortDataTransportsettings = settings;
            _serialPort = new System.IO.Ports.SerialPort(serialPortDataTransportsettings.PortName, serialPortDataTransportsettings.BaudRate, serialPortDataTransportsettings.Parity, serialPortDataTransportsettings.DataBits, serialPortDataTransportsettings.StopBits)
            {
                ReadTimeout = serialPortDataTransportsettings.ReadTimeout
            };

            _serialPort.Open();
            InnerChannels.Add(ChannelFactory.Create(_serialPort.BaseStream));
        }
    }
}
