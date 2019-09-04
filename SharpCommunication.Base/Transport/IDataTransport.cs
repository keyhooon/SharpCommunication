using System;
using System.Collections.ObjectModel;
using SharpCommunication.Base.Channels;

namespace SharpCommunication.Base.Transport
{
    interface IDataTransport : IDisposable
    {

        event EventHandler IsOpenChanged;
        event EventHandler CanOpenChanged;
        event EventHandler CanCloseChanged;
        event EventHandler<ChannelStructEventArg> ChannelStructed;
        event EventHandler<ChannelDestructEventArg> ChannelDestruct;

        ChannelFactory ChannelFactory { get; }

        ReadOnlyObservableCollection<IChannel> Channels { get; }



        void Open();

        void Close();

        bool IsOpen { get; }

        bool CanOpen { get; }

        bool CanClose { get; }




    }
}
