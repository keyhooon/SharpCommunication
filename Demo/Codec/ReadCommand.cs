using System;
using System.IO;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    public class ReadCommand : IFunctionPacket
    {
        public delegate void CommandDelegate(byte dataId);

        public CommandDelegate Command { get; private set; }
       
        public override string ToString()
        {

            return $"Read Command {{ {Command} }}";
        }

        public class Encoding : FunctionPacketEncoding<ReadCommand>
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
                var dataId = reader.ReadByte();
                var crc8 = dataId;
                if (crc8 != reader.ReadByte()) 
                    return null;
                var readCommand = new ReadCommand()
                {
                    Command = _command,
                };
                readCommand.Command(dataId);
                return readCommand;
            }
            public static PacketEncodingBuilder CreateBuilder(CommandDelegate command) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o,command));
        }

    }
}
