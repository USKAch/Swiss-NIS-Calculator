using System;
using System.ComponentModel;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Service for application localization.
/// Provides translated strings based on current language.
/// </summary>
public interface ILocalizationService : INotifyPropertyChanged
{
    /// <summary>
    /// The current language code (de, en, fr, it).
    /// Setting this will reload strings and raise LanguageChanged.
    /// </summary>
    string CurrentLanguage { get; set; }

    /// <summary>
    /// Gets a localized string by key.
    /// </summary>
    /// <param name="key">The string key (e.g., "Home.Welcome")</param>
    /// <returns>The translated string, or the key if not found</returns>
    string this[string key] { get; }

    /// <summary>
    /// Gets a localized string with format arguments.
    /// </summary>
    /// <param name="key">The string key</param>
    /// <param name="args">Format arguments</param>
    /// <returns>The formatted translated string</returns>
    string GetString(string key, params object[] args);

    /// <summary>
    /// Raised when the language changes.
    /// Views should refresh their bindings when this fires.
    /// </summary>
    event Action? LanguageChanged;

    /// <summary>
    /// Gets all available language codes.
    /// </summary>
    string[] AvailableLanguages { get; }
}
