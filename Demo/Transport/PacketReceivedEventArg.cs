using System;
using System.Net.Sockets;
using Demo.Codec;

namespace Demo.Transport
{
    public class PacketReceivedEventArg : EventArgs
    {
        public PacketReceivedEventArg(Device packet, Socket socket)
        {
            Packet = packet;
            Socket = socket;
        }
        public Device Packet { get; }
        public Socket Socket { get; }

    }
}
