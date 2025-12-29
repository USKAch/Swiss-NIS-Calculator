using Avalonia.Controls;
using Avalonia.Input;
using NIS.Desktop.New.ViewModels;

namespace NIS.Desktop.New.Views;

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
        if (sender is DataGrid grid && grid.SelectedItem is ConfigurationDisplayItem item
            && DataContext is ProjectOverviewViewModel vm)
        {
            vm.EditConfigurationCommand.Execute(item);
        }
    }
}
