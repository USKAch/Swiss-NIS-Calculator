using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Localization;
using NIS.Desktop.Services;

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

    /// <summary>
    /// Load project from database by ID and navigate to overview.
    /// </summary>
    public Action<int>? LoadProjectFromDatabase { get; set; }

    // Dialog callback (set by view)
    public Func<string, string, Task<bool>>? ShowConfirmDialog { get; set; }

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    /// <summary>
    /// List of projects in the database for selection.
    /// </summary>
    public List<ProjectListItem> ProjectList => DatabaseService.Instance.GetProjectList();

    /// <summary>
    /// Whether there are any projects in the database.
    /// </summary>
    public bool HasProjectsInDatabase => ProjectList.Count > 0;

    [RelayCommand]
    private void NewProject()
    {
        NavigateToProjectInfo?.Invoke(Strings.Instance.Language);
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
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }
}
