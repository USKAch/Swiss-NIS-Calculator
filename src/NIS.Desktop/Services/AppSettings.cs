using System;
using System.IO;
using System.Text.Json;

namespace NIS.Desktop.Services;

/// <summary>
/// Application settings that persist across sessions.
/// Stored in %APPDATA%/SwissNISCalculator/settings.json
/// </summary>
public class AppSettings
{
    private static readonly string SettingsFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SwissNISCalculator");

    private static readonly string SettingsFile = Path.Combine(SettingsFolder, "settings.json");

    public string Language { get; set; } = "de";
    public bool DarkMode { get; set; } = false;

    /// <summary>
    /// Loads settings from disk, or returns defaults if not found.
    /// </summary>
    public static AppSettings Load()
    {
        try
        {
            if (File.Exists(SettingsFile))
            {
                var json = File.ReadAllText(SettingsFile);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
        }
        catch
        {
            // If loading fails, return defaults
        }
        return new AppSettings();
    }

    /// <summary>
    /// Saves settings to disk.
    /// </summary>
    public void Save()
    {
        try
        {
            Directory.CreateDirectory(SettingsFolder);
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFile, json);
        }
        catch
        {
            // Silently fail if we can't save settings
        }
    }

    /// <summary>
    /// Gets the display name for a language code.
    /// </summary>
    public static string GetLanguageDisplayName(string code) => code switch
    {
        "de" => "Deutsch",
        "fr" => "Français",
        "it" => "Italiano",
        "en" => "English",
        _ => code
    };
}
