using System.Linq;
using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentAvalonia.Styling;
using NIS.Desktop.Localization;
using NIS.Desktop.Services;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the Settings view.
/// </summary>
public partial class SettingsViewModel : ViewModelBase
{
    private readonly AppSettings _settings;

    [ObservableProperty]
    private int _themeIndex;

    [ObservableProperty]
    private int _languageIndex;

    public SettingsViewModel()
    {
        _settings = AppSettings.Load();

        ThemeIndex = _settings.ThemeMode;
        LanguageIndex = _settings.Language switch
        {
            "de" => 0,
            "en" => 1,
            "fr" => 2,
            "it" => 3,
            _ => 0
        };

        Strings.Instance.Language = _settings.Language;
    }

    partial void OnThemeIndexChanged(int value)
    {
        _settings.ThemeMode = value;
        _settings.Save();

        ApplyTheme(value);
    }

    public static void ApplyTheme(int themeMode)
    {
        if (Application.Current == null) return;

        // Get the FluentAvaloniaTheme to control PreferSystemTheme
        var faTheme = Application.Current.Styles.OfType<FluentAvaloniaTheme>().FirstOrDefault();

        if (themeMode == 0)
        {
            // System theme - enable PreferSystemTheme
            if (faTheme != null) faTheme.PreferSystemTheme = true;
            Application.Current.RequestedThemeVariant = ThemeVariant.Default;
        }
        else
        {
            // Manual selection - disable PreferSystemTheme
            if (faTheme != null) faTheme.PreferSystemTheme = false;
            Application.Current.RequestedThemeVariant = themeMode == 2 ? ThemeVariant.Dark : ThemeVariant.Light;
        }
    }

    partial void OnLanguageIndexChanged(int value)
    {
        var language = value switch
        {
            0 => "de",
            1 => "en",
            2 => "fr",
            3 => "it",
            _ => "de"
        };

        _settings.Language = language;
        _settings.Save();
        Strings.Instance.Language = language;
    }
}
