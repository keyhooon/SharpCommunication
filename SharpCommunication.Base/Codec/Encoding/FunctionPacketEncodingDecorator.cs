using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
    public class FunctionPacketEncoding<T> : EncodingDecorator, IFunctionPacketEncoding where T : IFunctionPacket, new()
    {
        public byte ParameterByteCount { get; }

        public Action<byte[]> ActionToDo { get; set; }
        public FunctionPacketEncoding(EncodingDecorator encoding, Action<byte[]> actionToDo, byte parameterByteCount) : base(encoding)
        {
            ParameterByteCount = parameterByteCount;
            ActionToDo = actionToDo;
        }

        public override IPacket Decode(BinaryReader reader)
        {
            var ret = new T() { Param = reader.ReadBytes(ParameterByteCount) };
            ActionToDo?.Invoke(ret.Param);
            return ret;
        }

        public override void Encode(IPacket packet, BinaryWriter writer)
        {
            var command = (T)packet;
            writer.Write(command.Param);
        }

    }
    public interface IFunctionPacketEncoding: IEncoding<IPacket>
    {
        byte ParameterByteCount { get; }
    }

}
