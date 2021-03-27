using System;
using System.Linq;
using System.Reflection;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Codec
{
    public class Gps : IDescendantPacket, IPropertyPacket
    {
        public byte[] PropertyBinary { get; set; }
        public IAncestorPacket Content { get; set; }

        public class Encoding : DescendantGenericPacketEncodingDecorator<Gps, string>
        {
            public Encoding() : base(null)
            {
                var typeInfo = typeof(NmeaMessage).GetTypeInfo();
                foreach (var subclass in typeInfo.Assembly.DefinedTypes.Where(t => t.IsSubclassOf(typeof(NmeaMessage.Encoding))))
                {
                    var attr = subclass.GetCustomAttribute<NmeaMessageTypeAttribute>(false);
                    if (attr == null || subclass.IsAbstract)
                        continue;
                    Register(PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => (EncodingDecorator)subclass.DeclaredConstructors.First(c => c.GetParameters().Length == 1).Invoke(new object[] {item})).Build());
                }
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder()
                    .AddDecorate(o => new HeaderPacketEncoding(o, BitConverter.GetBytes('$')))
                    .AddDecorate(o => new PropertyPacketEncoding(o, 2))
                    .AddDecorate(o => new Encoding());
        }
    }
}
