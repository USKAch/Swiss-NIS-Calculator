using System;
using System.ComponentModel;
using NIS.Core.Models;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Service for managing application session state.
/// Holds the current project and selected view to reduce coupling between services.
/// </summary>
public interface ISessionService : INotifyPropertyChanged
{
    /// <summary>
    /// The currently loaded project, or null if no project is loaded.
    /// </summary>
    Project? CurrentProject { get; set; }

    /// <summary>
    /// The file path of the current project, or null if not saved.
    /// </summary>
    string? ProjectFilePath { get; set; }

    /// <summary>
    /// Whether the project has unsaved changes.
    /// </summary>
    bool IsDirty { get; set; }

    /// <summary>
    /// Whether a project is currently loaded.
    /// </summary>
    bool HasProject { get; }

    /// <summary>
    /// The currently selected navigation page tag.
    /// </summary>
    string SelectedPage { get; set; }

    /// <summary>
    /// The currently displayed ViewModel.
    /// </summary>
    object? CurrentView { get; set; }

    /// <summary>
    /// Raised when the project changes (load, new, close).
    /// </summary>
    event Action? ProjectChanged;

    /// <summary>
    /// Raised when the current view changes.
    /// </summary>
    event Action? ViewChanged;
}
