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
    public class GpsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

            var compositeMapNavigatorService = containerProvider.Resolve<CompositeMapNavigatorService>();
            compositeMapNavigatorService.RegisterItem("Device", MapItemBuilder
                .CreateDefaultBuilder("Device")
                    .WithImagePackIcon(PackIconKind.Plus)
                    .WithChild(new Collection<MapItem> {
                    compositeMapNavigatorService.RegisterItem("Device\\GPS",MapItemBuilder
                            .CreateDefaultBuilder("GPS")
                            .WithImagePackIcon(PackIconKind.CrosshairsGps)
                            .WithChild(new Collection<MapItem>{
                                compositeMapNavigatorService.RegisterItem("Device\\GPS\\Map",MapItemBuilder
                                    .CreateDefaultBuilder("Map")
                                    .WithImagePackIcon(PackIconKind.MapMarker)
                                    .WithToolBars(new[]{ typeof(GpsServiceToolBarView)})
                                    .WithView(typeof(MapView))
                                    .WithExtraView(new Dictionary<string, IEnumerable<Type>>
                                    {
                                        {"PopupToolBarRegion", new[]
                                        {
                                            typeof(CachedChannelView),
                                            typeof(NmeaView)
                                        }},
                                        { "ToolsRegion", new[] {typeof(GpsConfigurationView) }}
                                    })),
                            })
                    )
                })
            );
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
          
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppConfig.json", optional: true, true).Build();

            containerRegistry.RegisterServices(collection =>
            {
                collection
                    .AddSerialPortTransport(new Codec<Gps>(Gps.Encoding.CreateBuilder().Build()), SerialPortDataTransportSettings.Default)
                    .AddSingleton<GpsService>();
            });
        }
    }
}
