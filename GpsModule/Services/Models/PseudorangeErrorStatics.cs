using System;

namespace GPSModule.Services.Models
{
    public class PseudorangeErrorStatics
    {
        /// <summary>
        /// Gets the time of fix
        /// </summary>
        public TimeSpan UniversalTimeCoordinated { get; private set; }

        /// <summary>
        /// RMS value of the pseudo range residuals; includes carrier phase residuals during periods of RTK (double) and RTK (fixed) processing
        /// </summary>
        public double Rms { get; private set; }

        /// <summary>
        /// Error ellipse semi-major axis 1 sigma error, in meters
        /// </summary>
        public double SemiMajorDeviationError { get; private set; }

        /// <summary>
        /// Error ellipse semi-minor axis 1 sigma error, in meters
        /// </summary>
        public double SemiMinorDeviationError { get; private set; }

        /// <summary>
        /// Error ellipse orientation, degrees from true north
        /// </summary>
        public double SemiMajorOrientationError { get; private set; }

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
        public double SigmaAltitudeError { get; private set; }

        public PseudorangeErrorStatics(TimeSpan universalTimeCoordinated, double rms, double sigmaLatitudeError, double sigmaLongitudeError, double sigmaAltitudeError, double semiMajorDeviationError, double semiMinorDeviationError, double semiMajorOrientationError)
        {
            UniversalTimeCoordinated = universalTimeCoordinated;
            Rms = rms;
            SigmaAltitudeError = sigmaAltitudeError;
            SigmaLongitudeError = sigmaLongitudeError;
            SigmaLatitudeError = sigmaLatitudeError;
            SemiMajorOrientationError = semiMajorOrientationError;
            SemiMinorDeviationError = semiMinorDeviationError;
            SemiMajorDeviationError = semiMajorDeviationError;
        }
    }


}
