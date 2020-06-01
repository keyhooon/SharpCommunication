using SharpCommunication.Transport.SerialPort;
using System;
using System.IO;
using System.IO.Ports;
using SharpCommunication.Channels;
using Demo.Codec;
using Demo.Service;
using System.Threading;
using Demo.Transport;
using SharpCommunication.Transport;
using SharpCommunication.Codec.Packets;

namespace Demo
{
    class Program
    {

        static void Main(string[] args)
        {
            SerialPort serial = new SerialPort("com2", 9600);
            serial.Open();
            var reader = new BinaryReader(serial.BaseStream);
            var writer = new BinaryWriter(serial.BaseStream);
            var option = new SerialPortDataTransportOption("com3", 9600);
            var deviceService = new DeviceService();
            DeviceSerialDataTransport dataTransport = new DeviceSerialDataTransport(option);
            deviceService.RegisterDataTransport(dataTransport);
            deviceService.DataReceived += DeviceService_DataReceived;
            deviceService.Start();

            Device packet = new Device() { DescendantPacket = new Command() { DescendantPacket = new ReadCommand() { DataId = 1, Param = new byte[] { 3} } } };
            while (true)
            {
                if (serial.BytesToRead > 0)
                    //Console.WriteLine(reader.ReadBytes(serial.BytesToRead).ToHexString());
                    writer.Write(reader.ReadBytes(serial.BytesToRead));
                dataTransport.Channels[0].Transmit(packet);
                Thread.Sleep(200);
            }
        }

        private static void DeviceService_DataReceived(object sender, DataReceivedEventArg<Device> e)
        {
            Console.WriteLine(e.Data.ToString());
        }
    }
}
