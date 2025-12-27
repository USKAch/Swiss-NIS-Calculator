using System;
using NIS.Desktop.New.ViewModels;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Service for navigating between views in the application.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// The currently displayed view (ViewModel).
    /// </summary>
    object? CurrentView { get; }

    /// <summary>
    /// The tag/name of the current page.
    /// </summary>
    string CurrentPage { get; }

    /// <summary>
    /// Whether a project is currently loaded (enables Project nav item).
    /// </summary>
    bool HasProject { get; }

    /// <summary>
    /// Navigate to a view by its ViewModel type.
    /// </summary>
    void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;

    /// <summary>
    /// Navigate to a view by page tag.
    /// </summary>
    void NavigateTo(string pageTag);

    /// <summary>
    /// Raised when the current view changes.
    /// </summary>
    event Action<object?>? CurrentViewChanged;

    /// <summary>
    /// Raised when HasProject changes.
    /// </summary>
    event Action<bool>? HasProjectChanged;
}
