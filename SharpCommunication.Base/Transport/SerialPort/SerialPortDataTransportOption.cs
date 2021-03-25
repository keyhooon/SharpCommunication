using System.IO.Ports;

namespace SharpCommunication.Transport.SerialPort
{
    public class SerialPortDataTransportOption : DataTransportOption
    {
        public SerialPortDataTransportOption(  )
        {
            StopBits = StopBits.One;
            DataBits = 8;
            Parity= Parity.None;
            ReadTimeout = 1000;
        }

        public string PortName { get; set; }

        public int BaudRate { get; set; }

        public Parity Parity { get; set; }

        public StopBits StopBits { get; set; }

        public int DataBits { get; set; }

        public int ReadTimeout { get; set; }

    }
}
