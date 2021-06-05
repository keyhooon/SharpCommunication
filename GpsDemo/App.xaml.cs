using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using CompositeContentNavigator.Services;
using CompositeContentNavigator.Services.MapItems;
using CompositeContentNavigator.Services.MapItems.Data;
using GPSModule.Services;
using GPSModule.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SharpCommunication.Codec;
using SharpCommunication.Module.Services;
using SharpCommunication.Module.Views;
using SharpCommunication.Transport.SerialPort;
using Unity;

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
        }
    }
}
