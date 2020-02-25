using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;


namespace Demo.Codec
{
    class DevicePacketCodec : Codec<DevicePacket>
    {
        private readonly PacketEncodingBuilder EncodingBuilder;

        private PacketEncoding<DevicePacket> encoding;


        public override PacketEncoding<DevicePacket> Encoding
        {
            get
            {
                if (encoding == null)
                    encoding =(PacketEncoding<DevicePacket>) EncodingBuilder.Build();
                return encoding;
            }
        }

        public DevicePacketCodec()
        {
            EncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder().WithHeader(DevicePacket.Header).WithDescendant<DevicePacket>(new[] {
                PacketEncodingBuilder.CreateDefaultBuilder().WithDescendant<DataPacket>(),
                PacketEncodingBuilder.CreateDefaultBuilder().WithDescendant<CommandPacket>( new []{
                    PacketEncodingBuilder.CreateDefaultBuilder().WithFunction<ReadCommand>(1, ReadCommand.ID)
                })
            });


        }
        void RegisterCommand<T>(AncestorPacketEncoding<IAncestorPacket> encoding) where T : IFunctionPacket, new()
        {
            var commandEncoding = Encoding.FindDecoratedProperty<DescendantPacketEncoding<DevicePacket>, DevicePacket>().EncodingList[ReadCommand.ID];
            commandEncoding.FindDecoratedProperty<DescendantPacketEncoding<CommandPacket>, CommandPacket>().Register(encoding)
        }
        void RegisterData(AncestorPacketEncoding<IAncestorPacket> packet)
        {
            DataPacketEncoding.Register(packet);
        }
    }
}
