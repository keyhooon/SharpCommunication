using System.Collections.Specialized;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace SharpCommunication.Module.Behaviors
{
    class AutoScrollEndBehavior: Behavior<ListBox>
    {


    protected override void OnAttached()
    {

            ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged += (sender, e) =>
            {
                AssociatedObject.SelectedIndex = AssociatedObject.Items.Count - 1;
                AssociatedObject.ScrollIntoView(AssociatedObject.SelectedItem);
            };
    }

    
    }
}
