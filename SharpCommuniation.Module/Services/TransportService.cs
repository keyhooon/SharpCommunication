using System;
using Microsoft.Extensions.DependencyInjection;
using SharpCommunication.Channels;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Transport;
using SharpCommunication.Transport.Network;
using SharpCommunication.Transport.SerialPort;

namespace SharpCommunication.Module.Services
{
    public abstract class DeviceService<T> where T : IPacket, new()
    {
        public event EventHandler IsOpenChanged;

        public bool IsOpen => DataTransport.IsOpen;
        public void Open() => DataTransport.Open();

        public void Close() => DataTransport.Close();
        protected DataTransport<T> DataTransport { get; }

        protected Codec<T> Codec { get; }

        protected DeviceService(DataTransport<T> dataTransport, Codec<T> codec)
        {
            DataTransport = dataTransport;
            Codec = codec;
            dataTransport.IsOpenChanged += IsOpenChanged;
        }
    }

    public static class TransportServiceExtensions
    {
        public static IServiceCollection AddSerialPortTransport<T>(
            this IServiceCollection serviceCollection, 
            Codec<T> codec,
            SerialPortDataTransportSettings settings) where T: IPacket, new()
        {
            var monitoredCachedChannelFactory = new MonitoredCachedChannelFactory<T>(codec);
            return serviceCollection
                .AddSingleton(codec)
                .AddSingleton(monitoredCachedChannelFactory)
                .AddSingleton(new SerialPortDataTransport<T>(monitoredCachedChannelFactory,
                    settings));
        }
        public static IServiceCollection AddNetworkTransport<T>(
            this IServiceCollection serviceCollection,
            EncodingDecorator encoding,
            TcpDataTransportSettings settings) where T : IPacket, new()
        {

            var codec = new Codec<T>(encoding);
            return serviceCollection
                .AddSingleton(codec)
                .AddSingleton(new TcpDataTransport<T>(new MonitoredCachedChannelFactory<T>(codec),
                    settings));
        }
    }
}
