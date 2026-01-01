using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using FluentAvalonia.UI.Controls;
using NIS.Desktop.Localization;
using NIS.Desktop.ViewModels;

namespace NIS.Desktop;

public partial class MainWindow : Window
{
    private const string FactoryPassword = "HB9BLA";
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

    private async void OnNavigationSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (DataContext is MainShellViewModel vm && e.SelectedItem is NavigationViewItem nvi)
        {
            var tag = nvi.Tag?.ToString();
            if (!string.IsNullOrEmpty(tag))
            {
                // Factory mode requires password authentication
                if (tag == "Factory")
                {
                    var isAuthenticated = await ShowFactoryPasswordDialogAsync();
                    if (isAuthenticated)
                    {
                        vm.NavigateToMasterDataManager(true);
                    }
                    // Clear selection so Factory can be clicked again
                    if (sender is NavigationView navView)
                    {
                        navView.SelectedItem = null;
                    }
                }
                else
                {
                    vm.NavigateByTag(tag);
                }

                // Clear selection for actions so they can be clicked repeatedly
                if (tag is "Home" or "OpenProject")
                {
                    if (sender is NavigationView navView)
                    {
                        navView.SelectedItem = null;
                    }
                }
            }
        }
    }

    private async Task<bool> ShowFactoryPasswordDialogAsync()
    {
        var passwordBox = new TextBox
        {
            PasswordChar = '•',
            Watermark = Strings.Instance.EnterFactoryPassword,
            Width = 250
        };

        var dialog = new ContentDialog
        {
            Title = Strings.Instance.FactoryMode,
            Content = new StackPanel
            {
                Spacing = 10,
                Children =
                {
                    new TextBlock { Text = Strings.Instance.EnterFactoryPassword },
                    passwordBox
                }
            },
            PrimaryButtonText = "OK",
            CloseButtonText = Strings.Instance.Cancel,
            DefaultButton = ContentDialogButton.Primary
        };

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            if (passwordBox.Text == FactoryPassword)
            {
                return true;
            }

            // Wrong password
            var errorDialog = new ContentDialog
            {
                Title = Strings.Instance.FactoryMode,
                Content = Strings.Instance.WrongPassword,
                CloseButtonText = "OK"
            };
            await errorDialog.ShowAsync();
        }

        return false;
    }
}
