using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace SharpCommunication.Base.Transport.SerialPort
{
    public class SerialPortDataTransportOption
    {
        public SerialPortDataTransportOption(string portName, int baudRate, Parity parity)
        {
            this.portName = portName;
            this.baudRate = baudRate;
            Parity = parity;
        }

        public string portName { get; set; }
        public int baudRate { get; set; }
        public Parity Parity { get; set; }
    }
}
