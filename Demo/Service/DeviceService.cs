using Demo.Codec;
using Demo.Transport;
using SharpCommunication.Channels;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Transport;
using SharpCommunication.Transport.SerialPort;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Demo.Service
{
    class DeviceService
    {
        private List<DataTransport<Device>> DataTransports;
        public event EventHandler<DataReceivedEventArg<Device>> DataReceived;
        public DeviceService()
        {
            DataTransports = new List<DataTransport<Device>>();
        }

        public void Start()
        {
            foreach (var dataTransport in DataTransports)
            {
                dataTransport.Open();
            }
        }
        public void Stop()
        {
            foreach (var dataTransport in DataTransports)
            {
                dataTransport.Close();
            }
        }
        public void RegisterDataTransport(DataTransport<Device> dataTransport)
        {
            DataTransports.Add(dataTransport);
        }
        
    }
}
