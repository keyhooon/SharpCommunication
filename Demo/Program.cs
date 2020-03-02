using Demo.Transport;
using SharpCommunication.Base.Transport.SerialPort;
using System;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Channels;
using System.Collections.ObjectModel;
using Demo.Codec;
using System.Collections.Specialized;
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

            DevicePacket packet = new DevicePacket() { DescendantPacket = new CommandPacket() { DescendantPacket = new ReadCommand() { DataId = 1, Param = new byte[] { 3} } } };
            while (true)
            {
                if (serial.BytesToRead > 0)
                    //Console.WriteLine(reader.ReadBytes(serial.BytesToRead).ToHexString());
                    writer.Write(reader.ReadBytes(serial.BytesToRead));
                deviceService.devicePacketChannel.Transmit(packet);
                Thread.Sleep(200);
            }
        }

        private static void DeviceService_DataReceived(object sender, DataReceivedEventArg<DevicePacket> e)
        {
            Console.WriteLine(e.Data.ToString());
        }
    }
}
