using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Localization;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the project info view (project details).
/// </summary>
public partial class ProjectInfoViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _projectName = string.Empty;

    [ObservableProperty]
    private string _operatorName = string.Empty;

    [ObservableProperty]
    private string _callsign = string.Empty;

    [ObservableProperty]
    private string _address = string.Empty;

    [ObservableProperty]
    private string _location = string.Empty;

    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    public Action<ProjectInfoViewModel>? NavigateToProjectOverview { get; set; }


    // Localized labels
    public string TitleLabel => Strings.Instance.ProjectInfo;
    public string ProjectNameLabel => Strings.Instance.ProjectName + ":";
    public string OperatorLabel => Strings.Instance.Operator + ":";
    public string CallsignLabel => Strings.Instance.Callsign + ":";
    public string AddressLabel => Strings.Instance.Address + ":";
    public string LocationLabel => Strings.Instance.Location + ":";

    [ObservableProperty]
    private bool _isEditMode;

    public string CreateButtonLabel => IsEditMode
        ? Strings.Instance.Save
        : Strings.Instance.CreateProject;

    public string CancelButtonLabel => Strings.Instance.Cancel;

    public ProjectInfoViewModel()
    {
    }

    public ProjectInfoViewModel(string language)
    {
        // Language parameter is kept for compatibility but we use Strings.Instance.Language
    }

    [RelayCommand]
    private void Cancel()
    {
        NavigateBack?.Invoke();
    }

    [RelayCommand]
    private void Create()
    {
        NavigateToProjectOverview?.Invoke(this);
    }
}
