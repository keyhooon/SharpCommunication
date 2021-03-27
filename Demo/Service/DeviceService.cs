using Demo.Codec;
using Demo.Transport;
using SharpCommunication.Codec.Encoding;

namespace Demo.Service
{
    class DeviceService
    {

        private static readonly PacketEncodingBuilder[] _commandPacketEncodingBuilders = {
            CruiseCommand.Encoding.CreateBuilder((o)=>{}),
            LightCommand.Encoding.CreateBuilder((o,o2)=>{}),
            ReadCommand.Encoding.CreateBuilder((o)=>{})
        };

        private static readonly PacketEncodingBuilder[] _dataPacketEncodingBuilders = {
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
        };

        private static readonly PacketEncodingBuilder _devicePacketEncodingBuilders =
            Device.Encoding.CreateBuilder(new[] {
                Data.Encoding.CreateBuilder(_dataPacketEncodingBuilders),
                Command.Encoding.CreateBuilder(_commandPacketEncodingBuilders)});



        private DeviceSerialDataTransport _deviceSerialDataTransport;
        public DeviceService(DeviceSerialDataTransport deviceSerialDataTransport, EncodingDecorator deviceEncoding)
        {
            _deviceSerialDataTransport = deviceSerialDataTransport;
 //           deviceEncoding.GetChildEncoding<Device,Command>().GetChildEncoding<Command,CruiseCommand>().
        }
        
    }
}
