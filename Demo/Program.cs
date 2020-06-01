using SharpCommunication.Transport.SerialPort;
using System;
using System.IO;
using System.IO.Ports;
using SharpCommunication.Channels;
using Demo.Codec;
using Demo.Service;
using System.Threading;

namespace Demo
{
    class Program
    {

        static void Main(string[] args)
        {
            SerialPort serial = new SerialPort("com254", 9600);
            serial.Open();
            var reader = new BinaryReader(serial.BaseStream);
            var writer = new BinaryWriter(serial.BaseStream);
            var option = new SerialPortDataTransportOption("com253", 9600);
            var deviceService = new DeviceService(option);
            deviceService.DataReceived += DeviceService_DataReceived;
            deviceService.Start();

            Device packet = new Device() { DescendantPacket = new Command() { DescendantPacket = new ReadCommand() { DataId = 1, Param = new byte[] { 3} } } };
            while (true)
            {
                if (serial.BytesToRead > 0)
                    //Console.WriteLine(reader.ReadBytes(serial.BytesToRead).ToHexString());
                    writer.Write(reader.ReadBytes(serial.BytesToRead));
                deviceService.devicePacketChannel.Transmit(packet);
                Thread.Sleep(200);
            }
        }

        private static void DeviceService_DataReceived(object sender, DataReceivedEventArg<Device> e)
        {
            Console.WriteLine(e.Data.ToString());
        }
    }
}
