﻿using System.Windows.Controls;
using Gy955Module.ViewModels;
using Prism.Events;
using Prism.Regions;
using SharpCommunication.Codec;
using SharpCommunication.Transport.SerialPort;

namespace Gy955Module.Views
{
    /// <summary>
    /// Interaction logic for ToolBarView
    /// </summary>
    public partial class ImuServiceToolBarView : ToolBar, INavigationAware
    {
        private readonly SerialPortDataTransport<Gy955> _dataTransport;
        private readonly IEventAggregator _eventAggregator;

        public ImuServiceToolBarView(SerialPortDataTransport<Gy955> dataTransport, IEventAggregator eventAggregator)
        {
            _dataTransport = dataTransport;
            _eventAggregator = eventAggregator;
            DataContext = new ImuServiceToolbarViewModel(_dataTransport);

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
