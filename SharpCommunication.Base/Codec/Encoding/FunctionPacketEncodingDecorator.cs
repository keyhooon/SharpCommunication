using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
    public abstract class FunctionPacketEncoding<T> : AncestorPacketEncoding, IFunctionPacketEncoding where T : IFunctionPacket, new()
    {
        public override Type PacketType => typeof(T);
        public abstract byte ParameterByteCount { get; }

        public abstract Action<byte[]> ActionToDo { get; set; }
        public FunctionPacketEncoding(EncodingDecorator encoding) : base(encoding)
        {

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
