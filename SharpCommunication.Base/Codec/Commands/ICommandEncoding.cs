using SharpCommunication.Base.Codec;

namespace SharpCommunication.Base.Codec.Commands
{
    public interface ICommandEncoding : IEncoding<Command>
    {
        int CommandId { get; }
    }
}
