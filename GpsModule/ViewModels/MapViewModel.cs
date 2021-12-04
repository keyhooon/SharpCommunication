using GPSModule.Services;
using MapControl;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows;

namespace GPSModule.ViewModels
{
    public class MapViewModel : BindableBase, INavigationAware

    {
    public MapViewModel(GpsService gpsService)
    {
        gpsService.PositionChanged += (_, _) => Application.Current.Dispatcher.InvokeAsync(() => CurrentLocation = new Location(gpsService.Position.Latitude, gpsService.Position.Longitude));
    }

    private Location _location;
    private bool _isLoaded;

    public Location CurrentLocation
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
