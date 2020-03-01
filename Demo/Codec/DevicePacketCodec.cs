using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System.Collections.Generic;

namespace Demo.Codec
{
    public class DevicePacketCodec : Codec<DevicePacket>
    {
        private readonly PacketEncodingBuilder EncodingBuilder;

        private PacketEncoding encoding;


        public override PacketEncoding Encoding
        {
            get
            {
                if (encoding == null)
                    encoding = EncodingBuilder.Build();
                return encoding;
            }
        }

        public DevicePacketCodec()
        {

            EncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder().WithHeader(DevicePacket.Header).WithDescendant<DevicePacket>(new[] {
                PacketEncodingBuilder.CreateDefaultBuilder().CreateDataPacket(),
                PacketEncodingBuilder.CreateDefaultBuilder().CreateCommandPacket( new []{
                    PacketEncodingBuilder.CreateDefaultBuilder().CreateReadCommand()
                })
            });


        }
        public void RegisterCommand(PacketEncoding enc) 
        {
            var commandEncoding = Encoding.FindDecoratedEncoding<DescendantPacketEncoding<DevicePacket>>().EncodingList[CommandPacket.ID].FindDecoratedEncoding<DescendantPacketEncoding<CommandPacket>>();
            commandEncoding.Register(enc);
        }
        public void RegisterCommand(IEnumerable<PacketEncoding> encs) 
        {
            var commandEncoding = Encoding.FindDecoratedEncoding<DescendantPacketEncoding<DevicePacket>>().EncodingList[CommandPacket.ID].FindDecoratedEncoding<DescendantPacketEncoding<CommandPacket>>();
            foreach (var enc in encs)
            {
                commandEncoding.Register(enc);
            }
        }

        public void RegisterData(PacketEncoding enc) 
        {
            var dataEncoding = Encoding.FindDecoratedEncoding<DescendantPacketEncoding<DevicePacket>>().EncodingList[DataPacket.ID];
            dataEncoding.FindDecoratedEncoding<DescendantPacketEncoding<DataPacket>>().Register(enc);
        }
        public void RegisterData(IEnumerable<PacketEncoding> encs)
        {
            var dataEncoding = Encoding.FindDecoratedEncoding<DescendantPacketEncoding<DevicePacket>>().EncodingList[DataPacket.ID]
                .FindDecoratedEncoding<DescendantPacketEncoding<DataPacket>>();
            foreach (var enc in encs)
            {
                dataEncoding.Register(enc);
            }
        }
    }
}
