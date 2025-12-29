using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using NIS.Core.Models;
using NIS.Desktop.New.Localization;
using NIS.Desktop.New.Services;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// ViewModel for the Master Data Manager view with tabs for Antennas, Cables, Radios, OKAs, Translations.
/// </summary>
public partial class MasterDataManagerViewModel : ViewModelBase
{
    private readonly ProjectViewModel _projectViewModel;

    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    /// <summary>
    /// Navigate to antenna editor. Parameters: antenna (null for new), isReadOnly
    /// </summary>
    public Action<Antenna?, bool>? NavigateToAntennaEditor { get; set; }
    /// <summary>
    /// Navigate to cable editor. Parameters: cable (null for new), isReadOnly
    /// </summary>
    public Action<Cable?, bool>? NavigateToCableEditor { get; set; }
    /// <summary>
    /// Navigate to radio editor. Parameters: radio (null for new), isReadOnly
    /// </summary>
    public Action<Radio?, bool>? NavigateToRadioEditor { get; set; }
    public Action<Oka?, int>? NavigateToOkaEditor { get; set; }

    /// <summary>
    /// Admin mode allows editing embedded master data (activated by Shift+Click).
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEditMasterData))]
    private bool _isAdminMode;

    /// <summary>
    /// Whether embedded master data can be edited (admin mode only).
    /// </summary>
    public bool CanEditMasterData => IsAdminMode;

    // Translation editor
    public TranslationEditorViewModel TranslationEditor { get; } = new();

    public MasterDataManagerViewModel(ProjectViewModel projectViewModel)
    {
        _projectViewModel = projectViewModel;
        // Set up confirmation dialog for translation editor
        TranslationEditor.ConfirmDiscardChanges = async (title, message) =>
        {
            var result = await MessageBoxManager
                .GetMessageBoxStandard(title, message, ButtonEnum.YesNo, Icon.Question)
                .ShowAsync();
            return result == ButtonResult.Yes;
        };

        // Load all data from MasterDataStore (single source of truth)
        // Data is already sorted alphabetically
        AllAntennas = new ObservableCollection<Antenna>(MasterDataStore.Instance.Antennas);
        AllCables = new ObservableCollection<Cable>(MasterDataStore.Instance.Cables);
        AllRadios = new ObservableCollection<Radio>(MasterDataStore.Instance.Radios);

        // OKAs belong to the current project
        AllOkas = _projectViewModel.Okas;

        FilteredAntennas = new ObservableCollection<Antenna>(AllAntennas);
        FilteredCables = new ObservableCollection<Cable>(AllCables);
        FilteredRadios = new ObservableCollection<Radio>(AllRadios);
        FilteredOkas = new ObservableCollection<Oka>(AllOkas);
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
    [NotifyPropertyChangedFor(nameof(CanEditSelectedAntenna))]
    [NotifyPropertyChangedFor(nameof(CanDeleteSelectedAntenna))]
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
    [NotifyPropertyChangedFor(nameof(CanEditSelectedCable))]
    [NotifyPropertyChangedFor(nameof(CanDeleteSelectedCable))]
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
    [NotifyPropertyChangedFor(nameof(CanEditSelectedRadio))]
    [NotifyPropertyChangedFor(nameof(CanDeleteSelectedRadio))]
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

    // OKA data
    public ObservableCollection<Oka> AllOkas { get; }
    public ObservableCollection<Oka> FilteredOkas { get; }

    [ObservableProperty]
    private string _okaSearchText = string.Empty;

    [ObservableProperty]
    private Oka? _selectedOka;

    public int NextOkaId => _projectViewModel.NextOkaId;

    partial void OnOkaSearchTextChanged(string value)
    {
        FilterOkas();
    }

    private void FilterOkas()
    {
        FilteredOkas.Clear();
        var search = OkaSearchText.ToLowerInvariant();
        var filtered = string.IsNullOrWhiteSpace(search)
            ? AllOkas
            : AllOkas.Where(o => o.Name.ToLowerInvariant().Contains(search));

        // Maintain ID sorting
        foreach (var oka in filtered.OrderBy(o => o.Id))
        {
            FilteredOkas.Add(oka);
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
        // New items are always project-specific and editable
        NavigateToAntennaEditor?.Invoke(null, false);
    }

    /// <summary>
    /// Edit or View antenna based on permissions.
    /// </summary>
    [RelayCommand]
    private void EditAntenna()
    {
        if (SelectedAntenna != null)
        {
            // Can edit if: project-specific OR admin mode
            var isReadOnly = !SelectedAntenna.IsProjectSpecific && !IsAdminMode;
            NavigateToAntennaEditor?.Invoke(SelectedAntenna, isReadOnly);
        }
    }

    /// <summary>
    /// Check if selected antenna can be edited (not just viewed).
    /// </summary>
    public bool CanEditSelectedAntenna => SelectedAntenna != null &&
        (SelectedAntenna.IsProjectSpecific || IsAdminMode);

    /// <summary>
    /// Check if selected antenna can be deleted.
    /// </summary>
    public bool CanDeleteSelectedAntenna => SelectedAntenna != null &&
        (SelectedAntenna.IsProjectSpecific || IsAdminMode);

    [RelayCommand]
    private void DeleteAntenna()
    {
        if (SelectedAntenna != null && (SelectedAntenna.IsProjectSpecific || IsAdminMode))
        {
            // Delete from MasterDataStore (persists to file)
            MasterDataStore.Instance.DeleteAntenna(SelectedAntenna);
            AllAntennas.Remove(SelectedAntenna);
            FilteredAntennas.Remove(SelectedAntenna);
            SelectedAntenna = null;
        }
    }

    [RelayCommand]
    private void AddCable()
    {
        // New items are always project-specific and editable
        NavigateToCableEditor?.Invoke(null, false);
    }

    [RelayCommand]
    private void EditCable()
    {
        if (SelectedCable != null)
        {
            var isReadOnly = !SelectedCable.IsProjectSpecific && !IsAdminMode;
            NavigateToCableEditor?.Invoke(SelectedCable, isReadOnly);
        }
    }

    public bool CanEditSelectedCable => SelectedCable != null &&
        (SelectedCable.IsProjectSpecific || IsAdminMode);

    public bool CanDeleteSelectedCable => SelectedCable != null &&
        (SelectedCable.IsProjectSpecific || IsAdminMode);

    [RelayCommand]
    private void DeleteCable()
    {
        if (SelectedCable != null && (SelectedCable.IsProjectSpecific || IsAdminMode))
        {
            // Delete from MasterDataStore (persists to file)
            MasterDataStore.Instance.DeleteCable(SelectedCable);
            AllCables.Remove(SelectedCable);
            FilteredCables.Remove(SelectedCable);
            SelectedCable = null;
        }
    }

    [RelayCommand]
    private void AddRadio()
    {
        // New items are always project-specific and editable
        NavigateToRadioEditor?.Invoke(null, false);
    }

    [RelayCommand]
    private void EditRadio()
    {
        if (SelectedRadio != null)
        {
            var isReadOnly = !SelectedRadio.IsProjectSpecific && !IsAdminMode;
            NavigateToRadioEditor?.Invoke(SelectedRadio, isReadOnly);
        }
    }

    public bool CanEditSelectedRadio => SelectedRadio != null &&
        (SelectedRadio.IsProjectSpecific || IsAdminMode);

    public bool CanDeleteSelectedRadio => SelectedRadio != null &&
        (SelectedRadio.IsProjectSpecific || IsAdminMode);

    [RelayCommand]
    private void DeleteRadio()
    {
        if (SelectedRadio != null && (SelectedRadio.IsProjectSpecific || IsAdminMode))
        {
            // Delete from MasterDataStore (persists to file)
            MasterDataStore.Instance.DeleteRadio(SelectedRadio);
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

    [RelayCommand]
    private void AddOka()
    {
        NavigateToOkaEditor?.Invoke(null, NextOkaId);
    }

    [RelayCommand]
    private void EditOka()
    {
        if (SelectedOka != null)
        {
            NavigateToOkaEditor?.Invoke(SelectedOka, SelectedOka.Id);
        }
    }

    [RelayCommand]
    private void DeleteOka()
    {
        if (SelectedOka != null)
        {
            _projectViewModel.RemoveOka(SelectedOka);
            FilteredOkas.Remove(SelectedOka);
            SelectedOka = null;
        }
    }

    [RelayCommand]
    private void ClearOkaSearch()
    {
        OkaSearchText = string.Empty;
    }

    /// <summary>
    /// Check if an antenna with the same name already exists.
    /// </summary>
    public bool AntennaExists(string manufacturer, string model, Antenna? exclude = null)
    {
        return MasterDataStore.Instance.AntennaExists(manufacturer, model, exclude);
    }

    /// <summary>
    /// Check if a cable with the same name already exists.
    /// </summary>
    public bool CableExists(string name, Cable? exclude = null)
    {
        return MasterDataStore.Instance.CableExists(name, exclude);
    }

    /// <summary>
    /// Check if a radio with the same name already exists.
    /// </summary>
    public bool RadioExists(string manufacturer, string model, Radio? exclude = null)
    {
        return MasterDataStore.Instance.RadioExists(manufacturer, model, exclude);
    }

    /// <summary>
    /// Add a new antenna to MasterDataStore (persists to file).
    /// Returns false if duplicate exists.
    /// </summary>
    public bool AddAntennaToDatabase(Antenna antenna)
    {
        // Check for duplicates
        if (AntennaExists(antenna.Manufacturer, antenna.Model))
        {
            return false;
        }

        // Save to MasterDataStore (marks as IsProjectSpecific and persists)
        MasterDataStore.Instance.SaveAntenna(antenna);

        // Refresh local collection from store (already sorted)
        AllAntennas.Clear();
        foreach (var a in MasterDataStore.Instance.Antennas)
            AllAntennas.Add(a);
        FilterAntennas();
        return true;
    }

    /// <summary>
    /// Add a new cable to MasterDataStore (persists to file).
    /// Returns false if duplicate exists.
    /// </summary>
    public bool AddCableToDatabase(Cable cable)
    {
        // Check for duplicates
        if (CableExists(cable.Name))
        {
            return false;
        }

        // Save to MasterDataStore (marks as IsProjectSpecific and persists)
        MasterDataStore.Instance.SaveCable(cable);

        // Refresh local collection from store (already sorted)
        AllCables.Clear();
        foreach (var c in MasterDataStore.Instance.Cables)
            AllCables.Add(c);
        FilterCables();
        return true;
    }

    /// <summary>
    /// Add a new radio to MasterDataStore (persists to file).
    /// Returns false if duplicate exists.
    /// </summary>
    public bool AddRadioToDatabase(Radio radio)
    {
        // Check for duplicates
        if (RadioExists(radio.Manufacturer, radio.Model))
        {
            return false;
        }

        // Save to MasterDataStore (marks as IsProjectSpecific and persists)
        MasterDataStore.Instance.SaveRadio(radio);

        // Refresh local collection from store (already sorted)
        AllRadios.Clear();
        foreach (var r in MasterDataStore.Instance.Radios)
            AllRadios.Add(r);
        FilterRadios();
        return true;
    }

    /// <summary>
    /// Add a new OKA to the shared storage and local collection.
    /// </summary>
    public void AddOkaToDatabase(Oka oka)
    {
        _projectViewModel.AddOrUpdateOka(oka);
        ResortOkas();
        FilterOkas();
    }

    private void ResortOkas()
    {
        var sorted = AllOkas.OrderBy(o => o.Id).ToList();
        AllOkas.Clear();
        foreach (var oka in sorted)
        {
            AllOkas.Add(oka);
        }
    }
}
