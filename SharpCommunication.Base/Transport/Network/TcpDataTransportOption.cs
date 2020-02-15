using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCommunication.Base.Transport.Network
{
    public class TcpDataTransportOption
    {
        public int ListenPort { get; protected set; }

        public int BackLog { get; protected set; }
    }
}
