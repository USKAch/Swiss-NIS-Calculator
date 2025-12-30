using System;
using System.Collections.Generic;
using System.IO;
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

    // Storage provider for file dialogs
    public IStorageProvider? StorageProvider { get; set; }

    // Dialog callback
    public Func<string, string, Task<bool>>? ShowConfirmDialog { get; set; }

    // Navigation callback
    public Action? NavigateBack { get; set; }

    // Callback to refresh project list after import
    public Action? OnProjectImported { get; set; }

    // Available projects for export
    public List<ProjectListItem> ProjectList => DatabaseService.Instance.GetProjectList();

    [ObservableProperty]
    private ProjectListItem? _selectedProject;

    public new Strings Strings => Strings.Instance;

    [RelayCommand]
    private async Task ImportProject()
    {
        if (StorageProvider == null)
        {
            StatusMessage = "Import not available";
            return;
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
                var project = JsonSerializer.Deserialize<Project>(json, options);

                if (project != null)
                {
                    // Import OKAs from the project
                    foreach (var oka in project.Okas)
                    {
                        if (!DatabaseService.Instance.OkaExists(oka.Name))
                        {
                            DatabaseService.Instance.SaveOka(oka);
                        }
                    }

                    // Create the project in the database
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
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var json = JsonSerializer.Serialize(project, options);
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
    private void Back()
    {
        NavigateBack?.Invoke();
    }
}
