namespace SharpCommunication.Base.Codec.Packets
{
    public interface IAncestorPacket : IPacket
    {
        public int Id { get; }
    }

    public abstract class AncestorPacketEncoding<T> : PacketEncoding<T> where T : IAncestorPacket
    {
        public byte Id { get; protected set; }
        public AncestorPacketEncoding(IEncoding<IPacket> encoding, byte id) : base(encoding)
        {
            Id = id;
        }


    }
}
