using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec;

namespace SharpCommunication.Base.Transport.Network
{
    public class DeviceTcpDataTransport : TcpDataTransport
    {
        public DeviceTcpDataTransport() : base(new ChannelFactory<DevicePacket>(new Codec<DevicePacket, DevicePacket.Encoding>()))
        {
            ListenPort = 4500;
            BackLog = 30;
        }
    }
}
