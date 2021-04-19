using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCommunication.Channels;
using SharpCommunication.Codec;
using SharpCommunication.Module.Services;
using SharpCommunication.Transport;
using SharpCommunication.Transport.SerialPort;

namespace GPSModule.Services
{
    public class GpsService : TransportService<Gps>
    {
        private Bod _bod;
        private Gga _gga;
        private Gll _gll;
        private Zda _zda;
        private Vtg _vtg;
        private Rte _rte;
        private Rmc _rmc;
        private Rmb _rmb;
        private Gsv _gsv;
        private Gst _gst;
        private Gsa _gsa;
        private Gns _gns;

        public GpsService(SerialPortDataTransport<Gps> dataTransport, Codec<Gps> codec) : base(dataTransport, codec)
        {
            ((INotifyCollectionChanged) dataTransport.Channels).CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null)
                    foreach (var argsNewItem in args.NewItems)
                        ((IChannel<Gps>)argsNewItem).DataReceived += OnDataReceived;
                if (args.OldItems != null)
                    foreach (var argsOldItem in args.OldItems)
                        ((IChannel<Gps>)argsOldItem).DataReceived -= OnDataReceived;
            };
        }

        private void OnDataReceived(object? sender, DataReceivedEventArg<Gps> e)
        {
            switch (e.Data.Content)
            {
                case Bod bod:
                    Bod = bod;
                    break;
                case Gga gga:
                    Gga = gga;
                    break;
                case Gll gll:
                    Gll = gll;
                    break;
                case Gns gns:
                    Gns = gns;
                    break;
                case Gsa gsa:
                    Gsa = gsa;
                    break;
                case Gst gst:
                    Gst = gst;
                    break;
                case Gsv gsv:
                    Gsv = gsv;
                    break;
                case Rmb rmb:
                    Rmb = rmb;
                    break;
                case Rmc rmc:
                    Rmc = rmc;
                    break;
                case Rte rte:
                    Rte = rte;
                    break;
                case Vtg vtg:
                    Vtg = vtg;
                    break;
                case Zda zda:
                    Zda = zda;
                    break;
                case UnknownMessage unknownMessage:
                    break;
            }
        }

        public Bod Bod { get => _bod; set => SetProperty(ref _bod, value); }
        public Gga Gga { get => _gga; set => SetProperty(ref _gga, value); }
        public Gll Gll { get => _gll; set => SetProperty(ref _gll, value); }
        public Gns Gns { get => _gns; set => SetProperty(ref _gns, value); }
        public Gsa Gsa { get => _gsa; set => SetProperty(ref _gsa, value); }
        public Gst Gst { get => _gst; set => SetProperty(ref _gst, value); }
        public Gsv Gsv { get => _gsv; set => SetProperty(ref _gsv, value); }
        public Rmb Rmb { get => _rmb; set => SetProperty(ref _rmb, value); }
        public Rmc Rmc { get => _rmc; set => SetProperty(ref _rmc, value); }
        public Rte Rte { get => _rte; set => SetProperty(ref _rte, value); }
        public Vtg Vtg { get => _vtg; set => SetProperty(ref _vtg, value); }
        public Zda Zda { get => _zda; set => SetProperty(ref _zda, value); }

    }
}
