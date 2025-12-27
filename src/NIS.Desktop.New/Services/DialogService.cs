using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Provides dialog and file picker functionality.
/// </summary>
public class DialogService : IDialogService
{
    private Window? GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow;
        }
        return null;
    }

    public async Task<string?> ShowOpenFileDialogAsync(string title, params FileFilter[] filters)
    {
        var window = GetMainWindow();
        if (window == null) return null;

        var storageProvider = window.StorageProvider;

        var fileTypes = filters.Select(f => new FilePickerFileType(f.Name)
        {
            Patterns = f.Patterns
        }).ToList();

        var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = false,
            FileTypeFilter = fileTypes
        });

        return result.FirstOrDefault()?.Path.LocalPath;
    }

    public async Task<string?> ShowSaveFileDialogAsync(string title, string? defaultFileName, params FileFilter[] filters)
    {
        var window = GetMainWindow();
        if (window == null) return null;

        var storageProvider = window.StorageProvider;

        var fileTypes = filters.Select(f => new FilePickerFileType(f.Name)
        {
            Patterns = f.Patterns
        }).ToList();

        var result = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = title,
            SuggestedFileName = defaultFileName,
            FileTypeChoices = fileTypes
        });

        return result?.Path.LocalPath;
    }

    public async Task<bool> ShowConfirmationAsync(string title, string message)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(
            title,
            message,
            ButtonEnum.YesNo,
            Icon.Question);

        var result = await box.ShowWindowDialogAsync(GetMainWindow());
        return result == ButtonResult.Yes;
    }

    public async Task ShowMessageAsync(string title, string message)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(
            title,
            message,
            ButtonEnum.Ok,
            Icon.Info);

        await box.ShowWindowDialogAsync(GetMainWindow());
    }

    public async Task ShowErrorAsync(string title, string message)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(
            title,
            message,
            ButtonEnum.Ok,
            Icon.Error);

        await box.ShowWindowDialogAsync(GetMainWindow());
    }
}
