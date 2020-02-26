using System;
using System.IO;

namespace SharpCommunication.Base.Codec.Packets
{
    public interface IFunctionPacket : IPacket, IAncestorPacket
    {
        public byte[] Param { get; set; }
        public Action Action { get; }
    }
    public class FunctionPacketEncoding<T> : AncestorPacketEncoding<T> where T : IFunctionPacket, new()
    {
        public byte InputByteCount{ get; }
        public FunctionPacketEncoding(IEncoding<T> encoding, byte inputByteCount, byte id) : base(encoding, id)
        {
            InputByteCount = inputByteCount;
        }
        public override T DecodeCore(BinaryReader reader)
        {
            var functionPacket = new T() { Param = reader.ReadBytes(InputByteCount) };
            functionPacket.Action.Invoke();
            return functionPacket;
        }

        public override void EncodeCore(T packet, BinaryWriter writer)
        {
            writer.Write(packet.Param);
        }
    }
    public static class HasFunctionPacketHelper
    {
        public static PacketEncodingBuilder WithFunction<T>(this PacketEncodingBuilder mapItemBuilder, byte inputByteCount,byte id) where T : IFunctionPacket, new()
        {
            mapItemBuilder.SetupActions.Add(item => (IEncoding<IPacket>)new FunctionPacketEncoding<T>((IEncoding<T>)item, inputByteCount, id));
            return mapItemBuilder;
        }

    }

}
