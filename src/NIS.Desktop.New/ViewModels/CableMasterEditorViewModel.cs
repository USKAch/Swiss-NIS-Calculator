using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Core.Models;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// ViewModel for the Cable Master Editor - cable editing with frequency-dependent attenuation.
/// </summary>
public partial class CableMasterEditorViewModel : ViewModelBase
{
    private Cable? _originalCable;

    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    public Action<Cable>? OnSave { get; set; }

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _name = string.Empty;

    // Attenuation values for standard frequencies (dB per 100m)
    [ObservableProperty]
    private double? _atten1_8;

    [ObservableProperty]
    private double? _atten3_5;

    [ObservableProperty]
    private double? _atten7;

    [ObservableProperty]
    private double? _atten10;

    [ObservableProperty]
    private double? _atten14;

    [ObservableProperty]
    private double? _atten18;

    [ObservableProperty]
    private double? _atten21;

    [ObservableProperty]
    private double? _atten24;

    [ObservableProperty]
    private double? _atten28;

    [ObservableProperty]
    private double? _atten50;

    [ObservableProperty]
    private double? _atten144;

    [ObservableProperty]
    private double? _atten430;

    [ObservableProperty]
    private double? _atten1240;

    [ObservableProperty]
    private double? _atten2300;

    [ObservableProperty]
    private double? _atten5650;

    [ObservableProperty]
    private double? _atten10000;

    [ObservableProperty]
    private string _validationMessage = string.Empty;

    public string Title => IsEditing ? "Edit Cable" : "Add New Cable";

    /// <summary>
    /// Initialize for creating a new cable.
    /// </summary>
    public void InitializeNew()
    {
        _originalCable = null;
        IsEditing = false;
        Name = string.Empty;
        ClearAllAttenuation();
    }

    /// <summary>
    /// Initialize for editing an existing cable.
    /// </summary>
    public void InitializeEdit(Cable cable)
    {
        _originalCable = cable;
        IsEditing = true;
        Name = cable.Name;
        LoadAttenuation(cable.AttenuationPer100m);
    }

    private void ClearAllAttenuation()
    {
        Atten1_8 = null;
        Atten3_5 = null;
        Atten7 = null;
        Atten10 = null;
        Atten14 = null;
        Atten18 = null;
        Atten21 = null;
        Atten24 = null;
        Atten28 = null;
        Atten50 = null;
        Atten144 = null;
        Atten430 = null;
        Atten1240 = null;
        Atten2300 = null;
        Atten5650 = null;
        Atten10000 = null;
    }

    private void LoadAttenuation(Dictionary<string, double> data)
    {
        ClearAllAttenuation();
        foreach (var kvp in data)
        {
            if (double.TryParse(kvp.Key, out double freq))
            {
                SetAttenuationForFrequency(freq, kvp.Value);
            }
        }
    }

    private void SetAttenuationForFrequency(double freq, double value)
    {
        // Match to closest standard frequency
        if (Math.Abs(freq - 1.8) < 0.1) Atten1_8 = value;
        else if (Math.Abs(freq - 3.5) < 0.1) Atten3_5 = value;
        else if (Math.Abs(freq - 7) < 0.1) Atten7 = value;
        else if (Math.Abs(freq - 10) < 0.1) Atten10 = value;
        else if (Math.Abs(freq - 14) < 0.1) Atten14 = value;
        else if (Math.Abs(freq - 18) < 0.1) Atten18 = value;
        else if (Math.Abs(freq - 21) < 0.1) Atten21 = value;
        else if (Math.Abs(freq - 24) < 0.1) Atten24 = value;
        else if (Math.Abs(freq - 28) < 0.1) Atten28 = value;
        else if (Math.Abs(freq - 50) < 0.5) Atten50 = value;
        else if (Math.Abs(freq - 144) < 1) Atten144 = value;
        else if (Math.Abs(freq - 430) < 5) Atten430 = value;
        else if (Math.Abs(freq - 1240) < 10) Atten1240 = value;
        else if (Math.Abs(freq - 2300) < 50) Atten2300 = value;
        else if (Math.Abs(freq - 5650) < 100) Atten5650 = value;
        else if (Math.Abs(freq - 10000) < 500) Atten10000 = value;
    }

    private Dictionary<string, double> BuildAttenuationDictionary()
    {
        var dict = new Dictionary<string, double>();
        if (Atten1_8.HasValue) dict["1.8"] = Atten1_8.Value;
        if (Atten3_5.HasValue) dict["3.5"] = Atten3_5.Value;
        if (Atten7.HasValue) dict["7"] = Atten7.Value;
        if (Atten10.HasValue) dict["10"] = Atten10.Value;
        if (Atten14.HasValue) dict["14"] = Atten14.Value;
        if (Atten18.HasValue) dict["18"] = Atten18.Value;
        if (Atten21.HasValue) dict["21"] = Atten21.Value;
        if (Atten24.HasValue) dict["24"] = Atten24.Value;
        if (Atten28.HasValue) dict["28"] = Atten28.Value;
        if (Atten50.HasValue) dict["50"] = Atten50.Value;
        if (Atten144.HasValue) dict["144"] = Atten144.Value;
        if (Atten430.HasValue) dict["430"] = Atten430.Value;
        if (Atten1240.HasValue) dict["1240"] = Atten1240.Value;
        if (Atten2300.HasValue) dict["2300"] = Atten2300.Value;
        if (Atten5650.HasValue) dict["5650"] = Atten5650.Value;
        if (Atten10000.HasValue) dict["10000"] = Atten10000.Value;
        return dict;
    }

    [RelayCommand]
    private void Save()
    {
        ValidationMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Name))
        {
            ValidationMessage = "Please enter a cable name.";
            return;
        }

        var cable = new Cable
        {
            Name = Name.Trim(),
            AttenuationPer100m = BuildAttenuationDictionary()
        };

        OnSave?.Invoke(cable);
    }

    [RelayCommand]
    private void Cancel()
    {
        NavigateBack?.Invoke();
    }
}
