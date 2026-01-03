using System;
using System.Threading.Tasks;
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
    private string _operator = string.Empty;

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

    // Track dirty state for all fields
    partial void OnProjectNameChanged(string value) => MarkDirty();
    partial void OnOperatorChanged(string value) => MarkDirty();
    partial void OnCallsignChanged(string value) => MarkDirty();
    partial void OnAddressChanged(string value) => MarkDirty();
    partial void OnLocationChanged(string value) => MarkDirty();

    [RelayCommand]
    private async Task Cancel()
    {
        if (await CanNavigateAwayAsync())
        {
            NavigateBack?.Invoke();
        }
    }

    [RelayCommand]
    private void Create()
    {
        IsDirty = false;
        NavigateToProjectOverview?.Invoke(this);
    }
}
