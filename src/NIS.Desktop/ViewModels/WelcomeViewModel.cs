using System;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Localization;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the welcome screen with language selection and project actions.
/// </summary>
public partial class WelcomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _selectedLanguage = "de";

    [ObservableProperty]
    private bool _isDarkMode;

    // Storage provider for file dialogs (set by view)
    public IStorageProvider? StorageProvider { get; set; }

    // Project view model for loading projects
    public ProjectViewModel? ProjectViewModel { get; set; }

    // Navigation callbacks
    public Action<string>? NavigateToProjectInfo { get; set; }
    public Action? NavigateToProjectOverview { get; set; }
    public Action? NavigateToMasterData { get; set; }

    /// <summary>
    /// Event raised when dark mode changes.
    /// </summary>
    public event Action<bool>? DarkModeChanged;

    partial void OnIsDarkModeChanged(bool value)
    {
        DarkModeChanged?.Invoke(value);
    }

    public string[] AvailableLanguages => new[] { "de", "fr", "it", "en" };

    [RelayCommand]
    private void SelectLanguage(string language)
    {
        SelectedLanguage = language;
        Strings.Instance.Language = language;
    }

    [RelayCommand]
    private void NewProject()
    {
        NavigateToProjectInfo?.Invoke(SelectedLanguage);
    }

    [RelayCommand]
    private void MasterData()
    {
        NavigateToMasterData?.Invoke();
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
