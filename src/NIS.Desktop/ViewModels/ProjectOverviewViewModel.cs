using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Core.Models;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the Project Overview screen.
/// </summary>
public partial class ProjectOverviewViewModel : ViewModelBase
{
    private readonly ProjectViewModel _projectViewModel;

    // Storage provider for file dialogs (set by view)
    public IStorageProvider? StorageProvider { get; set; }

    // Navigation callbacks
    public Action<AntennaConfiguration?>? NavigateToConfigurationEditor { get; set; }
    public Action? NavigateToResults { get; set; }
    public Action? NavigateToProjectInfo { get; set; }

    public ProjectOverviewViewModel() : this(new ProjectViewModel())
    {
    }

    public ProjectOverviewViewModel(ProjectViewModel projectViewModel)
    {
        _projectViewModel = projectViewModel;
        _projectViewModel.PropertyChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(ProjectName));
            OnPropertyChanged(nameof(DirtyIndicator));
            OnPropertyChanged(nameof(Callsign));
            OnPropertyChanged(nameof(OperatorName));
            OnPropertyChanged(nameof(Address));
            OnPropertyChanged(nameof(Location));
            OnPropertyChanged(nameof(StationSummary));
            OnPropertyChanged(nameof(Configurations));
            OnPropertyChanged(nameof(HasConfigurations));
            OnPropertyChanged(nameof(CanCalculate));
            OnPropertyChanged(nameof(LanguageDisplay));
        };
    }

    // Project info
    public string ProjectName
    {
        get => _projectViewModel.ProjectName;
        set => _projectViewModel.ProjectName = value;
    }
    public string DirtyIndicator => _projectViewModel.IsDirty ? "*" : "";
    public string Callsign => _projectViewModel.Project.Station.Callsign;
    public string OperatorName => _projectViewModel.Project.Station.Operator;
    public string Address => _projectViewModel.Project.Station.Address;
    public string Location => _projectViewModel.Project.Station.Location;
    public string StationSummary => string.IsNullOrEmpty(Callsign) ? "No station info" : Callsign;

    // Language
    public string LanguageDisplay => _projectViewModel.Project.Language switch
    {
        "de" => "Deutsch",
        "fr" => "Français",
        "it" => "Italiano",
        "en" => "English",
        _ => _projectViewModel.Project.Language
    };

    // Collections
    public ObservableCollection<AntennaConfiguration> Configurations => _projectViewModel.Configurations;

    // State
    public bool HasConfigurations => Configurations.Count > 0;
    public bool CanCalculate => HasConfigurations && Configurations.Any(c => c.OkaDistanceMeters > 0);

    [ObservableProperty]
    private bool _hasResults;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    // Save commands
    [RelayCommand]
    private async Task SaveProject()
    {
        if (string.IsNullOrEmpty(_projectViewModel.ProjectFilePath))
        {
            await SaveProjectAs();
            return;
        }

        var success = await _projectViewModel.SaveProjectAsync();
        StatusMessage = success ? "Project saved" : "Could not save project. Please check file permissions.";
    }

    [RelayCommand]
    private async Task SaveProjectAs()
    {
        if (StorageProvider == null) return;

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Project",
            SuggestedFileName = $"{ProjectName}.nisproj",
            DefaultExtension = ".nisproj",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("NIS Project") { Patterns = new[] { "*.nisproj" } }
            }
        });

        if (file != null)
        {
            var success = await _projectViewModel.SaveProjectAsync(file.Path.LocalPath);
            StatusMessage = success ? $"Project saved to {file.Name}" : "Could not save project. Please check file permissions.";
        }
    }

    [RelayCommand]
    private void Export()
    {
        StatusMessage = "Export not implemented yet";
    }

    // Station info commands
    [RelayCommand]
    private void EditStationInfo()
    {
        NavigateToProjectInfo?.Invoke();
    }

    // Configuration commands
    [RelayCommand]
    private void AddConfiguration()
    {
        NavigateToConfigurationEditor?.Invoke(null);
    }

    [RelayCommand]
    private void EditConfiguration(AntennaConfiguration config)
    {
        NavigateToConfigurationEditor?.Invoke(config);
    }

    [RelayCommand]
    private void DeleteConfiguration(AntennaConfiguration config)
    {
        _projectViewModel.RemoveConfiguration(config);
        OnPropertyChanged(nameof(HasConfigurations));
        OnPropertyChanged(nameof(CanCalculate));
        StatusMessage = $"Deleted configuration: {config.Name}";
    }

    // Calculation commands
    [RelayCommand]
    private void CalculateAll()
    {
        StatusMessage = "Navigating to results...";
        NavigateToResults?.Invoke();
    }

    [RelayCommand]
    private void ExportReport()
    {
        StatusMessage = "Export report not implemented yet";
    }
}
