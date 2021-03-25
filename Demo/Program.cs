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

namespace Demo
{
    class Program
    {
        private static readonly List<string> _list = new List<string>();
        private static int _index;
        private static readonly object _o = new object();
        private static DeviceSerialDataTransport _dataTransport;

        private static void Main(string[] args)
        {
            
                var configurationRoot = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("AppConfig.Json").Build();
                var serial = new SerialPort("com2", 9600);
            serial.Open();
            var reader = new BinaryReader(serial.BaseStream);
            var writer = new BinaryWriter(serial.BaseStream);
            var option = new SerialPortDataTransportOption();
            configurationRoot.GetSection(nameof(SerialPortDataTransportOption)).Bind(option);
            
            _dataTransport = new DeviceSerialDataTransport((new OptionsWrapper<SerialPortDataTransportOption>(option)));
            _dataTransport.Open();
            var b = new byte[] { 0xaa, 0xaa, 0x00, 0x64, 0x04, 0x04 };
            _dataTransport.Channels[0].DataReceived += Channel_DataReceived;
            var packet = new Device() { Content = new Data() { Content = new Fault() { } } };
            while (true)
            {
                //writer.Write(b);
                if (serial.BytesToRead > 0)
                {
                    //Console.WriteLine(reader.ReadBytes(serial.BytesToRead).ToHexString());
                    writer.Write(reader.ReadBytes(serial.BytesToRead));
                    ;
                }
                lock(_o)
                    while (_list.Count > _index)
                        Console.WriteLine(_list[_index++]);

                (_dataTransport.Channels[0]).Transmit(packet);
                Thread.Sleep(100);
            }
        }

        private static void Channel_DataReceived(object sender, DataReceivedEventArg<Device> e)
        {
            lock (_o)
            {
                _list.Add(e.Data.ToString());
                _list.Add($" {_dataTransport.Channels[0].ToMonitoredChannel().GetDataReceivedCount}, {_dataTransport.Channels[0].ToMonitoredChannel().LastPacketTime}");
                _list.Add($" {_dataTransport.Channels[0].ToCachedChannel().Packet.Count}");

            }

        }

    }
}
