using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using NIS.Desktop.Localization;
using NIS.Desktop.Models;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// Wrapper for displaying antenna configuration with index and computed properties.
/// All lookups use pre-computed dictionaries for performance.
/// </summary>
public class ConfigurationDisplayItem
{
    public int Index { get; }
    public AntennaConfiguration Configuration { get; }

    // Pre-computed display values
    public string NumberDisplay { get; }
    public string AntennaDisplay { get; }
    public string FrequencyDisplay { get; }
    public string HeightDisplay { get; }
    public string CableDisplay { get; }
    public string PowerDisplay { get; }
    public string OkaNameDisplay { get; }
    public string OkaDistanceDisplay { get; }

    public ConfigurationDisplayItem(
        int index,
        AntennaConfiguration config,
        IReadOnlyDictionary<int, Antenna> antennaLookup,
        IReadOnlyDictionary<int, Oka> okaLookup)
    {
        Index = index;
        Configuration = config;

        // Pre-compute all display values
        NumberDisplay = $"{index}.";
        AntennaDisplay = config.Antenna.DisplayName;
        HeightDisplay = $"{config.Antenna.HeightMeters}m";
        CableDisplay = config.Cable.Type;
        OkaNameDisplay = config.OkaName ?? "";

        // Compute power display
        var effectivePower = (config.Linear != null && config.Linear.PowerWatts > 0)
            ? config.Linear.PowerWatts
            : config.PowerWatts;
        PowerDisplay = $"{effectivePower}W/{config.Modulation}";

        // Compute OKA distance from lookup
        OkaDistanceDisplay = config.OkaId.HasValue && okaLookup.TryGetValue(config.OkaId.Value, out var oka)
            ? $"{oka.DefaultDistanceMeters}m"
            : "";

        // Compute frequency display from antenna bands
        FrequencyDisplay = ComputeFrequencies(config, antennaLookup);
    }

    private static string ComputeFrequencies(AntennaConfiguration config, IReadOnlyDictionary<int, Antenna> antennaLookup)
    {
        if (!config.AntennaId.HasValue || !antennaLookup.TryGetValue(config.AntennaId.Value, out var antenna))
            return "";
        if (antenna.Bands.Count == 0)
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
            OnPropertyChanged(nameof(Operator));
            OnPropertyChanged(nameof(Callsign));
            OnPropertyChanged(nameof(Address));
            OnPropertyChanged(nameof(Location));
            OnPropertyChanged(nameof(Configurations));
            OnPropertyChanged(nameof(ConfigurationItems));
            OnPropertyChanged(nameof(HasConfigurations));
            OnPropertyChanged(nameof(CanCalculate));
        };
    }

    // Project info
    public string ProjectName
    {
        get => _projectViewModel.ProjectName;
        set => _projectViewModel.ProjectName = value;
    }
    public string DirtyIndicator => _projectViewModel.IsDirty ? "*" : "";
    public string Operator => _projectViewModel.Project.Operator;
    public string Callsign => _projectViewModel.Project.Callsign;
    public string Address => _projectViewModel.Project.Address;
    public string Location => _projectViewModel.Project.Location;

    // Collections
    public ObservableCollection<AntennaConfiguration> Configurations => _projectViewModel.Configurations;

    /// <summary>
    /// Wrapped configurations with index and computed display properties.
    /// Uses cached lookups for performance.
    /// </summary>
    public IEnumerable<ConfigurationDisplayItem> ConfigurationItems
    {
        get
        {
            // Cache lookups once per property access
            var antennaLookup = Services.DatabaseService.Instance.GetAllAntennas()
                .ToDictionary(a => a.Id);
            var okaLookup = Services.DatabaseService.Instance.GetAllOkas()
                .ToDictionary(o => o.Id);

            return Configurations.Select((c, i) => new ConfigurationDisplayItem(
                i + 1, c, antennaLookup, okaLookup));
        }
    }

    // State
    public bool HasConfigurations => Configurations.Count > 0;
    public bool CanCalculate => HasConfigurations && Configurations.Any(c =>
        c.OkaId.HasValue && Services.DatabaseService.Instance.GetOkaById(c.OkaId.Value) != null);

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

        NavigateToResults?.Invoke();
    }

    [RelayCommand]
    private void ExportReport()
    {
        // Navigate to Results view where export options are available
        NavigateToResults?.Invoke();
    }
}
