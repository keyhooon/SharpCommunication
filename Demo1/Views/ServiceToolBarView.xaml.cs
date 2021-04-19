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
    public partial class ServiceToolBar1View : ToolBar, INavigationAware
    {
        private readonly SerialPortDataTransport<Gps> _dataTransport;
        private readonly IEventAggregator _eventAggregator;

        public ServiceToolBar1View(SerialPortDataTransport<Gps> dataTransport, IEventAggregator eventAggregator)
        {
            _dataTransport = dataTransport;
            _eventAggregator = eventAggregator;
            this.DataContext = new ServiceToolbar1ViewModel(_dataTransport, _eventAggregator);

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
