using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Models;
using NIS.Desktop.Services;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// Converter to display pattern array as a formatted string.
/// </summary>
public class PatternConverter : IValueConverter
{
    public static readonly PatternConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double[] pattern && pattern.Length > 0)
        {
            // Show pattern summary: "Pattern: 0°=0.0, 30°=1.5, 60°=5.2, 90°=12.0"
            var samples = new[] { 0, 3, 6, 9 }; // 0°, 30°, 60°, 90°
            var parts = samples
                .Where(i => i < pattern.Length)
                .Select(i => $"{i * 10}°={pattern[i]:F1}");
            return "Pattern: " + string.Join(", ", parts);
        }
        return "No pattern";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// ViewModel for the Configuration Editor view.
/// </summary>
public partial class ConfigurationEditorViewModel : ViewModelBase
{
    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    public Action? NavigateToAntennaSelector { get; set; }
    public Action<AntennaConfiguration>? OnSave { get; set; }
    public Action<Antenna?>? NavigateToAntennaEditor { get; set; }
    public Action<Cable?>? NavigateToCableEditor { get; set; }
    public Action<Radio?>? NavigateToRadioEditor { get; set; }
    public Action<Radio?>? NavigateToLinearEditor { get; set; }
    public Action<Oka?>? NavigateToOkaEditor { get; set; }
    public Action? MarkProjectDirty { get; set; }

    public ConfigurationEditorViewModel()
    {
        // Load all data from DatabaseService (single source of truth, already sorted)
        Antennas = new ObservableCollection<Antenna>(DatabaseService.Instance.GetAllAntennas());
        Cables = new ObservableCollection<Cable>(DatabaseService.Instance.GetAllCables());
        Radios = new ObservableCollection<Radio>(DatabaseService.Instance.GetAllRadios());
        Okas = new ObservableCollection<Oka>(DatabaseService.Instance.GetAllOkas());
        Modulations = new ObservableCollection<Modulation>(DatabaseService.Instance.GetAllModulations());
        ActivityFactor = Services.MasterDataStore.Load().Constants.DefaultActivityFactor;
        SelectedModulation = Modulations.FirstOrDefault(m => m.Name.Equals("CW", StringComparison.OrdinalIgnoreCase))
            ?? Modulations.FirstOrDefault();
    }

    // Collections
    public ObservableCollection<Antenna> Antennas { get; }
    public ObservableCollection<Cable> Cables { get; }
    public ObservableCollection<Radio> Radios { get; }
    public ObservableCollection<Oka> Okas { get; }

    // Configuration Number (1-based)
    [ObservableProperty]
    private int _configurationNumber = 1;

    public string HeaderText => $"Configuration {ConfigurationNumber}";

    partial void OnConfigurationNumberChanged(int value)
    {
        OnPropertyChanged(nameof(HeaderText));
    }

    // Configuration Name
    [ObservableProperty]
    private string _name = string.Empty;

    // Transmitter Section
    [ObservableProperty]
    private Radio? _selectedRadio;

    [ObservableProperty]
    private Radio? _selectedLinear;

    [ObservableProperty]
    private bool _hasLinear;

    [ObservableProperty]
    private double _powerWatts = 100;

    // Cable Section
    [ObservableProperty]
    private Cable? _selectedCable;

    [ObservableProperty]
    private double _cableLengthMeters = 10;

    [ObservableProperty]
    private double _additionalLossDb;

    [ObservableProperty]
    private string _additionalLossDescription = string.Empty;

    // Antenna Section
    [ObservableProperty]
    private Antenna? _selectedAntenna;

    [ObservableProperty]
    private double _heightMeters = 10;

    // Operating Parameters (apply to all bands)
    [ObservableProperty]
    private double _activityFactor = 0.5;

    public ObservableCollection<Modulation> Modulations { get; }

    [ObservableProperty]
    private Modulation? _selectedModulation;

    // OKA (single evaluation point per configuration)
    [ObservableProperty]
    private Oka? _selectedOka;

    [ObservableProperty]
    private string _okaName = string.Empty;

    [ObservableProperty]
    private double _okaDistanceMeters = 10;

    [ObservableProperty]
    private double _okaBuildingDampingDb;

    partial void OnSelectedOkaChanged(Oka? value)
    {
        if (value != null)
        {
            OkaName = value.Name;
            OkaDistanceMeters = value.DefaultDistanceMeters;
            OkaBuildingDampingDb = value.DefaultDampingDb;
        }
    }

    [RelayCommand]
    private void AddOka()
    {
        NavigateToOkaEditor?.Invoke(null);
    }

