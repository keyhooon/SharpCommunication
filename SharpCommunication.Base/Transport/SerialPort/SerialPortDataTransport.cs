using SharpCommunication.Base.Channels;
using System;


namespace SharpCommunication.Base.Transport.SerialPort
{
    public class SerialPortDataTransport : DataTransport
    {
        public SerialPortDataTransport(ChannelFactory channelFactory) : base(channelFactory)
        {
        }
        protected override void CloseCore()
        {
            throw new NotImplementedException();
        }

        protected override bool GetIsOpenedCore()
        {
            throw new NotImplementedException();
        }

        protected override void OpenCore()
        {
            throw new NotImplementedException();
        }
    }
}
