using System.Windows.Controls;
using GPSModule.ViewModels;
using Prism.Events;
using Prism.Regions;
using SharpCommunication.Codec;
using SharpCommunication.Transport.SerialPort;

namespace GPSModule.Views
{
    /// <summary>
    /// Interaction logic for ToolBarView
    /// </summary>
    public partial class GpsServiceToolBarView : ToolBar, INavigationAware
    {
        private readonly SerialPortDataTransport<Gps> _dataTransport;
        private readonly IEventAggregator _eventAggregator;

        public GpsServiceToolBarView(SerialPortDataTransport<Gps> dataTransport, IEventAggregator eventAggregator)
        {
            _dataTransport = dataTransport;
            _eventAggregator = eventAggregator;
            this.DataContext = new GpsServiceToolbarViewModel(_dataTransport);

            InitializeComponent();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
