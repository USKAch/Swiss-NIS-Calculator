using System.Text.Json.Serialization;

namespace NIS.Core.Models;

/// <summary>
/// Represents a coaxial cable type with frequency-dependent attenuation values.
/// Attenuation is specified in dB per 100 meters.
/// </summary>
public class Cable
{
    /// <summary>
    /// Cable type name (e.g., "RG-213", "Aircom Plus").
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Attenuation values in dB/100m at standard frequencies.
    /// Key: frequency in MHz (as string), Value: attenuation in dB/100m.
    /// </summary>
    [JsonPropertyName("attenuationPer100m")]
    public Dictionary<string, double> AttenuationPer100m { get; set; } = new();

    /// <summary>
    /// Gets the interpolated attenuation at a specific frequency.
    /// </summary>
    public double GetAttenuationAt(double frequencyMHz)
    {
        if (AttenuationPer100m.Count == 0)
            return 0;

        // Convert string keys to doubles and sort
        var freqValues = AttenuationPer100m
            .Select(kv => (Freq: double.Parse(kv.Key), Atten: kv.Value))
            .OrderBy(x => x.Freq)
            .ToList();

        if (freqValues.Count == 0)
            return 0;

        // Exact match (with tolerance)
        var exactMatch = freqValues.FirstOrDefault(x => Math.Abs(x.Freq - frequencyMHz) < 0.01);
        if (exactMatch != default)
            return exactMatch.Atten;

        // Below lowest frequency
        if (frequencyMHz <= freqValues.First().Freq)
            return freqValues.First().Atten;

        // Above highest frequency
        if (frequencyMHz >= freqValues.Last().Freq)
            return freqValues.Last().Atten;

        // Linear interpolation between two nearest frequencies
        for (int i = 0; i < freqValues.Count - 1; i++)
        {
            if (frequencyMHz >= freqValues[i].Freq && frequencyMHz <= freqValues[i + 1].Freq)
            {
                double f1 = freqValues[i].Freq;
                double f2 = freqValues[i + 1].Freq;
                double a1 = freqValues[i].Atten;
                double a2 = freqValues[i + 1].Atten;

                double ratio = (frequencyMHz - f1) / (f2 - f1);
                return a1 + ratio * (a2 - a1);
            }
        }

        return 0;
    }

    /// <summary>
    /// Calculates total cable loss for a given length and frequency.
    /// </summary>
    /// <param name="lengthMeters">Cable length in meters</param>
    /// <param name="frequencyMHz">Operating frequency in MHz</param>
    /// <returns>Total attenuation in dB</returns>
    public double CalculateLoss(double lengthMeters, double frequencyMHz)
    {
        double attenuationPer100m = GetAttenuationAt(frequencyMHz);
        return attenuationPer100m * lengthMeters / 100.0;
    }

    /// <summary>
    /// Returns Name for text search in ComboBox.
    /// </summary>
    public override string ToString() => Name;
}
