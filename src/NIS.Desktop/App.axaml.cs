using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
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

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
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
