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

        public GpsService(SerialPortDataTransport<Gps> dataTransport, Codec<Gps> codec) : base(dataTransport, codec)
        {
            GpsSVs = new List<SatelliteVehicleInView>();
            GlonassSVs = new List<SatelliteVehicleInView>();
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["BOD"]).DecodeFinished +=
                (sender, e) =>
                {
                    Bod = (Bod)e.Packet;
                    RaisePropertyChanged(nameof(Bod));
                    BodChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GGA"]).DecodeFinished +=
                (sender, e) =>
                {
                    Gga = (Gga)e.Packet;
                    GpsData = new GpsData(
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
                    Gll = (Gll)e.Packet;
                    Position = new GeographicPosition(Gll.FixTime, Gll.Latitude, Gll.Longitude, Gll.DataActive, Gll.ModeIndicator);
                    RaisePropertyChanged(nameof(Gll));
                    RaisePropertyChanged(nameof(Position));
                    GllChanged?.Invoke(this, EventArgs.Empty);
                };
                ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["ZDA"]).DecodeFinished +=
                (sender, e) =>
                {
                    Zda = (Zda)e.Packet;
                    FixDateTime = Zda.FixDateTime;
                    RaisePropertyChanged(nameof(Zda));
                    RaisePropertyChanged(nameof(FixDateTime));
                    ZdaChanged?.Invoke(this, EventArgs.Empty);
                };
                ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["VTG"]).DecodeFinished +=
                (sender, e) =>
                {
                    Vtg = (Vtg)e.Packet;
                    RaisePropertyChanged(nameof(Vtg));
                    VtgChanged?.Invoke(this, EventArgs.Empty);
                };
                ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["RTE"]).DecodeFinished +=
                (sender, e) =>
                {
                    Rte = (Rte)e.Packet;
                    RaisePropertyChanged(nameof(Rte));
                    RteChanged?.Invoke(this, EventArgs.Empty);
                };
                ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["RMC"]).DecodeFinished +=
                (sender, e) =>
                {
                    Rmc = (Rmc)e.Packet;
                    RaisePropertyChanged(nameof(Rmc));
                    RmcChanged?.Invoke(this, EventArgs.Empty);
                };
                ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["RMB"]).DecodeFinished +=
                (sender, e) =>
                {
                    Rmb = (Rmb)e.Packet;
                    RaisePropertyChanged(nameof(Rmb));
                    RmbChanged?.Invoke(this, EventArgs.Empty);
                };
                ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GSV"]).DecodeFinished +=
                (sender, e) =>
                {
                    Gsv = (Gsv)e.Packet;
                    if (Gsv.SVs.Count == 0)
                        return;
                    if (Gsv.SVs.First().PrnNumber < 64)
                    {
                        if (Gsv.MessageNumber == 1)
                        {
                            GpsSVs = new List<SatelliteVehicleInView>(Gsv.TotalMessages * 4);
                        }
                        foreach (var sv in Gsv.SVs)
                        {
                            GpsSVs.Add(new SatelliteVehicleInView(sv.PrnNumber, sv.Elevation, sv.Azimuth, sv.SignalToNoiseRatio));
                        }
                        RaisePropertyChanged(nameof(Gsv));
                        if(Gsv.MessageNumber == Gsv.TotalMessages)
                            RaisePropertyChanged(nameof(GpsSVs));
                    }
                    else if (Gsv.SVs.First().PrnNumber < 128 )
                    {
                        if (Gsv.MessageNumber == 1)
                        {
                            GlonassSVs = new List<SatelliteVehicleInView>(Gsv.TotalMessages * 4);
                        }
                        foreach (var sv in Gsv.SVs)
                        {
                            GlonassSVs.Add(new SatelliteVehicleInView(sv.PrnNumber, sv.Elevation, sv.Azimuth, sv.SignalToNoiseRatio));
                        }
                        RaisePropertyChanged(nameof(Gsv));
                        if (Gsv.MessageNumber == Gsv.TotalMessages)
                            RaisePropertyChanged(nameof(GlonassSVs));
                    }


                    GsvChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GST"]).DecodeFinished +=
                (sender, e) =>
                {
                    Gst = (Gst)e.Packet;
                    Error = new PseudorangeErrorStatics(Gst.FixTime,
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
                    Gsa = (Gsa)e.Packet;
                    ActiveSatellites = Gsa.SVs.Select(o => new Satellite(o)).ToList();
                    Dop = new Dop(Gsa.Hdop, Gsa.Vdop, Gsa.Pdop, Gsa.FixMode, Gsa.GpsMode);
                    RaisePropertyChanged(nameof(Gsa));
                    RaisePropertyChanged(nameof(ActiveSatellites));
                    RaisePropertyChanged(nameof(Dop));
                    GsaChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GNS"]).DecodeFinished +=
                (sender, e) =>
                {
                    Gns = (Gns)e.Packet;
                    GnssData = new GnssData(
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



        public Bod Bod { get; private set; }

        public Gga Gga { get; private set; }

        public Gll Gll { get; private set; }

        public Gns Gns { get; private set; }

        public Gsa Gsa { get; private set; }

        public Gst Gst { get; private set; }

        public Gsv Gsv { get; private set; }

        public Rmb Rmb { get; private set; }

        public Rmc Rmc { get; private set; }

        public Rte Rte { get; private set; }

        public Vtg Vtg { get; private set; }

        public Zda Zda { get; private set; }


        public DateTime FixDateTime { get; private set; }

        public GpsData GpsData { get; private set; }

        public GnssData GnssData { get; private set; }

        public GeographicPosition Position { get; private set; }

        public Dop Dop { get; private set; }

        public PseudorangeErrorStatics Error { get; private set; }

        /// <summary>
        /// Satellite vehicles in this message part.
        /// </summary>
        public IReadOnlyList<Satellite> ActiveSatellites { get; private set; }


        /// <summary>
        /// Satellite vehicles in this message part.
        /// </summary>
        public List<SatelliteVehicleInView> GpsSVs { get; private set; }

        /// <summary>
        /// Satellite vehicles in this message part.
        /// </summary>
        public List<SatelliteVehicleInView> GlonassSVs { get; private set; }
    }
}
