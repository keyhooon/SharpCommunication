using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GPSModule.Services;
using GPSModule.Services.Models;
using Prism.Mvvm;
using Prism.Regions;
using SharpCommunication.Codec;

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
                {
                    sVs = GpsService.GpsSVs.Concat(GpsService.GlonassSVs).ToList();
                    RaisePropertyChanged(nameof(SVs));
                }              
            };
        }
        public bool IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }
        public DateTime FixDateTime => GpsService.FixDateTime;

        public GpsData GpsData => GpsService.GpsData;

        public GnssData GnssData => GpsService.GnssData;

        public GeographicPosition Position => GpsService.Position;

        public Dop Dop => GpsService.Dop;

        public PseudorangeErrorStatics Error => GpsService.Error;
        public List<SatelliteVehicleInView> SVs => sVs;

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
