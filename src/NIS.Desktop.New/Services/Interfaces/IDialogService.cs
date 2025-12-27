using System.Threading.Tasks;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Service for showing dialogs and file pickers.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Shows an open file dialog.
    /// </summary>
    /// <param name="title">Dialog title</param>
    /// <param name="filters">File type filters (e.g., "NIS Project|*.nisproj")</param>
    /// <returns>Selected file path, or null if cancelled</returns>
    Task<string?> ShowOpenFileDialogAsync(string title, params FileFilter[] filters);

    /// <summary>
    /// Shows a save file dialog.
    /// </summary>
    /// <param name="title">Dialog title</param>
    /// <param name="defaultFileName">Default file name</param>
    /// <param name="filters">File type filters</param>
    /// <returns>Selected file path, or null if cancelled</returns>
    Task<string?> ShowSaveFileDialogAsync(string title, string? defaultFileName, params FileFilter[] filters);

    /// <summary>
    /// Shows a confirmation dialog with Yes/No buttons.
    /// </summary>
    /// <param name="title">Dialog title</param>
    /// <param name="message">Message to display</param>
    /// <returns>True if user clicked Yes, false otherwise</returns>
    Task<bool> ShowConfirmationAsync(string title, string message);

    /// <summary>
    /// Shows an information message dialog.
    /// </summary>
    /// <param name="title">Dialog title</param>
    /// <param name="message">Message to display</param>
    Task ShowMessageAsync(string title, string message);

    /// <summary>
    /// Shows an error message dialog.
    /// </summary>
    /// <param name="title">Dialog title</param>
    /// <param name="message">Error message to display</param>
    Task ShowErrorAsync(string title, string message);
}

/// <summary>
/// Represents a file type filter for file dialogs.
/// </summary>
public record FileFilter(string Name, params string[] Patterns);
