using Microsoft.Extensions.Logging;
using SharpCommunication.Channels;
using SharpCommunication.Codec;
using SharpCommunication.Transport.SerialPort;

namespace Demo.Transport
{
    public class GpsSerialDataTransport : SerialPortDataTransport<Gps>
    {

        public GpsSerialDataTransport(SerialPortDataTransportSettings option, ILogger logger) :
            base(new MonitoredCachedChannelFactory<Gps>(
                    new Codec<Gps>(Gps.Encoding.CreateBuilder().Build())),
                option,
                logger)
        { }
        public GpsSerialDataTransport(SerialPortDataTransportSettings option) :
            base(new MonitoredCachedChannelFactory<Gps>(
                    new Codec<Gps>(Gps.Encoding.CreateBuilder().Build())),
                option)
        { }

    }
}
