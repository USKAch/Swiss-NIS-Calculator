using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Core.Models;
using NIS.Desktop.New.Services;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// ViewModel for the Project View - displays project overview and configurations.
/// </summary>
public partial class ProjectViewModel : ViewModelBase
{
    private readonly IProjectService _projectService;
    private readonly IDialogService _dialog;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    [ObservableProperty]
    private ObservableCollection<ConfigurationDisplayItem> _configurationItems = new();

    public ProjectViewModel(
        IProjectService projectService,
        IDialogService dialog,
        ILocalizationService localization)
    {
        Loc = localization;
        _projectService = projectService;
        _dialog = dialog;

        // Subscribe to project changes
        _projectService.ProjectChanged += OnProjectChanged;
        _projectService.DirtyChanged += _ => OnPropertyChanged(nameof(DirtyIndicator));

        SubscribeToLanguageChanges();

        // Initialize if project already loaded
        if (_projectService.HasProject)
        {
            RefreshConfigurationItems();
        }
    }

    private Project? Project => _projectService.CurrentProject;

    public string ProjectName
    {
        get => Project?.Name ?? "";
        set
        {
            if (Project != null && Project.Name != value)
            {
                Project.Name = value;
                OnPropertyChanged();
                _projectService.MarkDirty();
            }
        }
    }

    public string DirtyIndicator => _projectService.IsDirty ? " *" : "";
    public string Callsign => Project?.Station.Callsign ?? "";
    public string Address => Project?.Station.Address ?? "";
    public bool HasConfigurations => Project?.AntennaConfigurations.Count > 0;
    public bool CanCalculate => HasConfigurations && (Project?.AntennaConfigurations.Any(c => c.OkaDistanceMeters > 0) ?? false);

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrEmpty(_projectService.FilePath))
        {
            await SaveAs();
            return;
        }

        if (await _projectService.SaveAsync())
        {
            StatusMessage = L("Project.Status.Saved");
        }
        else
        {
            StatusMessage = L("Project.Status.Error").Replace("{0}", "Save failed");
        }
    }

    [RelayCommand]
    private async Task SaveAs()
    {
        var filePath = await _dialog.ShowSaveFileDialogAsync(
            L("Project.SaveAs"),
            ProjectName + ".nisproj",
            new FileFilter(L("Dialog.ProjectFilter"), "*.nisproj"));

        if (!string.IsNullOrEmpty(filePath))
        {
            if (await _projectService.SaveAsync(filePath))
            {
                StatusMessage = L("Project.Status.Saved");
            }
            else
            {
                StatusMessage = L("Project.Status.Error").Replace("{0}", "Save failed");
            }
        }
    }

    [RelayCommand]
    private void AddConfiguration()
    {
        // TODO: Navigate to configuration editor
        StatusMessage = "Add configuration - not implemented yet";
    }

    [RelayCommand]
    private void EditConfiguration(ConfigurationDisplayItem? item)
    {
        if (item == null) return;
        StatusMessage = $"Edit configuration {item.NumberDisplay} - not implemented yet";
    }

    [RelayCommand]
    private void DeleteConfiguration(ConfigurationDisplayItem? item)
    {
        if (item == null || Project == null) return;

        Project.AntennaConfigurations.Remove(item.Configuration);
        _projectService.MarkDirty();
        RefreshConfigurationItems();
        StatusMessage = $"Deleted configuration: {item.AntennaDisplay}";
    }

    [RelayCommand]
    private void Calculate()
    {
        StatusMessage = "Calculation - not implemented yet";
    }

    [RelayCommand]
    private void ExportReport()
    {
        StatusMessage = "Export - not implemented yet";
    }

    private void OnProjectChanged()
    {
        RefreshConfigurationItems();
        NotifyAllChanged();
    }

    private void RefreshConfigurationItems()
    {
        ConfigurationItems.Clear();

        if (Project == null) return;

        for (int i = 0; i < Project.AntennaConfigurations.Count; i++)
        {
            ConfigurationItems.Add(new ConfigurationDisplayItem(
                i + 1,
                Project.AntennaConfigurations[i],
                Project.CustomAntennas));
        }

        OnPropertyChanged(nameof(HasConfigurations));
        OnPropertyChanged(nameof(CanCalculate));
    }

    private void NotifyAllChanged()
    {
        OnPropertyChanged(nameof(ProjectName));
        OnPropertyChanged(nameof(DirtyIndicator));
        OnPropertyChanged(nameof(Callsign));
        OnPropertyChanged(nameof(Address));
        OnPropertyChanged(nameof(HasConfigurations));
        OnPropertyChanged(nameof(CanCalculate));
    }
}

/// <summary>
/// Display wrapper for antenna configuration.
/// </summary>
public class ConfigurationDisplayItem
{
    public int Index { get; }
    public AntennaConfiguration Configuration { get; }
    private readonly System.Collections.Generic.IEnumerable<Antenna> _antennas;

    public ConfigurationDisplayItem(int index, AntennaConfiguration config, System.Collections.Generic.IEnumerable<Antenna> antennas)
    {
        Index = index;
        Configuration = config;
        _antennas = antennas;
    }

    public string NumberDisplay => $"{Index}";
    public string AntennaDisplay => Configuration.Antenna?.DisplayName ?? "Unknown";
    public string BandsDisplay => GetBandsDisplay();
    public string HeightDisplay => $"{Configuration.Antenna?.HeightMeters ?? 0}m";
    public string PowerDisplay => $"{Configuration.PowerWatts}W";
    public string CableDisplay => Configuration.Cable?.Type ?? "-";
    public string OkaNameDisplay => Configuration.OkaName ?? "-";
    public string OkaDistanceDisplay => Configuration.OkaDistanceMeters > 0
        ? $"{Configuration.OkaDistanceMeters}m"
        : "-";

    private string GetBandsDisplay()
    {
        var antenna = _antennas.FirstOrDefault(a =>
            a.Manufacturer.Equals(Configuration.Antenna?.Manufacturer ?? "", StringComparison.OrdinalIgnoreCase) &&
            a.Model.Equals(Configuration.Antenna?.Model ?? "", StringComparison.OrdinalIgnoreCase));

        if (antenna == null || antenna.Bands.Count == 0)
            return "-";

        var bands = antenna.Bands.Select(b => FormatBand(b.FrequencyMHz));
        return string.Join(", ", bands);
    }

    private static string FormatBand(double mhz) => mhz switch
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
