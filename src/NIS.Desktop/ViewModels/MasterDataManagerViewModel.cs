using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using NIS.Core.Data;
using NIS.Core.Models;
using NIS.Desktop.Localization;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the Master Data Manager view with tabs for Antennas, Cables, Radios, Translations.
/// </summary>
public partial class MasterDataManagerViewModel : ViewModelBase
{
    private readonly AntennaDatabase _antennaDatabase = new();
    private readonly CableDatabase _cableDatabase = new();
    private readonly RadioDatabase _radioDatabase = new();

    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    public Action<Antenna?>? NavigateToAntennaEditor { get; set; }
    public Action<Cable?>? NavigateToCableEditor { get; set; }
    public Action<Radio?>? NavigateToRadioEditor { get; set; }

    // Translation editor
    public TranslationEditorViewModel TranslationEditor { get; } = new();

    public MasterDataManagerViewModel()
    {
        // Set up confirmation dialog for translation editor
        TranslationEditor.ConfirmDiscardChanges = async (title, message) =>
        {
            var result = await MessageBoxManager
                .GetMessageBoxStandard(title, message, ButtonEnum.YesNo, Icon.Question)
                .ShowAsync();
            return result == ButtonResult.Yes;
        };

        _antennaDatabase.LoadDefaults();
        _cableDatabase.LoadDefaults();
        _radioDatabase.LoadDefaults();

        // Sort alphabetically by manufacturer then model
        var sortedAntennas = _antennaDatabase.Antennas
            .OrderBy(a => a.Manufacturer)
            .ThenBy(a => a.Model);
        AllAntennas = new ObservableCollection<Antenna>(sortedAntennas);

        // Sort cables alphabetically by name
        var sortedCables = _cableDatabase.Cables.OrderBy(c => c.Name);
        AllCables = new ObservableCollection<Cable>(sortedCables);

        // Sort radios alphabetically by manufacturer then model
        var sortedRadios = _radioDatabase.Radios
            .OrderBy(r => r.Manufacturer)
            .ThenBy(r => r.Model);
        AllRadios = new ObservableCollection<Radio>(sortedRadios);

        FilteredAntennas = new ObservableCollection<Antenna>(AllAntennas);
        FilteredCables = new ObservableCollection<Cable>(AllCables);
        FilteredRadios = new ObservableCollection<Radio>(AllRadios);
    }

    // Tab selection
    [ObservableProperty]
    private int _selectedTabIndex;

    // Antenna data
    public ObservableCollection<Antenna> AllAntennas { get; }
    public ObservableCollection<Antenna> FilteredAntennas { get; }

    [ObservableProperty]
    private string _antennaSearchText = string.Empty;

    [ObservableProperty]
    private Antenna? _selectedAntenna;

    partial void OnAntennaSearchTextChanged(string value)
    {
        FilterAntennas();
    }

    private void FilterAntennas()
    {
        FilteredAntennas.Clear();
        var search = AntennaSearchText.ToLowerInvariant();
        var filtered = string.IsNullOrWhiteSpace(search)
            ? AllAntennas
            : AllAntennas.Where(a =>
                a.Manufacturer.ToLowerInvariant().Contains(search) ||
                a.Model.ToLowerInvariant().Contains(search));

        // Maintain alphabetic sorting
        foreach (var antenna in filtered.OrderBy(a => a.Manufacturer).ThenBy(a => a.Model))
        {
            FilteredAntennas.Add(antenna);
        }
    }

    // Cable data
    public ObservableCollection<Cable> AllCables { get; }
    public ObservableCollection<Cable> FilteredCables { get; }

    [ObservableProperty]
    private string _cableSearchText = string.Empty;

    [ObservableProperty]
    private Cable? _selectedCable;

    partial void OnCableSearchTextChanged(string value)
    {
        FilterCables();
    }

    private void FilterCables()
    {
        FilteredCables.Clear();
        var search = CableSearchText.ToLowerInvariant();
        var filtered = string.IsNullOrWhiteSpace(search)
            ? AllCables
            : AllCables.Where(c => c.Name.ToLowerInvariant().Contains(search));

        // Maintain alphabetic sorting
        foreach (var cable in filtered.OrderBy(c => c.Name))
        {
            FilteredCables.Add(cable);
        }
    }

    // Radio data
    public ObservableCollection<Radio> AllRadios { get; }
    public ObservableCollection<Radio> FilteredRadios { get; }

    [ObservableProperty]
    private string _radioSearchText = string.Empty;

    [ObservableProperty]
    private Radio? _selectedRadio;

    partial void OnRadioSearchTextChanged(string value)
    {
        FilterRadios();
    }

