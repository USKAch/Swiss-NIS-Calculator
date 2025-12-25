using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Core.Models;

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

    public string Language { get; }

    public string LanguageDisplayName => Language switch
    {
        "de" => "Deutsch",
        "fr" => "Français",
        "it" => "Italiano",
        "en" => "English",
        _ => Language
    };

    // Localized labels
    public string TitleLabel => Language switch
    {
        "de" => "Projekt-Informationen",
        "fr" => "Informations du projet",
        "it" => "Informazioni progetto",
        _ => "Project Information"
    };

    public string CallsignLabel => Language switch
    {
        "de" => "Rufzeichen:",
        "fr" => "Indicatif:",
        "it" => "Nominativo:",
        _ => "Callsign:"
    };

    public string OperatorLabel => Language switch
    {
        "de" => "Betreiber:",
        "fr" => "Opérateur:",
        "it" => "Operatore:",
        _ => "Operator:"
    };

    public string AddressLabel => Language switch
    {
        "de" => "Adresse:",
        "fr" => "Adresse:",
        "it" => "Indirizzo:",
        _ => "Address:"
    };

    public string LocationLabel => Language switch
    {
        "de" => "Ort:",
        "fr" => "Localité:",
        "it" => "Località:",
        _ => "Location:"
    };

    [ObservableProperty]
    private bool _isEditMode;

    public string CreateButtonLabel => IsEditMode
        ? Language switch
        {
            "de" => "Speichern",
            "fr" => "Enregistrer",
            "it" => "Salva",
            _ => "Save"
        }
        : Language switch
        {
            "de" => "Projekt erstellen",
            "fr" => "Créer le projet",
            "it" => "Crea progetto",
            _ => "Create Project"
        };

    public string CancelButtonLabel => Language switch
    {
        "de" => "Abbrechen",
        "fr" => "Annuler",
        "it" => "Annulla",
        _ => "Cancel"
    };

    public ProjectInfoViewModel(string language)
    {
        Language = language;
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
