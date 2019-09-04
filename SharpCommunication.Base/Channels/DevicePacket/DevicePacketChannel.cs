using System;
using System.IO;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec;

namespace Connection.Channels.DevicePacket
{
    public partial class DevicePacketChannel : Channel<SharpCommunication.Base.Codec.DevicePacket>
    {
        public DevicePacketChannel(Stream stream, IDisposable streamingObject) : base(new Codec<SharpCommunication.Base.Codec.DevicePacket, SharpCommunication.Base.Codec.DevicePacket.Encoding>(), stream, streamingObject)
        {



        }
    }
}
