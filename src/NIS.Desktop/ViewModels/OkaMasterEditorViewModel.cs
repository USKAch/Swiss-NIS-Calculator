using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Core.Models;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// ViewModel for the OKA Master Editor - simple name and default damping.
/// </summary>
public partial class OkaMasterEditorViewModel : ViewModelBase
{
    private Oka? _originalOka;

    // Navigation callbacks
    public Action? NavigateBack { get; set; }
    public Action<Oka>? OnSave { get; set; }

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private double _defaultDampingDb;

    [ObservableProperty]
    private double _defaultDistanceMeters = 10;

    [ObservableProperty]
    private string _validationMessage = string.Empty;

    public string Title => IsEditing ? Strings.EditOka : Strings.AddOka;

    /// <summary>
    /// Initialize for creating a new OKA.
    /// </summary>
    public void InitializeNew(int nextId)
    {
        _originalOka = null;
        IsEditing = false;
        Id = nextId;
        Name = string.Empty;
        DefaultDampingDb = 0;
        DefaultDistanceMeters = 10;
        ValidationMessage = string.Empty;
    }

    /// <summary>
    /// Initialize for editing an existing OKA.
    /// </summary>
    public void InitializeEdit(Oka oka)
    {
        _originalOka = oka;
        IsEditing = true;
        Id = oka.Id;
        Name = oka.Name;
        DefaultDampingDb = oka.DefaultDampingDb;
        DefaultDistanceMeters = oka.DefaultDistanceMeters;
        ValidationMessage = string.Empty;
    }

    [RelayCommand]
    private void Save()
    {
        ValidationMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Name))
        {
            ValidationMessage = Strings.OkaNameRequired;
            return;
        }

        var oka = new Oka
        {
            Id = Id,
            Name = Name.Trim(),
            DefaultDampingDb = DefaultDampingDb,
            DefaultDistanceMeters = DefaultDistanceMeters
        };

        OnSave?.Invoke(oka);
    }

    [RelayCommand]
    private void Cancel()
    {
        NavigateBack?.Invoke();
    }
}
