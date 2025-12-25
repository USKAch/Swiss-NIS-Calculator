using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Core.Models;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// Manages the current project state and provides project-level operations.
/// </summary>
public partial class ProjectViewModel : ViewModelBase
{
    private string? _projectFilePath;

    public string? ProjectFilePath => _projectFilePath;

    [ObservableProperty]
    private Project _project = new();

    [ObservableProperty]
    private bool _isDirty;

    [ObservableProperty]
    private string _projectName = "Untitled Project";

    [ObservableProperty]
    private AntennaConfiguration? _selectedConfiguration;

    public ObservableCollection<AntennaConfiguration> Configurations =>
        new(Project.AntennaConfigurations);

    public string[] AvailableLanguages => new[] { "de", "fr", "it", "en" };

    public string LanguageDisplayName => Project.Language switch
    {
        "de" => "Deutsch",
        "fr" => "Français",
        "it" => "Italiano",
        "en" => "English",
        _ => "Deutsch"
    };

    public void NewProject(string language)
    {
        Project = new Project { Language = language };
        _projectFilePath = null;
        ProjectName = "New Project";
        IsDirty = false;
        OnPropertyChanged(nameof(Configurations));
    }

    public async Task<bool> LoadProjectAsync(string filePath)
    {
        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var project = JsonSerializer.Deserialize<Project>(json, options);

            if (project != null)
            {
                Project = project;
                _projectFilePath = filePath;
                ProjectName = Path.GetFileNameWithoutExtension(filePath);
                IsDirty = false;
                OnPropertyChanged(nameof(Configurations));
                return true;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading project: {ex.Message}");
        }
        return false;
    }

    public async Task<bool> SaveProjectAsync(string? filePath = null)
    {
        filePath ??= _projectFilePath;
        if (string.IsNullOrEmpty(filePath)) return false;

        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(Project, options);
            await File.WriteAllTextAsync(filePath, json);

            _projectFilePath = filePath;
            ProjectName = Path.GetFileNameWithoutExtension(filePath);
            IsDirty = false;
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving project: {ex.Message}");
        }
        return false;
    }

    public void AddConfiguration(AntennaConfiguration config)
    {
        Project.AntennaConfigurations.Add(config);
        IsDirty = true;
        OnPropertyChanged(nameof(Configurations));
    }

    public void RemoveConfiguration(AntennaConfiguration config)
    {
        Project.AntennaConfigurations.Remove(config);
        IsDirty = true;
        OnPropertyChanged(nameof(Configurations));
    }

    public void MarkDirty()
    {
        IsDirty = true;
    }

    public void UpdateStationInfo(StationInfo station)
    {
        Project.Station = station;
        IsDirty = true;
        OnPropertyChanged(nameof(Project));
    }
}
