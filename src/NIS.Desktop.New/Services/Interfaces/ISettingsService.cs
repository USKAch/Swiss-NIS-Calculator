using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Styling;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Service for application settings persistence.
/// </summary>
public interface ISettingsService : INotifyPropertyChanged
{
    /// <summary>
    /// The current UI language code.
    /// </summary>
    string Language { get; set; }

    /// <summary>
    /// The current theme setting.
    /// 0 = System, 1 = Light, 2 = Dark
    /// </summary>
    int ThemeIndex { get; set; }

    /// <summary>
    /// List of recently opened project file paths.
    /// </summary>
    IReadOnlyList<string> RecentProjects { get; }

    /// <summary>
    /// Adds a project to the recent projects list.
    /// </summary>
    /// <param name="filePath">Path to the project file</param>
    void AddRecentProject(string filePath);

    /// <summary>
    /// Removes a project from the recent projects list.
    /// </summary>
    /// <param name="filePath">Path to remove</param>
    void RemoveRecentProject(string filePath);

    /// <summary>
    /// Saves settings to disk.
    /// </summary>
    void Save();

    /// <summary>
    /// Reloads settings from disk.
    /// </summary>
    void Load();
}
