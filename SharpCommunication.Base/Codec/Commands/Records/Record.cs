using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpCommunication.Base.Codec.Commands.Records
{

    public class Record
    {
        private ushort ByteLength { get; set; }

        public DateTime Time { get; set; }

        public IList<Field> Properties { get; set; }

        public class Encoding : IEncoding<Record>
        {
            private static IEncoding<Record> _instance;

            public static IEncoding<Record> Instance => _instance ?? (_instance = new Encoding());
            public void Encode(Record record, BinaryWriter writer)
            {
                if (record == null)
                    throw new ArgumentNullException(nameof(record));
                if (writer == null)
                    throw new ArgumentNullException(nameof(writer));
                if (!(record is Record record1))
                    throw new ArgumentException("The command type is not supported.");
                EncodeCore(record1, writer);
            }

            public Record Decode(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));
                var obj = DecodeCore(reader);
                if (obj == null)
                    throw new NotSupportedException("The encoding was unable to decode the command.");
                return obj;
            }

            private void EncodeCore(Record record, BinaryWriter writer)
            {
                writer.Write(record.ByteLength);
                writer.Write(record.Time.ToUnixEpoch());
                foreach (var property in record.Properties)
                {

                }
            }

            private Record DecodeCore(BinaryReader reader)
            {
                return new Record { ByteLength = reader.ReadUInt16(), Time = reader.ReadUInt32().ToUnixTime(), Properties = Enumerable.Range(0, reader.ReadByte()).Select(i => FieldEncoding.Instance.Decode(reader)).ToList() };
            }
        }

    }
}
