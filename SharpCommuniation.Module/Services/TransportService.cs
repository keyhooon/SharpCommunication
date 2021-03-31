﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prism.Mvvm;
using SharpCommunication.Channels;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Transport;
using SharpCommunication.Transport.Network;
using SharpCommunication.Transport.SerialPort;

namespace SharpCommunication.Module.Services
{
    public abstract class TransportService<T> : BindableBase where T : IPacket, new()
    {
        protected DataTransport<T> DataTransport { get; }
        protected Codec<T> Codec { get; }

        protected TransportService(DataTransport<T> dataTransport, Codec<T> codec)
        {
            DataTransport = dataTransport;
            Codec = codec;
        }
    }

    public static class TransportServiceExtensions
    {
        public static IServiceCollection AddSerialPortTransport<T>(
            this IServiceCollection serviceCollection, 
            EncodingDecorator encoding,
            IConfiguration namedConfigurationSection) where T: IPacket, new()
        {
            var dataTransportOption = new SerialPortDataTransportOption();
            namedConfigurationSection.Bind(dataTransportOption);
            var codec = new Codec<T>(encoding);
            return serviceCollection
                .AddSingleton(codec)
                .AddSingleton(new SerialPortDataTransport<T>(new MonitoredCachedChannelFactory<T>(codec),
                    dataTransportOption));
        }
        public static IServiceCollection AddNetworkTransport<T>(
            this IServiceCollection serviceCollection,
            EncodingDecorator encoding,
            IConfiguration namedConfigurationSection) where T : IPacket, new()
        {
            var dataTransportOption = new TcpDataTransportOption();
            namedConfigurationSection.Bind(dataTransportOption);
            var codec = new Codec<T>(encoding);
            return serviceCollection
                .AddSingleton(codec)
                .AddSingleton(new TcpDataTransport<T>(new MonitoredCachedChannelFactory<T>(codec),
                    dataTransportOption));
        }
    }
}
