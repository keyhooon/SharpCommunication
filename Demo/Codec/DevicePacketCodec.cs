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
                    PacketEncodingBuilder.CreateDefaultBuilder().WithFunction<ReadCommand>(ReadCommand.ParamByteCount, ReadCommand.ID)
                })
            });


        }
        public void RegisterCommand<T>(byte inputByteCount, byte id) where T : IFunctionPacket, new()
        {
            var commandEncoding = (IEncoding<CommandPacket>) Encoding.FindDecoratedProperty<DescendantPacketEncoding<DevicePacket>, DevicePacket>().EncodingList[CommandPacket.ID];
            commandEncoding.FindDecoratedProperty<DescendantPacketEncoding<CommandPacket>, CommandPacket>().Register(
               PacketEncodingBuilder.CreateDefaultBuilder().WithFunction<T>(inputByteCount, id).Build());
        }
        public void RegisterData<T>(AncestorPacketEncoding<IAncestorPacket> encoding) where T: IAncestorPacket, new()
        {
            var commandEncoding = (IEncoding<DataPacket>)Encoding.FindDecoratedProperty<DescendantPacketEncoding<DevicePacket>, DevicePacket>().EncodingList[DataPacket.ID];
            commandEncoding.FindDecoratedProperty<DescendantPacketEncoding<DataPacket>, DataPacket>().Register(encoding);
        }
    }
}
