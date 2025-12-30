using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
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
    private bool _suppressOkaSync;

    public string? ProjectFilePath => _projectFilePath;

    [ObservableProperty]
    private Project _project = new();

    [ObservableProperty]
    private bool _isDirty;

    public string ProjectName
    {
        get => Project.Name;
        set
        {
            if (Project.Name != value)
            {
                Project.Name = value;
                OnPropertyChanged();
                MarkDirty();
            }
        }
    }

    [ObservableProperty]
    private AntennaConfiguration? _selectedConfiguration;

    public ObservableCollection<Oka> Okas { get; } = new();

    public ObservableCollection<AntennaConfiguration> Configurations =>
        new(Project.AntennaConfigurations);

    public string[] AvailableLanguages => new[] { "de", "fr", "it", "en" };

    public string LanguageDisplayName => Localization.Strings.Instance.GetLanguageName(Project.Language);

    public void NewProject(string language)
    {
        Project = new Project { Language = language, Name = "New Project" };
        _projectFilePath = null;
        OnPropertyChanged(nameof(ProjectName));
        IsDirty = false;
        ReplaceOkas(Project.Okas);
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
                // If project has no name, use filename as fallback
                if (string.IsNullOrWhiteSpace(Project.Name))
                {
                    Project.Name = Path.GetFileNameWithoutExtension(filePath);
                }
                OnPropertyChanged(nameof(ProjectName));
                IsDirty = false;
                ReplaceOkas(Project.Okas);
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
            SyncOkasToProject();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(Project, options);
            await File.WriteAllTextAsync(filePath, json);

            _projectFilePath = filePath;
            OnPropertyChanged(nameof(ProjectName));
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

    public int NextOkaId => Okas.Count > 0 ? Okas.Max(o => o.Id) + 1 : 1;

    public void AddOrUpdateOka(Oka oka)
    {
        var existing = Okas.FirstOrDefault(o => o.Id == oka.Id);
        if (existing != null)
        {
            var index = Okas.IndexOf(existing);
            Okas[index] = oka;
        }
        else
        {
            if (oka.Id <= 0)
            {
                oka.Id = NextOkaId;
            }
            Okas.Add(oka);
        }
        MarkDirty();
    }

    public void RemoveOka(Oka oka)
    {
        if (Okas.Remove(oka))
        {
            MarkDirty();
        }
    }

    public ProjectViewModel()
    {
        Okas.CollectionChanged += OnOkasChanged;
    }

    private void ReplaceOkas(IEnumerable<Oka> okas)
    {
        _suppressOkaSync = true;
        Okas.Clear();
        foreach (var oka in okas)
        {
            Okas.Add(oka);
        }
        _suppressOkaSync = false;
    }

    private void SyncOkasToProject()
    {
        Project.Okas = Okas.ToList();
    }

    private void OnOkasChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_suppressOkaSync)
        {
            return;
        }
        SyncOkasToProject();
        IsDirty = true;
    }
}
