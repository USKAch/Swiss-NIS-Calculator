using Avalonia.Controls;
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
}
