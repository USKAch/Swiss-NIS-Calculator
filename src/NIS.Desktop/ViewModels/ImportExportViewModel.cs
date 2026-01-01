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
/// ViewModel for the Import/Export view.
/// Handles project and master data import/export.
/// </summary>
public partial class ImportExportViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _statusMessage = string.Empty;

    /// <summary>
    /// When true, shows factory mode indicator and development tools.
    /// </summary>
    [ObservableProperty]
    private bool _isFactoryMode;

    // Storage provider for file dialogs
    public IStorageProvider? StorageProvider { get; set; }

    // Dialog callback
    public new Func<string, string, Task<bool>>? ShowConfirmDialog { get; set; }

    // Navigation callback
    public Action? NavigateBack { get; set; }

    // Callback to refresh project list after import
    public Action? OnProjectImported { get; set; }

    // Available projects for export
    public List<ProjectListItem> ProjectList => DatabaseService.Instance.GetProjectList();

    [ObservableProperty]
    private ProjectListItem? _selectedProject;

    public new Strings Strings => Strings.Instance;

    private class NisProjectFile
    {
        public ProjectHeader Project { get; set; } = new();
        public List<ProjectConfiguration> Configurations { get; set; } = new();
    }

    private class ProjectHeader
    {
        public string Name { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string Callsign { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    private class ProjectConfiguration
    {
        public Reference Antenna { get; set; } = new();
        public double HeightMeters { get; set; }
        public string Polarization { get; set; } = "horizontal";
        public double RotationAngleDegrees { get; set; } = 360;
        public Reference Radio { get; set; } = new();
        public LinearReference? Linear { get; set; }
        public double PowerWatts { get; set; }
        public CableReference Cable { get; set; } = new();
        public double CableLengthMeters { get; set; }
        public double AdditionalLossDb { get; set; }
        public string AdditionalLossDescription { get; set; } = string.Empty;
        public string Modulation { get; set; } = "CW";
        public double ActivityFactor { get; set; } = 0.5;
        public OkaReference Oka { get; set; } = new();
        public double OkaDistanceMeters { get; set; }
        public double OkaBuildingDampingDb { get; set; }
    }

    private class Reference
    {
        public string Manufacturer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
    }

    private class LinearReference
    {
        public string Name { get; set; } = string.Empty;
        public double PowerWatts { get; set; }
    }

    private class CableReference
    {
        public string Name { get; set; } = string.Empty;
    }

    private class OkaReference
    {
        public string Name { get; set; } = string.Empty;
    }

    [RelayCommand]
    private async Task ImportProject()
    {
        if (StorageProvider == null)
        {
            StatusMessage = "Import not available";
            return;
        }

        if (ShowConfirmDialog != null)
        {
            var confirmed = await ShowConfirmDialog(
                Strings.ImportProject,
                Strings.ImportProjectConfirmMessage);
            if (!confirmed) return;
        }

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
                        EnsureMasterData(config);

                        project.AntennaConfigurations.Add(new AntennaConfiguration
                        {
                            Name = $"{config.Antenna.Manufacturer} {config.Antenna.Model}".Trim(),
                            PowerWatts = config.PowerWatts,
                            Radio = new RadioConfig { Manufacturer = config.Radio.Manufacturer, Model = config.Radio.Model },
                            Linear = config.Linear == null ? null : new LinearConfig
                            {
                                Name = config.Linear.Name,
                                PowerWatts = config.Linear.PowerWatts
                            },
                            Cable = new CableConfig
                            {
                                Type = config.Cable.Name,
                                LengthMeters = config.CableLengthMeters,
                                AdditionalLossDb = config.AdditionalLossDb,
                                AdditionalLossDescription = config.AdditionalLossDescription
                            },
                            Antenna = new AntennaPlacement
                            {
                                Manufacturer = config.Antenna.Manufacturer,
                                Model = config.Antenna.Model,
                                HeightMeters = config.HeightMeters,
                                IsRotatable = config.RotationAngleDegrees != 360,
                                HorizontalAngleDegrees = config.RotationAngleDegrees
                            },
                            Modulation = config.Modulation,
                            ActivityFactor = config.ActivityFactor,
                            OkaName = config.Oka.Name,
                            OkaDistanceMeters = config.OkaDistanceMeters,
                            OkaBuildingDampingDb = config.OkaBuildingDampingDb
                        });
                    }

                    DatabaseService.Instance.CreateProject(project);
                    StatusMessage = $"{Strings.ImportSuccess}: {project.Name}";

                    // Notify that project was imported
                    OnProjectImported?.Invoke();
                    OnPropertyChanged(nameof(ProjectList));
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"{Strings.ImportFailed}: {ex.Message}";
            }
        }
    }

    private void EnsureMasterData(ProjectConfiguration config)
    {
        var db = DatabaseService.Instance;

        var modulation = db.GetModulationByName(config.Modulation);
        if (modulation == null)
        {
            throw new InvalidOperationException($"Unknown modulation '{config.Modulation}'.");
        }

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

        if (!db.CableExists(config.Cable.Name))
        {
            db.SaveCable(new Cable
            {
                Name = config.Cable.Name,
                IsUserData = true
            });
        }

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

        if (!db.OkaExists(config.Oka.Name))
        {
            db.SaveOka(new Oka
            {
                Name = config.Oka.Name,
                DefaultDistanceMeters = config.OkaDistanceMeters,
                DefaultDampingDb = config.OkaBuildingDampingDb,
                IsUserData = true
            });
        }
    }

    [RelayCommand]
    private async Task ExportProject()
    {
        if (StorageProvider == null)
        {
            StatusMessage = "Export not available";
            return;
        }

        if (SelectedProject == null)
        {
            StatusMessage = Strings.SelectProjectToExport;
            return;
        }

        var project = DatabaseService.Instance.GetProject(SelectedProject.Id);
        if (project == null)
        {
            StatusMessage = "Project not found";
            return;
        }

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
                    Project = new ProjectHeader
                    {
                        Name = project.Name,
                        Operator = project.Operator,
                        Callsign = project.Callsign,
                        Address = project.Address,
                        Location = project.Location
                    },
                    Configurations = project.AntennaConfigurations.Select(c =>
                    {
                        var antenna = DatabaseService.Instance.GetAntenna(c.Antenna.Manufacturer, c.Antenna.Model);
                        return new ProjectConfiguration
                        {
                            Antenna = new Reference
                            {
                                Manufacturer = c.Antenna.Manufacturer,
                                Model = c.Antenna.Model
                            },
                            HeightMeters = c.Antenna.HeightMeters,
                            Polarization = antenna?.IsHorizontallyPolarized == false ? "vertical" : "horizontal",
                            RotationAngleDegrees = c.Antenna.HorizontalAngleDegrees,
                            Radio = new Reference
                            {
                                Manufacturer = c.Radio.Manufacturer,
                                Model = c.Radio.Model
                            },
                            Linear = c.Linear == null ? null : new LinearReference
                            {
                                Name = c.Linear.Name,
                                PowerWatts = c.Linear.PowerWatts
                            },
                            PowerWatts = c.PowerWatts,
                            Cable = new CableReference { Name = c.Cable.Type },
                            CableLengthMeters = c.Cable.LengthMeters,
                            AdditionalLossDb = c.Cable.AdditionalLossDb,
                            AdditionalLossDescription = c.Cable.AdditionalLossDescription,
                            Modulation = c.Modulation,
                            ActivityFactor = c.ActivityFactor,
                            Oka = new OkaReference { Name = c.OkaName },
                            OkaDistanceMeters = c.OkaDistanceMeters,
                            OkaBuildingDampingDb = c.OkaBuildingDampingDb
                        };
                    }).ToList()
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var json = JsonSerializer.Serialize(projectFile, options);
                await File.WriteAllTextAsync(file.Path.LocalPath, json);
                StatusMessage = $"{Strings.ExportSuccess}: {file.Name}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"{Strings.ExportFailed}: {ex.Message}";
            }
        }
    }

    [RelayCommand]
    private async Task ExportUserData()
    {
        if (StorageProvider == null)
        {
            StatusMessage = "Export not available";
            return;
        }

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = Strings.ExportUserData,
            SuggestedFileName = $"NIS_UserData_{DateTime.Now:yyyyMMdd}.json",
            DefaultExtension = ".json",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
            }
        });

        if (file != null)
        {
            try
            {
                DatabaseService.Instance.ExportUserData(file.Path.LocalPath);
                StatusMessage = $"{Strings.ExportSuccess}: {file.Name}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"{Strings.ExportFailed}: {ex.Message}";
            }
        }
    }

    [RelayCommand]
    private async Task ExportFactoryData()
    {
        if (StorageProvider == null)
        {
            StatusMessage = "Export not available";
            return;
        }

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = Strings.ExportFactoryData,
            SuggestedFileName = $"NIS_FactoryData_{DateTime.Now:yyyyMMdd}.json",
            DefaultExtension = ".json",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
            }
        });

        if (file != null)
        {
            try
            {
                DatabaseService.Instance.ExportFactoryData(file.Path.LocalPath);
                StatusMessage = $"{Strings.ExportSuccess}: {file.Name}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"{Strings.ExportFailed}: {ex.Message}";
            }
        }
    }

    [RelayCommand]
    private async Task ImportUserData()
    {
        if (StorageProvider == null)
        {
            StatusMessage = "Import not available";
            return;
        }

        // Confirm before import
        if (ShowConfirmDialog != null)
        {
            var confirmed = await ShowConfirmDialog(
                Strings.ImportUserData,
                Strings.ImportConfirmMessage);
            if (!confirmed) return;
        }

        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = Strings.ImportUserData,
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
            }
        });

        if (files.Count > 0)
        {
            try
            {
                DatabaseService.Instance.ImportUserData(files[0].Path.LocalPath);
                StatusMessage = Strings.ImportSuccess;
            }
            catch (Exception ex)
            {
                StatusMessage = $"{Strings.ImportFailed}: {ex.Message}";
            }
        }
    }

    [RelayCommand]
    private async Task ImportFactoryData()
    {
        if (StorageProvider == null)
        {
            StatusMessage = "Import not available";
            return;
        }

        if (ShowConfirmDialog != null)
        {
            var confirmed = await ShowConfirmDialog(
                Strings.ImportFactoryData,
                Strings.ImportFactoryConfirmMessage);
            if (!confirmed) return;
        }

        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = Strings.ImportFactoryData,
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
            }
        });

        if (files.Count > 0)
        {
            try
            {
                DatabaseService.Instance.ImportFactoryData(files[0].Path.LocalPath);
                StatusMessage = Strings.ImportSuccess;
            }
            catch (Exception ex)
            {
                StatusMessage = $"{Strings.ImportFailed}: {ex.Message}";
            }
        }
    }

    [RelayCommand]
    private void Back()
    {
        NavigateBack?.Invoke();
    }
}
