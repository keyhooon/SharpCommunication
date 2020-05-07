using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;

namespace SharpCommunication.Base.Transport
{
    public abstract class DataTransport<TPacket> : IDataTransport<TPacket> where TPacket : IPacket
    {
        private static DataTransportOption DefaultOption => new DataTransportOption(true, 500);
        private bool _isOpen;
        private readonly CancellationTokenSource _tokenSource;
        private readonly Task _checkingIsOpenedTask;
        protected ObservableCollection<IChannel<TPacket>> _channels;
        protected ILogger Log { get; }
        public readonly IChannelFactory<TPacket> ChannelFactory;

        public event EventHandler IsOpenChanged;
        public event EventHandler CanOpenChanged;
        public event EventHandler CanCloseChanged;

        protected DataTransport(IChannelFactory<TPacket> channelFactory) : this(channelFactory, DefaultOption, NullLoggerProvider.Instance.CreateLogger("DataTransport"))
        {

        }
        protected DataTransport(IChannelFactory<TPacket> channelFactory, DataTransportOption option, ILogger log)
        {
            Log = log;
            ChannelFactory = channelFactory;
            _tokenSource = new CancellationTokenSource();
            _channels = new ObservableCollection<IChannel<TPacket>>();
            var token = _tokenSource.Token;
            _checkingIsOpenedTask = Task.Factory.StartNew(async () =>
            {
                while (option.IsOpenCheckAutomatically)
                {
                    try
                    {
                        if (_isOpen != IsOpen)
                        {
                            _isOpen = IsOpen;
                            OnIsOpenChanged();
                        }
                        await Task.Delay(option.IsOpenCheckAutomaticallyDelay , token);
                        if (token.IsCancellationRequested)
                            break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        if (e.GetType() == typeof(OperationCanceledException))
                            break;
                    }
                }
            }, _tokenSource.Token, TaskCreationOptions.AttachedToParent, TaskScheduler.Current);
        }

        public void Open()
        {
            if (IsOpen)
                throw new InvalidOperationException();

            OpenCore();
            if (IsOpen)
            {

                OnIsOpenChanged();
                _isOpen = true;
            }
        }

        public void Close()
        {
            if (IsOpen == false)
                throw new InvalidOperationException();
            foreach (var ch in _channels)
            {
                ch.Dispose();
            }
            _channels.Clear();
            CloseCore();
            if (!IsOpen)
            {
                OnIsOpenChanged();
                _isOpen = false;
            }
        }


        public bool CanOpen => !IsOpen && CanOpenCore;

        public bool CanClose => IsOpen && CanCloseCore;

        public bool IsOpen => IsOpenCore;

        public ReadOnlyObservableCollection<IChannel<TPacket>> Channels => new ReadOnlyObservableCollection<IChannel<TPacket>> (_channels);


  

        protected abstract bool IsOpenCore { get; }

        protected abstract void OpenCore();

        protected abstract void CloseCore();

        protected virtual bool CanOpenCore => true;

        protected virtual bool CanCloseCore => true;


        public virtual void Dispose()
        {
            _tokenSource.Cancel();
        }

        protected virtual void OnIsOpenChanged()
        {
            IsOpenChanged?.Invoke(this, null);
            OnCanOpenChanged();
            OnCanCloseChanged();
        }


        protected virtual void OnCanOpenChanged()
        {
            CanOpenChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCanCloseChanged()
        {
            CanCloseChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
