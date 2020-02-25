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
        //public void Encode(T packet, BinaryWriter writer)
        //{
        //    if (packet == null)
        //        throw new ArgumentNullException(nameof(packet));
        //    if (writer == null)
        //        throw new ArgumentNullException(nameof(writer));
        //    EncodeCore(packet, writer);
        //}

        //public T Decode(BinaryReader reader)
        //{
        //    if (reader == null)
        //        throw new ArgumentNullException(nameof(reader));
        //    var obj = DecodeCore(reader);
        //    if (obj == null)
        //        throw new NotSupportedException("The encoding was unable to decode the command.");
        //    return obj;
        //}

        public abstract void EncodeCore(T packet, BinaryWriter writer);

        public abstract T DecodeCore(BinaryReader reader);

    }
    public static class PacketEncodingHelper
    {

        public static T FindDecoratedProperty<T, TPacket>(this PacketEncoding<TPacket> packetEncoding) where T : PacketEncoding<TPacket> where TPacket : IPacket
        {
            while (mapItem is MapItemDecorator item)
            {
                if (item is T extraViewMapItem)
                    return propertySelector(extraViewMapItem);
                mapItem = item.MapItem;
            }
            return null;
        }
    }
}
