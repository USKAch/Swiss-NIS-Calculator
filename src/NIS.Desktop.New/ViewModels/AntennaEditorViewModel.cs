using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Core.Models;
using NIS.Desktop.Services;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the Antenna Selector view - selects from existing antennas or navigates to add new.
/// </summary>
public partial class AntennaEditorViewModel : ViewModelBase
{
    private readonly ObservableCollection<Antenna> _allAntennas;

    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    public Action<Antenna>? OnSelect { get; set; }
    public Action? NavigateToAddNew { get; set; }

    public AntennaEditorViewModel()
    {
        // All antennas come from DatabaseService (single source of truth)
        _allAntennas = new ObservableCollection<Antenna>(DatabaseService.Instance.GetAllAntennas());
        FilteredAntennas = new ObservableCollection<Antenna>(_allAntennas);
    }

    /// <summary>
    /// Refreshes the antenna list from database.
    /// </summary>
    public void RefreshAntennaList()
    {
        _allAntennas.Clear();

        // Load all antennas from database (already sorted)
        foreach (var antenna in DatabaseService.Instance.GetAllAntennas())
        {
            _allAntennas.Add(antenna);
        }

        ApplyFilter();
    }

    /// <summary>
    /// Adds a newly created antenna to the list and selects it.
    /// </summary>
    public void AddAntennaToList(Antenna antenna)
    {
        // Refresh from store to get properly sorted list
        RefreshAntennaList();
        SelectedAntenna = _allAntennas.FirstOrDefault(a =>
            a.Manufacturer.Equals(antenna.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
            a.Model.Equals(antenna.Model, StringComparison.OrdinalIgnoreCase));
    }

    public ObservableCollection<Antenna> FilteredAntennas { get; }

    [ObservableProperty]
    private Antenna? _selectedAntenna;

    partial void OnSelectedAntennaChanged(Antenna? value)
    {
        OnPropertyChanged(nameof(HasSelection));
        // Auto-select and close when antenna is selected from dropdown
        if (value != null)
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
        NavigateBack?.Invoke();
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
        NavigateToAddNew?.Invoke();
    }
}
