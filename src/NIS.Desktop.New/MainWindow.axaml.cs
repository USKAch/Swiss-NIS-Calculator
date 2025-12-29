using System;
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

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        // Set StorageProvider for file dialogs
        if (DataContext is MainShellViewModel vm)
        {
            vm.StorageProvider = StorageProvider;
        }
    }

    private void OnNavigationSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (DataContext is MainShellViewModel vm && e.SelectedItem is NavigationViewItem nvi)
        {
            var tag = nvi.Tag?.ToString();
            if (!string.IsNullOrEmpty(tag))
            {
                vm.NavigateByTag(tag);
            }
        }
    }
}
