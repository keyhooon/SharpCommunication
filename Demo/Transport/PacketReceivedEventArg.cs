using System;
using System.Net.Sockets;
using Demo.Codec;

namespace Demo.Transport
{
    public class PacketReceivedEventArg : EventArgs
    {
        public PacketReceivedEventArg(DevicePacket packet, Socket socket)
        {
            Packet = packet;
            Socket = socket;
        }
        public DevicePacket Packet { get; }
        public Socket Socket { get; }

    }
}
