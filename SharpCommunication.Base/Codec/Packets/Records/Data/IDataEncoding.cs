namespace SharpCommunication.Base.Codec.Packets.Records.Data
{
    public interface IDataEncoding : IEncoding<Data>
    {
        int Id { get; }
    }
}
