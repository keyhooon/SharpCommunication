using System;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Gy955Module.Services;
using Prism.Mvvm;
using Prism.Regions;

namespace Gy955Module.ViewModels
{
    public class ImuViewModel : BindableBase, INavigationAware
    {
        private bool _isLoaded;
        private DispatcherTimer timer;
        public Vector3D Euler { get; private set; }

        private ImuService _imuService { get; }

        public ImuViewModel(ImuService imuService)
        {
            _imuService = imuService;
            timer = new DispatcherTimer(DispatcherPriority.Background, Application.Current.Dispatcher)
            {
                Interval = TimeSpan.FromSeconds(1)
            };

        }

        private void ImuService_IsOpenChanged(object sender, EventArgs e)
        {
            if (_imuService.IsOpen)
                timer.Start();
            else
                timer.Stop();
        }

        private void ImuService_DataReceived(object sender, EventArgs e) => Euler = new Vector3D(_imuService.Yrp?.X ?? 0, _imuService.Yrp?.Y ?? 0, _imuService.Yrp?.Z ?? 0);

        private void Timer_Tick(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(Euler));
        }

        public bool IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }
     
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Euler = new Vector3D(_imuService.Yrp?.X ?? 0, _imuService.Yrp?.Y ?? 0, _imuService.Yrp?.Z ?? 0);
            timer.Tick += Timer_Tick;
            _imuService.DataReceived += ImuService_DataReceived;
            _imuService.IsOpenChanged += ImuService_IsOpenChanged;
            IsLoaded = true;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            timer.Tick -= Timer_Tick;
            _imuService.DataReceived -= ImuService_DataReceived;
            _imuService.IsOpenChanged -= ImuService_IsOpenChanged;
            IsLoaded = false;
        }
    }
}
