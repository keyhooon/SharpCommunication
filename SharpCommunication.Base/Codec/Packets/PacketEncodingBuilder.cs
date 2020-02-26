using System;
using System.Collections.Generic;

namespace SharpCommunication.Base.Codec.Packets
{
    public class PacketEncodingBuilder
    {
        protected PacketEncodingBuilder(List<Func<IEncoding<IPacket>, IEncoding<IPacket>>> setupActions)
        {
            this.SetupActions = setupActions;
        }

        public static PacketEncodingBuilder CreateDefaultBuilder()
        {
            var setupActions = new List<Func<IEncoding<IPacket>, IEncoding<IPacket>>>();
            var builder = new PacketEncodingBuilder(setupActions);
            return builder;
        }


        public IEncoding<IPacket> Build() 
        {
            IEncoding<IPacket> encoding = null;
            foreach (var action in SetupActions)
            {
                encoding = action(encoding);
            }
            return encoding;
        }
        public List<Func<IEncoding<IPacket>, IEncoding<IPacket>>> SetupActions { get; }
    }
}
