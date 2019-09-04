using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SharpCommunication.Base.Channels;

namespace SharpCommunication.Base.Transport
{
    public abstract class DataTransport : IDataTransport
    {
        private bool _isOpen;
        readonly CancellationTokenSource _tokenSource;
        private readonly Task _checkingIsOpenedTask;
        protected Stream InStream;
        protected ObservableCollection<IChannel> _channels;
        protected ILogger Log { get; }

        public event EventHandler IsOpenChanged;
        public event EventHandler CanOpenChanged;
        public event EventHandler CanCloseChanged;
        public event EventHandler<ChannelStructEventArg> ChannelStructed;
        public event EventHandler<ChannelDestructEventArg> ChannelDestruct;
        public event PropertyChangedEventHandler PropertyChanged;

        protected DataTransport(ChannelFactory channelFactory)
        {
            Log = NullLoggerProvider.Instance.CreateLogger("DataTransport");
            ChannelFactory = channelFactory;
            _channels = new ObservableCollection<IChannel>();
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;
            _checkingIsOpenedTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        IsOpen = GetIsOpenedCore();
                        await Task.Delay(500, token);
                        token.ThrowIfCancellationRequested();
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

        public bool CanOpen => !IsOpen && CanOpenCore();

        public bool CanClose => IsOpen && CanCloseCore();

        public void Open()
        {
            if (IsOpen)
                throw new InvalidOperationException();
            OpenCore();
            IsOpen = GetIsOpenedCore();
        }

        public ReadOnlyObservableCollection<IChannel> Channels => new ReadOnlyObservableCollection<IChannel>(_channels);

        public void Close()
        {
            if (IsOpen == false)
                throw new InvalidOperationException();
            CloseCore();
            foreach (var channel in _channels)
                OnChannelDestruct(new ChannelDestructEventArg(channel));
            _channels.Clear();
            IsOpen = GetIsOpenedCore();
        }

        public bool IsOpen
        {
            get => _isOpen;
            protected set
            {
                if (_isOpen == value)
                    return;
                _isOpen = value;
                if (!_isOpen && _channels.Count != 0)
                {
                    foreach (var channel in _channels)
                        OnChannelDestruct(new ChannelDestructEventArg(channel));
                    _channels.Clear();
                }
                OnIsOpenChanged();
            }
        }

        public ChannelFactory ChannelFactory { get; private set; }

        protected abstract bool GetIsOpenedCore();

        protected abstract void OpenCore();

        protected abstract void CloseCore();

        protected virtual bool CanOpenCore()
        {
            return true;
        }

        protected virtual bool CanCloseCore()
        {
            return true;
        }

        public virtual void Dispose()
        {
            _tokenSource.Cancel();
            Task.WaitAll(new[] { _checkingIsOpenedTask }, 1000);
            IsOpen = false;
        }

        protected virtual void OnIsOpenChanged()
        {
            IsOpenChanged?.Invoke(this, null);
            OnCanOpenChanged();
            OnCanCloseChanged();
        }

        protected virtual void OnChannelStructed(ChannelStructEventArg e)
        {
            _channels.Add(e.Channel);
            ChannelStructed?.Invoke(this, e);
        }

        protected virtual void OnChannelDestruct(ChannelDestructEventArg e)
        {

            ChannelDestruct?.Invoke(this, e);
            e.Channel.Dispose();
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
