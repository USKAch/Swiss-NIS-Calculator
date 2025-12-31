using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Localization;
using NIS.Desktop.Services;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the project list view - main entry point of the application.
/// </summary>
public partial class ProjectListViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private ProjectListItem? _selectedProject;

    public ObservableCollection<ProjectListItem> Projects { get; } = new();
    public ObservableCollection<ProjectListItem> FilteredProjects { get; } = new();

    public bool HasProjects => Projects.Count > 0;
    public bool HasFilteredProjects => FilteredProjects.Count > 0;

    public List<string> SortOptions => new()
    {
        Strings.Instance.SortByModified,
        Strings.Instance.SortByName
    };

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _sortOption = Strings.Instance.SortByModified;

    // Navigation callbacks
    public Action<string>? NavigateToNewProject { get; set; }
    public Action<int>? NavigateToEditProject { get; set; }
    public Func<string, string, Task<bool>>? ShowConfirmDialog { get; set; }

    public ProjectListViewModel()
    {
        RefreshProjects();
    }

    public void RefreshProjects()
    {
        Projects.Clear();
        foreach (var project in DatabaseService.Instance.GetProjectList())
        {
            Projects.Add(project);
        }
        ApplyFilter();
        OnPropertyChanged(nameof(HasProjects));
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();
    partial void OnSortOptionChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        FilteredProjects.Clear();
        var search = SearchText.Trim();
        var filtered = string.IsNullOrEmpty(search)
            ? Projects
            : Projects.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));

        var sorted = SortOption == Strings.Instance.SortByName
            ? filtered.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            : filtered.OrderByDescending(p => p.ModifiedAt, StringComparer.OrdinalIgnoreCase);

        foreach (var project in sorted)
        {
            FilteredProjects.Add(project);
        }

        OnPropertyChanged(nameof(HasFilteredProjects));
    }

    [RelayCommand]
    private void AddProject()
    {
        NavigateToNewProject?.Invoke(Strings.Instance.Language);
    }

    [RelayCommand]
    private void EditProject(ProjectListItem? project)
    {
        if (project == null) return;
        NavigateToEditProject?.Invoke(project.Id);
    }

    [RelayCommand]
    private async Task DeleteProject(ProjectListItem? project)
    {
        if (project == null) return;

        // Show confirmation dialog
        if (ShowConfirmDialog != null)
        {
            var message = string.Format(Strings.Instance.DeleteProjectMessage, project.DisplayName);
            var confirmed = await ShowConfirmDialog(Strings.Instance.DeleteProjectConfirm, message);
            if (!confirmed) return;
        }

        try
        {
            DatabaseService.Instance.DeleteProject(project.Id);
            Projects.Remove(project);
            OnPropertyChanged(nameof(HasProjects));
            ApplyFilter();
            StatusMessage = Strings.Instance.ProjectDeleted;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }

    [RelayCommand]
    private void LoadDemoProject()
    {
        var projectId = DatabaseService.Instance.ImportDemoProject();
        if (projectId > 0)
        {
            StatusMessage = Strings.Instance.DemoProjectLoaded;
            RefreshProjects();
        }
        else
        {
            StatusMessage = "Demo project not found";
        }
    }
}
