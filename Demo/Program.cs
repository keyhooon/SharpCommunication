using SharpCommunication.Transport.SerialPort;
using System;
using System.IO;
using System.IO.Ports;
using SharpCommunication.Channels;
using Demo.Codec;
using System.Threading;
using Demo.Transport;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SharpCommunication.Channels.Decorator;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Encoding;

namespace Demo
{
    class Program
    {
        private static readonly List<string> _list = new List<string>();
        private static int _index;
        private static readonly object _o = new object();
        // private static DeviceSerialDataTransport _dataTransport;
        private static GpsSerialDataTransport _gpsDataTransport;

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
        private static void Main(string[] args)
        {
            // var serial = new SerialPort("com2", 115200);
            // serial.Open();
            // var reader = new BinaryReader(serial.BaseStream);
            // var writer = new BinaryWriter(serial.BaseStream);
            // var b = new byte[] { 0xaa, 0xaa, 0x00, 0x64, 0x04, 0x04 };

            var gpsSerial = new SerialPort("com3", 115200);
            gpsSerial.Open();
            var gpsReader = new BinaryReader(gpsSerial.BaseStream);
            var gpsWriter = new BinaryWriter(gpsSerial.BaseStream);
            var gpsTestData = "$GNGNS,014035.00,4332.69262,S,17235.48549,E,RR,13,0.9,25.63,11.24,,*70\r\n";

            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppConfig.Json").Build();
            // var option = new SerialPortDataTransportOption();
            // configurationRoot.GetSection(nameof(DeviceSerialDataTransport)).Bind(option);
            var gpsOption = new SerialPortDataTransportOption();
            configurationRoot.GetSection(nameof(GpsSerialDataTransport)).Bind(gpsOption);

            // var encoding = _devicePacketEncodingBuilders.Build();
            // _dataTransport = new DeviceSerialDataTransport(
            //     new OptionsWrapper<SerialPortDataTransportOption>(option),
            //     encoding);
            // _dataTransport.Open();

            _gpsDataTransport = new GpsSerialDataTransport(new OptionsWrapper<SerialPortDataTransportOption>(gpsOption), Gps.Encoding.CreateBuilder().Build());
            _gpsDataTransport.Open();
            _gpsDataTransport.Channels[0].DataReceived += (sender, arg) =>
            {
                lock (_o)
                {
                    _list.Add(
                        $" {_gpsDataTransport.Channels[0].ToMonitoredChannel().GetDataReceivedCount}, {_gpsDataTransport.Channels[0].ToMonitoredChannel().LastPacketTime}");
                    _list.Add($" {_gpsDataTransport.Channels[0].ToCachedChannel().Packet.Count}");
                    _list.Add(arg.Data.ToString());
                    _list.Add("\r\n");
                }
            };
            // _dataTransport.Channels[0].DataReceived += Channel_DataReceived;
            // var packet = new Device() { Content = new Data() { Content = new Fault { } } };
            while (true)
            {
                //writer.Write(b);
                // if (serial.BytesToRead > 0)
                // {
                    //Console.WriteLine(reader.ReadBytes(serial.BytesToRead).ToHexString());
                    //writer.Write(reader.ReadBytes(serial.BytesToRead));
                    gpsWriter.Write(gpsTestData); ;
                // }
                lock(_o)
                    while (_list.Count > _index)
                        Console.WriteLine(_list[_index++]);

                // (_dataTransport.Channels[0]).Transmit(packet);
                Thread.Sleep(1000);
            }
        }

        // private static void Channel_DataReceived(object sender, DataReceivedEventArg<Device> e)
        // {
        //     
        //     lock (_o)
        //     {
        //         _list.Add($" {_dataTransport.Channels[0].ToMonitoredChannel().GetDataReceivedCount}, {_dataTransport.Channels[0].ToMonitoredChannel().LastPacketTime}");
        //         _list.Add($" {_dataTransport.Channels[0].ToCachedChannel().Packet.Count}");
        //         _list.Add(e.Data.ToString());
        //         _list.Add("\r\n");
        //     }
        //
        // }

    }
}
