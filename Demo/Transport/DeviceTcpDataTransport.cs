using Demo.Codec;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Transport.Network;

namespace Demo.Transport
{
    public class DeviceTcpDataTransport : TcpDataTransport
    {
        public DeviceTcpDataTransport(TcpDataTransportOption option) : base(new ChannelFactory<DevicePacket>(new Codec<DevicePacket, DevicePacket.Encoding>()), option)
        {

        }
    }
}
