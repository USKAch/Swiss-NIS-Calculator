using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Models;
using NIS.Desktop.Services;

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
    private double _defaultDampingDb = 0;

    [ObservableProperty]
    private double _defaultDistanceMeters = 10;

    [ObservableProperty]
    private string _validationMessage = string.Empty;

    public string Title => IsEditing ? Strings.EditOka : Strings.AddOka;

    // Track dirty state for all editable properties
    partial void OnNameChanged(string value) => MarkDirty();
    partial void OnDefaultDampingDbChanged(double value) => MarkDirty();
    partial void OnDefaultDistanceMetersChanged(double value) => MarkDirty();

    /// <summary>
    /// Initialize for creating a new OKA.
    /// </summary>
    public void InitializeNew()
    {
        _originalOka = null;
        IsEditing = false;
        Id = 0;  // Will be assigned by database on save
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

        // Check for duplicate name (different OKA with same name)
        var trimmedName = Name.Trim();
        var existing = DatabaseService.Instance.GetOka(trimmedName);
        if (existing != null && existing.Id != Id)
        {
            ValidationMessage = Strings.OkaNameDuplicate;
            return;
        }

        // DefaultDistanceMeters must be > 0 (FSD 6.3)
        if (DefaultDistanceMeters <= 0)
        {
            ValidationMessage = Strings.OkaDistanceRequired;
            return;
        }

        // DefaultDampingDb must be >= 0 (FSD 6.3)
        if (DefaultDampingDb < 0)
        {
            ValidationMessage = Strings.OkaDampingNonNegative;
            return;
        }

        var oka = new Oka
        {
            Id = Id,
            Name = Name.Trim(),
            DefaultDampingDb = DefaultDampingDb,
            DefaultDistanceMeters = DefaultDistanceMeters,
            IsUserData = true
        };

        OnSave?.Invoke(oka);
    }

    [RelayCommand]
    private async Task Cancel()
    {
        if (await CanNavigateAwayAsync())
        {
            NavigateBack?.Invoke();
        }
    }
}
