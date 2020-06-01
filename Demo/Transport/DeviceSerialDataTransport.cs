using Demo.Channels;
using Demo.Codec;
using SharpCommunication.Transport.SerialPort;

namespace Demo.Transport
{
    public class DeviceSerialDataTransport : SerialPortDataTransport<Device>
    {
        public DeviceSerialDataTransport(SerialPortDataTransportOption option) : base(new DevicePacketChannelFactory(), option)
        {

        }
    }
}
