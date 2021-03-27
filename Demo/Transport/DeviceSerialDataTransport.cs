using Demo.Codec;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpCommunication.Channels;
using SharpCommunication.Channels.Decorator;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Transport.SerialPort;

namespace Demo.Transport
{
    public class DeviceSerialDataTransport : SerialPortDataTransport<Device>
    {

        public DeviceSerialDataTransport(IOptions<SerialPortDataTransportOption> option, EncodingDecorator deviceEncoding, ILogger logger) :
            base(new MonitoredCachedChannelFactory<Device>(
                    new Codec<Device>(deviceEncoding)),
                option,
                logger)
        { }
        public DeviceSerialDataTransport(IOptions<SerialPortDataTransportOption> option, EncodingDecorator deviceEncoding) :
            base(new MonitoredCachedChannelFactory<Device>(
                    new Codec<Device>(deviceEncoding)),
                option)
        { }

    }
}
