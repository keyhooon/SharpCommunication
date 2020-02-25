using System;
using System.Collections.Generic;

namespace SharpCommunication.Base.Codec.Packets
{
    public class PacketEncodingBuilder
    {
        protected PacketEncodingBuilder(List<Func<PacketEncoding<IPacket>, PacketEncoding<IPacket>>> setupActions)
        {
            this.SetupActions = setupActions;
        }

        public static PacketEncodingBuilder CreateDefaultBuilder()
        {
            var setupActions = new List<Func<PacketEncoding<IPacket>, PacketEncoding<IPacket>>>();
            var builder = new PacketEncodingBuilder(setupActions);
            return builder;
        }


        public PacketEncoding<IPacket> Build() 
        {
            PacketEncoding<IPacket> encoding = null;
            foreach (var action in SetupActions)
            {
                encoding = action(encoding);
            }
            return encoding;
        }
        public List<Func<PacketEncoding<IPacket>, PacketEncoding<IPacket>>> SetupActions { get; }
    }
}
