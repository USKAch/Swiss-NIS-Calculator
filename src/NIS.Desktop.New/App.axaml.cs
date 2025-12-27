using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using NIS.Desktop.New.Services;
using NIS.Desktop.New.ViewModels;
using NIS.Desktop.New.Views;

namespace NIS.Desktop.New;

public partial class App : Application
{
    /// <summary>
    /// Global service provider for dependency injection.
    /// </summary>
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Build the DI container
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Initialize services that need early setup
            var settingsService = Services.GetRequiredService<ISettingsService>();
            var themeService = Services.GetRequiredService<IThemeService>();
            var localizationService = Services.GetRequiredService<ILocalizationService>();

            // Apply saved theme
            themeService.ApplyTheme(settingsService.ThemeIndex switch
            {
                1 => Avalonia.Styling.ThemeVariant.Light,
                2 => Avalonia.Styling.ThemeVariant.Dark,
                _ => Avalonia.Styling.ThemeVariant.Default
            });

            // Set saved language
            localizationService.CurrentLanguage = settingsService.Language;

            // Create main window with DI
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Core services (singletons - shared state)
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<ILocalizationService, LocalizationService>();
        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IProjectService, ProjectService>();

        // ViewModels
        // MainWindowViewModel is singleton (one instance for app lifetime)
        services.AddSingleton<MainWindowViewModel>();

        // Other ViewModels are transient (created fresh, navigation service may cache)
        services.AddTransient<HomeViewModel>();
        services.AddTransient<ProjectViewModel>();
        services.AddTransient<MasterDataViewModel>();
        services.AddTransient<SettingsViewModel>();

        // Views (transient)
        services.AddTransient<HomeView>();
        services.AddTransient<ProjectView>();
        services.AddTransient<MasterDataView>();
        services.AddTransient<SettingsView>();
    }
}
