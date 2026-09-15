using System;
using System.Globalization;
using Avalonia.Data.Converters;
using NIS.Desktop.Services;

namespace NIS.Desktop.Converters;

/// <summary>
/// Converts a band frequency in MHz (double) to its band name from master data, e.g. 3.5 → "80m", 430 → "70cm".
/// Amateur bands span a frequency range, so the UI shows the band, not a single frequency.
/// </summary>
public class BandNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            double d => MasterDataStore.GetBandName(d),
            decimal m => MasterDataStore.GetBandName((double)m),
            int i => MasterDataStore.GetBandName(i),
            _ => value?.ToString()
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
