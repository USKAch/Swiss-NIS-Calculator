using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Localization;
using NIS.Desktop.Models;
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

    private Func<string, string, Task<bool>>? _showConfirmDialog;

    // Dialog callback (set by App.axaml.cs)
    public new Func<string, string, Task<bool>>? ShowConfirmDialog
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

    /// <summary>
    /// Helper to set the current view with breadcrumb and window title.
    /// </summary>
    private void SetView(ViewModelBase viewModel, string breadcrumb, string windowTitle)
    {
        CurrentView = viewModel;
        Breadcrumb = breadcrumb;
        WindowTitle = windowTitle;
    }

    /// <summary>
    /// Helper to navigate back to master data manager on a specific tab.
    /// </summary>
    private void ReturnToMasterDataManager(int tabIndex)
    {
        if (_masterDataManagerViewModel != null)
        {
            _masterDataManagerViewModel.SelectedTabIndex = tabIndex;
            var title = _masterDataManagerViewModel.IsAdminMode
                ? "Swiss NIS Calculator - Master Data (Admin)"
                : "Swiss NIS Calculator - Master Data";
            SetView(_masterDataManagerViewModel, Strings.Instance.MasterData, title);
        }
    }

    /// <summary>
    /// Helper to get master data editor window title.
    /// </summary>
    private static string GetEditorTitle(string entityName, object? existing, bool isReadOnly)
    {
        if (isReadOnly) return $"Swiss NIS Calculator - View {entityName}";
        return existing != null
            ? $"Swiss NIS Calculator - Edit {entityName}"
            : $"Swiss NIS Calculator - Add {entityName}";
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
                _ = ImportProjectFileAsync();
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
                if (HasProject) _ = ExportPdfDirectly();
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
            case "ImportProject":
                _ = ImportProjectFileAsync();
                break;
            case "ExportProject":
                if (HasProject) _ = ExportProjectFileAsync();
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
        SetView(_projectListViewModel, Strings.Instance.Home, "Swiss NIS Calculator");
        HasProject = false;
    }

    [RelayCommand]
    public void NavigateToWelcome()
    {
        _welcomeViewModel ??= new WelcomeViewModel();
        _welcomeViewModel.NavigateToProjectInfo = NavigateToProjectInfo;
        _welcomeViewModel.NavigateToProjectOverview = NavigateToProjectOverviewAfterOpen;
        _welcomeViewModel.NavigateToMasterData = (isAdminMode) => NavigateToMasterDataManager(isAdminMode);
        _welcomeViewModel.LoadProjectFromDatabase = async (id) => await LoadProjectFromDatabaseAsync(id);
        _welcomeViewModel.ProjectViewModel = ProjectViewModel;
        _welcomeViewModel.ShowConfirmDialog = ShowConfirmDialog;
        _welcomeViewModel.RefreshProjects();
        SetView(_welcomeViewModel, Strings.Instance.Home, "Swiss NIS Calculator");
    }

    // LoadLastProjectAsync removed - database is source of truth

    public void NavigateToProjectInfo(string _)
    {
        _projectInfoViewModel = new ProjectInfoViewModel();
        _projectInfoViewModel.NavigateBack = NavigateToProjectList;
        _projectInfoViewModel.NavigateToProjectOverview = NavigateToProjectOverviewAfterCreate;
        _projectInfoViewModel.ShowConfirmDialog = ShowConfirmDialog;
        SetView(_projectInfoViewModel, $"{Strings.Instance.Home} > {Strings.Instance.CreateProject}", "Swiss NIS Calculator");
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
        var projectName = GetProjectDisplayName();
        SetView(_projectOverviewViewModel, $"{Strings.Instance.Project} > {projectName}", $"Swiss NIS Calculator - {projectName}");
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
        _projectInfoViewModel.IsDirty = false; // Reset after loading
        _projectInfoViewModel.NavigateBack = NavigateToProjectOverview;
        _projectInfoViewModel.ShowConfirmDialog = ShowConfirmDialog;
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
        var projectName = GetProjectDisplayName();
        SetView(_projectInfoViewModel, $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.ProjectInfo}", $"Swiss NIS Calculator - {projectName}");
    }

    public void NavigateToConfigurationEditor(NIS.Desktop.Models.AntennaConfiguration? existing = null)
    {
        _configurationEditorViewModel = new ConfigurationEditorViewModel();
        _configurationEditorViewModel.MarkProjectDirty = ProjectViewModel.MarkDirty;
        _configurationEditorViewModel.ShowConfirmDialog = ShowConfirmDialog;

        // All master data comes from DatabaseService (single source of truth)

        if (existing != null)
        {
            var index = ProjectViewModel.Project.AntennaConfigurations.IndexOf(existing);
            _configurationEditorViewModel.ConfigurationNumber = index + 1;
            _configurationEditorViewModel.LoadFromConfiguration(existing);
            _configurationEditorViewModel.IsDirty = false; // Reset after loading
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
        var projectName = GetProjectDisplayName();
        SetView(_configurationEditorViewModel, $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.Configuration} {_configurationEditorViewModel.ConfigurationNumber}", $"Swiss NIS Calculator - {projectName}");
    }

    public void NavigateToAntennaSelector()
    {
        _antennaEditorViewModel = new AntennaEditorViewModel();
        _antennaEditorViewModel.RefreshAntennaList();
        _antennaEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _antennaEditorViewModel.OnSelect = (antenna) =>
        {
            if (_configurationEditorViewModel != null)
                _configurationEditorViewModel.SelectedAntenna = antenna;
            CurrentView = _configurationEditorViewModel;
        };
        _antennaEditorViewModel.NavigateToAddNew = NavigateToAntennaEditorForProject;
        var projectName = GetProjectDisplayName();
        SetView(_antennaEditorViewModel, $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.SelectAntennaPrompt}", "Swiss NIS Calculator - Select Antenna");
    }

    private void NavigateToAntennaEditorForProject()
    {
        var editorVm = new AntennaMasterEditorViewModel();
        editorVm.InitializeNew();
        editorVm.NavigateBack = NavigateToAntennaSelector;
        editorVm.OnSave = (antenna) =>
        {
            Services.DatabaseService.Instance.SaveAntenna(antenna);
            _antennaEditorViewModel?.AddAntennaToList(antenna);
            _antennaEditorViewModel?.OnSelect?.Invoke(antenna);
        };
        var projectName = GetProjectDisplayName();
        SetView(editorVm, $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.AntennaDetails}", "Swiss NIS Calculator - Add New Antenna");
    }

    public void NavigateToResults()
    {
        _resultsViewModel = new ResultsViewModel();
        _resultsViewModel.NavigateBack = NavigateToProjectOverview;
        _ = _resultsViewModel.CalculateAsync(ProjectViewModel.Project);
        var projectName = GetProjectDisplayName();
        SetView(_resultsViewModel, $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.CalculationResults}", $"Swiss NIS Calculator - {projectName}");
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
        SetView(_masterDataManagerViewModel, Strings.Instance.MasterData, "Swiss NIS Calculator");
    }

    [RelayCommand]
    public void NavigateToProject()
    {
        if (HasProject) NavigateToProjectOverview();
    }

    [RelayCommand]
    public void NavigateToSettings()
    {
        AutoSaveProjectIfDirty();
        _settingsViewModel ??= new SettingsViewModel();
        SetView(_settingsViewModel, Strings.Instance.Settings, "Swiss NIS Calculator");
    }


    private async Task ExportPdfDirectly()
    {
        if (StorageProvider == null || ProjectViewModel?.Project == null) return;

        var project = ProjectViewModel.Project;
        var reportLabel = !string.IsNullOrWhiteSpace(project.Name)
            ? project.Name
            : project.Operator ?? "Project";

        // Use ResultsViewModel to calculate and generate PDF
        var resultsVm = new ResultsViewModel();
        await resultsVm.CalculateAsync(project);

        if (resultsVm.Results.Count == 0) return;

        // Show file picker
        var startFolder = await StorageProvider.TryGetFolderFromPathAsync(Services.AppPaths.ExportFolder);
        var file = await StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
        {
            Title = "Export PDF Report",
            SuggestedStartLocation = startFolder,
            SuggestedFileName = $"{reportLabel}_{DateTime.Now:yyyyMMdd}.pdf",
            DefaultExtension = ".pdf",
            FileTypeChoices = new[]
            {
                new Avalonia.Platform.Storage.FilePickerFileType("PDF") { Patterns = new[] { "*.pdf" } }
            }
        });

        if (file != null)
        {
            try
            {
                var pdfGenerator = new Services.PdfReportGenerator();
                pdfGenerator.GenerateReport(project, resultsVm.Results.ToList(), file.Path.LocalPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PDF export failed: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Import a project from a .nisproj file.
    /// </summary>
    private async Task ImportProjectFileAsync()
    {
        if (StorageProvider == null) return;

        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = Strings.ImportProject,
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("NIS Project") { Patterns = new[] { "*.nisproj" } },
                new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
            }
        });

        if (files.Count > 0)
        {
            try
            {
                var json = await File.ReadAllTextAsync(files[0].Path.LocalPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };
                var projectFile = JsonSerializer.Deserialize<NisProjectFile>(json, options);

                if (projectFile != null)
                {
                    // Import included master data first and get ID mappings (file ID -> database ID)
                    var mapping = ImportIncludedMasterData(projectFile.MasterData);

                    var project = new Project
                    {
                        Name = projectFile.Project.Name,
                        Operator = projectFile.Project.Operator,
                        Callsign = projectFile.Project.Callsign,
                        Address = projectFile.Project.Address,
                        Location = projectFile.Project.Location
                    };

                    foreach (var config in projectFile.Configurations)
                    {
                        // Ensure any missing master data is created as placeholders
                        EnsureMasterDataExists(config);

                        // Map file IDs to database IDs using the mapping
                        // For user data: use ID mapping from imported master data
                        // For factory data: look up by name (factory data has same names everywhere)
                        int? antennaId = null;
                        if (config.AntennaId > 0 && mapping.Antennas.TryGetValue(config.AntennaId, out var mappedAntennaId))
                        {
                            antennaId = mappedAntennaId;
                        }
                        else
                        {
                            antennaId = DatabaseService.Instance.GetAntennaId(config.Antenna.Manufacturer, config.Antenna.Model);
                        }

                        int? cableId = null;
                        if (config.CableId > 0 && mapping.Cables.TryGetValue(config.CableId, out var mappedCableId))
                        {
                            cableId = mappedCableId;
                        }
                        else
                        {
                            cableId = DatabaseService.Instance.GetCableId(config.Cable.Name);
                        }

                        int? radioId = null;
                        if (config.RadioId > 0 && mapping.Radios.TryGetValue(config.RadioId, out var mappedRadioId))
                        {
                            radioId = mappedRadioId;
                        }
                        else
                        {
                            radioId = DatabaseService.Instance.GetRadioId(config.Radio.Manufacturer, config.Radio.Model);
                        }

                        int? okaId = null;
                        string okaName = string.Empty;
                        if (config.Oka.Id > 0 && mapping.Okas.TryGetValue(config.Oka.Id, out var mappedOkaId))
                        {
                            okaId = mappedOkaId;
                            var oka = DatabaseService.Instance.GetOkaById(mappedOkaId);
                            okaName = oka?.Name ?? string.Empty;
                        }

                        // Modulation is factory data only - look up by name
                        var modulation = DatabaseService.Instance.GetModulationByName(config.Modulation);

                        project.AntennaConfigurations.Add(new AntennaConfiguration
                        {
                            Name = $"{config.Antenna.Manufacturer} {config.Antenna.Model}".Trim(),
                            PowerWatts = config.PowerWatts,
                            // Set all IDs for master data references
                            RadioId = radioId,
                            Radio = new RadioConfig { Manufacturer = config.Radio.Manufacturer, Model = config.Radio.Model },
                            Linear = config.Linear == null ? null : new LinearConfig
                            {
                                Name = config.Linear.Name,
                                PowerWatts = config.Linear.PowerWatts
                            },
                            CableId = cableId,
                            Cable = new CableConfig
                            {
                                Type = config.Cable.Name,
                                LengthMeters = config.CableLengthMeters,
                                AdditionalLossDb = config.AdditionalLossDb,
                                AdditionalLossDescription = config.AdditionalLossDescription
                            },
                            AntennaId = antennaId,
                            Antenna = new AntennaPlacement
                            {
                                Manufacturer = config.Antenna.Manufacturer,
                                Model = config.Antenna.Model,
                                HeightMeters = config.HeightMeters,
                                IsRotatable = config.RotationAngleDegrees == 360,
                                HorizontalAngleDegrees = config.RotationAngleDegrees
                            },
                            ModulationId = modulation?.Id,
                            Modulation = config.Modulation,
                            ActivityFactor = config.ActivityFactor,
                            OkaId = okaId,
                            OkaName = okaName
                        });
                    }

                    var projectId = DatabaseService.Instance.CreateProject(project);

                    // Validate FK integrity after import
                    var integrityIssues = DatabaseService.Instance.ValidateConfigurationIntegrity();
                    if (integrityIssues.Count > 0)
                    {
                        var issueList = string.Join("\n", integrityIssues.Take(10));
                        if (integrityIssues.Count > 10)
                            issueList += $"\n... and {integrityIssues.Count - 10} more";

                        await MsBox.Avalonia.MessageBoxManager
                            .GetMessageBoxStandard(
                                Strings.Instance.ImportWarning,
                                $"{Strings.Instance.MissingMasterData}\n\n{issueList}",
                                MsBox.Avalonia.Enums.ButtonEnum.Ok,
                                MsBox.Avalonia.Enums.Icon.Warning)
                            .ShowAsync();
                    }

                    // Refresh project lists
                    _welcomeViewModel?.RefreshProjects();
                    _projectListViewModel?.RefreshProjects();

                    // Load and navigate to the imported project
                    await LoadProjectFromDatabaseAsync(projectId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Import failed: {ex.Message}");
                await MsBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandard(
                        Strings.ImportFailed,
                        ex.Message,
                        MsBox.Avalonia.Enums.ButtonEnum.Ok,
                        MsBox.Avalonia.Enums.Icon.Error)
                    .ShowAsync();
            }
        }
    }

    /// <summary>
    /// Export the current project to a .nisproj file.
    /// </summary>
    private async Task ExportProjectFileAsync()
    {
        if (StorageProvider == null || !HasProject) return;

        var project = ProjectViewModel.Project;
        var suggestedName = !string.IsNullOrWhiteSpace(project.Name)
            ? project.Name
            : !string.IsNullOrWhiteSpace(project.Operator)
                ? project.Operator
                : "project";

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = Strings.ExportProject,
            SuggestedFileName = $"{suggestedName}.nisproj",
            DefaultExtension = ".nisproj",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("NIS Project") { Patterns = new[] { "*.nisproj" } }
            }
        });

        if (file != null)
        {
            try
            {
                var projectFile = new NisProjectFile
                {
                    Project = new ProjectFileHeader
                    {
                        Name = project.Name,
                        Operator = project.Operator,
                        Callsign = project.Callsign,
                        Address = project.Address,
                        Location = project.Location
                    },
                    Configurations = project.AntennaConfigurations.Select(c =>
                    {
                        // Use ID-first lookups
                        var antenna = c.AntennaId.HasValue
                            ? DatabaseService.Instance.GetAntennaById(c.AntennaId.Value)
                            : null;
                        return new ProjectFileConfiguration
                        {
                            // Include source IDs for import mapping
                            AntennaId = c.AntennaId ?? 0,
                            RadioId = c.RadioId ?? 0,
                            CableId = c.CableId ?? 0,
                            ModulationId = c.ModulationId ?? 0,
                            // Include names for cross-system compatibility
                            Antenna = new ProjectFileReference
                            {
                                Manufacturer = c.Antenna.Manufacturer,
                                Model = c.Antenna.Model
                            },
                            HeightMeters = c.Antenna.HeightMeters,
                            Polarization = antenna?.IsHorizontallyPolarized == false ? "vertical" : "horizontal",
                            RotationAngleDegrees = c.Antenna.HorizontalAngleDegrees,
                            Radio = new ProjectFileReference
                            {
                                Manufacturer = c.Radio.Manufacturer,
                                Model = c.Radio.Model
                            },
                            Linear = c.Linear == null ? null : new ProjectFileLinearReference
                            {
                                Name = c.Linear.Name,
                                PowerWatts = c.Linear.PowerWatts
                            },
                            PowerWatts = c.PowerWatts,
                            Cable = new ProjectFileCableReference { Name = c.Cable.Type },
                            CableLengthMeters = c.Cable.LengthMeters,
                            AdditionalLossDb = c.Cable.AdditionalLossDb,
                            AdditionalLossDescription = c.Cable.AdditionalLossDescription,
                            Modulation = c.Modulation,
                            ActivityFactor = c.ActivityFactor,
                            Oka = new ProjectFileOkaReference { Id = c.OkaId ?? 0 }
                            // Distance and damping come from OKA master data
                        };
                    }).ToList()
                };

                // Collect user-specific master data used in configurations (ID-only lookups)
                var db = DatabaseService.Instance;
                var masterData = new ProjectFileMasterData();
                var addedAntennas = new HashSet<int>();
                var addedCables = new HashSet<int>();
                var addedRadios = new HashSet<int>();
                var addedOkas = new HashSet<int>();

                foreach (var config in project.AntennaConfigurations)
                {
                    // Antenna - ID only
                    if (config.AntennaId.HasValue && !addedAntennas.Contains(config.AntennaId.Value))
                    {
                        var antenna = db.GetAntennaById(config.AntennaId.Value);
                        if (antenna != null && antenna.IsUserData)
                        {
                            masterData.Antennas.Add(antenna);
                            addedAntennas.Add(antenna.Id);
                        }
                    }

                    // Cable - ID only
                    if (config.CableId.HasValue && !addedCables.Contains(config.CableId.Value))
                    {
                        var cable = db.GetCableById(config.CableId.Value);
                        if (cable != null && cable.IsUserData)
                        {
                            masterData.Cables.Add(cable);
                            addedCables.Add(cable.Id);
                        }
                    }

                    // Radio - ID only
                    if (config.RadioId.HasValue && !addedRadios.Contains(config.RadioId.Value))
                    {
                        var radio = db.GetRadioById(config.RadioId.Value);
                        if (radio != null && radio.IsUserData)
                        {
                            masterData.Radios.Add(radio);
                            addedRadios.Add(radio.Id);
                        }
                    }

                    // OKA - ID only
                    if (config.OkaId.HasValue && !addedOkas.Contains(config.OkaId.Value))
                    {
                        var oka = db.GetOkaById(config.OkaId.Value);
                        if (oka != null && oka.IsUserData)
                        {
                            masterData.Okas.Add(oka);
                            addedOkas.Add(config.OkaId.Value);
                        }
                    }
                }

                // Always include master data section (empty arrays show the structure)
                projectFile.MasterData = masterData;

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };
                var json = JsonSerializer.Serialize(projectFile, options);
                await File.WriteAllTextAsync(file.Path.LocalPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Export failed: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Holds ID mappings from old (file) IDs to new (database) IDs for all entity types.
    /// </summary>
    private class MasterDataIdMapping
    {
        public Dictionary<int, int> Antennas { get; } = new();
        public Dictionary<int, int> Cables { get; } = new();
        public Dictionary<int, int> Radios { get; } = new();
        public Dictionary<int, int> Okas { get; } = new();
    }

    /// <summary>
    /// Import master data included in the project file.
    /// Returns mappings of old IDs to new IDs for all entity types.
    /// </summary>
    private MasterDataIdMapping ImportIncludedMasterData(ProjectFileMasterData? masterData)
    {
        var mapping = new MasterDataIdMapping();
        if (masterData == null) return mapping;

        var db = DatabaseService.Instance;

        // Import antennas and track ID mapping
        foreach (var antenna in masterData.Antennas)
        {
            int oldId = antenna.Id;
            if (!db.AntennaExists(antenna.Manufacturer, antenna.Model))
            {
                antenna.Id = 0;
                antenna.IsUserData = true;
                db.SaveAntenna(antenna);
            }
            var newId = db.GetAntennaId(antenna.Manufacturer, antenna.Model);
            if (newId.HasValue)
            {
                mapping.Antennas[oldId] = newId.Value;
            }
        }

        // Import cables and track ID mapping
        foreach (var cable in masterData.Cables)
        {
            int oldId = cable.Id;
            if (!db.CableExists(cable.Name))
            {
                cable.Id = 0;
                cable.IsUserData = true;
                db.SaveCable(cable);
            }
            var newId = db.GetCableId(cable.Name);
            if (newId.HasValue)
            {
                mapping.Cables[oldId] = newId.Value;
            }
        }

        // Import radios and track ID mapping
        foreach (var radio in masterData.Radios)
        {
            int oldId = radio.Id;
            if (!db.RadioExists(radio.Manufacturer, radio.Model))
            {
                radio.Id = 0;
                radio.IsUserData = true;
                db.SaveRadio(radio);
            }
            var newId = db.GetRadioId(radio.Manufacturer, radio.Model);
            if (newId.HasValue)
            {
                mapping.Radios[oldId] = newId.Value;
            }
        }

        // Import OKAs and track ID mapping
        foreach (var oka in masterData.Okas)
        {
            int oldId = oka.Id;
            if (!db.OkaExists(oka.Name))
            {
                oka.Id = 0;
                oka.IsUserData = true;
                db.SaveOka(oka);
            }
            var newId = db.GetOkaId(oka.Name);
            if (newId.HasValue)
            {
                mapping.Okas[oldId] = newId.Value;
            }
        }

        return mapping;
    }

    /// <summary>
    /// Ensure master data exists for a configuration, creating placeholders if missing.
    /// Called after ImportIncludedMasterData has already imported user data with ID mappings.
    /// </summary>
    private void EnsureMasterDataExists(ProjectFileConfiguration config)
    {
        var db = DatabaseService.Instance;

        // Validate modulation exists (factory data only)
        var modulation = db.GetModulationByName(config.Modulation);
        if (modulation == null)
        {
            throw new InvalidOperationException($"Unknown modulation '{config.Modulation}'.");
        }

        // Create placeholder if antenna doesn't exist
        if (!db.AntennaExists(config.Antenna.Manufacturer, config.Antenna.Model))
        {
            db.SaveAntenna(new Antenna
            {
                Manufacturer = config.Antenna.Manufacturer,
                Model = config.Antenna.Model,
                AntennaType = AntennaTypes.Other,
                IsHorizontallyPolarized = config.Polarization.Equals("horizontal", StringComparison.OrdinalIgnoreCase),
                IsUserData = true
            });
        }

        // Create placeholder if cable doesn't exist
        if (!db.CableExists(config.Cable.Name))
        {
            db.SaveCable(new Cable
            {
                Name = config.Cable.Name,
                IsUserData = true
            });
        }

        // Create placeholder if radio doesn't exist
        if (!db.RadioExists(config.Radio.Manufacturer, config.Radio.Model))
        {
            db.SaveRadio(new Radio
            {
                Manufacturer = config.Radio.Manufacturer,
                Model = config.Radio.Model,
                MaxPowerWatts = config.PowerWatts,
                IsUserData = true
            });
        }

        // OKA is handled by ID mapping - no placeholder creation
        // OKA must exist in the file's included master data or import will have null OkaId
    }

    #region Project File DTOs

    private class NisProjectFile
    {
        public ProjectFileHeader Project { get; set; } = new();
        public List<ProjectFileConfiguration> Configurations { get; set; } = new();
        public ProjectFileMasterData? MasterData { get; set; }
    }

    private class ProjectFileMasterData
    {
        public List<Antenna> Antennas { get; set; } = new();
        public List<Cable> Cables { get; set; } = new();
        public List<Radio> Radios { get; set; } = new();
        public List<Oka> Okas { get; set; } = new();
    }

    private class ProjectFileHeader
    {
        public string Name { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string Callsign { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    private class ProjectFileConfiguration
    {
        // Master data IDs (for import mapping)
        public int AntennaId { get; set; }
        public int RadioId { get; set; }
        public int CableId { get; set; }
        public int ModulationId { get; set; }

        public ProjectFileReference Antenna { get; set; } = new();
        public double HeightMeters { get; set; }
        public string Polarization { get; set; } = "horizontal";
        public double RotationAngleDegrees { get; set; } = 360;
        public ProjectFileReference Radio { get; set; } = new();
        public ProjectFileLinearReference? Linear { get; set; }
        public double PowerWatts { get; set; }
        public ProjectFileCableReference Cable { get; set; } = new();
        public double CableLengthMeters { get; set; }
        public double AdditionalLossDb { get; set; }
        public string AdditionalLossDescription { get; set; } = string.Empty;
        public string Modulation { get; set; } = "CW";
        public double ActivityFactor { get; set; } = 0.5;
        public ProjectFileOkaReference Oka { get; set; } = new();
        // Distance and damping come from OKA master data
    }

    private class ProjectFileReference
    {
        public string Manufacturer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
    }

    private class ProjectFileLinearReference
    {
        public string Name { get; set; } = string.Empty;
        public double PowerWatts { get; set; }
    }

    private class ProjectFileCableReference
    {
        public string Name { get; set; } = string.Empty;
    }

    private class ProjectFileOkaReference
    {
        public int Id { get; set; }
    }

    #endregion

    public void NavigateToAntennaMasterEditor(NIS.Desktop.Models.Antenna? existing, bool isReadOnly = false)
    {
        _antennaMasterEditorViewModel = new AntennaMasterEditorViewModel();
        _antennaMasterEditorViewModel.ShowConfirmDialog = ShowConfirmDialog;
        if (existing != null)
        {
            _antennaMasterEditorViewModel.InitializeEdit(existing);
            _antennaMasterEditorViewModel.IsDirty = false;
        }
        else
        {
            _antennaMasterEditorViewModel.InitializeNew();
        }
        _antennaMasterEditorViewModel.IsReadOnly = isReadOnly;
        _antennaMasterEditorViewModel.NavigateBack = () => ReturnToMasterDataManager(0);
        _antennaMasterEditorViewModel.OnSave = (antenna) =>
        {
            if (_antennaMasterEditorViewModel!.IsEditing)
                _masterDataManagerViewModel?.UpdateAntennaInDatabase(antenna);
            else
            {
                var success = _masterDataManagerViewModel?.AddAntennaToDatabase(antenna) ?? false;
                if (!success)
                {
                    _antennaMasterEditorViewModel.ValidationMessage = Strings.Instance.DuplicateNameError;
                    return;
                }
            }
            ReturnToMasterDataManager(0);
        };
        SetView(_antennaMasterEditorViewModel, $"{Strings.Instance.MasterData} > {Strings.Instance.AntennaDetails}", GetEditorTitle("Antenna", existing, isReadOnly));
    }

    public void NavigateToCableMasterEditor(NIS.Desktop.Models.Cable? existing, bool isReadOnly = false)
    {
        _cableMasterEditorViewModel = new CableMasterEditorViewModel();
        _cableMasterEditorViewModel.ShowConfirmDialog = ShowConfirmDialog;
        if (existing != null)
        {
            _cableMasterEditorViewModel.InitializeEdit(existing);
            _cableMasterEditorViewModel.IsDirty = false;
        }
        else
        {
            _cableMasterEditorViewModel.InitializeNew();
        }
        _cableMasterEditorViewModel.IsReadOnly = isReadOnly;
        _cableMasterEditorViewModel.NavigateBack = () => ReturnToMasterDataManager(1);
        _cableMasterEditorViewModel.OnSave = (cable) =>
        {
            if (_cableMasterEditorViewModel!.IsEditing)
                _masterDataManagerViewModel?.UpdateCableInDatabase(cable);
            else
            {
                var success = _masterDataManagerViewModel?.AddCableToDatabase(cable) ?? false;
                if (!success)
                {
                    _cableMasterEditorViewModel.ValidationMessage = Strings.Instance.DuplicateNameError;
                    return;
                }
            }
            ReturnToMasterDataManager(1);
        };
        SetView(_cableMasterEditorViewModel, $"{Strings.Instance.MasterData} > {Strings.Instance.Cable}", GetEditorTitle("Cable", existing, isReadOnly));
    }

    public void NavigateToRadioMasterEditor(NIS.Desktop.Models.Radio? existing, bool isReadOnly = false)
    {
        _radioMasterEditorViewModel = new RadioMasterEditorViewModel();
        _radioMasterEditorViewModel.ShowConfirmDialog = ShowConfirmDialog;
        if (existing != null)
        {
            _radioMasterEditorViewModel.InitializeEdit(existing);
            _radioMasterEditorViewModel.IsDirty = false;
        }
        else
        {
            _radioMasterEditorViewModel.InitializeNew();
        }
        _radioMasterEditorViewModel.IsReadOnly = isReadOnly;
        _radioMasterEditorViewModel.NavigateBack = () => ReturnToMasterDataManager(2);
        _radioMasterEditorViewModel.OnSave = (radio) =>
        {
            if (_radioMasterEditorViewModel!.IsEditing)
                _masterDataManagerViewModel?.UpdateRadioInDatabase(radio);
            else
            {
                var success = _masterDataManagerViewModel?.AddRadioToDatabase(radio) ?? false;
                if (!success)
                {
                    _radioMasterEditorViewModel.ValidationMessage = Strings.Instance.DuplicateNameError;
                    return;
                }
            }
            ReturnToMasterDataManager(2);
        };
        SetView(_radioMasterEditorViewModel, $"{Strings.Instance.MasterData} > {Strings.Instance.RadioDetails}", GetEditorTitle("Radio", existing, isReadOnly));
    }

    public void NavigateToOkaMasterEditor(NIS.Desktop.Models.Oka? existing)
    {
        _okaMasterEditorViewModel = new OkaMasterEditorViewModel();
        _okaMasterEditorViewModel.ShowConfirmDialog = ShowConfirmDialog;
        if (existing != null)
        {
            _okaMasterEditorViewModel.InitializeEdit(existing);
            _okaMasterEditorViewModel.IsDirty = false;
        }
        else
        {
            _okaMasterEditorViewModel.InitializeNew();
        }
        _okaMasterEditorViewModel.NavigateBack = () => ReturnToMasterDataManager(3);
        _okaMasterEditorViewModel.OnSave = (oka) =>
        {
            if (_okaMasterEditorViewModel!.IsEditing)
                _masterDataManagerViewModel?.UpdateOkaInDatabase(oka);
            else
            {
                var success = _masterDataManagerViewModel?.AddOkaToDatabase(oka) ?? false;
                if (!success)
                {
                    _okaMasterEditorViewModel.ValidationMessage = Strings.Instance.DuplicateNameError;
                    return;
                }
            }
            ReturnToMasterDataManager(3);
        };
        SetView(_okaMasterEditorViewModel, $"{Strings.Instance.MasterData} > {Strings.Instance.OkaDetails}", GetEditorTitle("OKA", existing, false));
    }

    // Navigation from Configuration Editor to Master Editors
    // These return to the Configuration Editor after save

    /// <summary>
    /// Refresh a collection from database and select an item by predicate.
    /// </summary>
    private void RefreshAndSelect<T>(
        System.Collections.ObjectModel.ObservableCollection<T> collection,
        Func<IEnumerable<T>> getAll,
        Func<T, bool> predicate,
        Action<T?> setSelected) where T : class
    {
        collection.Clear();
        foreach (var item in getAll())
            collection.Add(item);
        setSelected(collection.FirstOrDefault(predicate));
    }

    private void NavigateToAntennaEditorFromConfig(NIS.Desktop.Models.Antenna? existing)
    {
        _antennaMasterEditorViewModel = new AntennaMasterEditorViewModel();
        if (existing != null)
        {
            _antennaMasterEditorViewModel.InitializeEdit(existing);
            _antennaMasterEditorViewModel.IsReadOnly = !existing.IsUserData;
        }
        else
        {
            _antennaMasterEditorViewModel.InitializeNew();
        }
        _antennaMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _antennaMasterEditorViewModel.OnSave = (antenna) =>
        {
            Services.DatabaseService.Instance.SaveAntenna(antenna);
            if (_configurationEditorViewModel != null)
            {
                RefreshAndSelect(
                    _configurationEditorViewModel.Antennas,
                    Services.DatabaseService.Instance.GetAllAntennas,
                    a => a.Manufacturer.Equals(antenna.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                         a.Model.Equals(antenna.Model, StringComparison.OrdinalIgnoreCase),
                    a => _configurationEditorViewModel.SelectedAntenna = a);
            }
            CurrentView = _configurationEditorViewModel;
        };
        var projectName = GetProjectDisplayName();
        SetView(_antennaMasterEditorViewModel, $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.AntennaDetails}", GetEditorTitle("Antenna", existing, false));
    }

    private void NavigateToCableEditorFromConfig(NIS.Desktop.Models.Cable? existing)
    {
        _cableMasterEditorViewModel = new CableMasterEditorViewModel();
        if (existing != null)
        {
            _cableMasterEditorViewModel.InitializeEdit(existing);
            _cableMasterEditorViewModel.IsReadOnly = !existing.IsUserData;
        }
        else
        {
            _cableMasterEditorViewModel.InitializeNew();
        }
        _cableMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _cableMasterEditorViewModel.OnSave = (cable) =>
        {
            Services.DatabaseService.Instance.SaveCable(cable);
            if (_configurationEditorViewModel != null)
            {
                RefreshAndSelect(
                    _configurationEditorViewModel.Cables,
                    Services.DatabaseService.Instance.GetAllCables,
                    c => c.Name.Equals(cable.Name, StringComparison.OrdinalIgnoreCase),
                    c => _configurationEditorViewModel.SelectedCable = c);
            }
            CurrentView = _configurationEditorViewModel;
        };
        var projectName = GetProjectDisplayName();
        SetView(_cableMasterEditorViewModel, $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.Cable}", GetEditorTitle("Cable", existing, false));
    }

    private void NavigateToRadioEditorFromConfig(NIS.Desktop.Models.Radio? existing)
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
                RefreshAndSelect(
                    _configurationEditorViewModel.Radios,
                    Services.DatabaseService.Instance.GetAllRadios,
                    r => r.Manufacturer.Equals(radio.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                         r.Model.Equals(radio.Model, StringComparison.OrdinalIgnoreCase),
                    r => _configurationEditorViewModel.SelectedRadio = r);
            }
            CurrentView = _configurationEditorViewModel;
        };
        var projectName = GetProjectDisplayName();
        SetView(_radioMasterEditorViewModel, $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.RadioDetails}", GetEditorTitle("Radio", existing, false));
    }

    private void NavigateToOkaEditorFromConfig(NIS.Desktop.Models.Oka? existing)
    {
        _okaMasterEditorViewModel = new OkaMasterEditorViewModel();
        if (existing != null)
            _okaMasterEditorViewModel.InitializeEdit(existing);
        else
            _okaMasterEditorViewModel.InitializeNew();
        _okaMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _okaMasterEditorViewModel.OnSave = (oka) =>
        {
            Services.DatabaseService.Instance.SaveOka(oka);
            if (_configurationEditorViewModel != null)
            {
                RefreshAndSelect(
                    _configurationEditorViewModel.Okas,
                    Services.DatabaseService.Instance.GetAllOkas,
                    o => o.Name.Equals(oka.Name, StringComparison.OrdinalIgnoreCase),
                    o => _configurationEditorViewModel.SelectedOka = o);
            }
            CurrentView = _configurationEditorViewModel;
        };
        var projectName = GetProjectDisplayName();
        SetView(_okaMasterEditorViewModel, $"{Strings.Instance.Project} > {projectName} > {Strings.Instance.OkaDetails}", GetEditorTitle("OKA", existing, false));
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
