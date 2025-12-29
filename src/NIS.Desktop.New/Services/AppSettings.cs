using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Application settings that persist across sessions.
/// Stored next to the executable for portability.
/// </summary>
public class AppSettings
{
    private const int MaxRecentProjects = 5;

    private static readonly string SettingsFolder = Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".";

    private static readonly string SettingsFile = Path.Combine(SettingsFolder, "settings.json");

    public string Language { get; set; } = "de";
    /// <summary>
    /// Theme mode: 0 = System, 1 = Light, 2 = Dark
    /// </summary>
    public int ThemeMode { get; set; } = 0;
    [Obsolete("Use ThemeMode instead")]
    public bool DarkMode { get => ThemeMode == 2; set => ThemeMode = value ? 2 : 0; }
    public List<string> RecentProjects { get; set; } = new();
    public string? LastProjectPath { get; set; }

    /// <summary>
    /// Adds a project file path to the recent projects list.
    /// Moves it to the top if already present, removes oldest if over limit.
    /// </summary>
    public void AddRecentProject(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return;

        // Remove if already in list (will re-add at top)
        RecentProjects.RemoveAll(p => p.Equals(filePath, StringComparison.OrdinalIgnoreCase));

        // Add at beginning
        RecentProjects.Insert(0, filePath);

        // Keep only MaxRecentProjects
        if (RecentProjects.Count > MaxRecentProjects)
        {
            RecentProjects = RecentProjects.Take(MaxRecentProjects).ToList();
        }
    }

    /// <summary>
    /// Gets recent projects that still exist on disk.
    /// </summary>
    public List<string> GetValidRecentProjects()
    {
        return RecentProjects.Where(File.Exists).ToList();
    }

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
    public static string GetLanguageDisplayName(string code) => Localization.Strings.Instance.GetLanguageName(code);
}
