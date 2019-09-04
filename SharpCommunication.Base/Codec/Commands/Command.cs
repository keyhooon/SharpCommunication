
using System;
using System.IO;

namespace SharpCommunication.Base.Codec.Commands
{
    public abstract class Command
    {
        public abstract int Id { get; }



        public abstract override string ToString();

        public abstract class Encoding<T> : ICommandEncoding where T : Command
        {
            public abstract int CommandId { get; }

            public void Encode(Command command, BinaryWriter writer)
            {
                if (command == null)
                    throw new ArgumentNullException(nameof(command));
                if (writer == null)
                    throw new ArgumentNullException(nameof(writer));
                if (!(command is T command1))
                    throw new ArgumentException("The command type is not supported.");
                EncodeCore(command1, writer);
            }

            public Command Decode(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));
                var obj = DecodeCore(reader);
                if (obj == null)
                    throw new NotSupportedException("The encoding was unable to decode the command.");
                return obj;
            }

            protected abstract void EncodeCore(T command, BinaryWriter writer);

            protected abstract T DecodeCore(BinaryReader reader);
        }
    }

}
