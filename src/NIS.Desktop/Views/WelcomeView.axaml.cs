using Avalonia.Controls;
using NIS.Desktop.ViewModels;

namespace NIS.Desktop.Views;

public partial class WelcomeView : UserControl
{
    public WelcomeView()
    {
        InitializeComponent();

        // Wire up StorageProvider when attached to visual tree
        AttachedToVisualTree += (_, _) =>
        {
            if (DataContext is WelcomeViewModel vm && TopLevel.GetTopLevel(this) is { } topLevel)
            {
                vm.StorageProvider = topLevel.StorageProvider;
            }
        };
    }
}
