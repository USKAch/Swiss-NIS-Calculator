using Avalonia.Controls;
using NIS.Desktop.New.ViewModels;

namespace NIS.Desktop.New.Views;

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
