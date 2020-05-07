namespace SharpCommunication.Base.Codec.Packets
{
    public interface IDescendantPacket : IPacket
    {
        IAncestorPacket DescendantPacket { get; set; }

    }
}
