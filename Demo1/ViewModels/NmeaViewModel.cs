using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPSModule.Services;
using Prism.Mvvm;
using Prism.Regions;

namespace GPSModule.ViewModels
{
    public class NmeaViewModel :BindableBase, INavigationAware
    {
        private bool _isLoaded;
        public GpsService GpsService { get; }

        public NmeaViewModel(GpsService gpsService)
        {
            GpsService = gpsService;
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
