using CommunityToolkit.Mvvm.ComponentModel;
using NIS.Desktop.New.Services;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// ViewModel for the main window.
/// Handles navigation and session state display.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly INavigationService _navigation;
    private readonly ISessionService _session;

    [ObservableProperty]
    private object? _currentView;

    [ObservableProperty]
    private bool _hasProject;

    [ObservableProperty]
    private string _selectedPage = "Home";

    public MainWindowViewModel(
        INavigationService navigation,
        ISessionService session,
        ILocalizationService localization)
    {
        Loc = localization;
        _navigation = navigation;
        _session = session;

        // Bind to session state
        _session.PropertyChanged += (_, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(ISessionService.CurrentView):
                    CurrentView = _session.CurrentView;
                    break;
                case nameof(ISessionService.HasProject):
                    HasProject = _session.HasProject;
                    break;
                case nameof(ISessionService.SelectedPage):
                    SelectedPage = _session.SelectedPage;
                    break;
            }
        };

        // Navigate to home initially
        _navigation.NavigateTo("Home");
    }

    /// <summary>
    /// Called when navigation selection changes.
    /// </summary>
    public void NavigateTo(string tag)
    {
        _navigation.NavigateTo(tag);
    }
}
