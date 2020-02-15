namespace SharpCommunication.Base.Codec.Packets
{
    public interface IPacketEncoding : IEncoding<Packet>
    {
        int TypeId { get; }
    }
}
