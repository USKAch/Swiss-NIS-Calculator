using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using NIS.Desktop.ViewModels;

namespace NIS.Desktop.Views;

public partial class ProjectOverviewView : UserControl
{
    public ProjectOverviewView()
    {
        InitializeComponent();

        // Wire up StorageProvider when attached to visual tree
        AttachedToVisualTree += (_, _) =>
        {
            if (DataContext is ProjectOverviewViewModel vm && TopLevel.GetTopLevel(this) is { } topLevel)
            {
                vm.StorageProvider = topLevel.StorageProvider;
            }
        };
    }

    private void OnConfigurationDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is ProjectOverviewViewModel vm)
        {
            // Get the item from the event source to handle case where selection hasn't updated yet
            var item = GetDataContextFromSource<ConfigurationDisplayItem>(e.Source);
            if (item != null)
            {
                vm.EditConfigurationCommand.Execute(item);
            }
        }
    }

    private static T? GetDataContextFromSource<T>(object? source) where T : class
    {
        if (source is Control control)
        {
            var current = control;
            while (current != null)
            {
                if (current.DataContext is T item)
                    return item;
                current = current.GetVisualParent() as Control;
            }
        }
        return null;
    }
}
