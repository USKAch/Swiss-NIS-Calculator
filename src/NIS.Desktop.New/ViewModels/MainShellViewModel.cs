using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.New.Localization;
using NIS.Desktop.New.Services;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// Main shell ViewModel that manages navigation between views.
/// </summary>
public partial class MainShellViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowSidebar))]
    private ViewModelBase? _currentView;

    [ObservableProperty]
    private string _windowTitle = "Swiss NIS Calculator";

    [ObservableProperty]
    private bool _hasProject;

    /// <summary>
    /// Show sidebar on all views except Welcome screen.
    /// </summary>
    public bool ShowSidebar => CurrentView is not WelcomeViewModel;

    // Shared state
    public ProjectViewModel ProjectViewModel { get; } = new();

    // View instances (lazy created)
    private WelcomeViewModel? _welcomeViewModel;
    private ProjectInfoViewModel? _projectInfoViewModel;
    private ProjectOverviewViewModel? _projectOverviewViewModel;
    private ConfigurationEditorViewModel? _configurationEditorViewModel;
    private AntennaEditorViewModel? _antennaEditorViewModel;
    private ResultsViewModel? _resultsViewModel;
    private MasterDataManagerViewModel? _masterDataManagerViewModel;
    private AntennaMasterEditorViewModel? _antennaMasterEditorViewModel;
    private CableMasterEditorViewModel? _cableMasterEditorViewModel;
    private RadioMasterEditorViewModel? _radioMasterEditorViewModel;
    private OkaMasterEditorViewModel? _okaMasterEditorViewModel;
    private SettingsViewModel? _settingsViewModel;

    private Func<string, string, Task<bool>>? _showConfirmDialog;

    // Dialog callback (set by App.axaml.cs)
    public Func<string, string, Task<bool>>? ShowConfirmDialog
    {
        get => _showConfirmDialog;
        set
        {
            _showConfirmDialog = value;
            // Update existing WelcomeViewModel if it exists
            if (_welcomeViewModel != null)
            {
                _welcomeViewModel.ShowConfirmDialog = value;
            }
        }
    }

    public MainShellViewModel()
    {
        // Start with welcome view
        NavigateToWelcome();
    }

    // Storage provider for file dialogs (set by MainWindow)
    public Avalonia.Platform.Storage.IStorageProvider? StorageProvider { get; set; }

    public void NavigateByTag(string tag)
    {
        switch (tag)
        {
            case "Home":
                NavigateToWelcome();
                break;
            case "NewProject":
                NavigateToProjectInfo(Strings.Instance.Language);
                break;
            case "OpenProject":
                _ = OpenProjectAsync();
                break;
            case "Save":
                _ = Save();
                break;
            case "SaveAs":
                _ = SaveAs();
                break;
            case "RecentProjects":
                // Show recent projects in welcome view
                NavigateToWelcome();
                break;
            case "Project":
                NavigateToProject();
                break;
            case "MasterData":
                NavigateToMasterDataManager();
                break;
            case "Settings":
                NavigateToSettings();
                break;
        }
    }

    private async Task OpenProjectAsync()
    {
        if (StorageProvider == null) return;

        var files = await StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
        {
            Title = "Open Project",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new Avalonia.Platform.Storage.FilePickerFileType("NIS Project") { Patterns = new[] { "*.nisproj" } },
                new Avalonia.Platform.Storage.FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
            }
        });

        if (files.Count > 0)
        {
            var filePath = files[0].Path.LocalPath;
            var success = await ProjectViewModel.LoadProjectAsync(filePath);
            if (success)
            {
                var settings = Services.AppSettings.Load();
                settings.AddRecentProject(filePath);
                settings.LastProjectPath = filePath;
                settings.Save();

                HasProject = true;
                NavigateToProjectOverview();
            }
        }
    }

    [RelayCommand]
    public void NavigateToWelcome()
    {
        var isFirstLoad = _welcomeViewModel == null;
        _welcomeViewModel ??= new WelcomeViewModel();
        _welcomeViewModel.NavigateToProjectInfo = NavigateToProjectInfo;
        _welcomeViewModel.NavigateToProjectOverview = NavigateToProjectOverviewAfterOpen;
        _welcomeViewModel.NavigateToMasterData = (isAdminMode) => NavigateToMasterDataManager(isAdminMode);
        _welcomeViewModel.ProjectViewModel = ProjectViewModel;
        _welcomeViewModel.ShowConfirmDialog = ShowConfirmDialog;
        CurrentView = _welcomeViewModel;
        WindowTitle = "Swiss NIS Calculator";

        // Load last project on first navigation to welcome
        if (isFirstLoad)
        {
            _ = LoadLastProjectAsync();
        }
    }

    private async Task LoadLastProjectAsync()
    {
        if (_welcomeViewModel == null) return;
        await _welcomeViewModel.TryLoadLastProjectAsync();

        // Update HasProject if a project was loaded
        if (_welcomeViewModel.HasCurrentProject)
        {
            HasProject = true;
        }
    }

    public void NavigateToProjectInfo(string _)
    {
        // Use the global language from Strings.Instance (set by WelcomeViewModel)
        _projectInfoViewModel = new ProjectInfoViewModel(Strings.Instance.Language);
        _projectInfoViewModel.NavigateBack = NavigateToWelcome;
        _projectInfoViewModel.NavigateToProjectOverview = NavigateToProjectOverviewAfterCreate;
        CurrentView = _projectInfoViewModel;
        WindowTitle = "Swiss NIS Calculator - New Project";
    }

    private void NavigateToProjectOverviewAfterCreate(ProjectInfoViewModel vm)
    {
        // Create new project with station info (use global language setting)
        ProjectViewModel.NewProject(Strings.Instance.Language);
        ProjectViewModel.UpdateStationInfo(vm.ToStationInfo());
        HasProject = true;

        NavigateToProjectOverview();
    }

    private void NavigateToProjectOverviewAfterOpen()
    {
        // Project already loaded by WelcomeViewModel
        // Language is a global app setting, don't override from project
        HasProject = true;
        NavigateToProjectOverview();
    }

    public void NavigateToProjectOverview()
    {
        _projectOverviewViewModel = new ProjectOverviewViewModel(ProjectViewModel);
        _projectOverviewViewModel.NavigateToConfigurationEditor = NavigateToConfigurationEditor;
        _projectOverviewViewModel.NavigateToResults = NavigateToResults;
        _projectOverviewViewModel.NavigateToProjectInfo = NavigateToEditProjectInfo;
        _projectOverviewViewModel.NavigateBack = NavigateToWelcome;
        CurrentView = _projectOverviewViewModel;
        WindowTitle = $"Swiss NIS Calculator - {ProjectViewModel.ProjectName}";
    }

    private void NavigateToEditProjectInfo()
    {
        _projectInfoViewModel = new ProjectInfoViewModel(ProjectViewModel.Project.Language)
        {
            IsEditMode = true
        };
        _projectInfoViewModel.FromStationInfo(ProjectViewModel.Project.Station);
        _projectInfoViewModel.NavigateBack = NavigateToProjectOverview;
        _projectInfoViewModel.NavigateToProjectOverview = (vm) =>
        {
            ProjectViewModel.UpdateStationInfo(vm.ToStationInfo());
            NavigateToProjectOverview();
        };
        CurrentView = _projectInfoViewModel;
        WindowTitle = "Swiss NIS Calculator - Edit Station Info";
    }

    public void NavigateToConfigurationEditor(NIS.Core.Models.AntennaConfiguration? existing = null)
    {
        _configurationEditorViewModel = new ConfigurationEditorViewModel();
        _configurationEditorViewModel.SetProjectOkas(ProjectViewModel.Okas);
        _configurationEditorViewModel.MarkProjectDirty = ProjectViewModel.MarkDirty;

        // All master data comes from MasterDataStore (single source of truth)

        if (existing != null)
        {
            var index = ProjectViewModel.Project.AntennaConfigurations.IndexOf(existing);
            _configurationEditorViewModel.ConfigurationNumber = index + 1;
            _configurationEditorViewModel.LoadFromConfiguration(existing);
        }
        else
        {
            _configurationEditorViewModel.ConfigurationNumber = ProjectViewModel.Project.AntennaConfigurations.Count + 1;
        }
        _configurationEditorViewModel.NavigateBack = NavigateToProjectOverview;
        _configurationEditorViewModel.NavigateToAntennaSelector = NavigateToAntennaSelector;
        _configurationEditorViewModel.NavigateToAntennaEditor = NavigateToAntennaEditorFromConfig;
        _configurationEditorViewModel.NavigateToCableEditor = NavigateToCableEditorFromConfig;
        _configurationEditorViewModel.NavigateToRadioEditor = NavigateToRadioEditorFromConfig;
        _configurationEditorViewModel.NavigateToOkaEditor = NavigateToOkaEditorFromConfig;
        _configurationEditorViewModel.OnSave = (config) =>
        {
            if (existing != null)
            {
                var index = ProjectViewModel.Project.AntennaConfigurations.IndexOf(existing);
                if (index >= 0)
                {
                    ProjectViewModel.Project.AntennaConfigurations[index] = config;
                    ProjectViewModel.MarkDirty();
                }
            }
            else
            {
                ProjectViewModel.AddConfiguration(config);
            }
            NavigateToProjectOverview();
        };
        CurrentView = _configurationEditorViewModel;
        WindowTitle = $"Swiss NIS Calculator - Configuration {_configurationEditorViewModel.ConfigurationNumber}";
    }

    public void NavigateToAntennaSelector()
    {
        _antennaEditorViewModel = new AntennaEditorViewModel();
        // All antennas come from MasterDataStore (single source of truth)
        _antennaEditorViewModel.RefreshAntennaList();
        _antennaEditorViewModel.NavigateBack = () =>
        {
            // Go back to configuration editor without selecting
            CurrentView = _configurationEditorViewModel;
        };
        _antennaEditorViewModel.OnSelect = (antenna) =>
        {
            if (_configurationEditorViewModel != null)
            {
                _configurationEditorViewModel.SelectedAntenna = antenna;
            }
            CurrentView = _configurationEditorViewModel;
        };
        _antennaEditorViewModel.NavigateToAddNew = () =>
        {
            // Navigate to antenna master editor for creating new antenna
            NavigateToAntennaEditorForProject();
        };
        CurrentView = _antennaEditorViewModel;
        WindowTitle = "Swiss NIS Calculator - Select Antenna";
    }

    private void NavigateToAntennaEditorForProject()
    {
        var editorVm = new AntennaMasterEditorViewModel();
        editorVm.InitializeNew();
        editorVm.NavigateBack = () =>
        {
            // Go back to antenna selector
            NavigateToAntennaSelector();
        };
        editorVm.OnSave = (antenna) =>
        {
            // Save to MasterDataStore (persists to file)
            Services.MasterDataStore.Instance.SaveAntenna(antenna);

            // Go back to selector and select the new antenna
            _antennaEditorViewModel?.AddAntennaToList(antenna);
            _antennaEditorViewModel?.OnSelect?.Invoke(antenna);
        };
        CurrentView = editorVm;
        WindowTitle = "Swiss NIS Calculator - Add New Antenna";
    }

    public void NavigateToResults()
    {
        _resultsViewModel = new ResultsViewModel();
        _resultsViewModel.NavigateBack = NavigateToProjectOverview;
        _ = _resultsViewModel.CalculateAsync(ProjectViewModel.Project);
        CurrentView = _resultsViewModel;
        WindowTitle = "Swiss NIS Calculator - Results";
    }

    [RelayCommand]
    public void NavigateToMasterData()
    {
        NavigateToMasterDataManager(false);
    }

    /// <summary>
    /// Navigates to Master Data Manager.
    /// </summary>
    /// <param name="isAdminMode">When true, allows editing embedded master data (Shift+Click)</param>
    public void NavigateToMasterDataManager(bool isAdminMode = false)
    {
        _masterDataManagerViewModel = new MasterDataManagerViewModel(ProjectViewModel);
        _masterDataManagerViewModel.IsAdminMode = isAdminMode;
        _masterDataManagerViewModel.NavigateBack = HasProject ? NavigateToProjectOverview : NavigateToWelcome;
        _masterDataManagerViewModel.NavigateToAntennaEditor = (a, ro) => NavigateToAntennaMasterEditor(a, ro);
        _masterDataManagerViewModel.NavigateToCableEditor = (c, ro) => NavigateToCableMasterEditor(c, ro);
        _masterDataManagerViewModel.NavigateToRadioEditor = (r, ro) => NavigateToRadioMasterEditor(r, ro);
        _masterDataManagerViewModel.NavigateToOkaEditor = NavigateToOkaMasterEditor;
        CurrentView = _masterDataManagerViewModel;
        WindowTitle = isAdminMode ? "Swiss NIS Calculator - Master Data (Admin)" : "Swiss NIS Calculator - Master Data";
    }

    [RelayCommand]
    public void NavigateToProject()
    {
        if (HasProject)
        {
            NavigateToProjectOverview();
        }
    }

    [RelayCommand]
    public void NavigateToSettings()
    {
        _settingsViewModel ??= new SettingsViewModel();
        CurrentView = _settingsViewModel;
        WindowTitle = "Swiss NIS Calculator - Settings";
    }

    public void NavigateToAntennaMasterEditor(NIS.Core.Models.Antenna? existing, bool isReadOnly = false)
    {
        _antennaMasterEditorViewModel = new AntennaMasterEditorViewModel();
        if (existing != null)
        {
            _antennaMasterEditorViewModel.InitializeEdit(existing);
        }
        else
        {
            _antennaMasterEditorViewModel.InitializeNew();
        }
        _antennaMasterEditorViewModel.IsReadOnly = isReadOnly;
        _antennaMasterEditorViewModel.NavigateBack = () =>
        {
            if (_masterDataManagerViewModel != null)
            {
                _masterDataManagerViewModel.SelectedTabIndex = 0; // Antennas tab
                CurrentView = _masterDataManagerViewModel;
                WindowTitle = _masterDataManagerViewModel.IsAdminMode
                    ? "Swiss NIS Calculator - Master Data (Admin)"
                    : "Swiss NIS Calculator - Master Data";
            }
        };
        _antennaMasterEditorViewModel.OnSave = (antenna) =>
        {
            var success = _masterDataManagerViewModel?.AddAntennaToDatabase(antenna) ?? false;
            if (!success)
            {
                // Duplicate exists - show validation message and stay on editor
                _antennaMasterEditorViewModel!.ValidationMessage = Strings.Instance.DuplicateNameError;
                return;
            }
            if (_masterDataManagerViewModel != null)
            {
                _masterDataManagerViewModel.SelectedTabIndex = 0; // Antennas tab
                CurrentView = _masterDataManagerViewModel;
                WindowTitle = _masterDataManagerViewModel.IsAdminMode
                    ? "Swiss NIS Calculator - Master Data (Admin)"
                    : "Swiss NIS Calculator - Master Data";
            }
        };
        CurrentView = _antennaMasterEditorViewModel;
        WindowTitle = isReadOnly ? "Swiss NIS Calculator - View Antenna"
            : (existing != null ? "Swiss NIS Calculator - Edit Antenna" : "Swiss NIS Calculator - Add Antenna");
    }

    public void NavigateToCableMasterEditor(NIS.Core.Models.Cable? existing, bool isReadOnly = false)
    {
        _cableMasterEditorViewModel = new CableMasterEditorViewModel();
        if (existing != null)
        {
            _cableMasterEditorViewModel.InitializeEdit(existing);
        }
        else
        {
            _cableMasterEditorViewModel.InitializeNew();
        }
        _cableMasterEditorViewModel.IsReadOnly = isReadOnly;
        _cableMasterEditorViewModel.NavigateBack = () =>
        {
            if (_masterDataManagerViewModel != null)
            {
                _masterDataManagerViewModel.SelectedTabIndex = 1; // Cables tab
                CurrentView = _masterDataManagerViewModel;
                WindowTitle = _masterDataManagerViewModel.IsAdminMode
                    ? "Swiss NIS Calculator - Master Data (Admin)"
                    : "Swiss NIS Calculator - Master Data";
            }
        };
        _cableMasterEditorViewModel.OnSave = (cable) =>
        {
            var success = _masterDataManagerViewModel?.AddCableToDatabase(cable) ?? false;
            if (!success)
            {
                // Duplicate exists - show validation message and stay on editor
                _cableMasterEditorViewModel!.ValidationMessage = Strings.Instance.DuplicateNameError;
                return;
            }
            if (_masterDataManagerViewModel != null)
            {
                _masterDataManagerViewModel.SelectedTabIndex = 1; // Cables tab
                CurrentView = _masterDataManagerViewModel;
                WindowTitle = _masterDataManagerViewModel.IsAdminMode
                    ? "Swiss NIS Calculator - Master Data (Admin)"
                    : "Swiss NIS Calculator - Master Data";
            }
        };
        CurrentView = _cableMasterEditorViewModel;
        WindowTitle = isReadOnly ? "Swiss NIS Calculator - View Cable"
            : (existing != null ? "Swiss NIS Calculator - Edit Cable" : "Swiss NIS Calculator - Add Cable");
    }

    public void NavigateToRadioMasterEditor(NIS.Core.Models.Radio? existing, bool isReadOnly = false)
    {
        _radioMasterEditorViewModel = new RadioMasterEditorViewModel();
        if (existing != null)
        {
            _radioMasterEditorViewModel.InitializeEdit(existing);
        }
        else
        {
            _radioMasterEditorViewModel.InitializeNew();
        }
        _radioMasterEditorViewModel.IsReadOnly = isReadOnly;
        _radioMasterEditorViewModel.NavigateBack = () =>
        {
            if (_masterDataManagerViewModel != null)
            {
                _masterDataManagerViewModel.SelectedTabIndex = 2; // Radios tab
                CurrentView = _masterDataManagerViewModel;
                WindowTitle = _masterDataManagerViewModel.IsAdminMode
                    ? "Swiss NIS Calculator - Master Data (Admin)"
                    : "Swiss NIS Calculator - Master Data";
            }
        };
        _radioMasterEditorViewModel.OnSave = (radio) =>
        {
            var success = _masterDataManagerViewModel?.AddRadioToDatabase(radio) ?? false;
            if (!success)
            {
                // Duplicate exists - show validation message and stay on editor
                _radioMasterEditorViewModel!.ValidationMessage = Strings.Instance.DuplicateNameError;
                return;
            }
            if (_masterDataManagerViewModel != null)
            {
                _masterDataManagerViewModel.SelectedTabIndex = 2; // Radios tab
                CurrentView = _masterDataManagerViewModel;
                WindowTitle = _masterDataManagerViewModel.IsAdminMode
                    ? "Swiss NIS Calculator - Master Data (Admin)"
                    : "Swiss NIS Calculator - Master Data";
            }
        };
        CurrentView = _radioMasterEditorViewModel;
        WindowTitle = isReadOnly ? "Swiss NIS Calculator - View Radio"
            : (existing != null ? "Swiss NIS Calculator - Edit Radio" : "Swiss NIS Calculator - Add Radio");
    }

    public void NavigateToOkaMasterEditor(NIS.Core.Models.Oka? existing, int nextId)
    {
        _okaMasterEditorViewModel = new OkaMasterEditorViewModel();
        if (existing != null)
        {
            _okaMasterEditorViewModel.InitializeEdit(existing);
        }
        else
        {
            _okaMasterEditorViewModel.InitializeNew(nextId);
        }
        _okaMasterEditorViewModel.NavigateBack = () =>
        {
            // Return to existing MasterDataManager on OKA tab
            if (_masterDataManagerViewModel != null)
            {
                _masterDataManagerViewModel.SelectedTabIndex = 3; // OKA tab
                CurrentView = _masterDataManagerViewModel;
                WindowTitle = "Swiss NIS Calculator - Master Data";
            }
        };
        _okaMasterEditorViewModel.OnSave = (oka) =>
        {
            _masterDataManagerViewModel?.AddOkaToDatabase(oka);
            // Return to existing MasterDataManager on OKA tab
            if (_masterDataManagerViewModel != null)
            {
                _masterDataManagerViewModel.SelectedTabIndex = 3; // OKA tab
                CurrentView = _masterDataManagerViewModel;
                WindowTitle = "Swiss NIS Calculator - Master Data";
            }
        };
        CurrentView = _okaMasterEditorViewModel;
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit OKA" : "Swiss NIS Calculator - Add OKA";
    }

    // Navigation from Configuration Editor to Master Editors
    // These return to the Configuration Editor after save

    private void NavigateToAntennaEditorFromConfig(NIS.Core.Models.Antenna? existing)
    {
        _antennaMasterEditorViewModel = new AntennaMasterEditorViewModel();
        if (existing != null)
        {
            _antennaMasterEditorViewModel.InitializeEdit(existing);
            // Shipped master data items are read-only in configuration editor
            _antennaMasterEditorViewModel.IsReadOnly = !existing.IsProjectSpecific;
        }
        else
        {
            _antennaMasterEditorViewModel.InitializeNew();
        }
        _antennaMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _antennaMasterEditorViewModel.OnSave = (antenna) =>
        {
            // Save to MasterDataStore (persists to file)
            Services.MasterDataStore.Instance.SaveAntenna(antenna);

            if (_configurationEditorViewModel != null)
            {
                // Refresh list from store and select the antenna
                _configurationEditorViewModel.Antennas.Clear();
                foreach (var a in Services.MasterDataStore.Instance.Antennas)
                    _configurationEditorViewModel.Antennas.Add(a);

                _configurationEditorViewModel.SelectedAntenna = Services.MasterDataStore.Instance.Antennas
                    .FirstOrDefault(a => a.Manufacturer.Equals(antenna.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                                         a.Model.Equals(antenna.Model, StringComparison.OrdinalIgnoreCase));
            }
            CurrentView = _configurationEditorViewModel;
        };
        CurrentView = _antennaMasterEditorViewModel;
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit Antenna" : "Swiss NIS Calculator - Add Antenna";
    }

    private void NavigateToCableEditorFromConfig(NIS.Core.Models.Cable? existing)
    {
        _cableMasterEditorViewModel = new CableMasterEditorViewModel();
        if (existing != null)
        {
            _cableMasterEditorViewModel.InitializeEdit(existing);
            // Shipped master data items are read-only in configuration editor
            _cableMasterEditorViewModel.IsReadOnly = !existing.IsProjectSpecific;
        }
        else
        {
            _cableMasterEditorViewModel.InitializeNew();
        }
        _cableMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _cableMasterEditorViewModel.OnSave = (cable) =>
        {
            // Save to MasterDataStore (persists to file)
            Services.MasterDataStore.Instance.SaveCable(cable);

            if (_configurationEditorViewModel != null)
            {
                // Refresh list from store and select the cable
                _configurationEditorViewModel.Cables.Clear();
                foreach (var c in Services.MasterDataStore.Instance.Cables)
                    _configurationEditorViewModel.Cables.Add(c);

                _configurationEditorViewModel.SelectedCable = Services.MasterDataStore.Instance.Cables
                    .FirstOrDefault(c => c.Name.Equals(cable.Name, StringComparison.OrdinalIgnoreCase));
            }
            CurrentView = _configurationEditorViewModel;
        };
        CurrentView = _cableMasterEditorViewModel;
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit Cable" : "Swiss NIS Calculator - Add Cable";
    }

    private void NavigateToRadioEditorFromConfig(NIS.Core.Models.Radio? existing)
    {
        _radioMasterEditorViewModel = new RadioMasterEditorViewModel();
        if (existing != null)
        {
            _radioMasterEditorViewModel.InitializeEdit(existing);
            // Shipped master data items are read-only in configuration editor
            _radioMasterEditorViewModel.IsReadOnly = !existing.IsProjectSpecific;
        }
        else
        {
            _radioMasterEditorViewModel.InitializeNew();
        }
        _radioMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _radioMasterEditorViewModel.OnSave = (radio) =>
        {
            // Save to MasterDataStore (persists to file)
            Services.MasterDataStore.Instance.SaveRadio(radio);

            if (_configurationEditorViewModel != null)
            {
                // Refresh list from store and select the radio
                _configurationEditorViewModel.Radios.Clear();
                foreach (var r in Services.MasterDataStore.Instance.Radios)
                    _configurationEditorViewModel.Radios.Add(r);

                _configurationEditorViewModel.SelectedRadio = Services.MasterDataStore.Instance.Radios
                    .FirstOrDefault(r => r.Manufacturer.Equals(radio.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                                         r.Model.Equals(radio.Model, StringComparison.OrdinalIgnoreCase));
            }
            CurrentView = _configurationEditorViewModel;
        };
        CurrentView = _radioMasterEditorViewModel;
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit Radio" : "Swiss NIS Calculator - Add Radio";
    }

    private void NavigateToOkaEditorFromConfig(NIS.Core.Models.Oka? existing, int nextId)
    {
        _okaMasterEditorViewModel = new OkaMasterEditorViewModel();
        if (existing != null)
        {
            _okaMasterEditorViewModel.InitializeEdit(existing);
        }
        else
        {
            _okaMasterEditorViewModel.InitializeNew(nextId);
        }
        _okaMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _okaMasterEditorViewModel.OnSave = (oka) =>
        {
            ProjectViewModel.AddOrUpdateOka(oka);

            // Add to config editor's OKA list if not already there
            if (_configurationEditorViewModel != null)
            {
                var existingOka = _configurationEditorViewModel.Okas.FirstOrDefault(o =>
                    o.Id == oka.Id);
                if (existingOka == null)
                {
                    _configurationEditorViewModel.Okas.Add(oka);
                }
                _configurationEditorViewModel.SelectedOka = oka;
            }
            CurrentView = _configurationEditorViewModel;
        };
        CurrentView = _okaMasterEditorViewModel;
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit OKA" : "Swiss NIS Calculator - Add OKA";
    }

    // Global Save/SaveAs commands (accessible via keyboard shortcuts)
    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task Save()
    {
        if (!HasProject) return;

        if (string.IsNullOrEmpty(ProjectViewModel.ProjectFilePath))
        {
            await SaveAs();
            return;
        }

        await ProjectViewModel.SaveProjectAsync();
    }

    private bool CanSave() => HasProject;

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAs()
    {
        if (!HasProject || StorageProvider == null) return;

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Project As",
            SuggestedFileName = $"{ProjectViewModel.ProjectName}.nisproj",
            DefaultExtension = ".nisproj",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("NIS Project") { Patterns = new[] { "*.nisproj" } }
            }
        });

        if (file != null)
        {
            await ProjectViewModel.SaveProjectAsync(file.Path.LocalPath);
        }
    }
}
