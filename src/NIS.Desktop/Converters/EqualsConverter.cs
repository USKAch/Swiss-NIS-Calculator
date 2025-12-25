using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace NIS.Desktop.ViewModels;

/// <summary>
/// Converter that returns true if the value equals the parameter.
/// </summary>
public class EqualsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null && parameter == null) return true;
        if (value == null || parameter == null) return false;
        return value.ToString() == parameter.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // When toggle is checked, return the parameter value
        if (value is true && parameter != null)
        {
            return parameter.ToString();
        }
        return null;
    }
}
