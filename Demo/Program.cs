using SharpCommunication.Transport.SerialPort;
using System;
using System.IO;
using System.IO.Ports;
using SharpCommunication.Channels;
using Demo.Codec;
using Demo.Service;
using System.Threading;
using Demo.Transport;
using SharpCommunication.Codec;
using System.Collections.Generic;
using SharpCommunication.Channels.Decorator;
using System.Linq;

namespace Demo
{
    class Program
    {
        static List<string> list = new List<string>();
        static int index = 0;
        static object o = new object();
        static DeviceSerialDataTransport dataTransport;
        static void Main(string[] args)
        {
            SerialPort serial = new SerialPort("com2", 9600);
            serial.Open();
            var reader = new BinaryReader(serial.BaseStream);
            var writer = new BinaryWriter(serial.BaseStream);
            var option = new SerialPortDataTransportOption("com4", 9600);
            dataTransport = new DeviceSerialDataTransport(option);
            dataTransport.Open();
            byte[] b = new byte[] { 0xaa, 0xaa, 0x00, 0x64, 0x04, 0x04 };
            dataTransport.Channels[0].DataReceived += Channel_DataReceived;
            Device packet = new Device() { DescendantPacket = new Data() { DescendantPacket = new Fault() { } } };
            while (true)
            {
                //writer.Write(b);
                if (serial.BytesToRead > 0)
                {
                    //Console.WriteLine(reader.ReadBytes(serial.BytesToRead).ToHexString());
                    writer.Write(reader.ReadBytes(serial.BytesToRead));
                    ;
                }
                lock(o)
                    while (list.Count > index)
                        Console.WriteLine(list[index++]);

                (dataTransport.Channels[0]).Transmit(packet);
                Thread.Sleep(100);
            }
        }

        private static void Channel_DataReceived(object sender, DataReceivedEventArg<Device> e)
        {
            lock (o)
            {
                list.Add(e.Data.ToString());
                list.Add($" {dataTransport.Channels[0].ToMonitoredChannel().MonitorBeginTime}, {dataTransport.Channels[0].ToMonitoredChannel().GetDataReceivedCount}");
                list.Add($" {dataTransport.Channels[0].ToCachedChannel().Packet.Count}");

            }

        }

    }
}
