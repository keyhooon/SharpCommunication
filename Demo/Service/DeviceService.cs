using Demo.Codec;
using SharpCommunication.Channels;
using SharpCommunication.Transport;
using System;
using System.Collections.Generic;

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
