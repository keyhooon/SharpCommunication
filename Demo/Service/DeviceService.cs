using Demo.Codec;
using Demo.Transport;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec.Packets;
using SharpCommunication.Base.Transport.SerialPort;
using System;
using System.Collections.Specialized;

namespace Demo.Service
{
    class DeviceService
    {
        private DeviceSerialDataTransport DataTransport;
        public event EventHandler<DataReceivedEventArg<DevicePacket>> DataReceived;
        public DeviceService(SerialPortDataTransportOption option)
        {
            DataTransport = new DeviceSerialDataTransport(option);
            var devicePacketCodec = (DevicePacketCodec)DataTransport.ChannelFactory.Codec;
            devicePacketCodec.RegisterData(new [] {
                PacketEncodingBuilder.CreateDefaultBuilder().CreateBatteryConfigurationEncodingBuilder().Build(),
                PacketEncodingBuilder.CreateDefaultBuilder().CreateBatteryOutputEncodingBuilder().Build(),
                PacketEncodingBuilder.CreateDefaultBuilder().CreateCoreConfigurationEncodingBuilder().Build(),
                PacketEncodingBuilder.CreateDefaultBuilder().CreateCoreSituationEncodingBuilder().Build()
            });
            devicePacketCodec.RegisterCommand(new[] {
                PacketEncodingBuilder.CreateDefaultBuilder().CreateLightCommand().Build(),
                PacketEncodingBuilder.CreateDefaultBuilder().CreateCruiseCommand().Build()
            });
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

        private void Channels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                DataTransport.Channels[0].DataReceived += DataReceived;
            }
        }
    }
}
