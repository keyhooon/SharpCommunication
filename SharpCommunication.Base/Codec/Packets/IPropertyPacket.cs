namespace SharpCommunication.Codec.Packets
{
    public interface IPropertyPacket<T> : IPacket 
    {
        T Property { get; set; }

    }
}

