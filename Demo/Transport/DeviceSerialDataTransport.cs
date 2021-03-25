using Demo.Channels;
using Demo.Codec;
using Microsoft.Extensions.Options;
using SharpCommunication.Transport.SerialPort;

namespace Demo.Transport
{
    public class DeviceSerialDataTransport : SerialPortDataTransport<Device>
    {
        public DeviceSerialDataTransport(IOptions<SerialPortDataTransportOption> option) : base(new DevicePacketChannelFactory(), option)
        {

        }
    }
}
