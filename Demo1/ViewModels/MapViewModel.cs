using GPSModule.Services;
using MapControl;
using MaterialDesignUnityBootStrap.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prism.Mvvm;
using Prism.Regions;

namespace GPSModule.ViewModels
{
    public class MapViewModel : BindableBase, INavigationAware
    {
        public MapViewModel(GpsService gpsService)
        {
            gpsService.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(gpsService.Gll))
                {
                    Location = new Location(gpsService.Gll.Latitude, gpsService.Gll.Longitude);
                }
            };
        }

        private Location _location;
        private bool _isLoaded;

        public Location Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        public bool IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }

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
