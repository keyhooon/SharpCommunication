using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using Gy955Module.Services;
using HelixToolkit.Wpf;
using Prism.Mvvm;
using Prism.Regions;

namespace Gy955Module.ViewModels
{
    public class DiamondViewModel : BindableBase, INavigationAware
    {
        private readonly ImuService _imuService;
        private bool _isLoaded;
		private Model3D _modelGroup;
        public Model3D ModelGroup { get => _modelGroup; set => SetProperty(ref _modelGroup, value); }
        private readonly IRegionManager _regionManager;
        private Quaternion _quaternion;
        public Quaternion Quaternion { get => _quaternion; set => SetProperty(ref _quaternion, value); }

        private QuaternionRotation3D _quaternionRotation;
        public QuaternionRotation3D QuaternionRotation { get => _quaternionRotation; init => SetProperty(ref _quaternionRotation, value); }

        public DiamondViewModel(ImuService imuService)
        {
            _imuService = imuService;
            QuaternionRotation = new QuaternionRotation3D();
            _imuService.DataReceived += (sender, args) =>
            {
                Quaternion = new Quaternion(imuService.Q4?.W ?? 0, imuService.Q4?.X ?? 0, imuService.Q4?.Y ?? 0,
                    imuService.Q4?.Z ?? 0);
                Application.Current.Dispatcher?.Invoke(() =>
                    QuaternionRotation.BeginAnimation(QuaternionRotation3D.QuaternionProperty,
                        new QuaternionAnimation(Quaternion, new Duration(TimeSpan.FromMilliseconds(100)))));
            };
            Task.Run(() =>
            {
                var importer = new ModelImporter();
                Material material = new DiffuseMaterial(new SolidColorBrush(Colors.Beige));
                importer.DefaultMaterial = material;
                var model3DGroup = importer.Load(@"Assets\Model.3DS");
                model3DGroup.Freeze();
                return model3DGroup;
            }).ContinueWith((result) =>
            {
                ModelGroup = result.Result;
                if (result.IsCompleted)
                {
                    Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        IsLoaded = true;
                    });
                }
            });
        }

        public bool IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {

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
