using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using NIS.Desktop.Models;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// Manages the current project state and provides project-level operations.
/// Database is the single source of truth - no file-based load/save.
/// </summary>
public partial class ProjectViewModel : ViewModelBase
{
    private bool _suppressOkaSync;
    private int _projectId;

    /// <summary>
    /// Database ID of the current project (0 if not saved to database).
    /// </summary>
    public int ProjectId
    {
        get => _projectId;
        set => _projectId = value;
    }

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

    public void NewProject()
    {
        Project = new Project { Name = "New Project" };
        _projectId = 0;
        OnPropertyChanged(nameof(ProjectName));
        IsDirty = false;
        ReplaceOkas(Project.Okas);
        OnPropertyChanged(nameof(Configurations));
    }

    /// <summary>
    /// Loads a project from a database Project model.
    /// </summary>
    public void LoadFromProject(Project project, int projectId)
    {
        Project = project;
        _projectId = projectId;
        OnPropertyChanged(nameof(ProjectName));
        IsDirty = false;
        ReplaceOkas(Project.Okas);
        OnPropertyChanged(nameof(Configurations));
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

    public void SyncOkasToProject()
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
