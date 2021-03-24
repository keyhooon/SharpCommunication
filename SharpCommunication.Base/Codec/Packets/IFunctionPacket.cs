namespace SharpCommunication.Codec.Packets
{
    public interface IFunctionPacket : IAncestorPacket 
    {
        byte[] Param { get; set; }
    }

}
