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
using NIS.Desktop.New.Localization;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// ViewModel for the Master Data Manager view with tabs for Antennas, Cables, Radios, OKAs, Translations.
/// </summary>
public partial class MasterDataManagerViewModel : ViewModelBase
{
    private readonly AntennaDatabase _antennaDatabase = new();
    private readonly CableDatabase _cableDatabase = new();
    private readonly RadioDatabase _radioDatabase = new();
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

        _antennaDatabase.LoadDefaults();
        _cableDatabase.LoadDefaults();
        _radioDatabase.LoadDefaults();

        // Mark master data as not project-specific (read-only for normal users)
        foreach (var a in _antennaDatabase.Antennas) a.IsProjectSpecific = false;
        foreach (var c in _cableDatabase.Cables) c.IsProjectSpecific = false;
        foreach (var r in _radioDatabase.Radios) r.IsProjectSpecific = false;

        // Mark project custom data as project-specific (always editable)
        foreach (var a in _projectViewModel.Project.CustomAntennas) a.IsProjectSpecific = true;
        foreach (var c in _projectViewModel.Project.CustomCables) c.IsProjectSpecific = true;
        foreach (var r in _projectViewModel.Project.CustomRadios) r.IsProjectSpecific = true;

        // Combine master data + project-specific data, sorted alphabetically
        var allAntennas = _antennaDatabase.Antennas
            .Concat(_projectViewModel.Project.CustomAntennas)
            .OrderBy(a => a.Manufacturer)
            .ThenBy(a => a.Model);
        AllAntennas = new ObservableCollection<Antenna>(allAntennas);

        var allCables = _cableDatabase.Cables
            .Concat(_projectViewModel.Project.CustomCables)
            .OrderBy(c => c.Name);
        AllCables = new ObservableCollection<Cable>(allCables);

        var allRadios = _radioDatabase.Radios
            .Concat(_projectViewModel.Project.CustomRadios)
            .OrderBy(r => r.Manufacturer)
            .ThenBy(r => r.Model);
        AllRadios = new ObservableCollection<Radio>(allRadios);

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
            // If project-specific, also remove from project
            if (SelectedAntenna.IsProjectSpecific)
            {
                _projectViewModel.Project.CustomAntennas.Remove(SelectedAntenna);
                _projectViewModel.MarkDirty();
            }
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
            if (SelectedCable.IsProjectSpecific)
            {
                _projectViewModel.Project.CustomCables.Remove(SelectedCable);
                _projectViewModel.MarkDirty();
            }
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
            if (SelectedRadio.IsProjectSpecific)
            {
                _projectViewModel.Project.CustomRadios.Remove(SelectedRadio);
                _projectViewModel.MarkDirty();
            }
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
    /// Add a new antenna to the local collection and project.
    /// </summary>
    public void AddAntennaToDatabase(Antenna antenna)
    {
        // Mark as project-specific and add to project
        antenna.IsProjectSpecific = true;
        _projectViewModel.Project.CustomAntennas.Add(antenna);
        _projectViewModel.MarkDirty();

        // Add to local collection and resort
        AllAntennas.Add(antenna);
        ResortAntennas();
        FilterAntennas();
    }

    /// <summary>
    /// Add a new cable to the local collection and project.
    /// </summary>
    public void AddCableToDatabase(Cable cable)
    {
        // Mark as project-specific and add to project
        cable.IsProjectSpecific = true;
        _projectViewModel.Project.CustomCables.Add(cable);
        _projectViewModel.MarkDirty();

        // Add to local collection and resort
        AllCables.Add(cable);
        ResortCables();
        FilterCables();
    }

    /// <summary>
    /// Add a new radio to the local collection and project.
    /// </summary>
    public void AddRadioToDatabase(Radio radio)
    {
        // Mark as project-specific and add to project
        radio.IsProjectSpecific = true;
        _projectViewModel.Project.CustomRadios.Add(radio);
        _projectViewModel.MarkDirty();

        // Add to local collection and resort
        AllRadios.Add(radio);
        ResortRadios();
        FilterRadios();
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
