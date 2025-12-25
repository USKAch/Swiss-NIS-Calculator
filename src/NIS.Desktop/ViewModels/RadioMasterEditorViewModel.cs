using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Core.Models;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the Radio Master Editor - radio/transceiver editing.
/// </summary>
public partial class RadioMasterEditorViewModel : ViewModelBase
{
    private Radio? _originalRadio;

    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    public Action<Radio>? OnSave { get; set; }

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _manufacturer = string.Empty;

    [ObservableProperty]
    private string _model = string.Empty;

    [ObservableProperty]
    private double _maxPowerWatts = 100;

    public string Title => IsEditing ? "Edit Radio" : "Add New Radio";

    /// <summary>
    /// Initialize for creating a new radio.
    /// </summary>
    public void InitializeNew()
    {
        _originalRadio = null;
        IsEditing = false;
        Manufacturer = string.Empty;
        Model = string.Empty;
        MaxPowerWatts = 100;
    }

    /// <summary>
    /// Initialize for editing an existing radio.
    /// </summary>
    public void InitializeEdit(Radio radio)
    {
        _originalRadio = radio;
        IsEditing = true;
        Manufacturer = radio.Manufacturer;
        Model = radio.Model;
        MaxPowerWatts = radio.MaxPowerWatts;
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Manufacturer) || string.IsNullOrWhiteSpace(Model))
        {
            return;
        }

        var radio = new Radio
        {
            Manufacturer = Manufacturer.Trim(),
            Model = Model.Trim(),
            MaxPowerWatts = MaxPowerWatts
        };

        OnSave?.Invoke(radio);
    }

    [RelayCommand]
    private void Cancel()
    {
        NavigateBack?.Invoke();
    }
}
