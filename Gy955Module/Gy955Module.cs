using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CompositeContentNavigator.Services;
using CompositeContentNavigator.Services.MapItems;
using CompositeContentNavigator.Services.MapItems.Data;
using ImuModule.Services;
using ImuModule.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Modularity;
using SharpCommunication.Codec;
using SharpCommunication.GY955.Codec;
using SharpCommunication.Module.Services;
using SharpCommunication.Transport.SerialPort;

namespace ImuModule
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
                                compositeMapNavigatorService.RegisterItem("Device\\IMU\\Diamond",MapItemBuilder
                                    .CreateDefaultBuilder("Diamond")
                                    .WithImagePackIcon(PackIconKind.MapMarker)
                                    .WithToolBars(new[]{ typeof(ImuServiceToolBarView)})
                                    .WithView(typeof(MapView))
                                    .WithExtraView(new Dictionary<string, IEnumerable<Type>>
                                    {
                                        {"PopupToolBarRegion", new[]
                                        {
                                            typeof(CachedChannelView),
                                            typeof(ImuView)
                                        }},
                                        { "ToolsRegion", new[] {typeof(ImuConfigurationView) }}
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
                    .AddSingleton<ImuService>();
            });
        }
    }
}