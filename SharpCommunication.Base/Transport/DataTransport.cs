using System;
using System.Collections.ObjectModel;
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
        private readonly CancellationTokenSource _tokenSource;
        private readonly Task _checkingIsOpenedTask;
        protected ObservableCollection<IChannel> _channels;
        protected ILogger Log { get; }

        public event EventHandler IsOpenChanged;
        public event EventHandler CanOpenChanged;
        public event EventHandler CanCloseChanged;

        protected DataTransport(ChannelFactory channelFactory)
        {
            Log = NullLoggerProvider.Instance.CreateLogger("DataTransport");
            ChannelFactory = channelFactory;

            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;
            _checkingIsOpenedTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (_isOpen != IsOpen)
                        {
                            _isOpen = IsOpen;
                            OnIsOpenChanged();
                        }
                        await Task.Delay(500, token);
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
                _channels = new ObservableCollection<IChannel>();
                OnIsOpenChanged();
                _isOpen = true;
            }
        }

        public void Close()
        {
            if (IsOpen == false)
                throw new InvalidOperationException();
            CloseCore();
            if (!IsOpen)
            {
                foreach (var ch in _channels)
                {
                    ch.Dispose();
                }
                _channels.Clear();
                OnIsOpenChanged();
                _isOpen = false;
            }
        }


        public bool CanOpen => !IsOpen && CanOpenCore;

        public bool CanClose => IsOpen && CanCloseCore;

        public bool IsOpen => IsOpenCore;

        public ReadOnlyObservableCollection<IChannel> Channels => new ReadOnlyObservableCollection<IChannel>(_channels);



        protected readonly ChannelFactory ChannelFactory;

        protected abstract bool IsOpenCore { get; }

        protected abstract void OpenCore();

        protected abstract void CloseCore();

        protected virtual bool CanOpenCore => true;

        protected virtual bool CanCloseCore => true;

        public virtual void Dispose()
        {
            _tokenSource.Cancel();
            Task.WaitAll(new[] { _checkingIsOpenedTask }, 1000);
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
