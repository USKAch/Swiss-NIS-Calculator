using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace NIS.Desktop.Converters;

/// <summary>
/// Maps an alternation index to a subtle row background brush.
/// </summary>
public class AlternationIndexToBrushConverter : IValueConverter
{
    private static readonly IBrush DefaultEvenBrush = new SolidColorBrush(Color.Parse("#FFFFFF"));
    private static readonly IBrush DefaultOddBrush = new SolidColorBrush(Color.Parse("#F7F7F7"));

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isOdd = value is int index && index % 2 == 1;
        var key = isOdd ? "SystemControlBackgroundAltMediumLowBrush" : "CardBackgroundFillColorDefault";
        if (Application.Current?.TryGetResource(key, null, out var resource) == true && resource is IBrush brush)
        {
            return brush;
        }

        return isOdd ? DefaultOddBrush : DefaultEvenBrush;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
