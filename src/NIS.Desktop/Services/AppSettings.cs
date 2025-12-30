using System.IO;
using System.Text.Json;

namespace NIS.Desktop.Services;

/// <summary>
/// Application settings that persist across sessions.
/// </summary>
public class AppSettings
{
    public string Language { get; set; } = "de";
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
