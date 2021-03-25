using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
    public class FunctionPacketEncoding<T> : AncestorPacketEncoding, IFunctionPacketEncoding where T : IFunctionPacket, new()
    {
        public FunctionPacketEncoding(EncodingDecorator encoding, byte id, Type packetType) : base(encoding, id, packetType)
        {
      
        }

        public override void Encode(IPacket packet, BinaryWriter writer)
        {
            throw new NotSupportedException();
        }

        public override IPacket Decode(BinaryReader reader)
        {
            var packet = Encoding?.Decode(reader);
            return packet;
        }

    }
    public interface IFunctionPacketEncoding: IEncoding<IPacket>
    {
    }

}
