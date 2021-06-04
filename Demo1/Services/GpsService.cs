using GPSModule.Services.Models;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Module.Services;
using SharpCommunication.Transport.SerialPort;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSModule.Services
{
    public class GpsService : TransportService<Gps>
    {
        public event EventHandler BodChanged;
        public event EventHandler GgaChanged;
        public event EventHandler GllChanged;
        public event EventHandler ZdaChanged;
        public event EventHandler VtgChanged;
        public event EventHandler RteChanged;
        public event EventHandler RmcChanged;
        public event EventHandler RmbChanged;
        public event EventHandler GsvChanged;
        public event EventHandler GstChanged;
        public event EventHandler GsaChanged;
        public event EventHandler GnsChanged;

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
        private DateTime fixDateTime;
        private GpsData gpsData;
        private GnssData gnssData;
        private GeographicPosition position;
        private PseudorangeErrorStatics error;
        private IReadOnlyList<Satellite> activeSatellites;
        private List<SatelliteVehicleInView> gpsSVs;
        private List<SatelliteVehicleInView> glonassSVs;
        private Dop dop;

        public GpsService(SerialPortDataTransport<Gps> dataTransport, Codec<Gps> codec) : base(dataTransport, codec)
        {
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["BOD"]).DecodeFinished +=
                (sender, e) =>
                {
                    _bod = (Bod)e.Packet;
                    RaisePropertyChanged(nameof(Bod));
                    BodChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GGA"]).DecodeFinished +=
                (sender, e) =>
                {
                    _gga = (Gga)e.Packet;
                    gpsData = new GpsData(
                        Gga.FixTime,
                        Gga.Latitude,
                        Gga.Longitude,
                        Gga.Quality,
                        Gga.Hdop,
                        Gga.NumberOfSatellites,
                        Gga.Altitude,
                        Gga.AltitudeUnits,
                        Gga.HeightOfGeoid,
                        Gga.HeightOfGeoidUnits,
                        Gga.TimeSinceLastDgpsUpdate,
                        Gga.DgpsStationId);
                    RaisePropertyChanged(nameof(Gga));
                    RaisePropertyChanged(nameof(GpsData));
                    GgaChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GLL"]).DecodeFinished +=
                (sender, e) =>
                {
                    _gll = (Gll)e.Packet;
                    position = new GeographicPosition(Gll.FixTime, Gll.Latitude, Gll.Longitude, Gll.DataActive, Gll.ModeIndicator);
                    RaisePropertyChanged(nameof(Gll));
                    RaisePropertyChanged(nameof(Position));
                    GllChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["ZDA"]).DecodeFinished +=
                (sender, e) =>
                {
                    _zda = (Zda)e.Packet;
                    fixDateTime = Zda.FixDateTime;
                    RaisePropertyChanged(nameof(Zda));
                    RaisePropertyChanged(nameof(FixDateTime));
                    ZdaChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["VTG"]).DecodeFinished +=
                (sender, e) =>
                {
                    _vtg = (Vtg)e.Packet;
                    RaisePropertyChanged(nameof(Vtg));
                    VtgChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["RTE"]).DecodeFinished +=
                (sender, e) =>
                {
                    _rte = (Rte)e.Packet;
                    RaisePropertyChanged(nameof(Rte));
                    RteChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["RMC"]).DecodeFinished +=
                (sender, e) =>
                {
                    _rmc = (Rmc)e.Packet;
                    RaisePropertyChanged(nameof(Rmc));
                    RmcChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["RMB"]).DecodeFinished +=
                (sender, e) =>
                {
                    _rmb = (Rmb)e.Packet;
                    RaisePropertyChanged(nameof(Rmb));
                    RmbChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GSV"]).DecodeFinished +=
                (sender, e) =>
                {
                    _gsv = (Gsv)e.Packet;
                    if (_gsv.MessageType == "GP")
                    {
                        if (_gsv.MessageNumber == 1)
                        {
                            gpsSVs = new List<SatelliteVehicleInView>(Gsv.TotalMessages * 4);
                        }
                        foreach (var sv in _gsv.SVs)
                        {
                            gpsSVs.Add(new SatelliteVehicleInView(sv.PrnNumber, sv.Elevation, sv.Azimuth, sv.SignalToNoiseRatio));
                        }
                        RaisePropertyChanged(nameof(Gsv));
                        if(_gsv.MessageNumber == _gsv.TotalMessages)
                            RaisePropertyChanged(nameof(GpsSVs));
                    }
                    if (_gsv.MessageType == "GL")
                    {
                        if (Gsv.MessageNumber == 1)
                        {
                            glonassSVs = new List<SatelliteVehicleInView>(_gsv.TotalMessages * 4);
                        }
                        foreach (var sv in _gsv.SVs)
                        {
                            glonassSVs.Add(new SatelliteVehicleInView(sv.PrnNumber, sv.Elevation, sv.Azimuth, sv.SignalToNoiseRatio));
                        }
                        RaisePropertyChanged(nameof(Gsv));
                        if (_gsv.MessageNumber == _gsv.TotalMessages)
                            RaisePropertyChanged(nameof(GlonassSVs));
                    }


                    GsvChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GST"]).DecodeFinished +=
                (sender, e) =>
                {
                    _gst = (Gst)e.Packet;
                    error = new PseudorangeErrorStatics(Gst.FixTime,
                        Gst.Rms,
                        Gst.SigmaLatitudeError,
                        Gst.SigmaLongitudeError,
                        Gst.SigmaHeightError,
                        Gst.SemiMajorError,
                        Gst.SemiMinorError,
                        Gst.ErrorOrientation);
                    RaisePropertyChanged(nameof(Gst));
                    RaisePropertyChanged(nameof(Error));
                    GstChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GSA"]).DecodeFinished +=
                (sender, e) =>
                {
                    _gsa = (Gsa)e.Packet;
                    activeSatellites = Gsa.SVs.Select(o => new Satellite(o)).ToList();
                    dop = new Dop(Gsa.Hdop, Gsa.Vdop, Gsa.Pdop, Gsa.FixMode, Gsa.GpsMode);
                    RaisePropertyChanged(nameof(Gsa));
                    RaisePropertyChanged(nameof(ActiveSatellites));
                    RaisePropertyChanged(nameof(Dop));
                    GsaChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GNS"]).DecodeFinished +=
                (sender, e) =>
                {
                    _gns = (Gns)e.Packet;
                    gnssData = new GnssData(
                        Gns.FixTime,
                        Gns.Latitude,
                        Gns.Longitude,
                        Gns.GlonassModeIndicator,
                        Gns.Hdop,
                        Gns.NumberOfSatellites,
                        Gns.OrhometricHeight,
                        Gns.GeoidalSeparation,
                        Gns.TimeSinceLastDgpsUpdate,
                        Gns.DgpsStationId,
                        Gns.Status);
                    RaisePropertyChanged(nameof(Gns));
                    RaisePropertyChanged(nameof(GnssData));
                    GnsChanged?.Invoke(this, EventArgs.Empty);
                };

        }



        public Bod Bod => _bod;
        public Gga Gga => _gga;
        public Gll Gll => _gll;
        public Gns Gns => _gns;
        public Gsa Gsa => _gsa;
        public Gst Gst => _gst;
        public Gsv Gsv => _gsv;
        public Rmb Rmb => _rmb;
        public Rmc Rmc => _rmc;
        public Rte Rte => _rte;
        public Vtg Vtg => _vtg;
        public Zda Zda => _zda;
        

        public DateTime FixDateTime => fixDateTime;

        public GpsData GpsData => gpsData;

        public GnssData GnssData => gnssData;

        public GeographicPosition Position => position;

        public Dop Dop => dop;

        public PseudorangeErrorStatics Error => error;

        /// <summary>
        /// Satellite vehicles in this message part.
        /// </summary>
        public IReadOnlyList<Satellite> ActiveSatellites => activeSatellites;


        /// <summary>
        /// Satellite vehicles in this message part.
        /// </summary>
        public List<SatelliteVehicleInView> GpsSVs => gpsSVs;

        /// <summary>
        /// Satellite vehicles in this message part.
        /// </summary>
        public List<SatelliteVehicleInView> GlonassSVs => glonassSVs;

    }
}
