using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Codec;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpCommunication.Channels;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Transport.SerialPort;

namespace Demo.Transport
{
    public class GpsSerialDataTransport : SerialPortDataTransport<Gps>
    {

        public GpsSerialDataTransport(SerialPortDataTransportSettings option, EncodingDecorator deviceEncoding, ILogger logger) :
            base(new MonitoredCachedChannelFactory<Gps>(
                    new Codec<Gps>(deviceEncoding)),
                option,
                logger)
        { }
        public GpsSerialDataTransport(SerialPortDataTransportSettings option, EncodingDecorator deviceEncoding) :
            base(new MonitoredCachedChannelFactory<Gps>(
                    new Codec<Gps>(deviceEncoding)),
                option)
        { }

    }
}
