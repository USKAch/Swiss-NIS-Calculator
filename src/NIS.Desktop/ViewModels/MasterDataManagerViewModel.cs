using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using NIS.Desktop.Models;
using NIS.Desktop.Localization;
using NIS.Desktop.Services;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the Master Data Manager view with tabs for Antennas, Cables, Radios, OKAs, Translations.
/// </summary>
public partial class MasterDataManagerViewModel : ViewModelBase
{
    private readonly ProjectViewModel _projectViewModel;

    public Action? NavigateBack { get; set; }
    public Action<Antenna?, bool>? NavigateToAntennaEditor { get; set; }
    public Action<Cable?, bool>? NavigateToCableEditor { get; set; }
    public Action<Radio?, bool>? NavigateToRadioEditor { get; set; }
    public Action<Oka?>? NavigateToOkaEditor { get; set; }

    private MasterDataFile _masterData = new();

    /// <summary>
    /// Admin mode allows editing embedded master data (activated by Shift+Click).
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEditMasterData))]
    [NotifyPropertyChangedFor(nameof(IsReadOnlyForFactory))]
    [NotifyPropertyChangedFor(nameof(CanEditSelectedAntenna))]
    [NotifyPropertyChangedFor(nameof(CanDeleteSelectedAntenna))]
    [NotifyPropertyChangedFor(nameof(CanEditSelectedCable))]
    [NotifyPropertyChangedFor(nameof(CanDeleteSelectedCable))]
    [NotifyPropertyChangedFor(nameof(CanEditSelectedRadio))]
    [NotifyPropertyChangedFor(nameof(CanDeleteSelectedRadio))]
    [NotifyPropertyChangedFor(nameof(CanEditSelectedOka))]
    [NotifyPropertyChangedFor(nameof(CanDeleteSelectedOka))]
    private bool _isAdminMode;

    public bool CanEditMasterData => IsAdminMode;
    public bool IsReadOnlyForFactory => !IsAdminMode;

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

        // Load all data from DatabaseService (single source of truth)
        // Data is already sorted alphabetically by SQL ORDER BY
        AllAntennas = new ObservableCollection<Antenna>(DatabaseService.Instance.GetAllAntennas());
        AllCables = new ObservableCollection<Cable>(DatabaseService.Instance.GetAllCables());
        AllRadios = new ObservableCollection<Radio>(DatabaseService.Instance.GetAllRadios());

        // OKAs are global master data
        AllOkas = new ObservableCollection<Oka>(DatabaseService.Instance.GetAllOkas());

        FilteredAntennas = new ObservableCollection<Antenna>(AllAntennas);
        FilteredCables = new ObservableCollection<Cable>(AllCables);
        FilteredRadios = new ObservableCollection<Radio>(AllRadios);
        FilteredOkas = new ObservableCollection<Oka>(AllOkas);

        Modulations = new ObservableCollection<Modulation>(DatabaseService.Instance.GetAllModulations());

        _masterData = MasterDataStore.Load();
        GroundReflectionFactor = _masterData.Constants.GroundReflectionFactor;
        DefaultActivityFactor = _masterData.Constants.DefaultActivityFactor;
        Bands = new ObservableCollection<BandDefinition>(_masterData.Bands.OrderBy(b => b.FrequencyMHz));
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
    [NotifyPropertyChangedFor(nameof(CanEditSelectedOka))]
    [NotifyPropertyChangedFor(nameof(CanDeleteSelectedOka))]
    private Oka? _selectedOka;
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

    [RelayCommand]
    private void EditAntenna()
    {
        if (SelectedAntenna != null)
        {
            // Can edit if: project-specific OR admin mode
            var isReadOnly = !SelectedAntenna.IsUserData && !IsAdminMode;
            NavigateToAntennaEditor?.Invoke(SelectedAntenna, isReadOnly);
        }
    }

    public bool CanEditSelectedAntenna => SelectedAntenna != null &&
        (SelectedAntenna.IsUserData || IsAdminMode);

    public bool CanDeleteSelectedAntenna => SelectedAntenna != null &&
        (SelectedAntenna.IsUserData || IsAdminMode);

    [RelayCommand]
    private async Task DeleteAntenna()
    {
        if (SelectedAntenna != null && (SelectedAntenna.IsUserData || IsAdminMode))
        {
            // Check if antenna is used in any configurations
            var usages = DatabaseService.Instance.GetAntennaUsage(SelectedAntenna.Id);
            if (usages.Count > 0)
            {
                var usageList = string.Join("\n", usages.Select(u => $"  - {u.DisplayName}"));
                await MessageBoxManager.GetMessageBoxStandard(
                    Strings.Instance.CannotDelete,
                    $"{Strings.Instance.ItemInUse}\n\n{usageList}",
                    ButtonEnum.Ok,
                    Icon.Warning).ShowAsync();
                return;
            }

            // Delete from database
            DatabaseService.Instance.DeleteAntenna(SelectedAntenna.Manufacturer, SelectedAntenna.Model);
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
            var isReadOnly = !SelectedCable.IsUserData && !IsAdminMode;
            NavigateToCableEditor?.Invoke(SelectedCable, isReadOnly);
        }
    }

    public bool CanEditSelectedCable => SelectedCable != null &&
        (SelectedCable.IsUserData || IsAdminMode);

    public bool CanDeleteSelectedCable => SelectedCable != null &&
        (SelectedCable.IsUserData || IsAdminMode);

    [RelayCommand]
    private async Task DeleteCable()
    {
        if (SelectedCable != null && (SelectedCable.IsUserData || IsAdminMode))
        {
            // Check if cable is used in any configurations
            var usages = DatabaseService.Instance.GetCableUsage(SelectedCable.Id);
            if (usages.Count > 0)
            {
                var usageList = string.Join("\n", usages.Select(u => $"  - {u.DisplayName}"));
                await MessageBoxManager.GetMessageBoxStandard(
                    Strings.Instance.CannotDelete,
                    $"{Strings.Instance.ItemInUse}\n\n{usageList}",
                    ButtonEnum.Ok,
                    Icon.Warning).ShowAsync();
                return;
            }

            // Delete from database
            DatabaseService.Instance.DeleteCable(SelectedCable.Name);
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
            var isReadOnly = !SelectedRadio.IsUserData && !IsAdminMode;
            NavigateToRadioEditor?.Invoke(SelectedRadio, isReadOnly);
        }
    }

    public bool CanEditSelectedRadio => SelectedRadio != null &&
        (SelectedRadio.IsUserData || IsAdminMode);

    public bool CanDeleteSelectedRadio => SelectedRadio != null &&
        (SelectedRadio.IsUserData || IsAdminMode);

    [RelayCommand]
    private async Task DeleteRadio()
    {
        if (SelectedRadio != null && (SelectedRadio.IsUserData || IsAdminMode))
        {
            // Check if radio is used in any configurations
            var usages = DatabaseService.Instance.GetRadioUsage(SelectedRadio.Id);
            if (usages.Count > 0)
            {
                var usageList = string.Join("\n", usages.Select(u => $"  - {u.DisplayName}"));
                await MessageBoxManager.GetMessageBoxStandard(
                    Strings.Instance.CannotDelete,
                    $"{Strings.Instance.ItemInUse}\n\n{usageList}",
                    ButtonEnum.Ok,
                    Icon.Warning).ShowAsync();
                return;
            }

            // Delete from database
            DatabaseService.Instance.DeleteRadio(SelectedRadio.Manufacturer, SelectedRadio.Model);
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
        NavigateToOkaEditor?.Invoke(null);
    }

    [RelayCommand]
    private void EditOka()
    {
        if (SelectedOka != null && (SelectedOka.IsUserData || IsAdminMode))
        {
            NavigateToOkaEditor?.Invoke(SelectedOka);
        }
    }

    // Modulations (factory mode)
    public ObservableCollection<Modulation> Modulations { get; }

    [ObservableProperty]
    private Modulation? _selectedModulation;

    // Constants (factory mode, stored in masterdata.json)
    [ObservableProperty]
    private double _groundReflectionFactor;

    [ObservableProperty]
    private double _defaultActivityFactor;

    // Bands (factory mode, stored in masterdata.json)
    public ObservableCollection<BandDefinition> Bands { get; }

    [ObservableProperty]
    private BandDefinition? _selectedBand;

    public bool CanEditSelectedOka => SelectedOka != null &&
        (SelectedOka.IsUserData || IsAdminMode);

    public bool CanDeleteSelectedOka => SelectedOka != null &&
        (SelectedOka.IsUserData || IsAdminMode);

    [RelayCommand]
    private async Task DeleteOka()
    {
        if (SelectedOka != null && (SelectedOka.IsUserData || IsAdminMode))
        {
            // Check if OKA is used in any configurations
            var usages = DatabaseService.Instance.GetOkaUsage(SelectedOka.Id);
            if (usages.Count > 0)
            {
                var usageList = string.Join("\n", usages.Select(u => $"  - {u.DisplayName}"));
                await MessageBoxManager.GetMessageBoxStandard(
                    Strings.Instance.CannotDelete,
                    $"{Strings.Instance.ItemInUse}\n\n{usageList}",
                    ButtonEnum.Ok,
                    Icon.Warning).ShowAsync();
                return;
            }

            DatabaseService.Instance.DeleteOka(SelectedOka.Name);
            AllOkas.Remove(SelectedOka);
            FilteredOkas.Remove(SelectedOka);
            SelectedOka = null;
        }
    }

    [RelayCommand]
    private void ClearOkaSearch()
    {
        OkaSearchText = string.Empty;
    }

    [RelayCommand]
    private void AddModulation()
    {
        if (!IsAdminMode) return;
        Modulations.Add(new Modulation { Name = "New", Factor = 1.0, IsUserData = false });
    }

    [RelayCommand]
    private void DeleteModulation()
    {
        if (!IsAdminMode || SelectedModulation == null) return;
        DatabaseService.Instance.DeleteModulation(SelectedModulation.Name);
        Modulations.Remove(SelectedModulation);
        SelectedModulation = null;
    }

    [RelayCommand]
    private void SaveModulations()
    {
        if (!IsAdminMode) return;
        foreach (var modulation in Modulations)
        {
            if (string.IsNullOrWhiteSpace(modulation.Name)) continue;
            DatabaseService.Instance.SaveModulation(modulation, isAdminMode: true);
        }
        RefreshModulations();
    }

    private void RefreshModulations()
    {
        Modulations.Clear();
        foreach (var modulation in DatabaseService.Instance.GetAllModulations())
        {
            Modulations.Add(modulation);
        }
    }

    [RelayCommand]
    private void AddBand()
    {
        if (!IsAdminMode) return;
        var band = new BandDefinition { Name = "New", FrequencyMHz = 0 };
        Bands.Add(band);
        SelectedBand = band;
    }

    [RelayCommand]
    private void DeleteBand()
    {
        if (!IsAdminMode || SelectedBand == null) return;
        Bands.Remove(SelectedBand);
        SelectedBand = null;
    }

    [RelayCommand]
    private void SaveBands()
    {
        if (!IsAdminMode) return;
        _masterData.Bands = Bands.OrderBy(b => b.FrequencyMHz).ToList();
        MasterDataStore.Save(_masterData);
        Bands.Clear();
        foreach (var band in _masterData.Bands)
        {
            Bands.Add(band);
        }
    }

    [RelayCommand]
    private void SaveConstants()
    {
        if (!IsAdminMode) return;
        _masterData.Constants.GroundReflectionFactor = GroundReflectionFactor;
        _masterData.Constants.DefaultActivityFactor = DefaultActivityFactor;
        MasterDataStore.Save(_masterData);
    }

    public Func<Task<string?>>? SelectExportFile { get; set; }
    public Func<Task<string?>>? SelectImportFile { get; set; }

    [RelayCommand]
    private async Task ExportDatabase()
    {
        if (!IsAdminMode) return;

        var filePath = await (SelectExportFile?.Invoke() ?? Task.FromResult<string?>(null));
        if (string.IsNullOrEmpty(filePath)) return;

        try
        {
            DatabaseService.Instance.ExportFactoryData(filePath);
            await MessageBoxManager
                .GetMessageBoxStandard(
                    "Export Complete",
                    $"Factory data exported to:\n{filePath}",
                    ButtonEnum.Ok, Icon.Success)
                .ShowAsync();
        }
        catch (Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard(
                    "Export Failed",
                    $"Export failed: {ex.Message}",
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
        }
    }

    [RelayCommand]
    private async Task ImportDatabase()
    {
        if (!IsAdminMode) return;

        var filePath = await (SelectImportFile?.Invoke() ?? Task.FromResult<string?>(null));
        if (string.IsNullOrEmpty(filePath)) return;

        // Confirm replacement
        var confirm = await MessageBoxManager
            .GetMessageBoxStandard(
                "Confirm Import",
                "This will replace ALL existing master data. Continue?",
                ButtonEnum.YesNo, Icon.Warning)
            .ShowAsync();

        if (confirm != ButtonResult.Yes) return;

        try
        {
            DatabaseService.Instance.ImportFactoryData(filePath);

            // Refresh all collections
            AllAntennas.Clear();
            foreach (var a in DatabaseService.Instance.GetAllAntennas())
                AllAntennas.Add(a);
            FilterAntennas();

            AllCables.Clear();
            foreach (var c in DatabaseService.Instance.GetAllCables())
                AllCables.Add(c);
            FilterCables();

            AllRadios.Clear();
            foreach (var r in DatabaseService.Instance.GetAllRadios())
                AllRadios.Add(r);
            FilterRadios();

            RefreshModulations();

            await MessageBoxManager
                .GetMessageBoxStandard(
                    "Import Complete",
                    $"Factory data imported from:\n{filePath}",
                    ButtonEnum.Ok, Icon.Success)
                .ShowAsync();
        }
        catch (Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard(
                    "Import Failed",
                    $"Import failed: {ex.Message}",
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
        }
    }

    [RelayCommand]
    private void OpenDataFolder()
    {
        if (!IsAdminMode) return;

        try
        {
            var folder = Services.AppPaths.DataFolder;
            if (System.IO.Directory.Exists(folder))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = folder,
                    UseShellExecute = true
                });
            }
        }
        catch (Exception ex)
        {
            MessageBoxManager
                .GetMessageBoxStandard(
                    "Error",
                    $"Failed to open data folder: {ex.Message}",
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
        }
    }

    public bool AntennaExists(string manufacturer, string model, Antenna? exclude = null)
    {
        return DatabaseService.Instance.AntennaExists(manufacturer, model);
    }

    public bool CableExists(string name, Cable? exclude = null)
    {
        return DatabaseService.Instance.CableExists(name);
    }

    public bool RadioExists(string manufacturer, string model, Radio? exclude = null)
    {
        return DatabaseService.Instance.RadioExists(manufacturer, model);
    }

    public bool AddAntennaToDatabase(Antenna antenna)
    {
        // Check for duplicates
        if (AntennaExists(antenna.Manufacturer, antenna.Model))
        {
            return false;
        }

        // Save to database - admin mode creates factory data
        DatabaseService.Instance.SaveAntenna(antenna, isAdminMode: IsAdminMode);

        // Refresh local collection from database (already sorted)
        AllAntennas.Clear();
        foreach (var a in DatabaseService.Instance.GetAllAntennas())
            AllAntennas.Add(a);
        FilterAntennas();
        return true;
    }

    public void UpdateAntennaInDatabase(Antenna antenna)
    {
        DatabaseService.Instance.SaveAntenna(antenna, isAdminMode: IsAdminMode);

        // Refresh local collection from database (already sorted)
        AllAntennas.Clear();
        foreach (var a in DatabaseService.Instance.GetAllAntennas())
            AllAntennas.Add(a);
        FilterAntennas();
    }

    public bool AddCableToDatabase(Cable cable)
    {
        // Check for duplicates
        if (CableExists(cable.Name))
        {
            return false;
        }

        // Save to database - admin mode creates factory data
        DatabaseService.Instance.SaveCable(cable, isAdminMode: IsAdminMode);

        // Refresh local collection from database (already sorted)
        AllCables.Clear();
        foreach (var c in DatabaseService.Instance.GetAllCables())
            AllCables.Add(c);
        FilterCables();
        return true;
    }

    public void UpdateCableInDatabase(Cable cable)
    {
        DatabaseService.Instance.SaveCable(cable, isAdminMode: IsAdminMode);

        // Refresh local collection from database (already sorted)
        AllCables.Clear();
        foreach (var c in DatabaseService.Instance.GetAllCables())
            AllCables.Add(c);
        FilterCables();
    }

    public bool AddRadioToDatabase(Radio radio)
    {
        // Check for duplicates
        if (RadioExists(radio.Manufacturer, radio.Model))
        {
            return false;
        }

        // Save to database - admin mode creates factory data
        DatabaseService.Instance.SaveRadio(radio, isAdminMode: IsAdminMode);

        // Refresh local collection from database (already sorted)
        AllRadios.Clear();
        foreach (var r in DatabaseService.Instance.GetAllRadios())
            AllRadios.Add(r);
        FilterRadios();
        return true;
    }

    public void UpdateRadioInDatabase(Radio radio)
    {
        DatabaseService.Instance.SaveRadio(radio, isAdminMode: IsAdminMode);

        // Refresh local collection from database (already sorted)
        AllRadios.Clear();
        foreach (var r in DatabaseService.Instance.GetAllRadios())
            AllRadios.Add(r);
        FilterRadios();
    }

    public bool AddOkaToDatabase(Oka oka)
    {
        // Check for duplicates
        if (OkaExists(oka.Name))
        {
            return false;
        }

        Services.DatabaseService.Instance.SaveOka(oka, isAdminMode: IsAdminMode);
        ResortOkas();
        FilterOkas();
        return true;
    }

    public void UpdateOkaInDatabase(Oka oka)
    {
        Services.DatabaseService.Instance.SaveOka(oka, isAdminMode: IsAdminMode);
        ResortOkas();
        FilterOkas();
    }

    private bool OkaExists(string name)
    {
        return AllOkas.Any(o => o.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private void ResortOkas()
    {
        var sorted = AllOkas.OrderBy(o => o.Name).ToList();
        AllOkas.Clear();
        foreach (var oka in sorted)
        {
            AllOkas.Add(oka);
        }
    }
}
