using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Core.Data;
using NIS.Core.Models;
using NIS.Core.Services;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// Editable band with individual pattern properties for UI binding.
/// </summary>
public partial class EditableBandItem : ObservableObject
{
    [ObservableProperty]
    private double _frequencyMHz = 14;

    [ObservableProperty]
    private decimal? _gainDbi = 0;

    [ObservableProperty]
    private decimal? _pattern0 = 0;

    [ObservableProperty]
    private decimal? _pattern10 = 0;

    [ObservableProperty]
    private decimal? _pattern20 = 0;

    [ObservableProperty]
    private decimal? _pattern30 = 0;

    [ObservableProperty]
    private decimal? _pattern40 = 0;

    [ObservableProperty]
    private decimal? _pattern50 = 0;

    [ObservableProperty]
    private decimal? _pattern60 = 0;

    [ObservableProperty]
    private decimal? _pattern70 = 0;

    [ObservableProperty]
    private decimal? _pattern80 = 0;

    [ObservableProperty]
    private decimal? _pattern90 = 0;

    public double[] GetPatternArray()
    {
        return new[] { (double)(Pattern0 ?? 0), (double)(Pattern10 ?? 0), (double)(Pattern20 ?? 0), (double)(Pattern30 ?? 0), (double)(Pattern40 ?? 0),
                       (double)(Pattern50 ?? 0), (double)(Pattern60 ?? 0), (double)(Pattern70 ?? 0), (double)(Pattern80 ?? 0), (double)(Pattern90 ?? 0) };
    }

    /// <summary>
    /// Returns true if any pattern value is non-zero.
    /// </summary>
    public bool HasPattern =>
        (Pattern0 ?? 0) != 0 || (Pattern10 ?? 0) != 0 || (Pattern20 ?? 0) != 0 ||
        (Pattern30 ?? 0) != 0 || (Pattern40 ?? 0) != 0 || (Pattern50 ?? 0) != 0 ||
        (Pattern60 ?? 0) != 0 || (Pattern70 ?? 0) != 0 || (Pattern80 ?? 0) != 0 ||
        (Pattern90 ?? 0) != 0;

    /// <summary>
    /// Applies a generated pattern array.
    /// </summary>
    public void ApplyPattern(double[] pattern)
    {
        if (pattern.Length != 10) return;
        Pattern0 = (decimal)pattern[0];
        Pattern10 = (decimal)pattern[1];
        Pattern20 = (decimal)pattern[2];
        Pattern30 = (decimal)pattern[3];
        Pattern40 = (decimal)pattern[4];
        Pattern50 = (decimal)pattern[5];
        Pattern60 = (decimal)pattern[6];
        Pattern70 = (decimal)pattern[7];
        Pattern80 = (decimal)pattern[8];
        Pattern90 = (decimal)pattern[9];
    }

    public void SetPatternFromArray(double[]? pattern)
    {
        if (pattern == null || pattern.Length == 0) return;

        Pattern0 = pattern.Length > 0 ? (decimal)pattern[0] : 0;
        Pattern10 = pattern.Length > 1 ? (decimal)pattern[1] : 0;
        Pattern20 = pattern.Length > 2 ? (decimal)pattern[2] : 0;
        Pattern30 = pattern.Length > 3 ? (decimal)pattern[3] : 0;
        Pattern40 = pattern.Length > 4 ? (decimal)pattern[4] : 0;
        Pattern50 = pattern.Length > 5 ? (decimal)pattern[5] : 0;
        Pattern60 = pattern.Length > 6 ? (decimal)pattern[6] : 0;
        Pattern70 = pattern.Length > 7 ? (decimal)pattern[7] : 0;
        Pattern80 = pattern.Length > 8 ? (decimal)pattern[8] : 0;
        Pattern90 = pattern.Length > 9 ? (decimal)pattern[9] : 0;
    }

    public AntennaBand ToBand()
    {
        return new AntennaBand
        {
            FrequencyMHz = FrequencyMHz,
            GainDbi = (double)(GainDbi ?? 0),
            Pattern = GetPatternArray()
        };
    }

    public static EditableBandItem FromBand(AntennaBand band)
    {
        var item = new EditableBandItem
        {
            FrequencyMHz = band.FrequencyMHz,
            GainDbi = (decimal)band.GainDbi
        };
        item.SetPatternFromArray(band.Pattern);
        return item;
    }
}

/// <summary>
/// ViewModel for the Antenna Master Editor - full antenna editing with bands and patterns.
/// </summary>
public partial class AntennaMasterEditorViewModel : ViewModelBase
{
    private Antenna? _originalAntenna;

    /// <summary>
    /// Available HAM radio frequencies for selection.
    /// </summary>
    public static IReadOnlyList<double> AvailableFrequencies => SwissNisLimits.StandardFrequencies;

    /// <summary>
    /// Available antenna types for selection.
    /// </summary>
    public static IReadOnlyList<string> AvailableAntennaTypes => AntennaTypes.All;

    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    public Action<Antenna>? OnSave { get; set; }

    [ObservableProperty]
    private bool _isEditing;

