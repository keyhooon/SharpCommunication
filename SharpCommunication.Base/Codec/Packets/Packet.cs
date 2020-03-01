using System.IO;

namespace SharpCommunication.Base.Codec.Packets
{
    public interface IPacket
    {
        string ToString();        
    }

    public abstract class PacketEncoding : IEncoding<IPacket>
    {
        public PacketEncoding Encoding { get; }
        public PacketEncoding(PacketEncoding encoding)
        {
            Encoding = encoding;
        }

        public abstract void EncodeCore(IPacket packet, BinaryWriter writer);

        public abstract IPacket DecodeCore(BinaryReader reader);

    }
    public static class PacketEncodingHelper
    {

        public static T FindDecoratedEncoding<T>(this PacketEncoding packetEncoding) where T : PacketEncoding 
        {
            while (packetEncoding is PacketEncoding item)
            {
                if (item is T packetEncodingDesire)
                    return packetEncodingDesire;
                packetEncoding = packetEncoding.Encoding;
            }
            return null;
        }
    }
}
