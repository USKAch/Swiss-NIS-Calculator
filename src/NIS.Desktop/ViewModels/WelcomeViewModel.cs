using System;
using System.Collections.Generic;
using System.IO;

using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Localization;
using NIS.Desktop.Services;

namespace NIS.Desktop.ViewModels;


/// <summary>
    /// List of projects in the database for selection.
/// ViewModel for the welcome screen with language selection and project actions.
/// </summary>
public partial class WelcomeViewModel : ViewModelBase
{
    private readonly AppSettings _settings;
    [ObservableProperty]
    private string _selectedLanguage = "de";

    [ObservableProperty]
    private bool _isDarkMode;

    // Storage provider for file dialogs (set by view)
    public IStorageProvider? StorageProvider { get; set; }

    // Project view model for loading projects
    public ProjectViewModel? ProjectViewModel { get; set; }

    /// <summary>
    /// List of projects in the database for selection.
    /// Whether there is a currently loaded project (saved to database or with station info).
    /// </summary>
    public bool HasCurrentProject =>
        ProjectViewModel?.ProjectId > 0 ||
        !string.IsNullOrWhiteSpace(ProjectViewModel?.Project?.Operator) ||
        (!string.IsNullOrWhiteSpace(ProjectViewModel?.Project?.Name) &&
         ProjectViewModel?.Project?.Name != "New Project");

    /// <summary>
    /// List of projects in the database for selection.
    /// Name of the current project for display.
    /// Shows "Operator / ProjectName" if both available.
    /// </summary>
    public string CurrentProjectName
    {
        get
        {
            var operatorName = ProjectViewModel?.Project?.Operator;
            var name = ProjectViewModel?.ProjectName;
            var hasOperator = !string.IsNullOrWhiteSpace(operatorName);
            var hasName = !string.IsNullOrWhiteSpace(name) && name != "New Project";

            if (hasOperator && hasName)
                return $"{operatorName} / {name}";
            if (hasOperator)
                return operatorName!;
            if (hasName)
                return name!;

            return Strings.CurrentProject;
        }
    }


    // Navigation callbacks
    public Action<string>? NavigateToProjectInfo { get; set; }
    public Action? NavigateToProjectOverview { get; set; }
    /// <summary>
    /// List of projects in the database for selection.
    /// Navigate to Master Data. Parameter is isAdminMode (true if Shift+Click).
    /// </summary>
    public Action<bool>? NavigateToMasterData { get; set; }
    /// <summary>
    /// List of projects in the database for selection.
    /// Load project from database by ID and navigate to overview.
    /// </summary>
    public Action<int>? LoadProjectFromDatabase { get; set; }

    // Dialog callback (set by view)
    public Func<string, string, Task<bool>>? ShowConfirmDialog { get; set; }

    public WelcomeViewModel()
    {
        // Load saved settings
        _settings = AppSettings.Load();
        _selectedLanguage = _settings.Language;
        _isDarkMode = _settings.ThemeMode == 2;

        // Apply saved language
        Strings.Instance.Language = _settings.Language;
    }

    /// <summary>
    /// List of projects in the database for selection.
    /// Event raised when dark mode changes.
    /// </summary>
    public event Action<bool>? DarkModeChanged;

    partial void OnIsDarkModeChanged(bool value)
    {
        DarkModeChanged?.Invoke(value);
        // Save and apply theme (1 = Light, 2 = Dark)
        _settings.ThemeMode = value ? 2 : 1;
        _settings.Save();
        SettingsViewModel.ApplyTheme(_settings.ThemeMode);
    }

    public string[] AvailableLanguages => new[] { "de", "fr", "it", "en" };

    /// <summary>
    /// List of projects in the database for selection.
    /// Index-based language selection for ComboBox binding.
    /// </summary>
    public int SelectedLanguageIndex
    {
        get => SelectedLanguage switch
        {
            "de" => 0,
            "fr" => 1,
            "it" => 2,
            "en" => 3,
            _ => 0
        };
        set
        {
            var language = value switch
            {
                0 => "de",
                1 => "fr",
                2 => "it",
                3 => "en",
                _ => "de"
            };
            SelectLanguage(language);
            OnPropertyChanged();
        }
    }

    [RelayCommand]
    private void SelectLanguage(string language)
    {
        if (language == SelectedLanguage)
            return;

        SelectedLanguage = language;
        Strings.Instance.Language = language;
        _settings.Language = language;
        _settings.Save();
        OnPropertyChanged(nameof(SelectedLanguageIndex));
    }

    [RelayCommand]
    private void NewProject()
    {
        NavigateToProjectInfo?.Invoke(SelectedLanguage);
    }

    [RelayCommand]
    private void ContinueProject()
    {
        if (HasCurrentProject)
        {
            NavigateToProjectOverview?.Invoke();
        }
    }

    public string GetLanguageDisplayName(string code) => Localization.Strings.Instance.GetLanguageName(code);

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    /// <summary>
    /// List of projects in the database for selection.
    /// </summary>
    public List<ProjectListItem> ProjectList => DatabaseService.Instance.GetProjectList();

    /// <summary>
    /// List of projects in the database for selection.
    /// Whether there are any projects in the database.
    /// </summary>
    public bool HasProjectsInDatabase => ProjectList.Count > 0;

    [RelayCommand]
    private void LoadDemoProject()
    {
        var projectId = DatabaseService.Instance.ImportDemoProject();
        if (projectId > 0)
        {
            StatusMessage = Strings.Instance.DemoProjectLoaded;
            OnPropertyChanged(nameof(ProjectList));
            OnPropertyChanged(nameof(HasProjectsInDatabase));
        }
        else
        {
            StatusMessage = "Demo project not found";
        }
    }
    [RelayCommand]
    private void OpenProjectFromList(ProjectListItem? project)
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

    [RelayCommand]
    private async Task ExportUserData()
    {
        if (StorageProvider == null)
        {
            StatusMessage = "Export not available";
            return;
        }

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = Strings.Instance.ExportUserData,
            SuggestedFileName = $"NIS_UserData_{DateTime.Now:yyyyMMdd}.json",
            DefaultExtension = ".json",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
            }
        });

        if (file != null)
        {
            try
            {
                DatabaseService.Instance.ExportUserData(file.Path.LocalPath);
                StatusMessage = $"{Strings.Instance.ExportSuccess}: {file.Name}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"{Strings.Instance.ExportFailed}: {ex.Message}";
            }
        }
    }

    [RelayCommand]
    private async Task ImportUserData()
    {
        if (StorageProvider == null)
        {
            StatusMessage = "Import not available";
            return;
        }

        // Confirm before import
        if (ShowConfirmDialog != null)
        {
            var confirmed = await ShowConfirmDialog(
                Strings.Instance.ImportUserData,
                Strings.Instance.ImportConfirmMessage);
            if (!confirmed) return;
        }

        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = Strings.Instance.ImportUserData,
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
            }
        });

        if (files.Count > 0)
        {
            try
            {
                DatabaseService.Instance.ImportUserData(files[0].Path.LocalPath);
                StatusMessage = Strings.Instance.ImportSuccess;

                // Refresh UI
                OnPropertyChanged(nameof(ProjectList));
                OnPropertyChanged(nameof(HasProjectsInDatabase));
            }
            catch (Exception ex)
            {
                StatusMessage = $"{Strings.Instance.ImportFailed}: {ex.Message}";
            }
        }
    }
}
