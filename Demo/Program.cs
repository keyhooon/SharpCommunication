﻿using Demo.Transport;
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
            SerialPort serial = new SerialPort("com253", 115200);
            serial.Open();
            var writer = new BinaryWriter(serial.BaseStream);

            var option = new SerialPortDataTransportOption("Com254", 115200);
            var deviceService = new DeviceService(option);
            deviceService.DataReceived += DeviceService_DataReceived;
            deviceService.Start();


            while (true)
            {
                writer.Write(new byte[] { 0x55, 0x55, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 , 0x00, 0x00, 0x00, 0x00, 0x00 , 0x00});
                Thread.Sleep(200);
            }
        }

        private static void DeviceService_DataReceived(object sender, DataReceivedEventArg<DevicePacket> e)
        {
            Console.WriteLine(e.Data.ToString());
        }
    }
}