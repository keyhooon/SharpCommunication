using System.IO;

namespace SharpCommunication.Base.Codec.Packets.Commands
{
    public sealed class CommandPacket : Packet
    {
        public const int CommandId = 2;

        /// <inheritdoc />
        public override int TypeId => CommandId;

        /// <inheritdoc />
        public override string ToString()
        {
            return "";
        }

        public bool Value { get; set; }
        public CommandPacket(bool value)
        {
            Value = value;
        }


        public sealed class Encoding : Encoding<CommandPacket>
        {
            private static Encoding<CommandPacket> _instance;

            public static Encoding<CommandPacket> Instance => _instance ??= new Encoding();

            public override int TypeId => CommandId;

            protected override CommandPacket DecodeCore(BinaryReader reader)
            {
                return new CommandPacket(reader.ReadByte() == 1);
            }

            protected override void EncodeCore(CommandPacket command, BinaryWriter writer)
            {
                writer.Write(command.Value);
            }
        }
    }

}
