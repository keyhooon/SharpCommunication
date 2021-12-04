using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GPSModule.Infrastructure.PolarPlacementControl
{
    public class PolarPlacementItem : ContentControl
    {
        public PolarPlacementItem()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
        }
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var az = (Azimuth - 90) / 180 * Math.PI;
            var e = (90 - Elevation) / 90;
            var x = Math.Cos(az) * e;
            var y = Math.Sin(az) * e;
            x = arrangeBounds.Width * .5 * x;
            y = arrangeBounds.Height * .5 * y;
            RenderTransform = new TranslateTransform(x, y);
            return base.ArrangeOverride(arrangeBounds);
        }

        public double Azimuth
        {
            get => (double)GetValue(AzimuthProperty);
            set => SetValue(AzimuthProperty, value);
        }

        public static readonly DependencyProperty AzimuthProperty =
            DependencyProperty.Register("Azimuth", typeof(double), typeof(PolarPlacementItem), new PropertyMetadata(0d, OnAzimuthPropertyChanged));

        private static void OnAzimuthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PolarPlacementItem)?.InvalidateArrange();
        }

        public double Elevation
        {
            get => (double)GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }

        public static readonly DependencyProperty ElevationProperty =
            DependencyProperty.Register("Elevation", typeof(double), typeof(PolarPlacementItem), new PropertyMetadata(0d, OnElevationPropertyChanged));

        private static void OnElevationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PolarPlacementItem)?.InvalidateArrange();
        }

    }
}
