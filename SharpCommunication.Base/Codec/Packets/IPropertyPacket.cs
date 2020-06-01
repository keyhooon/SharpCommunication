namespace SharpCommunication.Codec.Packets
{
    public interface IPropertyPacket : IPacket
    {
        byte[] PropertyBinary { get; set; }

    }
}

