using SharpCommunication.Base.Codec.Commands.Records;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpCommunication.Base.Codec.Commands
{
    public sealed class RecordCommand : Command
    {


        public const int CommandId = 1;
        public override int Id => CommandId;

        public IEnumerable<Records.Record> Records { get; set; }

        public RecordCommand(IEnumerable<Records.Record> records)
        {

            Records = records;
        }

        public override string ToString()
        {
            using (var stringWriter = new StringWriter())
            {
                stringWriter.Write("RECORD ");

                stringWriter.Write(", Records: ");
                stringWriter.Write(Records.Count());
                stringWriter.WriteLine("[");
                foreach (var record in Records)
                    stringWriter.WriteLine(record);
                stringWriter.Write("]");
                return stringWriter.ToString();
            }
        }

        public sealed class Encoding : Encoding<RecordCommand>
        {
            private static Encoding<RecordCommand> _instance;

            public static Encoding<RecordCommand> Instance => _instance ?? (_instance = new Encoding());
            public override int CommandId => RecordCommand.CommandId;

            protected override void EncodeCore(RecordCommand command, BinaryWriter writer)
            {
                var records = command.Records.ToArray();
                writer.Write((byte)records.Count());
                foreach (var record in records)
                    Record.Encoding.Instance.Encode(record, writer);
            }

            protected override RecordCommand DecodeCore(BinaryReader reader)
            {
                var recordCount = reader.ReadByte();

                return new RecordCommand(Enumerable.Range(0, recordCount).Select(i => Record.Encoding.Instance.Decode(reader)).ToList());
            }
        }
    }
}
