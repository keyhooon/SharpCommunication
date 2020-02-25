using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using SharpCommunication.Base.Channels;

namespace SharpCommunication.Base.Transport.Network
{
    public abstract class TcpDataTransport : DataTransport
    {
        private readonly TcpDataTransportOption _option;

        private TcpListener _tcpListener;





        protected TcpDataTransport(ChannelFactory channelFactory, TcpDataTransportOption option) : base(channelFactory)
        {
            _option = option;
        }

        protected override void OpenCore()
        {
            Log.LogInformation("Starting ...");
            var localEndPoint = new IPEndPoint(IPAddress.Any, _option.ListenPort);
            _tcpListener = new TcpListener(localEndPoint);
            _tcpListener.Start(_option.BackLog);



            _tcpListener.BeginAcceptSocket(TcpClientAccept, null);

            Log.LogDebug("- Starting thread workers");
            Log.LogInformation("Started");
        }

        private void TcpClientAccept(IAsyncResult ar)
        {
            try
            {
                var socket = _tcpListener.EndAcceptSocket(ar);
                socket.ReceiveTimeout = 2000;
                var networkStream = new NetworkStream(socket);
                _channels.Add(ChannelFactory.Create(networkStream));

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

        protected override bool IsOpenCore=> _tcpListener != null && _tcpListener.Server.IsBound;

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

