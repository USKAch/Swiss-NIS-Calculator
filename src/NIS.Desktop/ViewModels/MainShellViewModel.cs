using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Localization;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// Main shell ViewModel that manages navigation between views.
/// </summary>
public partial class MainShellViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase? _currentView;

    [ObservableProperty]
    private string _windowTitle = "Swiss NIS Calculator";

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

    public MainShellViewModel()
    {
        // Start with welcome view
        NavigateToWelcome();
    }

    [RelayCommand]
    public void NavigateToWelcome()
    {
        _welcomeViewModel ??= new WelcomeViewModel();
        _welcomeViewModel.NavigateToProjectInfo = NavigateToProjectInfo;
        _welcomeViewModel.NavigateToProjectOverview = NavigateToProjectOverviewAfterOpen;
        _welcomeViewModel.NavigateToMasterData = NavigateToMasterDataManager;
        _welcomeViewModel.ProjectViewModel = ProjectViewModel;
        CurrentView = _welcomeViewModel;
        WindowTitle = "Swiss NIS Calculator";
    }

    public void NavigateToProjectInfo(string language)
    {
        _projectInfoViewModel = new ProjectInfoViewModel(language);
        _projectInfoViewModel.NavigateBack = NavigateToWelcome;
        _projectInfoViewModel.NavigateToProjectOverview = NavigateToProjectOverviewAfterCreate;
        CurrentView = _projectInfoViewModel;
        WindowTitle = "Swiss NIS Calculator - New Project";
    }

    private void NavigateToProjectOverviewAfterCreate(ProjectInfoViewModel vm)
    {
        // Create new project with station info
        ProjectViewModel.NewProject(vm.Language);
        ProjectViewModel.UpdateStationInfo(vm.ToStationInfo());

        // Update localization
        Strings.Instance.Language = vm.Language;

        NavigateToProjectOverview();
    }

    private void NavigateToProjectOverviewAfterOpen()
    {
        // Project already loaded by WelcomeViewModel
        // Update localization from project language
        Strings.Instance.Language = ProjectViewModel.Project.Language;

        NavigateToProjectOverview();
    }

    public void NavigateToProjectOverview()
    {
        _projectOverviewViewModel = new ProjectOverviewViewModel(ProjectViewModel);
        _projectOverviewViewModel.NavigateToConfigurationEditor = NavigateToConfigurationEditor;
        _projectOverviewViewModel.NavigateToResults = NavigateToResults;
        _projectOverviewViewModel.NavigateToProjectInfo = NavigateToEditProjectInfo;
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

        // Add project's custom antennas to the dropdown
        _configurationEditorViewModel.AddProjectAntennas(ProjectViewModel.Project.CustomAntennas);

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
        _configurationEditorViewModel.OnSave = (config) =>
        {
            // Copy selected antenna to project if not already there
            var selectedAntenna = _configurationEditorViewModel.SelectedAntenna;
            if (selectedAntenna != null)
            {
                var existingAntenna = ProjectViewModel.Project.CustomAntennas.FirstOrDefault(a =>
                    a.Manufacturer.Equals(selectedAntenna.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                    a.Model.Equals(selectedAntenna.Model, StringComparison.OrdinalIgnoreCase));
                if (existingAntenna == null)
                {
                    ProjectViewModel.Project.CustomAntennas.Add(selectedAntenna);
                }
            }

            // Copy selected cable to project if not already there
            var selectedCable = _configurationEditorViewModel.SelectedCable;
            if (selectedCable != null)
            {
                var existingCable = ProjectViewModel.Project.CustomCables.FirstOrDefault(c =>
                    c.Name.Equals(selectedCable.Name, StringComparison.OrdinalIgnoreCase));
                if (existingCable == null)
                {
                    ProjectViewModel.Project.CustomCables.Add(selectedCable);
                }
            }

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
        _antennaEditorViewModel.ProjectAntennas = ProjectViewModel.Project.CustomAntennas;
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
            // Ensure antenna is in project's CustomAntennas for calculations
            var existingAntenna = ProjectViewModel.Project.CustomAntennas.FirstOrDefault(a =>
                a.Manufacturer.Equals(antenna.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                a.Model.Equals(antenna.Model, StringComparison.OrdinalIgnoreCase));
            if (existingAntenna == null)
            {
                ProjectViewModel.Project.CustomAntennas.Add(antenna);
                ProjectViewModel.MarkDirty();
            }
            CurrentView = _configurationEditorViewModel;
        };
        _antennaEditorViewModel.OnAddAntenna = (antenna) =>
        {
            // Add antenna to project's custom antennas
            ProjectViewModel.Project.CustomAntennas.Add(antenna);
            ProjectViewModel.MarkDirty();
        };
        CurrentView = _antennaEditorViewModel;
        WindowTitle = "Swiss NIS Calculator - Select Antenna";
    }

    public void NavigateToResults()
    {
        _resultsViewModel = new ResultsViewModel();
        _resultsViewModel.NavigateBack = NavigateToProjectOverview;
        _ = _resultsViewModel.CalculateAsync(ProjectViewModel.Project);
        CurrentView = _resultsViewModel;
        WindowTitle = "Swiss NIS Calculator - Results";
    }

    public void NavigateToMasterDataManager()
    {
        _masterDataManagerViewModel = new MasterDataManagerViewModel();
        _masterDataManagerViewModel.NavigateBack = NavigateToWelcome;
        _masterDataManagerViewModel.NavigateToAntennaEditor = NavigateToAntennaMasterEditor;
        _masterDataManagerViewModel.NavigateToCableEditor = NavigateToCableMasterEditor;
        _masterDataManagerViewModel.NavigateToRadioEditor = NavigateToRadioMasterEditor;
        CurrentView = _masterDataManagerViewModel;
        WindowTitle = "Swiss NIS Calculator - Master Data";
    }

    public void NavigateToAntennaMasterEditor(NIS.Core.Models.Antenna? existing)
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
        _antennaMasterEditorViewModel.NavigateBack = NavigateToMasterDataManager;
        _antennaMasterEditorViewModel.OnSave = (antenna) =>
        {
            _masterDataManagerViewModel?.AddAntennaToDatabase(antenna);
            NavigateToMasterDataManager();
        };
        CurrentView = _antennaMasterEditorViewModel;
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit Antenna" : "Swiss NIS Calculator - Add Antenna";
    }

    public void NavigateToCableMasterEditor(NIS.Core.Models.Cable? existing)
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
        _cableMasterEditorViewModel.NavigateBack = NavigateToMasterDataManager;
        _cableMasterEditorViewModel.OnSave = (cable) =>
        {
            _masterDataManagerViewModel?.AddCableToDatabase(cable);
            NavigateToMasterDataManager();
        };
        CurrentView = _cableMasterEditorViewModel;
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit Cable" : "Swiss NIS Calculator - Add Cable";
    }

    public void NavigateToRadioMasterEditor(NIS.Core.Models.Radio? existing)
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
        _radioMasterEditorViewModel.NavigateBack = NavigateToMasterDataManager;
        _radioMasterEditorViewModel.OnSave = (radio) =>
        {
            _masterDataManagerViewModel?.AddRadioToDatabase(radio);
            NavigateToMasterDataManager();
        };
        CurrentView = _radioMasterEditorViewModel;
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit Radio" : "Swiss NIS Calculator - Add Radio";
    }

    // Navigation from Configuration Editor to Master Editors
    // These return to the Configuration Editor after save

    private void NavigateToAntennaEditorFromConfig(NIS.Core.Models.Antenna? existing)
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
        _antennaMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _antennaMasterEditorViewModel.OnSave = (antenna) =>
        {
            if (_configurationEditorViewModel != null)
            {
                // Find existing antenna in collection
                var existingAntenna = _configurationEditorViewModel.Antennas.FirstOrDefault(a =>
                    a.Manufacturer.Equals(antenna.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                    a.Model.Equals(antenna.Model, StringComparison.OrdinalIgnoreCase));

                if (existingAntenna != null)
                {
                    // Update existing and select it
                    var index = _configurationEditorViewModel.Antennas.IndexOf(existingAntenna);
                    _configurationEditorViewModel.Antennas[index] = antenna;
                    _configurationEditorViewModel.SelectedAntenna = antenna;
                }
                else
                {
                    // Add new and select it
                    _configurationEditorViewModel.Antennas.Add(antenna);
                    _configurationEditorViewModel.SelectedAntenna = antenna;
                }
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
        }
        else
        {
            _cableMasterEditorViewModel.InitializeNew();
        }
        _cableMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _cableMasterEditorViewModel.OnSave = (cable) =>
        {
            // Add to config editor's cable list if not already there
            if (_configurationEditorViewModel != null)
            {
                var existingCable = _configurationEditorViewModel.Cables.FirstOrDefault(c =>
                    c.Name.Equals(cable.Name, StringComparison.OrdinalIgnoreCase));
                if (existingCable == null)
                {
                    _configurationEditorViewModel.Cables.Add(cable);
                }
                _configurationEditorViewModel.SelectedCable = cable;
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
        }
        else
        {
            _radioMasterEditorViewModel.InitializeNew();
        }
        _radioMasterEditorViewModel.NavigateBack = () => CurrentView = _configurationEditorViewModel;
        _radioMasterEditorViewModel.OnSave = (radio) =>
        {
            // Add to config editor's radio list if not already there
            if (_configurationEditorViewModel != null)
            {
                var existingRadio = _configurationEditorViewModel.Radios.FirstOrDefault(r =>
                    r.Manufacturer.Equals(radio.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
                    r.Model.Equals(radio.Model, StringComparison.OrdinalIgnoreCase));
                if (existingRadio == null)
                {
                    _configurationEditorViewModel.Radios.Add(radio);
                }
                _configurationEditorViewModel.SelectedRadio = radio;
            }
            CurrentView = _configurationEditorViewModel;
        };
        CurrentView = _radioMasterEditorViewModel;
        WindowTitle = existing != null ? "Swiss NIS Calculator - Edit Radio" : "Swiss NIS Calculator - Add Radio";
    }
}
