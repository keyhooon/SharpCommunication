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
using Prism.Regions;
using SharpCommunication.Codec;
using SharpCommunication.Module.Services;
using SharpCommunication.Module.Views;
using Unity;

namespace GPSModule
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MaterialDesignUnityBootStrap.App
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppConfig.json", optional: true, true).Build();
            containerRegistry.RegisterServices(collection =>
            {
                collection
                    .AddSerialPortTransport<Gps>(new Gps.Encoding(), configurationRoot.GetSection("GpsConfig"))
                    .AddSingleton<GpsService>();
            });
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            


            var compositeMapNavigatorService = Container.Resolve<CompositeMapNavigatorService>();
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
                                    .WithToolBars(new[]{ typeof(ServiceToolBar1View)})
                                    .WithView(typeof(MapView))
                                    .WithExtraView(new Dictionary<string, IEnumerable<Type>>
                                    {
                                        {"PopupToolBarRegion", new[] {typeof(CachedChannelView) }}, 
                                        { "ToolsRegion", new[] {typeof(TransportConfigView) }}
                                    })),
                                compositeMapNavigatorService.RegisterItem("Device\\GPS\\Message",MapItemBuilder
                                    .CreateDefaultBuilder("Message")
                                    .WithImagePackIcon(PackIconKind.MessageBookmark)
                                    .WithToolBars(new[]{ typeof(ServiceToolBar1View)})
                                    .WithView(typeof(NmeaView))
                                    .WithExtraView(new Dictionary<string, IEnumerable<Type>>
                                    {
                                        {"PopupToolBarRegion", new[] {typeof(CachedChannelView) }},
                                        { "ToolsRegion", new[] {typeof(TransportConfigView) }}
                                    })),
                            })
                    )
                })
            );
          
        }
    }
}
