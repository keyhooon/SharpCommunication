using System.Collections.Generic;
using System.Linq;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Encoding;

namespace Demo.Codec
{
    public class DevicePacketCodec : Codec<Device>
    {
        private readonly PacketEncodingBuilder _encodingBuilder;

        private EncodingDecorator _encoding;

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
                if (_encoding == null)
                    _encoding = _encodingBuilder.Build();
                return _encoding;
            }
        }

        public DevicePacketCodec(IEnumerable<PacketEncodingBuilder> PacketEncodingBuilderList)
        {
            var packetEncodingBuilderList = PacketEncodingBuilderList.ToList();
            _defaultCommandPacketEncodingBuilders.AddRange(packetEncodingBuilderList.Where(o => o.Build().GetType().BaseType?.GetGenericTypeDefinition() == typeof(FunctionPacketEncoding<>)));
            _defaultDataPacketEncodingBuilders.AddRange(packetEncodingBuilderList.Where(o => o.Build().GetType().BaseType?.GetGenericTypeDefinition() == typeof(AncestorPacketEncoding)));

            _encodingBuilder = Device.Encoding.CreateBuilder(new[] {
                Data.Encoding.CreateBuilder(_defaultDataPacketEncodingBuilders),
                Command.Encoding.CreateBuilder(_defaultCommandPacketEncodingBuilders)
            });
        }
        public DevicePacketCodec()
        {
            _encodingBuilder = Device.Encoding.CreateBuilder(new[] {
                Data.Encoding.CreateBuilder(_defaultDataPacketEncodingBuilders),
                Command.Encoding.CreateBuilder(_defaultCommandPacketEncodingBuilders)
            });
        }

    }
}
