using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using NIS.Core.Models;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Manages project lifecycle: create, load, save.
/// OKAs are stored within the project (no separate storage).
/// </summary>
public partial class ProjectService : ObservableObject, IProjectService
{
    private readonly ISessionService _session;
    private readonly ISettingsService _settings;

    private static readonly JsonSerializerOptions ReadOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly JsonSerializerOptions WriteOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ProjectService(ISessionService session, ISettingsService settings)
    {
        _session = session;
        _settings = settings;

        // Forward session property changes
        _session.PropertyChanged += (_, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(ISessionService.CurrentProject):
                    OnPropertyChanged(nameof(CurrentProject));
                    OnPropertyChanged(nameof(HasProject));
                    ProjectChanged?.Invoke();
                    break;
                case nameof(ISessionService.ProjectFilePath):
                    OnPropertyChanged(nameof(FilePath));
                    break;
                case nameof(ISessionService.IsDirty):
                    OnPropertyChanged(nameof(IsDirty));
                    DirtyChanged?.Invoke(_session.IsDirty);
                    break;
            }
        };
    }

    public Project? CurrentProject => _session.CurrentProject;
    public string? FilePath => _session.ProjectFilePath;
    public bool IsDirty => _session.IsDirty;
    public bool HasProject => _session.HasProject;

    public event Action? ProjectChanged;
    public event Action<bool>? DirtyChanged;

    public void NewProject(string language = "de")
    {
        _session.CurrentProject = new Project
        {
            Language = language,
            Name = "New Project",
            Station = new StationInfo()
        };
        _session.ProjectFilePath = null;
        _session.IsDirty = false;
    }

    public async Task<bool> LoadAsync(string filePath)
    {
        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            var project = JsonSerializer.Deserialize<Project>(json, ReadOptions);

            if (project == null)
            {
                return false;
            }

            // Migration: If project has no OKAs but legacy OKA file exists, offer to import
            if (project.Okas.Count == 0)
            {
                await TryMigrateLegacyOkasAsync(project);
            }

            // Set project name from filename if not set
            if (string.IsNullOrWhiteSpace(project.Name))
            {
                project.Name = Path.GetFileNameWithoutExtension(filePath);
            }

            _session.CurrentProject = project;
            _session.ProjectFilePath = filePath;
            _session.IsDirty = false;

            // Add to recent projects
            _settings.AddRecentProject(filePath);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> SaveAsync(string? filePath = null)
    {
        filePath ??= _session.ProjectFilePath;
        if (string.IsNullOrEmpty(filePath) || _session.CurrentProject == null)
        {
            return false;
        }

        try
        {
            var json = JsonSerializer.Serialize(_session.CurrentProject, WriteOptions);
            await File.WriteAllTextAsync(filePath, json);

            _session.ProjectFilePath = filePath;
            _session.IsDirty = false;

            // Add to recent projects
            _settings.AddRecentProject(filePath);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void MarkDirty()
    {
        _session.IsDirty = true;
    }

    public void ClearDirty()
    {
        _session.IsDirty = false;
    }

    public void CloseProject()
    {
        _session.CurrentProject = null;
        _session.ProjectFilePath = null;
        _session.IsDirty = false;
    }

    /// <summary>
    /// Attempts to migrate OKAs from the legacy OkaStorageService location.
    /// This is a one-time migration path for existing users.
    /// </summary>
    private async Task TryMigrateLegacyOkasAsync(Project project)
    {
        try
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var legacyOkaFile = Path.Combine(appData, "SwissNISCalculator", "okas.json");

            if (!File.Exists(legacyOkaFile))
            {
                return;
            }

            var json = await File.ReadAllTextAsync(legacyOkaFile);
            var data = JsonSerializer.Deserialize<LegacyOkaData>(json, ReadOptions);

            if (data?.Okas != null && data.Okas.Count > 0)
            {
                // Import legacy OKAs into project
                foreach (var oka in data.Okas)
                {
                    project.Okas.Add(oka);
                }

                // Note: We don't delete the legacy file - user might have multiple projects
                // The migration happens per-project on first load
            }
        }
        catch
        {
            // Silently fail migration - not critical
        }
    }

    private class LegacyOkaData
    {
        public System.Collections.Generic.List<Oka>? Okas { get; set; }
    }
}
