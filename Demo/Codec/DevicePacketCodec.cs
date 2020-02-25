using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Codec
{
    class DevicePacketCodec : Codec<DevicePacket>
    {
        private readonly PacketEncodingBuilder EncodingBuilder;

        private IEncoding<DevicePacket> encoding;


        public override IEncoding<DevicePacket> Encoding
        {
            get
            {
                if (encoding == null)
                    encoding =(IEncoding<DevicePacket>) EncodingBuilder.Build();
                return encoding;
            }
        }

        public DevicePacketCodec()
        {
            EncodingBuilder = PacketEncodingBuilder.CreateDefaultBuilder().WithHeader(DevicePacket.Header).WithDescendant<DevicePacket>(new[] {
                PacketEncodingBuilder.CreateDefaultBuilder().WithDescendant<DataPacket>( new []{
                    PacketEncodingBuilder.CreateDefaultBuilder().WithFunction<ReadCommand>(1, ReadCommand.ID)
                }),
                PacketEncodingBuilder.CreateDefaultBuilder().WithDescendant<CommandPacket>()
            });

            DescendantPacketEncoding<DevicePacket>
        }
        void RegisterCommand<T>(AncestorPacketEncoding<IAncestorPacket> encoding) where T : IFunctionPacket, new()
        {
            CommandPacketEncoding.Register(encoding);
        }
        void RegisterData(AncestorPacketEncoding<IAncestorPacket> packet)
        {
            DataPacketEncoding.Register(packet);
        }
    }
}
