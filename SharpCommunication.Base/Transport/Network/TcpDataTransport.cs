using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Transport;

namespace SharpCommunication.Base.Transport.Network
{
    public abstract class TcpDataTransport : DataTransport
    {

        private TcpListener _tcpListener;

        public int ListenPort { get; protected set; }

        public int BackLog { get; protected set; }



        protected TcpDataTransport(ChannelFactory channelFactory) : base(channelFactory)
        {
        }

        protected override void OpenCore()
        {
            Log.LogInformation("Starting ...");
            var localEndPoint = new IPEndPoint(IPAddress.Any, ListenPort);
            _tcpListener = new TcpListener(localEndPoint);
            _tcpListener.Start(BackLog);



            _tcpListener.BeginAcceptSocket(TcpClientAccept, null);

            Log.LogDebug("- Starting thread workers");
            Log.LogInformation("Started");
        }
        void TcpClientAccept(IAsyncResult ar)
        {
            try
            {
                var socket = _tcpListener.EndAcceptSocket(ar);
                socket.ReceiveTimeout = 2000;
                var networkStream = new NetworkStream(socket);
                OnChannelStructed(new ChannelStructEventArg(ChannelFactory.Create(networkStream, socket)));
                _tcpListener.BeginAcceptSocket(TcpClientAccept, null);
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected override bool GetIsOpenedCore()
        {
            return _tcpListener != null && _tcpListener.Server.IsBound;
        }

        protected override void CloseCore()
        {
            Log.LogInformation("Stopping ...");

            _tcpListener.Server.Close();
            _tcpListener.Stop();
            Log.LogInformation("- Destroying Socket");
            Log.LogInformation("- Closing active connections");
            Log.LogInformation("Stopped");
        }



        public override void Dispose()
        {
            base.Dispose();
            Close();
        }

    }

}

