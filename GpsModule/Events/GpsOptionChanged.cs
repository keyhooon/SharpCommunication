using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using SharpCommunication.Transport.SerialPort;

namespace GPSModule.Events
{
    public class GpsOptionChanged : PubSubEvent<SerialPortDataTransportOption> {
    }
}
