using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec;
using System.Collections.Generic;
using System.Linq;
using SharpCommunication.Codec.Packets;
using Communication.Codec;

namespace Demo.Codec
{
    public class DevicePacketCodec : Codec<Device>
    {
        private readonly PacketEncodingBuilder EncodingBuilder;

        private EncodingDecorator encoding;

        private readonly List<PacketEncodingBuilder> _defaultCommandPacketEncodingBuilders = new List<PacketEncodingBuilder>(
            new [] 
            {
                CruiseCommand.Encoding.CreateBuilder(),
                LightCommand.Encoding.CreateBuilder(),
                ReadCommand.Encoding.CreateBuilder()
            });

        private readonly List<PacketEncodingBuilder> _defaultDataPacketEncodingBuilders = new List<PacketEncodingBuilder>(
            new[]
            {
                BatteryConfiguration.Encoding.CreateBuilder(),
                BatteryOutput.Encoding.CreateBuilder(),
                CoreConfiguration.Encoding.CreateBuilder(),
                CoreSituation.Encoding.CreateBuilder(),
                Fault.Encoding.CreateBuilder(),
                LightSetting.Encoding.CreateBuilder(),
                LightState.Encoding.CreateBuilder(),
                PedalConfiguration.Encoding.CreateBuilder(),
                PedalSetting.Encoding.CreateBuilder(),
                ServoInput.Encoding.CreateBuilder(),
                ServoOutput.Encoding.CreateBuilder(),
                ThrottleConfiguration.Encoding.CreateBuilder()
            });

        public override EncodingDecorator Encoding
        {
            get
            {
                if (encoding == null)
                    encoding = EncodingBuilder.Build();
                return encoding;
            }
        }

        public DevicePacketCodec(IEnumerable<PacketEncodingBuilder> PacketEncodingBuilderList)
        {
            _defaultCommandPacketEncodingBuilders.AddRange(PacketEncodingBuilderList.Where(o => o.Build().GetType().BaseType.GetGenericTypeDefinition() == typeof(FunctionPacketEncoding<>)));
            _defaultDataPacketEncodingBuilders.AddRange(PacketEncodingBuilderList.Where(o => o.Build().GetType().BaseType.GetGenericTypeDefinition() == typeof(AncestorPacketEncoding<>)));

            EncodingBuilder = Device.Encoding.CreateBuilder(new[] {
                Data.Encoding.CreateBuilder(_defaultDataPacketEncodingBuilders),
                Command.Encoding.CreateBuilder(_defaultCommandPacketEncodingBuilders)
            });
        }
        public DevicePacketCodec()
        {
            EncodingBuilder = Device.Encoding.CreateBuilder(new[] {
                Data.Encoding.CreateBuilder(_defaultDataPacketEncodingBuilders),
                Command.Encoding.CreateBuilder(_defaultCommandPacketEncodingBuilders)
            });
        }

    }
}
