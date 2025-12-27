using Avalonia.Styling;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Service for managing application theme.
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// The current theme variant.
    /// </summary>
    ThemeVariant CurrentTheme { get; set; }

    /// <summary>
    /// Applies a theme to the application.
    /// </summary>
    /// <param name="theme">The theme to apply</param>
    void ApplyTheme(ThemeVariant theme);
}
