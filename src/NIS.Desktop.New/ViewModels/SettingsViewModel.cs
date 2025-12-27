using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using NIS.Desktop.New.Services;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// ViewModel for the Settings view.
/// Handles theme and language preferences.
/// </summary>
public partial class SettingsViewModel : ViewModelBase
{
    private readonly ISettingsService _settings;
    private readonly IThemeService _theme;
    private readonly ILocalizationService _localization;

    [ObservableProperty]
    private int _themeIndex;

    [ObservableProperty]
    private int _languageIndex;

    public SettingsViewModel(
        ISettingsService settings,
        IThemeService theme,
        ILocalizationService localization)
    {
        Loc = localization;
        _settings = settings;
        _theme = theme;
        _localization = localization;

        // Load current settings
        ThemeIndex = _settings.ThemeIndex;
        LanguageIndex = GetLanguageIndex(_settings.Language);

        SubscribeToLanguageChanges();
    }

    partial void OnThemeIndexChanged(int value)
    {
        _settings.ThemeIndex = value;

        var theme = value switch
        {
            1 => ThemeVariant.Light,
            2 => ThemeVariant.Dark,
            _ => ThemeVariant.Default
        };

        _theme.ApplyTheme(theme);
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
        _localization.CurrentLanguage = language;
    }

    private static int GetLanguageIndex(string language) => language switch
    {
        "de" => 0,
        "en" => 1,
        "fr" => 2,
        "it" => 3,
        _ => 0
    };
}
