using Demo.Codec;
using SharpCommunication.Channels;
using SharpCommunication.Channels.Decorator;
using System.IO;

namespace Demo.Channels
{
        public class DevicePacketChannelFactory : ChannelFactory<Device>
        {

            public DevicePacketChannelFactory() : base(new DevicePacketCodec())
            {

            }

            public override IChannel<Device> Create(Stream stream)
            {
                return ( new MonitoredChannel<Device>( new DevicePacketChannel(stream) ) );
            }

        }
}
