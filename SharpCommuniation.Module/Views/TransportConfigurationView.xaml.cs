using System.Windows;
using System.Windows.Controls;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Module.ViewModels;
using SharpCommunication.Transport;

namespace SharpCommunication.Module.Views
{
    /// <summary>
    /// Interaction logic for SerialPortConfigView.xaml
    /// </summary>
    public partial class TransportConfigurationView<T> : UserControl where T : IPacket,new()
    {
        public static readonly DependencyProperty SerialPortProperty =
        DependencyProperty.Register(
        "DataTransport", typeof(DataTransport<T>),
        typeof(DataTransport<T>), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,(sender, e)=>
        {
            if ((DataTransport<T>)e.NewValue != null)
                ((TransportConfigurationView<T>)sender).DataContext = new TransportConfigurationViewModel<T>((DataTransport<T>)e.NewValue);
        }));

        public DataTransport<T> Transport
        {
            get => (DataTransport<T>)GetValue(SerialPortProperty);
            set => SetValue(SerialPortProperty, value);
        }
    }
}
