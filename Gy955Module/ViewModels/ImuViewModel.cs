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

        private void ImuService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(global::Gy955Module.Services.ImuService.Yrp):
                    Euler = new Vector3D(_imuService.Yrp?.X ?? 0, _imuService.Yrp?.Y ?? 0, _imuService.Yrp?.Z ?? 0);
                    break;
            }
        }

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
            _imuService.PropertyChanged += ImuService_PropertyChanged;
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
            _imuService.PropertyChanged -= ImuService_PropertyChanged;
            _imuService.IsOpenChanged -= ImuService_IsOpenChanged;
            IsLoaded = false;
        }
    }
}
