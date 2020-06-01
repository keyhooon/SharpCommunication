using System;
using System.Collections.Generic;

namespace SharpCommunication.Codec.Encoding
{
    public class PacketEncodingBuilder
    {
        protected PacketEncodingBuilder(List<Func<PacketEncoding, PacketEncoding>> setupActions)
        {
            this.SetupActions = setupActions;
        }

        public static PacketEncodingBuilder CreateDefaultBuilder()
        {
            var setupActions = new List<Func<PacketEncoding, PacketEncoding>>();
            var builder = new PacketEncodingBuilder(setupActions);
            return builder;
        }


        public PacketEncoding Build() 
        {
            PacketEncoding encoding = null;
            for (int i = SetupActions.Count - 1; i >= 0; i--)
                encoding = SetupActions[i](encoding);
            return encoding;
        }
        private List<Func<PacketEncoding, PacketEncoding>> SetupActions { get; }

        public PacketEncodingBuilder AddDecorate(Func<PacketEncoding, PacketEncoding> func)
        {
            SetupActions.Add(func);
            return this;
        }
    }
}
