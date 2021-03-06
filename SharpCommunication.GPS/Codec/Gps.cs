﻿using System;
using System.Linq;
using System.Reflection;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Codec
{
    public class Gps : IDescendantPacket, IPropertyPacket<string>
    {
        public string Property { get; set; }
        public IAncestorPacket Content { get; set; }

        public override string ToString()
        {
            return $"Gps {{ Type : {Property}, Content : {Content?.ToString()} }}";
        }

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
                    var createBuilder = subclass.GetDeclaredMethod("CreateBuilder");
                    ;
                    Register(((PacketEncodingBuilder)createBuilder.Invoke(null, null)).Build());
                }
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder()
                    .WithHeader(new [] {BitConverter.GetBytes('$')[0]})
                    .WithStringProperty(2)
                    .AddDecorate(o => new Encoding());
        }
    }
}
