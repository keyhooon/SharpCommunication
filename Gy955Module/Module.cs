using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CompositeContentNavigator.Services;
using CompositeContentNavigator.Services.MapItems;
using CompositeContentNavigator.Services.MapItems.Data;
using Gy955Module.Services;
using Gy955Module.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Modularity;
using SharpCommunication.Codec;
using SharpCommunication.Module.Services;
using SharpCommunication.Transport.SerialPort;

namespace Gy955Module
{
    public class Module : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

            var compositeMapNavigatorService = containerProvider.Resolve<CompositeMapNavigatorService>();
            if (!compositeMapNavigatorService.TryGetItemByName("Config", out var _))
                compositeMapNavigatorService.RegisterItem("Config", MapItemBuilder
                    .CreateDefaultBuilder("Config")
                    .WithImagePackIcon(PackIconKind.BoxView));

            compositeMapNavigatorService.RegisterItem("Config\\IMU", MapItemBuilder
                    .CreateDefaultBuilder("IMU")
                    .WithImagePackIcon(PackIconKind.Compass)
                    .WithChild(new Collection<MapItem>
                    {
                        compositeMapNavigatorService.RegisterItem("Config\\IMU\\Diamond", MapItemBuilder
                            .CreateDefaultBuilder("Diamond")
                            .WithImagePackIcon(PackIconKind.Diamond)
                            .WithToolBars(new[] {typeof(ImuServiceToolBarView)})
                            .WithView(typeof(DiamondView))
                            .WithExtraView(new Dictionary<string, IEnumerable<Type>>
                            {
                                {
                                    "PopupToolBarRegion", new[]
                                    {
                                        typeof(CachedChannelView),
                                        typeof(ImuView),
                                    }
                                },
                                {"ToolsRegion", new[] {typeof(ImuConfigurationView)}}
                            })
                        ),
                    })
                , "Config");


        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterServices(collection =>
            {
                var codec = new Codec<Gy955>(new Gy955.Encoding());
                collection
                    .AddSerialPortTransport(codec, SerialPortDataTransportSettings.Default)
                    .AddSingleton<ImuService>();
            });
        }
    }
}