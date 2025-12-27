using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.New.Services;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// ViewModel for the Home view.
/// Handles new project, open project, and recent projects.
/// </summary>
public partial class HomeViewModel : ViewModelBase
{
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialog;
    private readonly IProjectService _project;
    private readonly ISettingsService _settings;

    public HomeViewModel(
        INavigationService navigation,
        IDialogService dialog,
        IProjectService project,
        ISettingsService settings,
        ILocalizationService localization)
    {
        Loc = localization;
        _navigation = navigation;
        _dialog = dialog;
        _project = project;
        _settings = settings;

        SubscribeToLanguageChanges();
    }

    public IReadOnlyList<string> RecentProjects => _settings.RecentProjects;

    [RelayCommand]
    private void NewProject()
    {
        _project.NewProject();
        _navigation.NavigateTo<ProjectViewModel>();
    }

    [RelayCommand]
    private async Task OpenProject()
    {
        var filePath = await _dialog.ShowOpenFileDialogAsync(
            L("Dialog.Open"),
            new FileFilter(L("Dialog.ProjectFilter"), "*.nisproj"),
            new FileFilter("All Files", "*.*"));

        if (!string.IsNullOrEmpty(filePath))
        {
            await OpenProjectFile(filePath);
        }
    }

    [RelayCommand]
    private async Task OpenRecentProject(string filePath)
    {
        await OpenProjectFile(filePath);
    }

    private async Task OpenProjectFile(string filePath)
    {
        if (await _project.LoadAsync(filePath))
        {
            _navigation.NavigateTo<ProjectViewModel>();
        }
        else
        {
            await _dialog.ShowErrorAsync(
                L("Dialog.Error"),
                L("Project.Status.Error").Replace("{0}", filePath));

            // Remove from recent if file doesn't exist or can't be loaded
            _settings.RemoveRecentProject(filePath);
        }
    }
}
