using Demo.Codec;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec;
using System.IO;


namespace Demo.Channels
{
    public partial class DevicePacketChannel : Channel<DevicePacket>
    {
        public DevicePacketChannel(Stream stream) : base(new DevicePacketCodec(), stream)
        {



        }
    }
}
