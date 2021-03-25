using Demo.Codec;
using SharpCommunication.Channels;
using SharpCommunication.Transport;
using System;
using System.Collections.Generic;

namespace Demo.Service
{
    class DeviceService
    {
        private readonly List<DataTransport<Device>> _dataTransports;
        public event EventHandler<DataReceivedEventArg<Device>> DataReceived;
        public DeviceService()
        {
            _dataTransports = new List<DataTransport<Device>>();
        }

        public void Start()
        {
            foreach (var dataTransport in _dataTransports)
            {
                dataTransport.Open();
            }
        }
        public void Stop()
        {
            foreach (var dataTransport in _dataTransports)
            {
                dataTransport.Close();
            }
        }
        public void RegisterDataTransport(DataTransport<Device> dataTransport)
        {
            _dataTransports.Add(dataTransport);
        }
        
    }
}
