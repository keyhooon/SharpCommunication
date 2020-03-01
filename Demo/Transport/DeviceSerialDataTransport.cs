using Demo.Codec;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Transport.SerialPort;

namespace Demo.Transport
{
    public class DeviceSerialDataTransport : SerialPortDataTransport<DevicePacket>
    {
        public DeviceSerialDataTransport(SerialPortDataTransportOption option) : base(new ChannelFactory<DevicePacket>(new DevicePacketCodec()), option)
        {

        }
    }
}
