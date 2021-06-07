using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CompositeContentNavigator.Services;
using CompositeContentNavigator.Services.MapItems;
using CompositeContentNavigator.Services.MapItems.Data;
using Gy955Module.Views;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SharpCommunication.Codec;
using SharpCommunication.GY955.Codec;
using SharpCommunication.Module.Services;
using SharpCommunication.Transport.SerialPort;

namespace Gy955Module
{
    public class Gy955Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

            var compositeMapNavigatorService = containerProvider.Resolve<CompositeMapNavigatorService>();
            compositeMapNavigatorService.RegisterItem("Device", MapItemBuilder
                .CreateDefaultBuilder("Device")
                    .WithImagePackIcon(PackIconKind.Plus)
                    .WithChild(new Collection<MapItem> {
                    compositeMapNavigatorService.RegisterItem("Device\\IMU",MapItemBuilder
                            .CreateDefaultBuilder("IMU")
                            .WithImagePackIcon(PackIconKind.Compass)
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
            containerRegistry.RegisterServices(collection =>
            {
                collection
                    .AddSerialPortTransport(new Codec<Gy955>(new Gy955.Encoding()), SerialPortDataTransportSettings.Default)
                    .AddSingleton<GpsService>();
            });
        }
    }
}