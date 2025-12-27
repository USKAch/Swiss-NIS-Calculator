using Avalonia;
using Avalonia.Styling;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Manages application theme.
/// </summary>
public class ThemeService : IThemeService
{
    private ThemeVariant _currentTheme = ThemeVariant.Default;

    public ThemeVariant CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (_currentTheme != value)
            {
                _currentTheme = value;
                ApplyTheme(value);
            }
        }
    }

    public void ApplyTheme(ThemeVariant theme)
    {
        _currentTheme = theme;

        if (Application.Current != null)
        {
            Application.Current.RequestedThemeVariant = theme;
        }
    }
}
