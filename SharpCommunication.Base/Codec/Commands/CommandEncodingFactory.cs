using System;
using System.Collections.Generic;

namespace SharpCommunication.Base.Codec.Commands
{
    public sealed class CommandEncodingFactory
    {
        private static CommandEncodingFactory _instance;

        public static CommandEncodingFactory Instance => _instance ?? (_instance = new CommandEncodingFactory());

        private readonly IDictionary<int, ICommandEncoding> _encodings = new Dictionary<int, ICommandEncoding>();

        private CommandEncodingFactory()
        {
            Register(new RecordCommand.Encoding());
            Register(new RecordAckCommand.Encoding());
        }

        public ICommandEncoding Create(int commandId)
        {
            if (!_encodings.TryGetValue(commandId & 0xf, out var commandEncoding) || commandEncoding == null)
                throw new NotSupportedException($"The command encoding for command 0x{commandId:X} is not supported.");
            return commandEncoding;
        }

        private void Register(ICommandEncoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));
            _encodings.Add(encoding.CommandId, encoding);
        }
    }
}
