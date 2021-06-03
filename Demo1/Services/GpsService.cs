using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using SharpCommunication.Channels;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Module.Services;
using SharpCommunication.Transport.SerialPort;

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

        public GpsService(SerialPortDataTransport<Gps> dataTransport, Codec<Gps> codec) : base(dataTransport, codec)
        {
            //((INotifyCollectionChanged) dataTransport.Channels).CollectionChanged += (sender, args) =>
            //{
            //    if (args.NewItems != null)
            //        foreach (var argsNewItem in args.NewItems)
            //            ((IChannel<Gps>)argsNewItem).DataReceived += OnDataReceived;
            //    if (args.OldItems != null)
            //        foreach (var argsOldItem in args.OldItems)
            //            ((IChannel<Gps>)argsOldItem).DataReceived -= OnDataReceived;
            //};

            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["BOD"]).DecodeFinished +=
                (sender, e) =>
                {
                    Bod = (Bod)e.Packet;
                    BodChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GGA"]).DecodeFinished +=
                (sender, e) =>
                {
                    Gga = (Gga)e.Packet;
                    GgaChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GLL"]).DecodeFinished +=
                (sender, e) =>
                {
                    Gll = (Gll)e.Packet;
                    GllChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["ZDA"]).DecodeFinished +=
                (sender, e) =>
                {
                    Zda = (Zda)e.Packet;
                    ZdaChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding< DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["VTG"]).DecodeFinished +=
                (sender, e) =>
                {
                    Vtg = (Vtg)e.Packet;
                    VtgChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding< DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["RTE"]).DecodeFinished +=
                (sender, e) =>
                {
                    Rte = (Rte)e.Packet;
                    RteChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["RMC"]).DecodeFinished +=
                (sender, e) =>
                {
                    Rmc = (Rmc)e.Packet;
                    RmcChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["RMB"]).DecodeFinished +=
                (sender, e) =>
                {
                    Rmb = (Rmb)e.Packet;
                    RmbChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GSV"]).DecodeFinished +=
                (sender, e) =>
                {
                    Gsv = (Gsv)e.Packet;
                    GsvChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GST"]).DecodeFinished +=
                (sender, e) =>
                {
                    Gst = (Gst)e.Packet;
                    GstChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GSA"]).DecodeFinished +=
                (sender, e) =>
                {
                    Gsa = (Gsa)e.Packet;
                    GsaChanged?.Invoke(this, EventArgs.Empty);
                };
            ((AncestorGenericPacketEncodingDecorator<string>)codec.Encoding.FindDecoratedEncoding<DescendantGenericPacketEncodingDecorator<Gps, string>>().EncodingDictionary["GNS"]).DecodeFinished +=
                (sender, e) =>
                {
                    Gns = (Gns)e.Packet;
                    GnsChanged?.Invoke(this, EventArgs.Empty);
                };

        }

        private void OnDataReceived(object sender, DataReceivedEventArg<Gps> e)
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
                case UnknownMessage:
                    break;
            }
        }

        public Bod Bod { get => _bod; private set => SetProperty(ref _bod, value,()=>
        {
            TrueBearing = Bod.TrueBearing;
            MagneticBearing = Bod.MagneticBearing;
            OriginId = Bod.OriginId;
            DestinationId = Bod.DestinationId;
        }); }
        public Gga Gga { get => _gga; private set => SetProperty(ref _gga, value,()=> 
        {
            FixTime = Gga.FixTime;
            Latitude = Gga.Latitude;
            Longitude = Gga.Longitude;
            Quality = Gga.Quality;
            NumberOfSatellites = Gga.NumberOfSatellites;
            Hdop = Gga.Hdop;
            Altitude = Gga.Altitude;
            AltitudeUnits = Gga.AltitudeUnits;
            HeightOfGeoid = Gga.HeightOfGeoid;
            HeightOfGeoidUnits = Gga.HeightOfGeoidUnits;
            TimeSinceLastDgpsUpdate = Gga.TimeSinceLastDgpsUpdate;
            DgpsStationId = Gga.DgpsStationId;
        }); }
        public Gll Gll { get => _gll; private set => SetProperty(ref _gll, value,()=> 
        {
            Latitude = Gll.Latitude;
            Longitude = Gll.Longitude;
            FixTime = Gll.FixTime;
            DataActive = Gll.DataActive;
            ModeIndicator = Gll.ModeIndicator;
        }); }
        public Gns Gns { get => _gns; private set => SetProperty(ref _gns, value, ()=>
        {
            FixTime = Gns.FixTime;
            Latitude = Gns.Latitude;
            Longitude = Gns.Longitude;
            GpsModeIndicator = Gns.GpsModeIndicator;
            GlonassModeIndicator = Gns.GlonassModeIndicator;
            FutureModeIndicator = Gns.FutureModeIndicator;
            NumberOfSatellites = Gns.NumberOfSatellites;
            Hdop = Gns.Hdop;
            OrhometricHeight = Gns.OrhometricHeight;
            GeoidalSeparation = Gns.GeoidalSeparation;
            TimeSinceLastDgpsUpdate = Gns.TimeSinceLastDgpsUpdate;
            DgpsStationId = Gns.DgpsStationId;
            NavigationalStatus = Gns.Status;
        }); }
        public Gsa Gsa { get => _gsa; private set => SetProperty(ref _gsa, value,()=>
        {
            GpsMode = Gsa.GpsMode;
            FixMode = Gsa.FixMode;
            SVs = Gsa.SVs;
            Pdop = Gsa.Pdop;
            Hdop = Gsa.Hdop;
            Vdop = Gsa.Vdop;
        }); }
        public Gst Gst { get => _gst; private set => SetProperty(ref _gst, value, ()=>
        {
            Rms = Gst.Rms;
            SemiMajorError = Gst.SemiMajorError;
            SemiMinorError = Gst.SemiMinorError;
            ErrorOrientation = Gst.ErrorOrientation;
            SigmaLatitudeError = Gst.SigmaLatitudeError;
            SigmaLongitudeError = Gst.SigmaLongitudeError;
            SigmaHeightError = Gst.SigmaHeightError;
        }); }
        public Gsv Gsv { get => _gsv; private set => SetProperty(ref _gsv, value, ()=>
        {
            TotalMessages = Gsv.TotalMessages;
            MessageNumber = Gsv.MessageNumber;
            SVsInView = Gsv.SVsInView;
            SVsDetail = Gsv.SVs;
        }); }
        public Rmb Rmb { get => _rmb; private set => SetProperty(ref _rmb, value,()=> 
        {
            DataStatus = Rmb.Status;
            CrossTrackError = Rmb.CrossTrackError;
            OriginWaypointId = Rmb.OriginWaypointId;
            DestinationWaypointId = Rmb.DestinationWaypointId;
            DestinationLatitude = Rmb.DestinationLatitude;
            DestinationLongitude = Rmb.DestinationLongitude;
            RangeToDestination = Rmb.RangeToDestination;
            TrueBearing = Rmb.TrueBearing;
            Velocity = Rmb.Velocity;
            Arrived = Rmb.Arrived;
        }); }
        public Rmc Rmc { get => _rmc; private set => SetProperty(ref _rmc, value,()=>
        {
            Active = Rmc.Active;
            Speed = Rmc.Speed;
            Course = Rmc.Course;
            MagneticVariation = Rmc.MagneticVariation;
        }); }
        public Rte Rte { get => _rte; private set => SetProperty(ref _rte, value,()=>
        {

        }); }
        public Vtg Vtg { get => _vtg; private set => SetProperty(ref _vtg, value , ()=>
        {
            TrueCourseOverGround = Vtg.TrueCourseOverGround;
            MagneticCourseOverGround = Vtg.MagneticCourseOverGround;
            SpeedInKnots = Vtg.SpeedInKnots;
            SpeedInKph = Vtg.SpeedInKph;
        }); }
        public Zda Zda { get => _zda; private set => SetProperty(ref _zda, value,()=>
        {
            FixDateTime = Zda.FixDateTime;
        }); }



        /// <summary>
        /// True Bearing from start to destination
        /// </summary>
        public double TrueBearing { get; private set; }

        /// <summary>
        /// Magnetic Bearing from start to destination
        /// </summary>
        public double MagneticBearing { get; private set; }

        /// <summary>
        /// Name of origin
        /// </summary>
        public string OriginId { get; private set; }

        /// <summary>
        /// Name of destination
        /// </summary>
        public string DestinationId { get; private set; }

        /// <summary>
        /// Time of day fix was taken
        /// </summary>
        public TimeSpan FixTime { get; private set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; private set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; private set; }

        /// <summary>
        /// Fix Quality
        /// </summary>
        public Gga.FixQuality Quality { get; private set; }

        /// <summary>
        /// Number of satellites being tracked
        /// </summary>
        public int NumberOfSatellites { get; private set; }

        /// <summary>
        /// Horizontal Dilution of Precision
        /// </summary>
        public double Hdop { get; private set; }

        /// <summary>
        /// Altitude
        /// </summary>
        public double Altitude { get; private set; }

        /// <summary>
        /// Altitude units ('M' for Meters)
        /// </summary>
        public string AltitudeUnits { get; private set; }

        /// <summary>
        /// Height of geoid (mean sea level) above WGS84
        /// </summary>
        public double HeightOfGeoid { get; private set; }

        /// <summary>
        /// Altitude units ('M' for Meters)
        /// </summary>
        public string HeightOfGeoidUnits { get; private set; }

        /// <summary>
        /// Time since last DGPS update
        /// </summary>
        public TimeSpan TimeSinceLastDgpsUpdate { get; private set; }

        /// <summary>
        /// DGPS Station ID Number
        /// </summary>
        public int DgpsStationId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether data is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if data is active; otherwise, <c>false</c>.
        /// </value>
        public bool DataActive { get; private set; }

        /// <summary>
        /// Positioning system Mode Indicator
        /// </summary>
        public Gll.Mode ModeIndicator { get; private set; }
        /// <summary>
        /// Mode indicator for GPS
        /// </summary>
        /// <seealso cref="GlonassModeIndicator"/>
        /// <see cref="FutureModeIndicator"/>
        public Gns.Mode GpsModeIndicator { get; private set; }

        /// <summary>
        /// Mode indicator for GLONASS
        /// </summary>
        /// <seealso cref="GpsModeIndicator"/>
        /// <see cref="FutureModeIndicator"/>
        public Gns.Mode GlonassModeIndicator { get; private set; }

        /// <summary>
        /// Mode indicator for future constallations
        /// </summary>
        /// <seealso cref="GlonassModeIndicator"/>
        /// <seealso cref="GpsModeIndicator"/>
        public Gns.Mode[] FutureModeIndicator { get; private set; }

        /// <summary>
        /// Orthometric height in meters (MSL reference)
        /// </summary>
        public double OrhometricHeight { get; private set; }

        /// <summary>
        /// Geoidal separation in meters - the difference between the earth ellipsoid surface and mean-sea-level (geoid) surface defined by the reference datum used in the position solution<br/>
        /// '-' = mean-sea-level surface below ellipsoid.
        /// </summary>
        public double GeoidalSeparation { get; private set; }

        /// <summary>
        /// Navigational status
        /// </summary>
        public Gns.NavigationalStatus NavigationalStatus { get; private set; }

        /// <summary>
        /// Mode
        /// </summary>
        public Gsa.ModeSelection GpsMode { get; private set; }

        /// <summary>
        /// Mode
        /// </summary>
        public Gsa.Mode FixMode { get; private set; }

        /// <summary>
        /// IDs of SVs used in position fix
        /// </summary>
        public IReadOnlyList<int> SVs { get; private set; }

        /// <summary>
        /// Dilution of precision
        /// </summary>
        public double Pdop { get; private set; }

        /// <summary>
        /// Vertical dilution of precision
        /// </summary>
        public double Vdop { get; private set; }

        /// <summary>
        /// Gets a value whether the device is active
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// Speed over the ground in knots
        /// </summary>
        public double Speed { get; private set; }

        /// <summary>
        /// Track angle in degrees True
        /// </summary>
        public double Course { get; private set; }

        /// <summary>
        /// Magnetic Variation
        /// </summary>
        public double MagneticVariation { get; private set; }

        /// <summary>
        /// Data Status
        /// </summary>
        public Rmb.DataStatus DataStatus { get; private set; }

        /// <summary>
        /// Cross-track error (steer left when negative, right when positive)
        /// </summary>
        public double CrossTrackError { get; private set; }

        /// <summary>
        /// Origin waypoint ID
        /// </summary>
        public double OriginWaypointId { get; private set; }

        /// <summary>
        /// Destination waypoint ID
        /// </summary>
        public double DestinationWaypointId { get; private set; }

        /// <summary>
        /// Destination Latitude
        /// </summary>
        public double DestinationLatitude { get; private set; }

        /// <summary>
        /// Destination Longitude
        /// </summary>
        public double DestinationLongitude { get; private set; }

        /// <summary>
        /// Range to destination in nautical miles
        /// </summary>
        public double RangeToDestination { get; private set; }

        /// <summary>
        /// Velocity towards destination in knots
        /// </summary>
        public double Velocity { get; private set; }

        /// <summary>
        /// Arrived (<c>true</c> if arrived)
        /// </summary>
        public bool Arrived { get; private set; }

        /// <summary>
        /// Gets the time of fix
        /// </summary>
        public DateTime FixDateTime { get; private set; }

        /// <summary>
        /// Total number of messages of this type in this cycle
        /// </summary>
        public int TotalMessages { get; private set; }

        /// <summary>
        /// Message number
        /// </summary>
        public int MessageNumber { get; private set; }

        /// <summary>
        /// Total number of SVs in view
        /// </summary>
        public int SVsInView { get; private set; }

        /// <summary>
        /// Satellite vehicles in this message part.
        /// </summary>
        public IReadOnlyList<SatelliteVehicle> SVsDetail { get; private set; }


        /// <summary>
        /// RMS value of the pseudo range residuals; includes carrier phase residuals during periods of RTK (double) and RTK (fixed) processing
        /// </summary>
        public double Rms { get; private set; }

        /// <summary>
        /// Error ellipse semi-major axis 1 sigma error, in meters
        /// </summary>
        public double SemiMajorError { get; private set; }

        /// <summary>
        /// Error ellipse semi-minor axis 1 sigma error, in meters
        /// </summary>
        public double SemiMinorError { get; private set; }

        /// <summary>
        /// Error ellipse orientation, degrees from true north
        /// </summary>
        public double ErrorOrientation { get; private set; }

        /// <summary>
        /// Latitude 1 sigma error, in meters
        /// </summary>
        /// <remarks>
        /// The error expressed as one standard deviation.
        /// </remarks>
        public double SigmaLatitudeError { get; private set; }

        /// <summary >
        /// Longitude 1 sigma error, in meters
        /// </summary>
        /// <remarks>
        /// The error expressed as one standard deviation.
        /// </remarks>
        public double SigmaLongitudeError { get; private set; }

        /// <summary >
        /// Height 1 sigma error, in meters
        /// </summary>
        /// <remarks>
        /// The error expressed as one standard deviation.
        /// </remarks>
        public double SigmaHeightError { get; private set; }

        /// <summary>
        ///  Course over ground relative to true north
        /// </summary>
        public double TrueCourseOverGround { get; private set; }

        /// <summary>
        ///  Course over ground relative to magnetic north
        /// </summary>
        public double MagneticCourseOverGround { get; private set; }

        /// <summary>
        /// Speed over ground in knots
        /// </summary>
        public double SpeedInKnots { get; private set; }

        /// <summary>
        /// Speed over ground in kilometers/hour
        /// </summary>
        public double SpeedInKph { get; private set; }
    }
}
