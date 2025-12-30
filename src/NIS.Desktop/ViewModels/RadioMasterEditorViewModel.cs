using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Models;

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

    /// <summary>
    /// When true, the editor is in read-only mode (viewing master data).
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEdit))]
    private bool _isReadOnly;

    /// <summary>
    /// Whether editing is allowed (inverse of IsReadOnly).
    /// </summary>
    public bool CanEdit => !IsReadOnly;

    [ObservableProperty]
    private string _manufacturer = string.Empty;

    [ObservableProperty]
    private string _model = string.Empty;

    [ObservableProperty]
    private double _maxPowerWatts = 100;

    [ObservableProperty]
    private string _validationMessage = string.Empty;

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
        ValidationMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Manufacturer))
        {
            ValidationMessage = "Please enter a manufacturer.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Model))
        {
            ValidationMessage = "Please enter a model.";
            return;
        }

        if (MaxPowerWatts <= 0)
        {
            ValidationMessage = "Power must be greater than 0 W.";
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