    /// <summary>
    /// When true, the editor is in read-only mode (viewing master data).
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEdit))]
    private bool _isReadOnly;

    /// <summary>
    /// Whether editing is allowed (inverse of IsReadOnly).
    /// </summary>
    public bool CanEdit => !IsReadOnly;

    [ObservableProperty]
    private string _manufacturer = string.Empty;

    [ObservableProperty]
    private string _model = string.Empty;

    [ObservableProperty]
    private bool _isRotatable = true;

    [ObservableProperty]
    private bool _isHorizontallyPolarized = true;

    [ObservableProperty]
    private double _horizontalAngleDegrees = 360;

    [ObservableProperty]
    private string _antennaType = AntennaTypes.Yagi;

    public bool IsVerticallyPolarized
    {
        get => !IsHorizontallyPolarized;
        set => IsHorizontallyPolarized = !value;
    }

    partial void OnIsHorizontallyPolarizedChanged(bool value)
    {
        OnPropertyChanged(nameof(IsVerticallyPolarized));
        // Vertical antennas cannot be rotatable
        if (!value)
        {
            IsRotatable = false;
        }
    }

    public ObservableCollection<EditableBandItem> Bands { get; } = new();

    [ObservableProperty]
    private string _validationMessage = string.Empty;

    public string Title => IsEditing ? "Edit Antenna" : "Add New Antenna";

    /// <summary>
    /// Initialize for creating a new antenna.
    /// </summary>
    public void InitializeNew()
    {
        _originalAntenna = null;
        IsEditing = false;
        Manufacturer = string.Empty;
        Model = string.Empty;
        IsRotatable = true;
        IsHorizontallyPolarized = true;
        HorizontalAngleDegrees = 360;
        AntennaType = AntennaTypes.Yagi;
        Bands.Clear();

        // Add a default band
        Bands.Add(new EditableBandItem { FrequencyMHz = 14, GainDbi = 0 });
    }

    /// <summary>
    /// Initialize for editing an existing antenna.
    /// </summary>
    public void InitializeEdit(Antenna antenna)
    {
        _originalAntenna = antenna;
        IsEditing = true;
        Manufacturer = antenna.Manufacturer;
        Model = antenna.Model;
        IsRotatable = antenna.IsRotatable;
        IsHorizontallyPolarized = antenna.IsHorizontallyPolarized;
        HorizontalAngleDegrees = antenna.HorizontalAngleDegrees;
        AntennaType = antenna.AntennaType;

        Bands.Clear();
        foreach (var band in antenna.Bands)
        {
            Bands.Add(EditableBandItem.FromBand(band));
        }

        if (Bands.Count == 0)
        {
            Bands.Add(new EditableBandItem { FrequencyMHz = 14, GainDbi = 0 });
        }
    }

    [RelayCommand]
    private void AddBand()
    {
        Bands.Add(new EditableBandItem { FrequencyMHz = 50, GainDbi = 0 });
    }

    [RelayCommand]
    private void RemoveBand(EditableBandItem band)
    {
        if (Bands.Count > 1)
        {
            Bands.Remove(band);
        }
    }

    [RelayCommand]
    private void GeneratePattern(EditableBandItem band)
    {
        if (band.GainDbi == null || band.GainDbi <= 0) return;

        var pattern = PatternGenerator.GeneratePattern(AntennaType, (double)band.GainDbi);
        band.ApplyPattern(pattern);
    }

    [RelayCommand]
    private void Save()
    {
        ValidationMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Manufacturer))
        {
            ValidationMessage = "Please enter a manufacturer.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Model))
        {
            ValidationMessage = "Please enter a model.";
            return;
        }

        if (Bands.Count == 0)
        {
            ValidationMessage = "Please add at least one frequency band.";
            return;
        }

        // Validate bands
        foreach (var band in Bands)
        {
            if (band.GainDbi < -20 || band.GainDbi > 50)
            {
                ValidationMessage = $"Gain for {band.FrequencyMHz} MHz must be between -20 and 50 dBi.";
                return;
            }

            // Check pattern values (attenuation should be 0-60 dB)
            var pattern = band.GetPatternArray();
            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] < 0 || pattern[i] > 60)
                {
                    ValidationMessage = $"Pattern attenuation values must be between 0 and 60 dB.";
                    return;
                }
            }
        }

        if (IsRotatable && (HorizontalAngleDegrees < 0 || HorizontalAngleDegrees > 360))
        {
            ValidationMessage = "Rotation angle must be between 0 and 360 degrees.";
            return;
        }

        var antenna = new Antenna
        {
            Manufacturer = Manufacturer.Trim(),
            Model = Model.Trim(),
            IsRotatable = IsRotatable,
            IsHorizontallyPolarized = IsHorizontallyPolarized,
            HorizontalAngleDegrees = HorizontalAngleDegrees,
            AntennaType = AntennaType,
            Bands = Bands.Select(b => b.ToBand()).ToList()
        };

        OnSave?.Invoke(antenna);
    }

    [RelayCommand]
    private void Cancel()
    {
        NavigateBack?.Invoke();
    }
}
