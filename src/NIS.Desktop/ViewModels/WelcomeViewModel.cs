using System;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Localization;
using NIS.Desktop.Services;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the welcome screen with language selection and project actions.
/// </summary>
public partial class WelcomeViewModel : ViewModelBase
{
    private readonly AppSettings _settings;
    private string _previousLanguage;

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
    /// Uses callsign if available, otherwise project name.
    /// </summary>
    public string CurrentProjectName
    {
        get
        {
            var callsign = ProjectViewModel?.Project?.Station?.Callsign;
            if (!string.IsNullOrWhiteSpace(callsign))
                return callsign;

            var name = ProjectViewModel?.ProjectName;
            if (!string.IsNullOrWhiteSpace(name) && name != "New Project")
                return name;

            return Strings.CurrentProject;
        }
    }

    // Navigation callbacks
    public Action<string>? NavigateToProjectInfo { get; set; }
    public Action? NavigateToProjectOverview { get; set; }
    public Action? NavigateToMasterData { get; set; }

    // Dialog callback (set by view)
    public Func<string, string, Task<bool>>? ShowConfirmDialog { get; set; }

    public WelcomeViewModel()
    {
        // Load saved settings
        _settings = AppSettings.Load();
        _selectedLanguage = _settings.Language;
        _previousLanguage = _settings.Language;
        _isDarkMode = _settings.DarkMode;

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
        // Save dark mode preference immediately
        _settings.DarkMode = value;
        _settings.Save();
    }

    public string[] AvailableLanguages => new[] { "de", "fr", "it", "en" };

    [RelayCommand]
    private async Task SelectLanguage(string language)
    {
        if (language == _previousLanguage)
            return;

        // Apply language immediately (preview)
        SelectedLanguage = language;
        Strings.Instance.Language = language;

        // Ask user if they want to save
        var langName = AppSettings.GetLanguageDisplayName(language);
        var title = Strings.Instance.Language switch
        {
            "de" => "Sprache speichern?",
            "fr" => "Enregistrer la langue?",
            "it" => "Salvare la lingua?",
            _ => "Save language?"
        };
        var message = Strings.Instance.Language switch
        {
            "de" => $"Möchten Sie {langName} als Standardsprache speichern?",
            "fr" => $"Voulez-vous enregistrer {langName} comme langue par défaut?",
            "it" => $"Vuoi salvare {langName} come lingua predefinita?",
            _ => $"Do you want to save {langName} as your default language?"
        };

        if (ShowConfirmDialog != null)
        {
            var confirmed = await ShowConfirmDialog(title, message);
            if (confirmed)
            {
                // Save the new language permanently
                _settings.Language = language;
                _settings.Save();
                _previousLanguage = language;
            }
            // If not confirmed, keep language for this session only (don't revert)
            // Next app start will use the previously saved language
        }
        else
        {
            // No dialog available, just save
            _settings.Language = language;
            _settings.Save();
            _previousLanguage = language;
        }
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
            var success = await ProjectViewModel.LoadProjectAsync(filePath);
            if (success)
            {
                NavigateToProjectOverview?.Invoke();
            }
        }
    }

    public string GetLanguageDisplayName(string code) => code switch
    {
        "de" => "Deutsch",
        "fr" => "Français",
        "it" => "Italiano",
        "en" => "English",
        _ => code
    };
}
