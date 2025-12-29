using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Core.Models;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// Wrapper for displaying antenna configuration with index and computed properties.
/// </summary>
public class ConfigurationDisplayItem
{
    public int Index { get; }
    public AntennaConfiguration Configuration { get; }
    public IEnumerable<Antenna> ProjectAntennas { get; }

    public ConfigurationDisplayItem(int index, AntennaConfiguration config, IEnumerable<Antenna> projectAntennas)
    {
        Index = index;
        Configuration = config;
        ProjectAntennas = projectAntennas;
    }

    public string NumberDisplay => $"{Index}.";
    public string AntennaDisplay => Configuration.Antenna.DisplayName;
    public string FrequencyDisplay => GetFrequencies();
    public string HeightDisplay => $"{Configuration.Antenna.HeightMeters}m";
    public string CableDisplay => Configuration.Cable.Type;
    public string PowerDisplay => $"{Configuration.PowerWatts}W";
    public string OkaNameDisplay => Configuration.OkaName ?? "";
    public string OkaDistanceDisplay => Configuration.OkaDistanceMeters > 0 ? $"{Configuration.OkaDistanceMeters}m" : "";

    private string GetFrequencies()
    {
        var antenna = ProjectAntennas.FirstOrDefault(a =>
            a.Manufacturer.Equals(Configuration.Antenna.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
            a.Model.Equals(Configuration.Antenna.Model, StringComparison.OrdinalIgnoreCase));

        if (antenna == null || antenna.Bands.Count == 0)
            return "";

        var freqs = antenna.Bands.Select(b => FormatFrequency(b.FrequencyMHz));
        return string.Join(", ", freqs);
    }

    private static string FormatFrequency(double mhz)
    {
        return mhz switch
        {
            >= 1.8 and < 2 => "160m",
            >= 3.5 and < 4 => "80m",
            >= 7 and < 7.3 => "40m",
            >= 10.1 and < 10.2 => "30m",
            >= 14 and < 14.4 => "20m",
            >= 18 and < 18.2 => "17m",
            >= 21 and < 21.5 => "15m",
            >= 24.8 and < 25 => "12m",
            >= 28 and < 30 => "10m",
            >= 50 and < 54 => "6m",
            >= 144 and < 148 => "2m",
            >= 430 and < 440 => "70cm",
            _ => $"{mhz}MHz"
        };
    }
}

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
    public Action? NavigateBack { get; set; }

    [RelayCommand(CanExecute = nameof(CanNavigateBack))]
    private void GoBack()
    {
        NavigateBack?.Invoke();
    }

    private bool CanNavigateBack() => NavigateBack != null;

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
            OnPropertyChanged(nameof(ConfigurationItems));
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

    // Language (shows current UI language)
    public string LanguageDisplay => Localization.Strings.Instance.GetLanguageName(Localization.Strings.Instance.Language);

    // Collections
    public ObservableCollection<AntennaConfiguration> Configurations => _projectViewModel.Configurations;

    /// <summary>
    /// Wrapped configurations with index and computed display properties.
    /// </summary>
    public IEnumerable<ConfigurationDisplayItem> ConfigurationItems =>
        Configurations.Select((c, i) => new ConfigurationDisplayItem(
            i + 1,
            c,
            _projectViewModel.Project.CustomAntennas));

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
    private void EditConfiguration(ConfigurationDisplayItem item)
    {
        NavigateToConfigurationEditor?.Invoke(item.Configuration);
    }

    [RelayCommand]
    private void DeleteConfiguration(ConfigurationDisplayItem item)
    {
        _projectViewModel.RemoveConfiguration(item.Configuration);
        OnPropertyChanged(nameof(HasConfigurations));
        OnPropertyChanged(nameof(ConfigurationItems));
        OnPropertyChanged(nameof(CanCalculate));
        StatusMessage = $"Deleted configuration: {item.Configuration.Name}";
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
