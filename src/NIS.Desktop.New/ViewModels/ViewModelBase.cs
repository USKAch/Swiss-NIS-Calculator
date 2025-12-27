using CommunityToolkit.Mvvm.ComponentModel;
using NIS.Desktop.New.Services;

namespace NIS.Desktop.New.ViewModels;

/// <summary>
/// Base class for all ViewModels.
/// Provides common infrastructure and localization support.
/// </summary>
public abstract class ViewModelBase : ObservableObject
{
    /// <summary>
    /// Localization service for translated strings.
    /// Exposed publicly for XAML binding: {Binding Loc[Key]}
    /// </summary>
    public ILocalizationService? Loc { get; protected init; }

    /// <summary>
    /// Shorthand for getting a localized string.
    /// Returns the key if Loc is null or key not found.
    /// </summary>
    protected string L(string key) => Loc?[key] ?? $"[{key}]";

    /// <summary>
    /// Called when the language changes.
    /// Override in derived classes to refresh localized properties.
    /// </summary>
    protected virtual void OnLanguageChanged()
    {
        // Derived classes can override to refresh bindings
    }

    /// <summary>
    /// Subscribes to language changes if localization is available.
    /// Call this in constructor after setting Loc.
    /// </summary>
    protected void SubscribeToLanguageChanges()
    {
        if (Loc != null)
        {
            Loc.LanguageChanged += OnLanguageChanged;
        }
    }
}
