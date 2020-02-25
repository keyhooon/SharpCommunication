using Demo.Codec;
using SharpCommunication.Base.Channels;
using System.IO;

namespace Demo.Channels
{
    public partial class DevicePacketChannel
    {
        public class DevicePacketChannelFactory : ChannelFactory<DevicePacket>
        {
            public override Channel Create(Stream stream)
            {
                return new DevicePacketChannel(stream);
            }

        }
    }
}
