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
        NewBands.Add(new EditableBand { FrequencyMHz = 14, GainDbi = 6 });
    }

    [RelayCommand]
    private void AddBand()
    {
        NewBands.Add(new EditableBand { FrequencyMHz = 21, GainDbi = 6 });
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
                GainDbi = band.GainDbi,
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
    private double _gainDbi;

    // Vertical radiation pattern - attenuation in dB at angles 0°, 10°, 20°... 90°
    [ObservableProperty]
    private double _pattern0;  // 0° (horizon)

    [ObservableProperty]
    private double _pattern10; // 10°

    [ObservableProperty]
    private double _pattern20; // 20°

    [ObservableProperty]
    private double _pattern30; // 30°

    [ObservableProperty]
    private double _pattern40; // 40°

    [ObservableProperty]
    private double _pattern50; // 50°

    [ObservableProperty]
    private double _pattern60; // 60°

    [ObservableProperty]
    private double _pattern70; // 70°

    [ObservableProperty]
    private double _pattern80; // 80°

    [ObservableProperty]
    private double _pattern90; // 90° (straight up)

    /// <summary>
    /// Gets the pattern as an array for saving.
    /// </summary>
    public double[] GetPatternArray()
    {
        return new[] { Pattern0, Pattern10, Pattern20, Pattern30, Pattern40,
                       Pattern50, Pattern60, Pattern70, Pattern80, Pattern90 };
    }
}
