namespace SharpCommunication.Codec.Packets
{
    public interface IDescendantPacket : IPacket
    {
        IAncestorPacket Content { get; set; }

    }
}
