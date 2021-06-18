using ImuModule;
using Prism.Modularity;

namespace GPSModule
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MaterialDesignUnityBootStrap.App
    {
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<GpsModule>();
            moduleCatalog.AddModule<Gy955Module>();
        }
    }
}
