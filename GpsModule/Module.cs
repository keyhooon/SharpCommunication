using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using CompositeContentNavigator.Services;
using CompositeContentNavigator.Services.MapItems;
using CompositeContentNavigator.Services.MapItems.Data;
using GPSModule.Services;
using GPSModule.Views;
using MapControl;
using MapControl.Caching;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Modularity;
using SharpCommunication.Codec;
using SharpCommunication.Module.Services;
using SharpCommunication.Transport.SerialPort;
using MapItem = CompositeContentNavigator.Services.MapItems.Data.MapItem;

namespace GPSModule
{
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

            var compositeMapNavigatorService = containerProvider.Resolve<CompositeMapNavigatorService>();
            if (!compositeMapNavigatorService.TryGetItemByName("Config", out _))
                compositeMapNavigatorService.RegisterItem("Config", MapItemBuilder
                    .CreateDefaultBuilder("Config")
                    .WithImagePackIcon(PackIconKind.BoxView));
            compositeMapNavigatorService.RegisterItem("Config\\GPS", MapItemBuilder
                    .CreateDefaultBuilder("GPS")
                    .WithImagePackIcon(PackIconKind.CrosshairsGps)
                    .WithChild(new Collection<MapItem>
                    {
                        compositeMapNavigatorService.RegisterItem("Config\\GPS\\Map", MapItemBuilder
                            .CreateDefaultBuilder("Map")
                            .WithImagePackIcon(PackIconKind.MapMarker)
                            .WithToolBars(new[] {typeof(GpsServiceToolBarView)})
                            .WithView(typeof(MapView))
                            .WithExtraView(new Dictionary<string, IEnumerable<Type>>
                            {
                                {
                                    "PopupToolBarRegion", new[]
                                    {
                                        typeof(CachedChannelView),
                                        typeof(NmeaView)
                                    }
                                },
                                {"ToolsRegion", new[] {typeof(GpsConfigurationView)}}
                            })),
                    }), "Config"
            );

            ImageLoader.HttpClient.DefaultRequestHeaders.Add("User-Agent", "XAML Map Control Test Application");
            var defaultCacheFolder = TileImageLoader.DefaultCacheFolder;
            TileImageLoader.Cache = new ImageFileCache(defaultCacheFolder);
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterServices(collection =>
            {
                collection
                    .AddSerialPortTransport(new Codec<Gps>(Gps.Encoding.CreateBuilder().Build()), SerialPortDataTransportSettings.Default)
                    .AddSingleton<GpsService>();
            });
        }
    }
}
