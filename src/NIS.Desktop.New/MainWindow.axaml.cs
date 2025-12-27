using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using NIS.Desktop.New.ViewModels;

namespace NIS.Desktop.New;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnNavigationSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm && e.SelectedItem is NavigationViewItem nvi)
        {
            var tag = nvi.Tag?.ToString();
            if (!string.IsNullOrEmpty(tag))
            {
                vm.NavigateTo(tag);
            }
        }
    }
}
