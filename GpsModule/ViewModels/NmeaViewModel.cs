using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using GPSModule.Services;
using GPSModule.Services.Models;
using Prism.Mvvm;
using Prism.Regions;

namespace GPSModule.ViewModels
{
    public class NmeaViewModel : BindableBase, INavigationAware
    {
        private bool _isLoaded;
        private List<SatelliteVehicleInView> sVs;

        public GpsService GpsService { get; }

        public NmeaViewModel(GpsService gpsService)
        {
            GpsService = gpsService;
            GpsService.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(GpsService.GpsSVs) || e.PropertyName == nameof(GpsService.GlonassSVs))
                    SVs = GpsService.GpsSVs.Concat(GpsService.GlonassSVs).ToList();
            };
            var timer = new DispatcherTimer(DispatcherPriority.Background, Application.Current.Dispatcher)
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, e) =>
            {
                    RaisePropertyChanged(nameof(SVs));
                    RaisePropertyChanged(nameof(FixDateTime));
                    RaisePropertyChanged(nameof(Position));
                    RaisePropertyChanged(nameof(Dop));
            };

            gpsService.IsOpenChanged += (_, _) =>
            {
                timer.IsEnabled = gpsService.IsOpen;
            };
        }
        public bool IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }
        public TimeSpan FixDateTime => GpsService.Position?.UniversalTimeCoordinated??TimeSpan.Zero;

        public GpsData GpsData => GpsService.GpsData;

        public GnssData GnssData => GpsService.GnssData;

        public GeographicPosition Position => GpsService.Position;

        public Dop Dop => GpsService.Dop;

        public PseudorangeErrorStatics Error => GpsService.Error;
        public List<SatelliteVehicleInView> SVs { get; private set; }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsLoaded = true;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
