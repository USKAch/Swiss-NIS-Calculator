using Avalonia.Controls;
using NIS.Desktop.ViewModels;

namespace NIS.Desktop.Views;

public partial class ResultsView : UserControl
{
    public ResultsView()
    {
        InitializeComponent();

        // Wire up StorageProvider when attached to visual tree
        AttachedToVisualTree += (_, _) =>
        {
            if (DataContext is ResultsViewModel vm && TopLevel.GetTopLevel(this) is { } topLevel)
            {
                vm.StorageProvider = topLevel.StorageProvider;
            }
        };
    }
}
