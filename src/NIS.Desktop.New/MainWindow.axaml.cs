using System;
using Avalonia.Controls;
using Avalonia.Input;
using FluentAvalonia.UI.Controls;
using NIS.Desktop.ViewModels;

namespace NIS.Desktop;

public partial class MainWindow : Window
{
    private KeyModifiers _lastKeyModifiers = KeyModifiers.None;

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

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        _lastKeyModifiers = e.KeyModifiers;
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        _lastKeyModifiers = e.KeyModifiers;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        _lastKeyModifiers = e.KeyModifiers;
    }

    private void OnNavigationSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (DataContext is MainShellViewModel vm && e.SelectedItem is NavigationViewItem nvi)
        {
            var tag = nvi.Tag?.ToString();
            if (!string.IsNullOrEmpty(tag))
            {
                // Check for Shift key on Master Data navigation (admin mode)
                if (tag == "MasterData")
                {
                    var isShiftPressed = _lastKeyModifiers.HasFlag(KeyModifiers.Shift);
                    vm.NavigateToMasterDataManager(isShiftPressed);
                }
                else
                {
                    vm.NavigateByTag(tag);
                }
            }
        }
    }
}
