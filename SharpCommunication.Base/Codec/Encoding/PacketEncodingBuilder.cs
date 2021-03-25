using System;
using System.Collections.Generic;

namespace SharpCommunication.Codec.Encoding
{
    public class PacketEncodingBuilder
    {
        protected PacketEncodingBuilder(List<Func<EncodingDecorator, EncodingDecorator>> setupActions)
        {
            SetupActions = setupActions;
        }

        public static PacketEncodingBuilder CreateDefaultBuilder()
        {
            var setupActions = new List<Func<EncodingDecorator, EncodingDecorator>>();
            var builder = new PacketEncodingBuilder(setupActions);
            return builder;
        }


        public EncodingDecorator Build() 
        {
            EncodingDecorator encoding = null;
            for (var i = SetupActions.Count - 1; i >= 0; i--)
                encoding = SetupActions[i](encoding);
            return encoding;
        }
        private List<Func<EncodingDecorator, EncodingDecorator>> SetupActions { get; }

        public PacketEncodingBuilder AddDecorate(Func<EncodingDecorator, EncodingDecorator> func)
        {
            SetupActions.Add(func);
            return this;
        }
    }
}
