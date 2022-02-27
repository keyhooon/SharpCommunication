using System;
using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Input;
using MapControl;

namespace GPSModule.Infrastructure.XamlMap
{
    /// <summary>
    /// Interaction logic for map.xaml
    /// </summary>
    public partial class Map 
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource", typeof(IEnumerable),
                typeof(Map), new FrameworkPropertyMetadata(null
                    , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(
                "ItemTemplate", typeof(DataTemplate),
                typeof(Map), new FrameworkPropertyMetadata(null
                    , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }


        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem", typeof(object),
                typeof(Map), new FrameworkPropertyMetadata(null
                    , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty CurrentLocationProperty =
            DependencyProperty.Register(
                "CurrentLocation", typeof(Location),
                typeof(Map), new FrameworkPropertyMetadata(new Location(0,0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Location CurrentLocation
        {
            get => (Location)GetValue(CurrentLocationProperty);
            set => SetValue(CurrentLocationProperty, value);
        }


        public Map()
        {
            //MapLayer = "{x:Static map:MapTileLayer.OpenStreetMapTileLayer}"
            MapLayer = MapTileLayer.OpenStreetMapTileLayer;
            InitializeComponent();

            TileImageLoader.Cache = new MapControl.Caching.ImageFileCache($"{Directory.GetCurrentDirectory()}\\Map");
            TileImageLoader.MaxCacheExpiration = TimeSpan.MaxValue;
            InitializeComponent();
        }



        private void MapMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ItemsControl.SelectedItem = null;
            if (e.ClickCount == 2)
            {
                TargetCenter = ViewToLocation(e.GetPosition(this));
            }
        }

        private void MapMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ZoomMap(e.GetPosition(this), Math.Ceiling(ZoomLevel - 1.5));
            }
        }

        private void MapMouseMove(object sender, MouseEventArgs e)
        {
            var location = ViewToLocation(e.GetPosition(this));

            MouseLocation.Text = location.GetPrettyString();
        }

        private void MapMouseLeave(object sender, MouseEventArgs e)
        {
            MouseLocation.Text = string.Empty;
        }

        private void MapManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            e.TranslationBehavior.DesiredDeceleration = .001;

        }

        private void CurrentLocationViewButton_OnClick(object sender, RoutedEventArgs e)
        {
            Center = CurrentLocation;
        }
    }
}