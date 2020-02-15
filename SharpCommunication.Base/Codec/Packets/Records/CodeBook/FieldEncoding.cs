using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SharpCommunication.Base.Codec.Packets.Records.CodeBook
{
    public class FieldEncoding : IEncoding<Field>
    {
        private static IEncoding<Field> _instance;

        public static IEncoding<Field> Instance => _instance ?? (_instance = new FieldEncoding());


        public PropertyCodeBook CodeBook { get; private set; }

        public FieldEncoding()
        {
            var codeBookWatcher = new FileSystemWatcher($"{Environment.CurrentDirectory}\\CodeBook", ".xml");
            var directory = new DirectoryInfo(codeBookWatcher.Path);
            var codebookFile = directory.EnumerateFiles("*.xml").FirstOrDefault();
            CodeBook = LoadCodebook(codebookFile);
        }

        private PropertyCodeBook LoadCodebook(FileInfo fileInfo)
        {
            PropertyCodeBook result;
            var serializer = new XmlSerializer(typeof(PropertyCodeBook));
            var reader = new StreamReader(fileInfo.FullName);
            result = (PropertyCodeBook)serializer.Deserialize(reader);
            reader.Close();
            return result;
        }

        public void Encode(Field property, BinaryWriter writer)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (property.Size != CodeBook.Properties[property.Id].Size)
                EncodeCore(property, writer);
        }

        public Field Decode(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            var obj = DecodeCore(reader);
            if (obj == null)
                throw new NotSupportedException("The encoding was unable to decode the command.");
            return obj;
        }

        private void EncodeCore(Field property, BinaryWriter writer)
        {
            writer.Write(property.Id);
            writer.Write(property.ToBuffer());

        }

        private Field DecodeCore(BinaryReader reader)
        {
            var propertyId = reader.ReadByte();
            Field property;
            switch (CodeBook.Properties[propertyId].Format)
            {
                case PropertyFormat.Number:
                    switch (CodeBook.Properties[propertyId].Size)
                    {
                        case 1:
                            property = Field.Create(propertyId, reader.ReadByte());
                            break;
                        case 2:
                            property = Field.Create(propertyId, reader.ReadUInt16());
                            break;
                        case 4:
                            property = Field.Create(propertyId, reader.ReadUInt32());
                            break;
                        case 8:
                            property = Field.Create(propertyId, reader.ReadUInt64());
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case PropertyFormat.Buffer:
                    property = Field.Create(propertyId, reader.ReadBytes(CodeBook.Properties[propertyId].Size));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return property;
        }

    }
}
