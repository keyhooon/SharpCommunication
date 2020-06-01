using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
    public class FunctionPacketEncoding<T> : AncestorPacketEncoding<T>, IFunctionPacketEncoding<T> where T: IFunctionPacket,new()
    {
        public byte InputByteCount{ get; }
        public FunctionPacketEncoding(EncodingDecorator encoding, byte inputByteCount, byte id) : base(encoding, id)
        {
            InputByteCount = inputByteCount;
        }
        public override IPacket DecodeCore(BinaryReader reader)
        {
            var functionPacket = new T() { Param = reader.ReadBytes(InputByteCount) };
            functionPacket?.Action?.Invoke();
            return functionPacket;
        }

        public override void EncodeCore(IPacket packet, BinaryWriter writer)
        {
            var functionPacket = (T) packet;
            writer.Write(functionPacket.Param);
        }

    }
    public interface IFunctionPacketEncoding<T> : IEncoding<T> where T : IFunctionPacket
    {
        byte InputByteCount { get; }
    }

}
