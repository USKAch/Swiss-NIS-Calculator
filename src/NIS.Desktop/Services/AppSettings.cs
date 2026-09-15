using System.IO;
using System.Text.Json;

namespace NIS.Desktop.Services;

/// <summary>
/// Application settings that persist across sessions.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// UI language: "de", "fr", "it", "en" or "system" (follow the OS UI language).
    /// Existing settings files without the value default to German (previous behaviour).
    /// </summary>
    public string Language { get; set; } = "de";

    public const string SystemLanguage = "system";

    /// <summary>
    /// Resolves the effective UI language code ("de"/"fr"/"it"/"en"), mapping
    /// "system" to the OS UI culture with German as fallback.
    /// </summary>
    public string ResolveLanguage() => ResolveLanguage(Language);

    public static string ResolveLanguage(string? language)
    {
        if (!string.Equals(language, SystemLanguage, System.StringComparison.OrdinalIgnoreCase))
            return string.IsNullOrWhiteSpace(language) ? "de" : language;

        var os = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        return os switch
        {
            "de" or "fr" or "it" or "en" => os,
            _ => "de"
        };
    }
    /// <summary>
    /// Theme mode: 0 = System, 1 = Light, 2 = Dark
    /// </summary>
    public int ThemeMode { get; set; } = 0;


    /// <summary>
    /// Loads settings from disk, or returns defaults if not found.
    /// </summary>
    public static AppSettings Load()
    {
        try
        {
            if (File.Exists(AppPaths.SettingsFile))
            {
                var json = File.ReadAllText(AppPaths.SettingsFile);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
        }
        catch
        {
            // If loading fails, return defaults
        }
        return new AppSettings();
    }

    public void Save()
    {
        try
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(AppPaths.SettingsFile, json);
        }
        catch
        {
            // Silently fail if we can't save settings
        }
    }

    /// <summary>
    /// Gets the display name for a language code.
    /// </summary>
    public static string GetLanguageDisplayName(string code) => Localization.Strings.Instance.GetLanguageName(code);
}
