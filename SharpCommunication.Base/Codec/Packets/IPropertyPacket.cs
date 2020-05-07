namespace SharpCommunication.Base.Codec.Packets
{
    public interface IPropertyPacket : IPacket
    {
        byte[] PropertyBinary { get; set; }

    }
}

