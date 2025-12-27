using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Persists application settings to disk.
/// </summary>
public partial class SettingsService : ObservableObject, ISettingsService
{
    private static readonly string SettingsFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SwissNISCalculator");

    private static readonly string SettingsFile = Path.Combine(SettingsFolder, "settings.json");
    private const int MaxRecentProjects = 10;

    [ObservableProperty]
    private string _language = "de";

    [ObservableProperty]
    private int _themeIndex = 0; // 0=System, 1=Light, 2=Dark

    private List<string> _recentProjects = new();
    public IReadOnlyList<string> RecentProjects => _recentProjects;

    public SettingsService()
    {
        Load();
    }

    public void AddRecentProject(string filePath)
    {
        // Remove if already exists (will re-add at top)
        _recentProjects.Remove(filePath);

        // Add to top
        _recentProjects.Insert(0, filePath);

        // Trim to max
        while (_recentProjects.Count > MaxRecentProjects)
        {
            _recentProjects.RemoveAt(_recentProjects.Count - 1);
        }

        OnPropertyChanged(nameof(RecentProjects));
        Save();
    }

    public void RemoveRecentProject(string filePath)
    {
        if (_recentProjects.Remove(filePath))
        {
            OnPropertyChanged(nameof(RecentProjects));
            Save();
        }
    }

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(SettingsFolder);

            var data = new SettingsData
            {
                Language = Language,
                ThemeIndex = ThemeIndex,
                RecentProjects = _recentProjects.ToList()
            };

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFile, json);
        }
        catch
        {
            // Silently fail if we can't save settings
        }
    }

    public void Load()
    {
        try
        {
            if (File.Exists(SettingsFile))
            {
                var json = File.ReadAllText(SettingsFile);
                var data = JsonSerializer.Deserialize<SettingsData>(json);

                if (data != null)
                {
                    Language = data.Language ?? "de";
                    ThemeIndex = data.ThemeIndex;
                    _recentProjects = data.RecentProjects ?? new List<string>();

                    // Clean up non-existent files
                    _recentProjects = _recentProjects.Where(File.Exists).ToList();
                }
            }
        }
        catch
        {
            // Use defaults if loading fails
            Language = "de";
            ThemeIndex = 0;
            _recentProjects = new List<string>();
        }
    }

    partial void OnLanguageChanged(string value) => Save();
    partial void OnThemeIndexChanged(int value) => Save();

    private class SettingsData
    {
        public string? Language { get; set; }
        public int ThemeIndex { get; set; }
        public List<string>? RecentProjects { get; set; }
    }
}
