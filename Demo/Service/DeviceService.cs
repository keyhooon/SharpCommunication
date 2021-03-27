using Demo.Codec;
using SharpCommunication.Channels;
using SharpCommunication.Transport;
using System;
using System.Collections.Generic;
using Demo.Transport;
using Microsoft.Extensions.Options;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Transport.SerialPort;

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
