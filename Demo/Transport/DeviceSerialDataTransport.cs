using Demo.Codec;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpCommunication.Channels;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Transport.SerialPort;

namespace Demo.Transport
{
    public class DeviceSerialDataTransport : SerialPortDataTransport<Device>
    {

        public DeviceSerialDataTransport(SerialPortDataTransportSettings settings, EncodingDecorator deviceEncoding, ILogger logger) :
            base(new MonitoredCachedChannelFactory<Device>(
                    new Codec<Device>(deviceEncoding)),
                settings,
                logger)
        { }
        public DeviceSerialDataTransport(SerialPortDataTransportSettings settings, EncodingDecorator deviceEncoding) :
            base(new MonitoredCachedChannelFactory<Device>(
                    new Codec<Device>(deviceEncoding)),
                settings)
        { }

    }
}
