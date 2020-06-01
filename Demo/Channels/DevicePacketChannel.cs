using Demo.Codec;
using SharpCommunication.Channels;
using System.IO;


namespace Demo.Channels
{
    public partial class DevicePacketChannel : Channel<Device>
    {
        public DevicePacketChannel(Stream stream) : base(new DevicePacketCodec(), stream)
        {



        }
    }
}
