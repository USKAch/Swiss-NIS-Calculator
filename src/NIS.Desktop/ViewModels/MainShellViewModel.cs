using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Localization;
using NIS.Desktop.Services;

namespace NIS.Desktop.ViewModels;

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

    [ObservableProperty]
    private string _breadcrumb = "";

    /// <summary>
    /// Show sidebar on all views except Welcome screen.
    /// </summary>
    public bool ShowSidebar => CurrentView is not WelcomeViewModel;

    // Shared state
    public ProjectViewModel ProjectViewModel { get; } = new();

    // View instances (lazy created)
    private ProjectListViewModel? _projectListViewModel;
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
    private ImportExportViewModel? _importExportViewModel;

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
        // Load and apply settings
        var settings = Services.AppSettings.Load();
        Strings.Instance.Language = settings.Language;
        SettingsViewModel.ApplyTheme(settings.ThemeMode);

        // Start with project list view
        NavigateToProjectList();
    }

    // Storage provider for file dialogs (set by MainWindow)
    public Avalonia.Platform.Storage.IStorageProvider? StorageProvider { get; set; }

    /// <summary>
    /// Auto-save the current project to database if it has unsaved changes.
    /// </summary>
    private void AutoSaveProjectIfDirty()
    {
        if (HasProject && ProjectViewModel.IsDirty && ProjectViewModel.ProjectId > 0)
        {
            Services.DatabaseService.Instance.UpdateProject(ProjectViewModel.ProjectId, ProjectViewModel.Project);
            ProjectViewModel.IsDirty = false;
        }
    }

    private string GetProjectDisplayName()
    {
        if (!string.IsNullOrEmpty(ProjectViewModel.Project.Name))
        {
            return ProjectViewModel.Project.Name;
        }
        if (!string.IsNullOrEmpty(ProjectViewModel.Project.Operator))
        {
            return ProjectViewModel.Project.Operator;
        }
        return Strings.Instance.Project;
    }

    public void NavigateByTag(string tag)
    {
        switch (tag)
        {
            case "Home":
                NavigateToProjectList();
                break;
            case "NewProject":
                NavigateToProjectInfo(Strings.Instance.Language);
                break;
            case "OpenProject":
                _ = OpenProjectFileAsync();
                break;
            case "SelectProject":
                NavigateToProjectList();
                break;
            case "DeleteProject":
                _ = DeleteCurrentProjectAsync();
                break;
            case "CreateReport":
                if (HasProject) NavigateToResults();
                break;
            case "ExportPdf":
                if (HasProject) NavigateToResults();
                break;
            case "StationInfo":
                if (HasProject) NavigateToEditProjectInfo();
                break;
            case "ProjectOverview":
                if (HasProject) NavigateToProjectOverview();
                break;
            case "Configurations":
                if (HasProject) NavigateToConfigurationEditor();
                break;
            case "Calculate":
                if (HasProject) NavigateToResults();
                break;
            case "Project":
                if (HasProject) NavigateToProjectOverview();
                break;
            case "MasterData":
                NavigateToMasterDataManager();
                break;
            case "ImportExport":
                NavigateToImportExport();
                break;
            case "Settings":
                NavigateToSettings();
                break;
        }
    }

    [RelayCommand]
    public void NavigateToProjectList()
    {
        AutoSaveProjectIfDirty();

        _projectListViewModel ??= new ProjectListViewModel();
        _projectListViewModel.NavigateToNewProject = NavigateToProjectInfo;
        _projectListViewModel.NavigateToEditProject = async (id) => await LoadProjectFromDatabaseAsync(id);
        _projectListViewModel.ShowConfirmDialog = ShowConfirmDialog;
        _projectListViewModel.RefreshProjects();
        CurrentView = _projectListViewModel;
        Breadcrumb = Strings.Instance.Home;
        WindowTitle = "Swiss NIS Calculator";
        HasProject = false;
    }

    [RelayCommand]
    public void NavigateToWelcome()
    {
        var isFirstLoad = _welcomeViewModel == null;
        _welcomeViewModel ??= new WelcomeViewModel();
        _welcomeViewModel.NavigateToProjectInfo = NavigateToProjectInfo;
        _welcomeViewModel.NavigateToProjectOverview = NavigateToProjectOverviewAfterOpen;
        _welcomeViewModel.NavigateToMasterData = (isAdminMode) => NavigateToMasterDataManager(isAdminMode);
        _welcomeViewModel.LoadProjectFromDatabase = async (id) => await LoadProjectFromDatabaseAsync(id);
        _welcomeViewModel.ProjectViewModel = ProjectViewModel;
        _welcomeViewModel.ShowConfirmDialog = ShowConfirmDialog;
        CurrentView = _welcomeViewModel;
        Breadcrumb = Strings.Instance.Home;
        WindowTitle = "Swiss NIS Calculator";

        // Load last project on first navigation to welcome
        if (isFirstLoad)
        {
            // Database is source of truth, no last project loading
        }
    }

    // LoadLastProjectAsync removed - database is source of truth

    public void NavigateToProjectInfo(string _)
    {
        // Use the global language from Strings.Instance (set by WelcomeViewModel)
        _projectInfoViewModel = new ProjectInfoViewModel(Strings.Instance.Language);
        _projectInfoViewModel.NavigateBack = NavigateToProjectList;
        _projectInfoViewModel.NavigateToProjectOverview = NavigateToProjectOverviewAfterCreate;
        CurrentView = _projectInfoViewModel;
        Breadcrumb = $"{Strings.Instance.Home} > {Strings.Instance.CreateProject}";
        WindowTitle = "Swiss NIS Calculator";
    }

    private void NavigateToProjectOverviewAfterCreate(ProjectInfoViewModel vm)
    {
        // Create new project (use global language setting)
        ProjectViewModel.NewProject();
        ProjectViewModel.Project.Name = vm.ProjectName;
        ProjectViewModel.Project.Operator = vm.OperatorName;
        ProjectViewModel.Project.Callsign = vm.Callsign;
        ProjectViewModel.Project.Address = vm.Address;
        ProjectViewModel.Project.Location = vm.Location;

        // Save to database immediately so project appears in list
        var projectId = Services.DatabaseService.Instance.CreateProject(ProjectViewModel.Project);
        ProjectViewModel.ProjectId = projectId;
        ProjectViewModel.IsDirty = false;

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
        AutoSaveProjectIfDirty();
        _projectOverviewViewModel = new ProjectOverviewViewModel(ProjectViewModel);
        _projectOverviewViewModel.NavigateToConfigurationEditor = NavigateToConfigurationEditor;
        _projectOverviewViewModel.NavigateToResults = NavigateToResults;
        _projectOverviewViewModel.NavigateToProjectInfo = NavigateToEditProjectInfo;
        _projectOverviewViewModel.NavigateBack = NavigateToProjectList;
        CurrentView = _projectOverviewViewModel;
        var projectName = GetProjectDisplayName();
        Breadcrumb = $"{Strings.Instance.Project} > {projectName}";
        WindowTitle = $"Swiss NIS Calculator - {projectName}";
    }

    private void NavigateToEditProjectInfo()
    {
        _projectInfoViewModel = new ProjectInfoViewModel()
        {
            IsEditMode = true
        };
        _projectInfoViewModel.ProjectName = ProjectViewModel.Project.Name;
        _projectInfoViewModel.OperatorName = ProjectViewModel.Project.Operator;
        _projectInfoViewModel.Callsign = ProjectViewModel.Project.Callsign;
        _projectInfoViewModel.Address = ProjectViewModel.Project.Address;
        _projectInfoViewModel.Location = ProjectViewModel.Project.Location;
        _projectInfoViewModel.NavigateBack = NavigateToProjectOverview;
        _projectInfoViewModel.NavigateToProjectOverview = (vm) =>
        {
            ProjectViewModel.Project.Name = vm.ProjectName;
            ProjectViewModel.Project.Operator = vm.OperatorName;
            ProjectViewModel.Project.Callsign = vm.Callsign;
            ProjectViewModel.Project.Address = vm.Address;
            ProjectViewModel.Project.Location = vm.Location;
            ProjectViewModel.MarkDirty();
            NavigateToProjectOverview();
        };
        CurrentView = _projectInfoViewModel;
        var projectName = GetProjectDisplayName();
        Breadcrumb = $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.ProjectInfo}";
        WindowTitle = $"Swiss NIS Calculator - {projectName}";
    }

    public void NavigateToConfigurationEditor(NIS.Desktop.Models.AntennaConfiguration? existing = null)
    {
        _configurationEditorViewModel = new ConfigurationEditorViewModel();
        _configurationEditorViewModel.MarkProjectDirty = ProjectViewModel.MarkDirty;

        // All master data comes from DatabaseService (single source of truth)

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
        _configurationEditorViewModel.NavigateToLinearEditor = NavigateToLinearEditorFromConfig;
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
        var projectName = GetProjectDisplayName();
        Breadcrumb = $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.Configuration} {_configurationEditorViewModel.ConfigurationNumber}";
        WindowTitle = $"Swiss NIS Calculator - {projectName}";
    }

    public void NavigateToAntennaSelector()
    {
        _antennaEditorViewModel = new AntennaEditorViewModel();
        // All antennas come from DatabaseService (single source of truth)
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
        var projectName = GetProjectDisplayName();
        Breadcrumb = $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.SelectAntennaPrompt}";
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
            // Save to database
            Services.DatabaseService.Instance.SaveAntenna(antenna);

            // Go back to selector and select the new antenna
            _antennaEditorViewModel?.AddAntennaToList(antenna);
            _antennaEditorViewModel?.OnSelect?.Invoke(antenna);
        };
        CurrentView = editorVm;
        var projectName = GetProjectDisplayName();
        Breadcrumb = $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.AntennaDetails}";
        WindowTitle = "Swiss NIS Calculator - Add New Antenna";
    }

    public void NavigateToResults()
    {
        _resultsViewModel = new ResultsViewModel();
        _resultsViewModel.NavigateBack = NavigateToProjectOverview;
        _ = _resultsViewModel.CalculateAsync(ProjectViewModel.Project);
        CurrentView = _resultsViewModel;
        var projectName = GetProjectDisplayName();
        Breadcrumb = $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.CalculationResults}";
        WindowTitle = $"Swiss NIS Calculator - {projectName}";
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
        AutoSaveProjectIfDirty();
        _masterDataManagerViewModel = new MasterDataManagerViewModel(ProjectViewModel);
        _masterDataManagerViewModel.IsAdminMode = isAdminMode;
        _masterDataManagerViewModel.NavigateBack = HasProject ? NavigateToProjectOverview : NavigateToProjectList;
        _masterDataManagerViewModel.NavigateToAntennaEditor = (a, ro) => NavigateToAntennaMasterEditor(a, ro);
        _masterDataManagerViewModel.NavigateToCableEditor = (c, ro) => NavigateToCableMasterEditor(c, ro);
        _masterDataManagerViewModel.NavigateToRadioEditor = (r, ro) => NavigateToRadioMasterEditor(r, ro);
        _masterDataManagerViewModel.NavigateToOkaEditor = NavigateToOkaMasterEditor;
        _masterDataManagerViewModel.SelectExportFile = SelectFileForFactoryExport;
        _masterDataManagerViewModel.SelectImportFile = SelectFileForFactoryImport;
        CurrentView = _masterDataManagerViewModel;
        Breadcrumb = Strings.Instance.MasterData;
        WindowTitle = "Swiss NIS Calculator";
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
        AutoSaveProjectIfDirty();
        _settingsViewModel ??= new SettingsViewModel();
        CurrentView = _settingsViewModel;
        Breadcrumb = Strings.Instance.Settings;
        WindowTitle = "Swiss NIS Calculator";
    }

    [RelayCommand]
    public void NavigateToImportExport()
    {
        AutoSaveProjectIfDirty();
        _importExportViewModel ??= new ImportExportViewModel();
        _importExportViewModel.StorageProvider = StorageProvider;
        _importExportViewModel.ShowConfirmDialog = ShowConfirmDialog;
        _importExportViewModel.NavigateBack = HasProject ? NavigateToProjectOverview : NavigateToProjectList;
        CurrentView = _importExportViewModel;
        Breadcrumb = Strings.Instance.ImportExport;
        WindowTitle = "Swiss NIS Calculator";
    }

    private async Task OpenProjectFileAsync()
    {
        if (StorageProvider == null) return;

        _importExportViewModel ??= new ImportExportViewModel();
        _importExportViewModel.StorageProvider = StorageProvider;
        _importExportViewModel.ShowConfirmDialog = ShowConfirmDialog;
        _importExportViewModel.OnProjectImported = () =>
        {
            _projectListViewModel?.RefreshProjects();
        };

        if (_importExportViewModel.ImportProjectCommand is IAsyncRelayCommand importCommand)
        {
            await importCommand.ExecuteAsync(null);
        }
    }

    public void NavigateToAntennaMasterEditor(NIS.Desktop.Models.Antenna? existing, bool isReadOnly = false)
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
        Breadcrumb = $"{Strings.Instance.MasterData} > {Strings.Instance.AntennaDetails}";
        WindowTitle = isReadOnly ? "Swiss NIS Calculator - View Antenna"
            : (existing != null ? "Swiss NIS Calculator - Edit Antenna" : "Swiss NIS Calculator - Add Antenna");
    }

    public void NavigateToCableMasterEditor(NIS.Desktop.Models.Cable? existing, bool isReadOnly = false)
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
        Breadcrumb = $"{Strings.Instance.MasterData} > {Strings.Instance.Cable}";
        WindowTitle = isReadOnly ? "Swiss NIS Calculator - View Cable"
            : (existing != null ? "Swiss NIS Calculator - Edit Cable" : "Swiss NIS Calculator - Add Cable");
    }

    public void NavigateToRadioMasterEditor(NIS.Desktop.Models.Radio? existing, bool isReadOnly = false)
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
        Breadcrumb = $"{Strings.Instance.MasterData} > {Strings.Instance.RadioDetails}";
        WindowTitle = isReadOnly ? "Swiss NIS Calculator - View Radio"
            : (existing != null ? "Swiss NIS Calculator - Edit Radio" : "Swiss NIS Calculator - Add Radio");
    }

    public void NavigateToOkaMasterEditor(NIS.Desktop.Models.Oka? existing)
    {
        _okaMasterEditorViewModel = new OkaMasterEditorViewModel();
        if (existing != null)
        {
            _okaMasterEditorViewModel.InitializeEdit(existing);
        }
        else
        {
            _okaMasterEditorViewModel.InitializeNew();
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
        Breadcrumb = $"{Strings.Instance.MasterData} > {Strings.Instance.OkaDetails}";
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit OKA" : "Swiss NIS Calculator - Add OKA";
    }

    // Navigation from Configuration Editor to Master Editors
    // These return to the Configuration Editor after save

    private void NavigateToAntennaEditorFromConfig(NIS.Desktop.Models.Antenna? existing)
    {
        _antennaMasterEditorViewModel = new AntennaMasterEditorViewModel();
        if (existing != null)
        {
            _antennaMasterEditorViewModel.InitializeEdit(existing);
            // Shipped master data items are read-only in configuration editor
            _antennaMasterEditorViewModel.IsReadOnly = !existing.IsUserData;
        }
        else
        {
            _antennaMasterEditorViewModel.InitializeNew();
        }
        _antennaMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _antennaMasterEditorViewModel.OnSave = (antenna) =>
        {
            // Save to database
            Services.DatabaseService.Instance.SaveAntenna(antenna);

            if (_configurationEditorViewModel != null)
            {
                // Refresh list from database and select the antenna
                _configurationEditorViewModel.Antennas.Clear();
                foreach (var a in Services.DatabaseService.Instance.GetAllAntennas())
                    _configurationEditorViewModel.Antennas.Add(a);

                _configurationEditorViewModel.SelectedAntenna = _configurationEditorViewModel.Antennas
                    .FirstOrDefault(a => a.Manufacturer.Equals(antenna.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                                         a.Model.Equals(antenna.Model, StringComparison.OrdinalIgnoreCase));
            }
            CurrentView = _configurationEditorViewModel;
        };
        CurrentView = _antennaMasterEditorViewModel;
        var projectName = GetProjectDisplayName();
        Breadcrumb = $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.AntennaDetails}";
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit Antenna" : "Swiss NIS Calculator - Add Antenna";
    }

    private void NavigateToCableEditorFromConfig(NIS.Desktop.Models.Cable? existing)
    {
        _cableMasterEditorViewModel = new CableMasterEditorViewModel();
        if (existing != null)
        {
            _cableMasterEditorViewModel.InitializeEdit(existing);
            // Shipped master data items are read-only in configuration editor
            _cableMasterEditorViewModel.IsReadOnly = !existing.IsUserData;
        }
        else
        {
            _cableMasterEditorViewModel.InitializeNew();
        }
        _cableMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _cableMasterEditorViewModel.OnSave = (cable) =>
        {
            // Save to database
            Services.DatabaseService.Instance.SaveCable(cable);

            if (_configurationEditorViewModel != null)
            {
                // Refresh list from database and select the cable
                _configurationEditorViewModel.Cables.Clear();
                foreach (var c in Services.DatabaseService.Instance.GetAllCables())
                    _configurationEditorViewModel.Cables.Add(c);

                _configurationEditorViewModel.SelectedCable = _configurationEditorViewModel.Cables
                    .FirstOrDefault(c => c.Name.Equals(cable.Name, StringComparison.OrdinalIgnoreCase));
            }
            CurrentView = _configurationEditorViewModel;
        };
        CurrentView = _cableMasterEditorViewModel;
        var projectName = GetProjectDisplayName();
        Breadcrumb = $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.Cable}";
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit Cable" : "Swiss NIS Calculator - Add Cable";
    }

    private void NavigateToRadioEditorFromConfig(NIS.Desktop.Models.Radio? existing)
    {
        _radioMasterEditorViewModel = new RadioMasterEditorViewModel();
        if (existing != null)
        {
            _radioMasterEditorViewModel.InitializeEdit(existing);
            // Shipped master data items are read-only in configuration editor
            _radioMasterEditorViewModel.IsReadOnly = !existing.IsUserData;
        }
        else
        {
            _radioMasterEditorViewModel.InitializeNew();
        }
        _radioMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _radioMasterEditorViewModel.OnSave = (radio) =>
        {
            // Save to database
            Services.DatabaseService.Instance.SaveRadio(radio);

            if (_configurationEditorViewModel != null)
            {
                // Refresh list from database and select the radio
                _configurationEditorViewModel.Radios.Clear();
                foreach (var r in Services.DatabaseService.Instance.GetAllRadios())
                    _configurationEditorViewModel.Radios.Add(r);

                _configurationEditorViewModel.SelectedRadio = _configurationEditorViewModel.Radios
                    .FirstOrDefault(r => r.Manufacturer.Equals(radio.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                                         r.Model.Equals(radio.Model, StringComparison.OrdinalIgnoreCase));
            }
            CurrentView = _configurationEditorViewModel;
        };
        CurrentView = _radioMasterEditorViewModel;
        var projectName = GetProjectDisplayName();
        Breadcrumb = $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.RadioDetails}";
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit Radio" : "Swiss NIS Calculator - Add Radio";
    }

    private void NavigateToLinearEditorFromConfig(NIS.Desktop.Models.Radio? existing)
    {
        _radioMasterEditorViewModel = new RadioMasterEditorViewModel();
        if (existing != null)
        {
            _radioMasterEditorViewModel.InitializeEdit(existing);
            _radioMasterEditorViewModel.IsReadOnly = !existing.IsUserData;
        }
        else
        {
            _radioMasterEditorViewModel.InitializeNew();
        }
        _radioMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _radioMasterEditorViewModel.OnSave = (radio) =>
        {
            Services.DatabaseService.Instance.SaveRadio(radio);

            if (_configurationEditorViewModel != null)
            {
                _configurationEditorViewModel.Radios.Clear();
                foreach (var r in Services.DatabaseService.Instance.GetAllRadios())
                    _configurationEditorViewModel.Radios.Add(r);

                _configurationEditorViewModel.SelectedLinear = _configurationEditorViewModel.Radios
                    .FirstOrDefault(r => r.Manufacturer.Equals(radio.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                                         r.Model.Equals(radio.Model, StringComparison.OrdinalIgnoreCase));
            }
            CurrentView = _configurationEditorViewModel;
        };
        CurrentView = _radioMasterEditorViewModel;
        var projectName = GetProjectDisplayName();
        Breadcrumb = $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.RadioDetails}";
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit Linear" : "Swiss NIS Calculator - Add Linear";
    }

    private void NavigateToOkaEditorFromConfig(NIS.Desktop.Models.Oka? existing)
    {
        _okaMasterEditorViewModel = new OkaMasterEditorViewModel();
        if (existing != null)
        {
            _okaMasterEditorViewModel.InitializeEdit(existing);
        }
        else
        {
            _okaMasterEditorViewModel.InitializeNew();
        }
        _okaMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _okaMasterEditorViewModel.OnSave = (oka) =>
        {
            Services.DatabaseService.Instance.SaveOka(oka);
            // Refresh OKA list from database and select the new/updated entry
            if (_configurationEditorViewModel != null)
            {
                _configurationEditorViewModel.Okas.Clear();
                foreach (var dbOka in Services.DatabaseService.Instance.GetAllOkas())
                {
                    _configurationEditorViewModel.Okas.Add(dbOka);
                }
                _configurationEditorViewModel.SelectedOka = _configurationEditorViewModel.Okas
                    .FirstOrDefault(o => o.Name.Equals(oka.Name, StringComparison.OrdinalIgnoreCase));
            }
            CurrentView = _configurationEditorViewModel;
        };
        CurrentView = _okaMasterEditorViewModel;
        var projectName = GetProjectDisplayName();
        Breadcrumb = $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.OkaDetails}";
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit OKA" : "Swiss NIS Calculator - Add OKA";
    }

    // Global Save command (Ctrl+S) - saves to database immediately
    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        if (!HasProject) return;
        AutoSaveProjectIfDirty();
    }

    private bool CanSave() => HasProject;

    // SaveAs kept for backward compatibility but now just triggers Save
    [RelayCommand(CanExecute = nameof(CanSave))]
    private void SaveAs()
    {
        Save();
    }
    /// <summary>
    /// Shows project selection dialog and loads the selected project.
    /// </summary>
    private async Task ShowProjectSelectionAsync()
    {
        var projects = Services.DatabaseService.Instance.GetProjectList();
        if (projects.Count == 0)
        {
            // No projects, navigate to new project
            NavigateToProjectInfo(Strings.Instance.Language);
            return;
        }

        if (projects.Count == 1)
        {
            // Only one project, load it directly
            await LoadProjectFromDatabaseAsync(projects[0].Id);
            return;
        }

        // Multiple projects - navigate to welcome where user can select
        NavigateToWelcome();
    }

    /// <summary>
    /// Deletes the current project after confirmation.
    /// </summary>
    private async Task DeleteCurrentProjectAsync()
    {
        if (!HasProject)
        {
            // No current project, show project list to delete from
            NavigateToWelcome();
            return;
        }

        var projectName = ProjectViewModel.ProjectName;
        if (ShowConfirmDialog != null)
        {
            var message = string.Format(Strings.Instance.DeleteProjectMessage, projectName);
            var confirmed = await ShowConfirmDialog(Strings.Instance.DeleteProjectConfirm, message);
            if (!confirmed) return;
        }

        // Delete from database if it exists there
        if (ProjectViewModel.ProjectId > 0)
        {
            Services.DatabaseService.Instance.DeleteProject(ProjectViewModel.ProjectId);
        }

        // Reset current project
        HasProject = false;
        ProjectViewModel.NewProject();
        NavigateToWelcome();
    }

    /// <summary>
    /// Loads a project from the database by ID.
    /// </summary>
    private async Task LoadProjectFromDatabaseAsync(int projectId)
    {
        var project = Services.DatabaseService.Instance.GetProject(projectId);
        if (project != null)
        {
            ProjectViewModel.LoadFromProject(project, projectId);
            HasProject = true;
            NavigateToProjectOverview();
        }
        await Task.CompletedTask;
    }

    /// <summary>
    /// Select folder for database export.
    /// </summary>
    private async Task<string?> SelectFileForFactoryExport()
    {
        if (StorageProvider == null) return null;

        var startFolder = await StorageProvider.TryGetFolderFromPathAsync(Services.AppPaths.ExportFolder);
        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Export Factory Data",
            SuggestedStartLocation = startFolder,
            SuggestedFileName = $"NIS_FactoryData_{DateTime.Now:yyyyMMdd}.json",
            DefaultExtension = ".json",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
            }
        });

        return file?.Path.LocalPath;
    }

    /// <summary>
    /// Select file for factory data import.
    /// </summary>
    private async Task<string?> SelectFileForFactoryImport()
    {
        if (StorageProvider == null) return null;

        var startFolder = await StorageProvider.TryGetFolderFromPathAsync(Services.AppPaths.DataFolder);
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Import Factory Data",
            SuggestedStartLocation = startFolder,
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
            }
        });

        return files.FirstOrDefault()?.Path.LocalPath;
    }
}
