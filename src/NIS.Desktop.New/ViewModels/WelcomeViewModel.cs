using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.New.Localization;
using NIS.Desktop.New.Services;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// Represents a recent project for display in the UI.
/// </summary>
public class RecentProjectItem
{
    public string FilePath { get; set; } = string.Empty;
    public string DisplayName => Path.GetFileNameWithoutExtension(FilePath);
    public string FolderPath => Path.GetDirectoryName(FilePath) ?? string.Empty;
}

/// <summary>
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
    /// Whether there's a currently loaded project (opened from file or created with station info).
    /// </summary>
    public bool HasCurrentProject =>
        ProjectViewModel?.ProjectFilePath != null ||
        !string.IsNullOrWhiteSpace(ProjectViewModel?.Project?.Station?.Callsign);

    /// <summary>
    /// Name of the current project for display.
    /// Shows "Callsign / ProjectName" if both available.
    /// </summary>
    public string CurrentProjectName
    {
        get
        {
            var callsign = ProjectViewModel?.Project?.Station?.Callsign;
            var name = ProjectViewModel?.ProjectName;
            var hasCallsign = !string.IsNullOrWhiteSpace(callsign);
            var hasName = !string.IsNullOrWhiteSpace(name) && name != "New Project";

            if (hasCallsign && hasName)
                return $"{callsign} / {name}";
            if (hasCallsign)
                return callsign!;
            if (hasName)
                return name!;

            return Strings.CurrentProject;
        }
    }

    /// <summary>
    /// List of recent projects for display.
    /// </summary>
    public List<RecentProjectItem> RecentProjects => _settings.GetValidRecentProjects()
        .Select(p => new RecentProjectItem { FilePath = p })
        .ToList();

    /// <summary>
    /// Whether there are any recent projects to display.
    /// </summary>
    public bool HasRecentProjects => RecentProjects.Count > 0;

    // Navigation callbacks
    public Action<string>? NavigateToProjectInfo { get; set; }
    public Action? NavigateToProjectOverview { get; set; }
    /// <summary>
    /// Navigate to Master Data. Parameter is isAdminMode (true if Shift+Click).
    /// </summary>
    public Action<bool>? NavigateToMasterData { get; set; }

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

    [RelayCommand]
    private async Task OpenProject()
    {
        if (StorageProvider == null || ProjectViewModel == null)
        {
            return;
        }

        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Project",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("NIS Project") { Patterns = new[] { "*.nisproj" } },
                new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
            }
        });

        if (files.Count > 0)
        {
            var filePath = files[0].Path.LocalPath;
            await OpenProjectFromPath(filePath);
        }
    }

    [RelayCommand]
    private async Task OpenRecentProject(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || ProjectViewModel == null)
            return;

        await OpenProjectFromPath(filePath);
    }

    private async Task OpenProjectFromPath(string filePath, bool navigate = true)
    {
        if (ProjectViewModel == null) return;

        var success = await ProjectViewModel.LoadProjectAsync(filePath);
        if (success)
        {
            // Add to recent projects and save as last project
            _settings.AddRecentProject(filePath);
            _settings.LastProjectPath = filePath;
            _settings.Save();
            OnPropertyChanged(nameof(RecentProjects));
            OnPropertyChanged(nameof(HasRecentProjects));
            OnPropertyChanged(nameof(HasCurrentProject));
            OnPropertyChanged(nameof(CurrentProjectName));

            if (navigate)
            {
                NavigateToProjectOverview?.Invoke();
            }
        }
    }

    /// <summary>
    /// Tries to load the last project at startup.
    /// </summary>
    public async Task TryLoadLastProjectAsync()
    {
        if (string.IsNullOrWhiteSpace(_settings.LastProjectPath)) return;
        if (!File.Exists(_settings.LastProjectPath)) return;
        if (ProjectViewModel == null) return;

        await OpenProjectFromPath(_settings.LastProjectPath, navigate: false);
    }

    public string GetLanguageDisplayName(string code) => Localization.Strings.Instance.GetLanguageName(code);
}