    private void FilterRadios()
    {
        FilteredRadios.Clear();
        var search = RadioSearchText.ToLowerInvariant();
        var filtered = string.IsNullOrWhiteSpace(search)
            ? AllRadios
            : AllRadios.Where(r =>
                r.Manufacturer.ToLowerInvariant().Contains(search) ||
                r.Model.ToLowerInvariant().Contains(search));

        // Maintain alphabetic sorting
        foreach (var radio in filtered.OrderBy(r => r.Manufacturer).ThenBy(r => r.Model))
        {
            FilteredRadios.Add(radio);
        }
    }

    // Commands
    [RelayCommand]
    private async Task Back()
    {
        // Check for unsaved translation changes
        if (TranslationEditor.HasModifiedItems)
        {
            var result = await MessageBoxManager
                .GetMessageBoxStandard(
                    Strings.Instance.UnsavedChanges,
                    Strings.Instance.DiscardChangesPrompt,
                    ButtonEnum.YesNo, Icon.Question)
                .ShowAsync();

            if (result != ButtonResult.Yes)
                return;
        }

        NavigateBack?.Invoke();
    }

    [RelayCommand]
    private void AddAntenna()
    {
        NavigateToAntennaEditor?.Invoke(null);
    }

    [RelayCommand]
    private void EditAntenna()
    {
        if (SelectedAntenna != null)
        {
            NavigateToAntennaEditor?.Invoke(SelectedAntenna);
        }
    }

    [RelayCommand]
    private void DeleteAntenna()
    {
        if (SelectedAntenna != null)
        {
            AllAntennas.Remove(SelectedAntenna);
            FilteredAntennas.Remove(SelectedAntenna);
            SelectedAntenna = null;
        }
    }

    [RelayCommand]
    private void AddCable()
    {
        NavigateToCableEditor?.Invoke(null);
    }

    [RelayCommand]
    private void EditCable()
    {
        if (SelectedCable != null)
        {
            NavigateToCableEditor?.Invoke(SelectedCable);
        }
    }

    [RelayCommand]
    private void DeleteCable()
    {
        if (SelectedCable != null)
        {
            AllCables.Remove(SelectedCable);
            FilteredCables.Remove(SelectedCable);
            SelectedCable = null;
        }
    }

    [RelayCommand]
    private void AddRadio()
    {
        NavigateToRadioEditor?.Invoke(null);
    }

    [RelayCommand]
    private void EditRadio()
    {
        if (SelectedRadio != null)
        {
            NavigateToRadioEditor?.Invoke(SelectedRadio);
        }
    }

    [RelayCommand]
    private void DeleteRadio()
    {
        if (SelectedRadio != null)
        {
            AllRadios.Remove(SelectedRadio);
            FilteredRadios.Remove(SelectedRadio);
            SelectedRadio = null;
        }
    }

    [RelayCommand]
    private void ClearAntennaSearch()
    {
        AntennaSearchText = string.Empty;
    }

    [RelayCommand]
    private void ClearCableSearch()
    {
        CableSearchText = string.Empty;
    }

    [RelayCommand]
    private void ClearRadioSearch()
    {
        RadioSearchText = string.Empty;
    }

    /// <summary>
    /// Add a new antenna to the local collection.
    /// </summary>
    public void AddAntennaToDatabase(Antenna antenna)
    {
        // Add to local collection and resort
        AllAntennas.Add(antenna);
        ResortAntennas();
        FilterAntennas();
    }

    /// <summary>
    /// Add a new cable to the local collection.
    /// </summary>
    public void AddCableToDatabase(Cable cable)
    {
        // Add to local collection and resort
        AllCables.Add(cable);
        ResortCables();
        FilterCables();
    }

    /// <summary>
    /// Add a new radio to the local collection.
    /// </summary>
    public void AddRadioToDatabase(Radio radio)
    {
        // Add to local collection and resort
        AllRadios.Add(radio);
        ResortRadios();
        FilterRadios();
    }

    private void ResortAntennas()
    {
        var sorted = AllAntennas.OrderBy(a => a.Manufacturer).ThenBy(a => a.Model).ToList();
        AllAntennas.Clear();
        foreach (var antenna in sorted)
        {
            AllAntennas.Add(antenna);
        }
    }

    private void ResortCables()
    {
        var sorted = AllCables.OrderBy(c => c.Name).ToList();
        AllCables.Clear();
        foreach (var cable in sorted)
        {
            AllCables.Add(cable);
        }
    }

    private void ResortRadios()
    {
        var sorted = AllRadios.OrderBy(r => r.Manufacturer).ThenBy(r => r.Model).ToList();
        AllRadios.Clear();
        foreach (var radio in sorted)
        {
            AllRadios.Add(radio);
        }
    }
}
