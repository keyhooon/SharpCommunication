using Demo.Codec;
using SharpCommunication.Base.Channels;
using System.IO;

namespace Demo.Channels
{
    public partial class DevicePacketChannel
    {
        public class DevicePacketChannelFactory : ChannelFactory<DevicePacket>
        {

            public DevicePacketChannelFactory() : base(new DevicePacketCodec())
            {

            }

            public override IChannel<DevicePacket> Create(Stream stream)
            {
                return new DevicePacketChannel(stream);
            }

        }
    }
}
