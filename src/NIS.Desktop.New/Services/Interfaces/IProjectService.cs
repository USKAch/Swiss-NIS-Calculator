using System;
using System.ComponentModel;
using System.Threading.Tasks;
using NIS.Core.Models;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Service for managing the current project state.
/// Single source of truth for project data.
/// </summary>
public interface IProjectService : INotifyPropertyChanged
{
    /// <summary>
    /// The currently loaded project, or null if no project is loaded.
    /// </summary>
    Project? CurrentProject { get; }

    /// <summary>
    /// The file path of the current project, or null if not saved.
    /// </summary>
    string? FilePath { get; }

    /// <summary>
    /// Whether the project has unsaved changes.
    /// </summary>
    bool IsDirty { get; }

    /// <summary>
    /// Whether a project is currently loaded.
    /// </summary>
    bool HasProject { get; }

    /// <summary>
    /// Creates a new empty project.
    /// </summary>
    /// <param name="language">Project language (de, en, fr, it)</param>
    void NewProject(string language = "de");

    /// <summary>
    /// Loads a project from a file.
    /// </summary>
    /// <param name="filePath">Path to the project file</param>
    /// <returns>True if loaded successfully</returns>
    Task<bool> LoadAsync(string filePath);

    /// <summary>
    /// Saves the current project.
    /// </summary>
    /// <param name="filePath">Path to save to, or null to use current path</param>
    /// <returns>True if saved successfully</returns>
    Task<bool> SaveAsync(string? filePath = null);

    /// <summary>
    /// Marks the project as having unsaved changes.
    /// </summary>
    void MarkDirty();

    /// <summary>
    /// Clears the dirty flag (called after save).
    /// </summary>
    void ClearDirty();

    /// <summary>
    /// Closes the current project.
    /// </summary>
    void CloseProject();

    /// <summary>
    /// Raised when the project changes (load, new, close).
    /// </summary>
    event Action? ProjectChanged;

    /// <summary>
    /// Raised when the dirty state changes.
    /// </summary>
    event Action<bool>? DirtyChanged;
}
