using SharpCommunication.Base.Channels;
using System;
using System.IO;

namespace Connection.Channels.DevicePacket
{
    public partial class DevicePacketChannel
    {
        public class DevicePacketChannelFactory : ChannelFactory<SharpCommunication.Base.Codec.DevicePacket>
        {
            public override Channel Create(Stream stream, IDisposable streamingObject)
            {
                return new DevicePacketChannel(stream, streamingObject);
            }

        }
    }
}
