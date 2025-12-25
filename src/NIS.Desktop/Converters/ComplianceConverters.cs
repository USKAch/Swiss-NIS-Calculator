using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace NIS.Desktop.Converters;

/// <summary>
/// Converts compliance boolean to color (green for pass, red for fail).
/// </summary>
public class ComplianceColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCompliant)
        {
            if (targetType == typeof(IBrush))
            {
                return isCompliant
                    ? new SolidColorBrush(Color.Parse("#4CAF50"))
                    : new SolidColorBrush(Color.Parse("#F44336"));
            }
            return isCompliant ? "#4CAF50" : "#F44336";
        }
        return "#999999";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts compliance boolean to text (PASS/FAIL or OK/FAIL).
/// </summary>
public class ComplianceTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCompliant)
        {
            return isCompliant ? "PASS" : "FAIL";
        }
        return "?";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
