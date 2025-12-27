using System;
using CommunityToolkit.Mvvm.ComponentModel;
using NIS.Core.Models;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Manages application session state (current project, current view).
/// Single source of truth for app-wide state.
/// </summary>
public partial class SessionService : ObservableObject, ISessionService
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasProject))]
    private Project? _currentProject;

    [ObservableProperty]
    private string? _projectFilePath;

    [ObservableProperty]
    private bool _isDirty;

    [ObservableProperty]
    private string _selectedPage = "Home";

    [ObservableProperty]
    private object? _currentView;

    public bool HasProject => CurrentProject != null;

    public event Action? ProjectChanged;
    public event Action? ViewChanged;

    partial void OnCurrentProjectChanged(Project? value)
    {
        ProjectChanged?.Invoke();
    }

    partial void OnCurrentViewChanged(object? value)
    {
        ViewChanged?.Invoke();
    }
}
