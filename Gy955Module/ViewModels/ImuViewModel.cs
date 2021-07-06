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

        public Vector3D Euler { get; private set; }

        public ImuService ImuService { get; }

        public ImuViewModel(ImuService imuService)
        {
            ImuService = imuService;
            var timer = new DispatcherTimer(DispatcherPriority.Background, Application.Current.Dispatcher)
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (_, _) =>
            {
                RaisePropertyChanged(nameof(Euler));
            };

            imuService.PropertyChanged += (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(global::Gy955Module.Services.ImuService.Yrp):
                        Euler = new Vector3D(ImuService.Yrp?.X ?? 0, ImuService.Yrp?.Y ?? 0, ImuService.Yrp?.Z ?? 0);
                        break;
                }
            };
            imuService.IsOpenChanged += (sender, args) =>
            {
                if (imuService.IsOpen)
                    timer.Start();
                else
                    timer.Stop();
            };

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
            IsLoaded = false;
        }
    }
}
