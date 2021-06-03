using System;
using System.IO;
using System.Linq;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    public class CruiseCommand : IFunctionPacket
    {
        public delegate void CommandDelegate(bool isOn);

        public CommandDelegate Command { get; private set; }
        public override string ToString()
        {

            return $"Cruise Command {{ {Command} }}";
        }

        public class Encoding : FunctionPacketEncoding<CruiseCommand>
        {
            private readonly CommandDelegate _command;
            public Encoding(EncodingDecorator encoding, CommandDelegate command) : base(encoding, 1, typeof(CruiseCommand))
            {
                _command = command;
            }
            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                throw new NotSupportedException();
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var isOn = reader.ReadByte();
                var crc8 = isOn;
                if (crc8 != reader.ReadByte()) 
                    return null;
                var packet = new CruiseCommand() {Command = _command};
                packet.Command.Invoke(isOn != 0);
                return packet;

            }
            public static PacketEncodingBuilder CreateBuilder(CommandDelegate command) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o, command));
        }
    }
}
