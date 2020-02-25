using System.IO;

namespace SharpCommunication.Base.Codec.Packets
{
    public interface IPacket
    {
        string ToString();        
    }

    public abstract class PacketEncoding<T> : IEncoding<T> where T : IPacket
    {
        public IEncoding<IPacket> Encoding { get; }
        public PacketEncoding(IEncoding<IPacket> encoding)
        {
            Encoding = encoding;
        }

        public abstract void EncodeCore(T packet, BinaryWriter writer);

        public abstract T DecodeCore(BinaryReader reader);

    }
    public static class PacketEncodingHelper
    {

        public static T FindDecoratedProperty<T, TPacket>(this PacketEncoding<TPacket> packetEncoding) where T : PacketEncoding<TPacket> where TPacket : IPacket
        {
            while (packetEncoding is PacketEncoding<TPacket> item)
            {
                if (item is T packetEncodingDesire)
                    return packetEncodingDesire;
                packetEncoding = (PacketEncoding<TPacket>)packetEncoding.Encoding;
            }
            return null;
        }
    }
}
