using System;
using System.IO;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    public class LightCommand : IFunctionPacket
    {
        public delegate void CommandDelegate(byte lightId, bool isOn);

        public CommandDelegate Command { get; private set; }
        public override string ToString()
        {

            return $"Cruise Command {{ {Command} }}";
        }

        public class Encoding : FunctionPacketEncoding<LightCommand>
        {
            private readonly CommandDelegate _command;
            public Encoding(EncodingDecorator encoding, CommandDelegate command) : base(encoding, 3, typeof(CruiseCommand))
            {
                _command = command;
            }
            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                throw new NotSupportedException();
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var lightId = reader.ReadByte();
                var isOn = reader.ReadByte();
                var crc8 =(byte) (isOn + lightId);
                if (crc8 != reader.ReadByte()) 
                    return null;
                var lightCommand = new LightCommand()
                {
                    Command = _command,
                };
                lightCommand.Command.Invoke(lightId, isOn != 0);
                return lightCommand;
            }
            public static PacketEncodingBuilder CreateBuilder(CommandDelegate command) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o, command));
        }

    }
}
