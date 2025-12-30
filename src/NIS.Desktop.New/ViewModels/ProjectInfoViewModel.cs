using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Core.Models;
using NIS.Desktop.Localization;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the project info view (station details).
/// </summary>
public partial class ProjectInfoViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _callsign = string.Empty;

    [ObservableProperty]
    private string _operatorName = string.Empty;

    [ObservableProperty]
    private string _address = string.Empty;

    [ObservableProperty]
    private string _location = string.Empty;

    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    public Action<ProjectInfoViewModel>? NavigateToProjectOverview { get; set; }

    public string Language => Strings.Instance.Language;

    public string LanguageDisplayName => Strings.Instance.GetLanguageName(Language);

    // Localized labels - use centralized Strings class
    public string TitleLabel => Strings.Instance.StationInfo;
    public string CallsignLabel => Strings.Instance.Callsign + ":";
    public string OperatorLabel => Strings.Instance.Operator + ":";
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

    /// <summary>
    /// Creates a StationInfo from the current values.
    /// </summary>
    public StationInfo ToStationInfo()
    {
        return new StationInfo
        {
            Callsign = Callsign,
            Operator = OperatorName,
            Address = Address,
            Location = Location
        };
    }

    /// <summary>
    /// Populates from existing StationInfo.
    /// </summary>
    public void FromStationInfo(StationInfo station)
    {
        Callsign = station.Callsign;
        OperatorName = station.Operator;
        Address = station.Address;
        Location = station.Location;
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
