﻿using System;
using System.Collections.ObjectModel;
using SharpCommunication.Channels;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Transport
{
    interface IDataTransport<TPacket> where TPacket : IPacket
    {

        event EventHandler IsOpenChanged;
        event EventHandler CanOpenChanged;
        event EventHandler CanCloseChanged;
        ReadOnlyObservableCollection<IChannel<TPacket>> Channels { get; }

        void Open();

        void Close();

        bool IsOpen { get; }

        bool CanOpen { get; }

        bool CanClose { get; }

    }
}
