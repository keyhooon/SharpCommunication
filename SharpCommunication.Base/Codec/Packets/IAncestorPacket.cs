namespace SharpCommunication.Base.Codec.Packets
{
    public interface IAncestorPacket : IPacket
    {
        byte Id { get; }
    }
}
