using System.IO;

namespace SharpCommunication.Base.Codec.Commands
{
    public sealed class RecordAckCommand : Command
    {
        public const int CommandId = 100;

        /// <inheritdoc />
        public override int Id => CommandId;

        /// <inheritdoc />
        public override string ToString()
        {
            return "";
        }

        public bool Value { get; set; }
        public RecordAckCommand(bool value)
        {
            Value = value;
        }


        public sealed class Encoding : Encoding<RecordAckCommand>
        {
            private static Encoding<RecordAckCommand> _instance;

            public static Encoding<RecordAckCommand> Instance => _instance ?? (_instance = new Encoding());

            public override int CommandId => RecordAckCommand.CommandId;

            protected override RecordAckCommand DecodeCore(BinaryReader reader)
            {
                return new RecordAckCommand(reader.ReadByte() == 1);
            }

            protected override void EncodeCore(RecordAckCommand command, BinaryWriter writer)
            {
                writer.Write(command.Value);
            }
        }
    }

}
