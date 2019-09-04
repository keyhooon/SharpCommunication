using System;
using System.Net.Sockets;
using SharpCommunication.Base.Codec;

namespace SharpCommunication.Base.Transport.Network
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
