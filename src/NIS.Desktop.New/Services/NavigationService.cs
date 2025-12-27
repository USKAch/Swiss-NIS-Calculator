using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NIS.Desktop.New.ViewModels;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Handles navigation between views.
/// Uses DI to resolve ViewModels and caches persistent ones.
/// </summary>
public class NavigationService : INavigationService
{
    private readonly ISessionService _session;
    private readonly IServiceProvider _serviceProvider;

    // Cache for ViewModels that should persist across navigations
    private readonly Dictionary<Type, ViewModelBase> _viewModelCache = new();

    // Map page tags to ViewModel types
    private static readonly Dictionary<string, Type> PageTagMap = new()
    {
        { "Home", typeof(HomeViewModel) },
        { "Project", typeof(ProjectViewModel) },
        { "MasterData", typeof(MasterDataViewModel) },
        { "Settings", typeof(SettingsViewModel) }
    };

    // ViewModels that should be cached (persist state across navigations)
    private static readonly HashSet<Type> CachedViewModels = new()
    {
        typeof(ProjectViewModel)
    };

    public NavigationService(ISessionService session, IServiceProvider serviceProvider)
    {
        _session = session;
        _serviceProvider = serviceProvider;
    }

    public object? CurrentView => _session.CurrentView;
    public string CurrentPage => _session.SelectedPage;
    public bool HasProject => _session.HasProject;

    public event Action<object?>? CurrentViewChanged;
    public event Action<bool>? HasProjectChanged;

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        var viewModel = GetOrCreateViewModel<TViewModel>();
        SetCurrentView(viewModel, GetPageTagForType(typeof(TViewModel)));
    }

    public void NavigateTo(string pageTag)
    {
        if (!PageTagMap.TryGetValue(pageTag, out var viewModelType))
        {
            return;
        }

        // Don't navigate to Project if no project is loaded
        if (pageTag == "Project" && !_session.HasProject)
        {
            return;
        }

        var viewModel = GetOrCreateViewModel(viewModelType);
        SetCurrentView(viewModel, pageTag);
    }

    private ViewModelBase GetOrCreateViewModel<TViewModel>() where TViewModel : ViewModelBase
    {
        return GetOrCreateViewModel(typeof(TViewModel));
    }

    private ViewModelBase GetOrCreateViewModel(Type viewModelType)
    {
        // Check cache first for persistent ViewModels
        if (CachedViewModels.Contains(viewModelType) && _viewModelCache.TryGetValue(viewModelType, out var cached))
        {
            return cached;
        }

        // Resolve from DI
        var viewModel = (ViewModelBase)_serviceProvider.GetRequiredService(viewModelType);

        // Cache if persistent
        if (CachedViewModels.Contains(viewModelType))
        {
            _viewModelCache[viewModelType] = viewModel;
        }

        return viewModel;
    }

    private void SetCurrentView(ViewModelBase viewModel, string pageTag)
    {
        _session.SelectedPage = pageTag;
        _session.CurrentView = viewModel;
        CurrentViewChanged?.Invoke(viewModel);
    }

    private static string GetPageTagForType(Type type)
    {
        foreach (var kvp in PageTagMap)
        {
            if (kvp.Value == type)
            {
                return kvp.Key;
            }
        }
        return "Home";
    }

    /// <summary>
    /// Clears the cached ProjectViewModel (call when project is closed).
    /// </summary>
    public void ClearProjectCache()
    {
        _viewModelCache.Remove(typeof(ProjectViewModel));
    }
}
