using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace SharpCommunication.Module.Behaviors
{
    public class FillAvailableComPortBehavior : Behavior<ComboBox>
    {


        protected override void OnAttached()
        {
            AssociatedObject.ItemsSource = System.IO.Ports.SerialPort.GetPortNames();
            AssociatedObject.DropDownOpened += (sender, e) =>
            {
                var selectedItem = AssociatedObject.SelectedItem;
                AssociatedObject.ItemsSource = System.IO.Ports.SerialPort.GetPortNames();
                if (selectedItem != null)
                    AssociatedObject.SelectedItem = selectedItem;
            };
            AssociatedObject.Loaded += (sender, args) => AssociatedObject.ItemsSource = System.IO.Ports.SerialPort.GetPortNames();
        }

    }
}
