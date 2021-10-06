using Prism.Events;
using SharpCommunication.Transport.SerialPort;

namespace GPSModule.Events
{
    public class GpsOptionChanged : PubSubEvent<SerialPortDataTransportOption> {
    }
}