    /// <summary>
    /// Load from existing configuration for editing.
    /// </summary>
    public void LoadFromConfiguration(AntennaConfiguration config)
    {
        Name = config.Name;
        PowerWatts = config.PowerWatts;

        // Radio
        SelectedRadio = Radios.FirstOrDefault(r =>
            r.Manufacturer == config.Radio.Manufacturer &&
            r.Model == config.Radio.Model);

        // Linear
        HasLinear = config.Linear != null;
        if (config.Linear != null)
        {
            SelectedLinear = Radios.FirstOrDefault(r =>
                r.Manufacturer.Equals(config.Linear.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                r.Model.Equals(config.Linear.Model, StringComparison.OrdinalIgnoreCase));
        }

        // Cable
        SelectedCable = Cables.FirstOrDefault(c => c.Name == config.Cable.Type);
        CableLengthMeters = config.Cable.LengthMeters;
        AdditionalLossDb = config.Cable.AdditionalLossDb;
        AdditionalLossDescription = config.Cable.AdditionalLossDescription;

        // Antenna - use case-insensitive comparison
        SelectedAntenna = Antennas.FirstOrDefault(a =>
            a.Manufacturer.Equals(config.Antenna.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
            a.Model.Equals(config.Antenna.Model, StringComparison.OrdinalIgnoreCase));
        HeightMeters = config.Antenna.HeightMeters;
        // Operating parameters
        ActivityFactor = config.ActivityFactor;
        SelectedModulation = Modulations.FirstOrDefault(m =>
            m.Name.Equals(config.Modulation, StringComparison.OrdinalIgnoreCase));

        // OKA - try to find matching OKA, but don't auto-create if deleted
        if (!string.IsNullOrWhiteSpace(config.OkaName))
        {
            SelectedOka = Okas.FirstOrDefault(o => o.Name.Equals(config.OkaName, StringComparison.OrdinalIgnoreCase));
            // If OKA was deleted, just use the name without selecting from dropdown
        }
        OkaName = config.OkaName;
        OkaDistanceMeters = config.OkaDistanceMeters;
        OkaBuildingDampingDb = config.OkaBuildingDampingDb;
    }

    /// <summary>
    /// Create AntennaConfiguration from current values.
    /// </summary>
    public AntennaConfiguration ToConfiguration()
    {
        return new AntennaConfiguration
        {
            Name = Name,
            PowerWatts = PowerWatts,
            Radio = new RadioConfig
            {
                Manufacturer = SelectedRadio?.Manufacturer ?? "",
                Model = SelectedRadio?.Model ?? ""
            },
            Linear = HasLinear ? new LinearConfig
            {
                Manufacturer = SelectedLinear?.Manufacturer ?? "",
                Model = SelectedLinear?.Model ?? ""
            } : null,
            Cable = new CableConfig
            {
                Type = SelectedCable?.Name ?? "",
                LengthMeters = CableLengthMeters,
                AdditionalLossDb = AdditionalLossDb,
                AdditionalLossDescription = AdditionalLossDescription
            },
            Antenna = new AntennaPlacement
            {
                Manufacturer = SelectedAntenna?.Manufacturer ?? "",
                Model = SelectedAntenna?.Model ?? "",
                HeightMeters = HeightMeters
            },
            Modulation = SelectedModulation?.Name ?? "CW",
            ActivityFactor = ActivityFactor,
            OkaName = OkaName,
            OkaDistanceMeters = OkaDistanceMeters,
            OkaBuildingDampingDb = OkaBuildingDampingDb
        };
    }

    [RelayCommand]
    private void Cancel()
    {
        NavigateBack?.Invoke();
    }

    [RelayCommand]
    private void Save()
    {
        OnSave?.Invoke(ToConfiguration());
    }

    [RelayCommand]
    private void SelectAntenna()
    {
        NavigateToAntennaSelector?.Invoke();
    }

    [RelayCommand]
    private void EditAntenna()
    {
        NavigateToAntennaEditor?.Invoke(SelectedAntenna);
    }

    [RelayCommand]
    private void AddAntenna()
    {
        NavigateToAntennaEditor?.Invoke(null);
    }

    [RelayCommand]
    private void EditCable()
    {
        NavigateToCableEditor?.Invoke(SelectedCable);
    }

    [RelayCommand]
    private void AddCable()
    {
        NavigateToCableEditor?.Invoke(null);
    }

    [RelayCommand]
    private void EditRadio()
    {
        NavigateToRadioEditor?.Invoke(SelectedRadio);
    }

    [RelayCommand]
    private void AddRadio()
    {
        NavigateToRadioEditor?.Invoke(null);
    }

    [RelayCommand]
    private void EditLinear()
    {
        NavigateToLinearEditor?.Invoke(SelectedLinear);
    }

    [RelayCommand]
    private void AddLinear()
    {
        NavigateToLinearEditor?.Invoke(null);
    }
}
    partial void OnHasLinearChanged(bool value)
    {
        if (!value)
        {
            SelectedLinear = null;
        }
    }
