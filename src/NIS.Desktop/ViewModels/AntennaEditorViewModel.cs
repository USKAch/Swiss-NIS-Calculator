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
/// ViewModel for the Antenna Editor/Selector view.
/// </summary>
public partial class AntennaEditorViewModel : ViewModelBase
{
    private readonly AntennaDatabase _antennaDatabase = new();
    private readonly ObservableCollection<Antenna> _allAntennas;

    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    public Action<Antenna>? OnSelect { get; set; }
    public Action<Antenna>? OnAddAntenna { get; set; }

    // Project custom antennas (set by MainShellViewModel)
    public List<Antenna>? ProjectAntennas { get; set; }

    /// <summary>
    /// Available HAM radio frequencies for selection.
    /// </summary>
    public static IReadOnlyList<double> AvailableFrequencies => SwissNisLimits.StandardFrequencies;

    [ObservableProperty]
    private bool _isAddingNew;

    // New antenna fields
    [ObservableProperty]
    private string _newManufacturer = string.Empty;

    [ObservableProperty]
    private string _newModel = string.Empty;

    [ObservableProperty]
    private bool _newIsRotatable;

    public ObservableCollection<EditableBand> NewBands { get; } = new();

    public AntennaEditorViewModel()
    {
        _antennaDatabase.LoadDefaults();
        _allAntennas = new ObservableCollection<Antenna>(_antennaDatabase.Antennas);
        FilteredAntennas = new ObservableCollection<Antenna>(_allAntennas);
    }

    /// <summary>
    /// Refreshes the antenna list including project custom antennas.
    /// </summary>
    public void RefreshAntennaList()
    {
        _allAntennas.Clear();

        // Add project custom antennas first (marked as custom)
        if (ProjectAntennas != null)
        {
            foreach (var antenna in ProjectAntennas)
            {
                _allAntennas.Add(antenna);
            }
        }

        // Add master data antennas
        foreach (var antenna in _antennaDatabase.Antennas)
        {
            // Skip if already in custom list
            if (!_allAntennas.Any(a =>
                a.Manufacturer.Equals(antenna.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                a.Model.Equals(antenna.Model, StringComparison.OrdinalIgnoreCase)))
            {
                _allAntennas.Add(antenna);
            }
        }

        ApplyFilter();
    }

    public ObservableCollection<Antenna> FilteredAntennas { get; }

    [ObservableProperty]
    private Antenna? _selectedAntenna;

    partial void OnSelectedAntennaChanged(Antenna? value)
    {
        OnPropertyChanged(nameof(HasSelection));
        // Auto-select and close when antenna is selected from dropdown
        if (value != null && !IsAddingNew)
        {
            OnSelect?.Invoke(value);
        }
    }

    [ObservableProperty]
    private string _searchText = string.Empty;

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        FilteredAntennas.Clear();

        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? _allAntennas
            : _allAntennas.Where(a =>
                a.Manufacturer.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.Model.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                a.DisplayName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        foreach (var antenna in filtered)
        {
            FilteredAntennas.Add(antenna);
        }
    }

    public bool HasSelection => SelectedAntenna != null;

    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = string.Empty;
    }

    [RelayCommand]
    private void Cancel()
    {
        if (IsAddingNew)
        {
            IsAddingNew = false;
            ClearNewAntennaForm();
        }
        else
        {
            NavigateBack?.Invoke();
        }
    }

    [RelayCommand]
    private void Select()
    {
        if (SelectedAntenna != null)
        {
            OnSelect?.Invoke(SelectedAntenna);
        }
    }

    [RelayCommand]
    private void StartAddNew()
    {
        IsAddingNew = true;
        ClearNewAntennaForm();
        // Add default HF bands
        NewBands.Add(new EditableBand { FrequencyMHz = 14, GainDbi = 6m });
    }

    [RelayCommand]
    private void AddBand()
    {
        NewBands.Add(new EditableBand { FrequencyMHz = 21, GainDbi = 6m });
    }

    [RelayCommand]
    private void RemoveBand(EditableBand band)
    {
        NewBands.Remove(band);
    }

    [RelayCommand]
    private void SaveNewAntenna()
    {
        if (string.IsNullOrWhiteSpace(NewManufacturer) || string.IsNullOrWhiteSpace(NewModel))
        {
            return;
        }

        var antenna = new Antenna
        {
            Manufacturer = NewManufacturer.Trim(),
            Model = NewModel.Trim(),
            IsRotatable = NewIsRotatable
        };

        foreach (var band in NewBands)
        {
            antenna.Bands.Add(new AntennaBand
            {
                FrequencyMHz = band.FrequencyMHz,
                GainDbi = (double)(band.GainDbi ?? 0),
                Pattern = band.GetPatternArray()
            });
        }

        // Add to project and notify
        OnAddAntenna?.Invoke(antenna);

        // Add to local list and select it
        _allAntennas.Insert(0, antenna);
        ApplyFilter();
        SelectedAntenna = antenna;

        IsAddingNew = false;
        ClearNewAntennaForm();
    }

    private void ClearNewAntennaForm()
    {
        NewManufacturer = string.Empty;
        NewModel = string.Empty;
        NewIsRotatable = false;
        NewBands.Clear();
    }
}

/// <summary>
/// Editable band for the new antenna form.
/// </summary>
public partial class EditableBand : ObservableObject
{
    [ObservableProperty]
    private double _frequencyMHz;

    [ObservableProperty]
    private decimal? _gainDbi = 0;

    // Vertical radiation pattern - attenuation in dB at angles 0°, 10°, 20°... 90°
    [ObservableProperty]
    private decimal? _pattern0 = 0;  // 0° (horizon)

    [ObservableProperty]
    private decimal? _pattern10 = 0; // 10°

    [ObservableProperty]
    private decimal? _pattern20 = 0; // 20°

    [ObservableProperty]
    private decimal? _pattern30 = 0; // 30°

    [ObservableProperty]
    private decimal? _pattern40 = 0; // 40°

    [ObservableProperty]
    private decimal? _pattern50 = 0; // 50°

    [ObservableProperty]
    private decimal? _pattern60 = 0; // 60°

    [ObservableProperty]
    private decimal? _pattern70 = 0; // 70°

    [ObservableProperty]
    private decimal? _pattern80 = 0; // 80°

    [ObservableProperty]
    private decimal? _pattern90 = 0; // 90° (straight up)

    /// <summary>
    /// Gets the pattern as an array for saving.
    /// </summary>
    public double[] GetPatternArray()
    {
        return new[] { (double)(Pattern0 ?? 0), (double)(Pattern10 ?? 0), (double)(Pattern20 ?? 0), (double)(Pattern30 ?? 0), (double)(Pattern40 ?? 0),
                       (double)(Pattern50 ?? 0), (double)(Pattern60 ?? 0), (double)(Pattern70 ?? 0), (double)(Pattern80 ?? 0), (double)(Pattern90 ?? 0) };
    }
}
