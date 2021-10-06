using CompositeContentNavigator.Services;
using CompositeContentNavigator.Services.MapItems;
using CompositeContentNavigator.Services.MapItems.Data;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Modularity;

namespace GpsDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MaterialDesignUnityBootStrap.App
    {
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule<GPSModule.GpsModule>();
            moduleCatalog.AddModule<Gy955Module.Gy955Module>();
        }

    }
}
