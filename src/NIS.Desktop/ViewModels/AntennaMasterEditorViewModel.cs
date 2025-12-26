using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Core.Data;
using NIS.Core.Models;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// Editable band with individual pattern properties for UI binding.
/// </summary>
public partial class EditableBandItem : ObservableObject
{
    [ObservableProperty]
    private double _frequencyMHz = 14;

    [ObservableProperty]
    private double _gainDbi = 0;

    [ObservableProperty]
    private double _pattern0;

    [ObservableProperty]
    private double _pattern10;

    [ObservableProperty]
    private double _pattern20;

    [ObservableProperty]
    private double _pattern30;

    [ObservableProperty]
    private double _pattern40;

    [ObservableProperty]
    private double _pattern50;

    [ObservableProperty]
    private double _pattern60;

    [ObservableProperty]
    private double _pattern70;

    [ObservableProperty]
    private double _pattern80;

    [ObservableProperty]
    private double _pattern90;

    public double[] GetPatternArray()
    {
        return new[] { Pattern0, Pattern10, Pattern20, Pattern30, Pattern40,
                       Pattern50, Pattern60, Pattern70, Pattern80, Pattern90 };
    }

    public void SetPatternFromArray(double[]? pattern)
    {
        if (pattern == null || pattern.Length == 0) return;

        Pattern0 = pattern.Length > 0 ? pattern[0] : 0;
        Pattern10 = pattern.Length > 1 ? pattern[1] : 0;
        Pattern20 = pattern.Length > 2 ? pattern[2] : 0;
        Pattern30 = pattern.Length > 3 ? pattern[3] : 0;
        Pattern40 = pattern.Length > 4 ? pattern[4] : 0;
        Pattern50 = pattern.Length > 5 ? pattern[5] : 0;
        Pattern60 = pattern.Length > 6 ? pattern[6] : 0;
        Pattern70 = pattern.Length > 7 ? pattern[7] : 0;
        Pattern80 = pattern.Length > 8 ? pattern[8] : 0;
        Pattern90 = pattern.Length > 9 ? pattern[9] : 0;
    }

    public AntennaBand ToBand()
    {
        return new AntennaBand
        {
            FrequencyMHz = FrequencyMHz,
            GainDbi = GainDbi,
            Pattern = GetPatternArray()
        };
    }

    public static EditableBandItem FromBand(AntennaBand band)
    {
        var item = new EditableBandItem
        {
            FrequencyMHz = band.FrequencyMHz,
            GainDbi = band.GainDbi
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

    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    public Action<Antenna>? OnSave { get; set; }

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _manufacturer = string.Empty;

    [ObservableProperty]
    private string _model = string.Empty;

    [ObservableProperty]
    private bool _isRotatable = true;

    public ObservableCollection<EditableBandItem> Bands { get; } = new();

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
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Manufacturer) || string.IsNullOrWhiteSpace(Model))
        {
            return;
        }

        var antenna = new Antenna
        {
            Manufacturer = Manufacturer.Trim(),
            Model = Model.Trim(),
            IsRotatable = IsRotatable,
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
