using Demo.Codec;
using SharpCommunication.Base.Codec.Packets;

namespace Demo.Service
{
    class DeviceService
    {
        private DevicePacketCodec devicePacketCodec;

        public DeviceService()
        {
            devicePacketCodec = new DevicePacketCodec();


        }
        public DescendantPacketEncoding<CommandPacket> CommandEncoding { get; private set; }

        public DescendantPacketEncoding<DataPacket> DataEncoding { get; private set; }

    }
}
