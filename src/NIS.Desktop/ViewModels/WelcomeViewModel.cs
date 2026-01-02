using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Localization;
using NIS.Desktop.Services;
using NIS.Desktop.Services.Repositories;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the welcome screen with project actions.
/// </summary>
public partial class WelcomeViewModel : ViewModelBase
{
    // Storage provider for file dialogs (set by view)
    public IStorageProvider? StorageProvider { get; set; }

    // Project view model for loading projects
    public ProjectViewModel? ProjectViewModel { get; set; }

    // Navigation callbacks
    public Action<string>? NavigateToProjectInfo { get; set; }
    public Action? NavigateToProjectOverview { get; set; }
    public Action<bool>? NavigateToMasterData { get; set; }

    /// <summary>
    /// Load project from database by ID and navigate to overview.
    /// </summary>
    public Action<int>? LoadProjectFromDatabase { get; set; }

    // Dialog callback (set by view)
    public new Func<string, string, Task<bool>>? ShowConfirmDialog { get; set; }

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    /// <summary>
    /// List of projects in the database for selection.
    /// </summary>
    public List<ProjectListItem> ProjectList => DatabaseService.Instance.GetProjectList();
    public ObservableCollection<ProjectListItem> FilteredProjects { get; } = new();

    /// <summary>
    /// Whether there are any projects in the database.
    /// </summary>
    public bool HasProjectsInDatabase => ProjectList.Count > 0;

    /// <summary>
    /// Whether there are any filtered projects to display.
    /// </summary>
    public bool HasFilteredProjects => FilteredProjects.Count > 0;

    /// <summary>
    /// Whether to show the "no search results" message.
    /// </summary>
    public bool ShowNoSearchResults => HasProjectsInDatabase && !HasFilteredProjects && !string.IsNullOrEmpty(SearchText);

    public List<string> SortOptions => new()
    {
        Strings.Instance.SortByModified,
        Strings.Instance.SortByName
    };

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _sortOption = Strings.Instance.SortByModified;

    public WelcomeViewModel()
    {
        ApplyFilter();
    }

    public void RefreshProjects()
    {
        OnPropertyChanged(nameof(ProjectList));
        OnPropertyChanged(nameof(HasProjectsInDatabase));
        ApplyFilter();
    }

    [RelayCommand]
    private void NewProject()
    {
        NavigateToProjectInfo?.Invoke(Strings.Instance.Language);
    }

    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = string.Empty;
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();
    partial void OnSortOptionChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        FilteredProjects.Clear();
        var search = SearchText.Trim();
        var filtered = string.IsNullOrEmpty(search)
            ? ProjectList
            : ProjectList.FindAll(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));

        var sorted = SortOption == Strings.Instance.SortByName
            ? filtered.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            : filtered.OrderByDescending(p => p.ModifiedAt, StringComparer.OrdinalIgnoreCase);

        foreach (var project in sorted)
        {
            FilteredProjects.Add(project);
        }
        OnPropertyChanged(nameof(HasFilteredProjects));
        OnPropertyChanged(nameof(ShowNoSearchResults));
    }

    [RelayCommand]
    private void EditProjectFromList(ProjectListItem? project)
    {
        if (project == null) return;
        LoadProjectFromDatabase?.Invoke(project.Id);
    }

    [RelayCommand]
    private async Task DeleteProjectFromList(ProjectListItem? project)
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
            StatusMessage = Strings.Instance.ProjectDeleted;

            // Refresh project list
            OnPropertyChanged(nameof(ProjectList));
            OnPropertyChanged(nameof(HasProjectsInDatabase));
            ApplyFilter();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }
}
