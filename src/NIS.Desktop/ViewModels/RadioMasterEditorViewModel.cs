using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Calculations;
using NIS.Desktop.Models;
using NIS.Desktop.Services;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// Editable band with power for UI binding.
/// </summary>
public partial class EditableRadioBandItem : ObservableObject
{
    [ObservableProperty]
    private double _frequencyMHz = 14;

    [ObservableProperty]
    private double _maxPowerWatts = 100;

    public RadioBand ToBand()
    {
        return new RadioBand
        {
            FrequencyMHz = FrequencyMHz,
            MaxPowerWatts = MaxPowerWatts
        };
    }

    public static EditableRadioBandItem FromBand(RadioBand band)
    {
        return new EditableRadioBandItem
        {
            FrequencyMHz = band.FrequencyMHz,
            MaxPowerWatts = band.MaxPowerWatts
        };
    }
}

/// <summary>
/// ViewModel for the Radio Master Editor - radio/transceiver editing.
/// </summary>
public partial class RadioMasterEditorViewModel : ViewModelBase
{
    private Radio? _originalRadio;

    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    public Action<Radio>? OnSave { get; set; }
    public Action<Radio>? OnCopy { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanCopy))]
    private bool _isEditing;

    /// <summary>
    /// When true, the editor is in read-only mode (viewing master data).
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEdit))]
    [NotifyPropertyChangedFor(nameof(CanCopy))]
    private bool _isReadOnly;

    /// <summary>
    /// Whether editing is allowed (inverse of IsReadOnly).
    /// </summary>
    public bool CanEdit => !IsReadOnly;

    /// <summary>
    /// Whether copying is allowed (always visible).
    /// </summary>
    public bool CanCopy => true;

    [ObservableProperty]
    private string _manufacturer = string.Empty;

    [ObservableProperty]
    private string _model = string.Empty;

    [ObservableProperty]
    private double _maxPowerWatts = 100;

    /// <summary>
    /// Whether to use band-specific power (vs single global power).
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowBands))]
    [NotifyPropertyChangedFor(nameof(ShowSinglePower))]
    [NotifyPropertyChangedFor(nameof(UseSinglePower))]
    private bool _useBandSpecificPower;

    public bool ShowBands => UseBandSpecificPower;
    public bool ShowSinglePower => !UseBandSpecificPower;

    /// <summary>
    /// Inverse of UseBandSpecificPower for two-way binding on first radio button.
    /// </summary>
    public bool UseSinglePower
    {
        get => !UseBandSpecificPower;
        set
        {
            if (value)
                UseBandSpecificPower = false;
        }
    }

    /// <summary>
    /// Available HAM radio frequencies for band selection.
    /// </summary>
    public static IReadOnlyList<double> AvailableFrequencies => SwissNisLimits.StandardFrequencies;

    public ObservableCollection<EditableRadioBandItem> Bands { get; } = new();

    [ObservableProperty]
    private string _validationMessage = string.Empty;

    public string Title => IsEditing ? "Edit Radio" : "Add New Radio";

    // Track dirty state for all editable properties
    partial void OnManufacturerChanged(string value) => MarkDirty();
    partial void OnModelChanged(string value) => MarkDirty();
    partial void OnMaxPowerWattsChanged(double value) => MarkDirty();
    partial void OnUseBandSpecificPowerChanged(bool value) => MarkDirty();

    /// <summary>
    /// Initialize for creating a new radio.
    /// </summary>
    public void InitializeNew()
    {
        _originalRadio = null;
        IsEditing = false;
        Manufacturer = string.Empty;
        Model = string.Empty;
        MaxPowerWatts = 100;
        UseBandSpecificPower = false;
        Bands.Clear();
    }

    /// <summary>
    /// Initialize for editing an existing radio.
    /// </summary>
    public void InitializeEdit(Radio radio)
    {
        _originalRadio = radio;
        IsEditing = true;
        Manufacturer = radio.Manufacturer;
        Model = radio.Model;
        MaxPowerWatts = radio.MaxPowerWatts;

        Bands.Clear();
        foreach (var band in radio.Bands)
        {
            Bands.Add(EditableRadioBandItem.FromBand(band));
        }

        // Set band-specific power AFTER loading bands to ensure proper UI update
        UseBandSpecificPower = radio.HasBandSpecificPower;
        // Force UI refresh for radio buttons
        OnPropertyChanged(nameof(UseBandSpecificPower));
        OnPropertyChanged(nameof(UseSinglePower));
        OnPropertyChanged(nameof(ShowBands));
        OnPropertyChanged(nameof(ShowSinglePower));
    }

    [RelayCommand]
    private void AddBand()
    {
        // Find a frequency not already used
        var usedFreqs = Bands.Select(b => b.FrequencyMHz).ToHashSet();
        var nextFreq = AvailableFrequencies.FirstOrDefault(f => !usedFreqs.Contains(f));
        if (nextFreq == 0) nextFreq = 14; // Default if all used

        Bands.Add(new EditableRadioBandItem
        {
            FrequencyMHz = nextFreq,
            MaxPowerWatts = MaxPowerWatts
        });
        MarkDirty();
    }

    [RelayCommand]
    private void RemoveBand(EditableRadioBandItem band)
    {
        Bands.Remove(band);
        MarkDirty();
    }

    [RelayCommand]
    private void Save()
    {
        ValidationMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Manufacturer))
        {
            ValidationMessage = Localization.Strings.Instance.ValidationManufacturerRequired;
            return;
        }

        if (string.IsNullOrWhiteSpace(Model))
        {
            ValidationMessage = Localization.Strings.Instance.ValidationModelRequired;
            return;
        }

        if (MaxPowerWatts <= 0)
        {
            ValidationMessage = Localization.Strings.Instance.ValidationPowerPositive;
            return;
        }

        // Validate band power values if using band-specific power
        if (UseBandSpecificPower)
        {
            foreach (var band in Bands)
            {
                if (band.MaxPowerWatts <= 0)
                {
                    ValidationMessage = string.Format(Localization.Strings.Instance.ValidationBandPowerPositive, MasterDataStore.GetBandName(band.FrequencyMHz));
                    return;
                }
            }
        }

        var radio = new Radio
        {
            Id = _originalRadio?.Id ?? 0,
            Manufacturer = Manufacturer.Trim(),
            Model = Model.Trim(),
            MaxPowerWatts = MaxPowerWatts,
            Bands = UseBandSpecificPower
                ? Bands.Select(b => b.ToBand()).ToList()
                : new List<RadioBand>()
        };

        OnSave?.Invoke(radio);
    }

    [RelayCommand]
    private void Copy()
    {
        var suffix = Localization.Strings.Instance.CustomSuffix;
        var copy = new Radio
        {
            Id = 0, // New entry
            Manufacturer = Manufacturer.Trim(),
            Model = $"{Model.Trim()} {suffix}",
            MaxPowerWatts = MaxPowerWatts,
            IsUserData = true,
            Bands = UseBandSpecificPower
                ? Bands.Select(b => b.ToBand()).ToList()
                : new List<RadioBand>()
        };

        OnCopy?.Invoke(copy);
    }

    [RelayCommand]
    private async Task Cancel()
    {
        if (await CanNavigateAwayAsync())
        {
            NavigateBack?.Invoke();
        }
    }
}
