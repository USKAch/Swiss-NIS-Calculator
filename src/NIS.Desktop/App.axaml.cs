using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Threading;
using NIS.Desktop.ViewModels;
using NIS.Desktop.Views;

namespace NIS.Desktop;

public partial class App : Application
{
    private MainShellViewModel? _mainShellViewModel;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Load any custom translations from previous sessions
            TranslationEditorViewModel.LoadCustomTranslations();

            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            DisableAvaloniaDataAnnotationValidation();

            _mainShellViewModel = new MainShellViewModel();

            // Subscribe to dark mode changes from WelcomeViewModel
            _mainShellViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainShellViewModel.CurrentView) &&
                    _mainShellViewModel.CurrentView is WelcomeViewModel welcomeVm)
                {
                    welcomeVm.DarkModeChanged += isDark =>
                    {
                        RequestedThemeVariant = isDark ? ThemeVariant.Dark : ThemeVariant.Light;
                    };
                }
            };

            var mainWindow = new MainShellWindow
            {
                DataContext = _mainShellViewModel
            };

            // Add global handler for NumericUpDown to select all on focus
            mainWindow.AddHandler(InputElement.GotFocusEvent, OnNumericUpDownGotFocus, handledEventsToo: true);

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void OnNumericUpDownGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (e.Source is TextBox textBox && textBox.Parent?.Parent is NumericUpDown)
        {
            // Select all text when NumericUpDown's inner TextBox gets focus
            Dispatcher.UIThread.Post(() => textBox.SelectAll());
        }
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
