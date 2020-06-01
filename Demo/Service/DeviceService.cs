using Demo.Codec;
using Demo.Transport;
using SharpCommunication.Channels;
using SharpCommunication.Transport.SerialPort;
using System;
using System.Collections.Specialized;

namespace Demo.Service
{
    class DeviceService
    {
        private DeviceSerialDataTransport DataTransport;
        public event EventHandler<DataReceivedEventArg<Device>> DataReceived;
        public DeviceService(SerialPortDataTransportOption option)
        {
            DataTransport = new DeviceSerialDataTransport(option);

            ((INotifyCollectionChanged)DataTransport.Channels).CollectionChanged += Channels_CollectionChanged;
        }
        public void Start()
        {
            DataTransport.Open();
        }
        public void Stop()
        {
            DataTransport.Close();
        }
        public Channel<Device> devicePacketChannel => (Channel<Device>)DataTransport.Channels[0];

        private void Channels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                DataTransport.Channels[0].DataReceived += DataReceived;
            }
        }
    }
}
