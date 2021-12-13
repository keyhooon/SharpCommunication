using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            if (!compositeMapNavigatorService.TryGetItemByName("Device", out var _))
                compositeMapNavigatorService.RegisterItem("Device", MapItemBuilder
                    .CreateDefaultBuilder("Device")
                    .WithImagePackIcon(PackIconKind.BoxView));
            compositeMapNavigatorService.RegisterItem("Device\\GPS", MapItemBuilder
                    .CreateDefaultBuilder("GPS")
                    .WithImagePackIcon(PackIconKind.CrosshairsGps)
                    .WithChild(new Collection<MapItem>
                    {
                        compositeMapNavigatorService.RegisterItem("Device\\GPS\\Map", MapItemBuilder
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
                    }), "Device"
            );

            ImageLoader.HttpClient.DefaultRequestHeaders.Add("User-Agent", "XAML Map Control Test Application");
            
            var defaultCacheFolder = TileImageLoader.DefaultCacheFolder;
            TileImageLoader.Cache = new SQLiteCache(defaultCacheFolder);
            
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
