using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using NIS.Desktop.Localization;
using NIS.Desktop.Models;

namespace NIS.Desktop.ViewModels;

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
    public string PowerDisplay
    {
        get
        {
            var effectivePower = (Configuration.Linear != null && Configuration.Linear.PowerWatts > 0)
                ? Configuration.Linear.PowerWatts
                : Configuration.PowerWatts;
            return $"{effectivePower}W/{Configuration.Modulation}";
        }
    }
    public string OkaNameDisplay => Configuration.OkaName ?? "";
    public string OkaDistanceDisplay
    {
        get
        {
            if (!Configuration.OkaId.HasValue)
                return "";
            var oka = Services.DatabaseService.Instance.GetOkaById(Configuration.OkaId.Value);
            return oka != null ? $"{oka.DefaultDistanceMeters}m" : "";
        }
    }

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
            OnPropertyChanged(nameof(OperatorName));
            OnPropertyChanged(nameof(Callsign));
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
    public string OperatorName => _projectViewModel.Project.Operator;
    public string Callsign => _projectViewModel.Project.Callsign;
    public string Address => _projectViewModel.Project.Address;
    public string Location => _projectViewModel.Project.Location;
    public string StationSummary => !string.IsNullOrEmpty(Callsign)
        ? Callsign
        : string.IsNullOrEmpty(OperatorName) ? "No station info" : OperatorName;

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
            Services.DatabaseService.Instance.GetAllAntennas()));

    // State
    public bool HasConfigurations => Configurations.Count > 0;
    public bool CanCalculate => HasConfigurations && Configurations.Any(c =>
        c.OkaId.HasValue &&
        Services.DatabaseService.Instance.GetOkaById(c.OkaId.Value) != null);

    [ObservableProperty]
    private bool _hasResults;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    // Database saves are automatic - these methods kept for compatibility
    [RelayCommand]
    private void SaveProject()
    {
        OnPropertyChanged(nameof(StatusMessage));
        StatusMessage = "Project saved to database";
    }

    [RelayCommand]
    private void SaveProjectAs()
    {
        SaveProject();
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
    private async Task CalculateAll()
    {
        // Validate all configurations before navigating
        var errors = ResultsViewModel.ValidateAllConfigurations(_projectViewModel.Project);
        if (errors.Count > 0)
        {
            var errorList = string.Join("\n", errors);
            await MessageBoxManager.GetMessageBoxStandard(
                Strings.Instance.ConfigurationIncomplete,
                $"{Strings.Instance.FixErrorsBeforeCalculating}\n\n{errorList}",
                ButtonEnum.Ok,
                Icon.Warning).ShowAsync();
            return;
        }

        StatusMessage = Strings.Instance.Calculating;
        NavigateToResults?.Invoke();
    }

    [RelayCommand]
    private void ExportReport()
    {
        // Navigate to Results view where export options are available
        NavigateToResults?.Invoke();
    }
}
