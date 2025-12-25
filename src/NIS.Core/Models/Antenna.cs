using System.Text.Json.Serialization;

namespace NIS.Core.Models;

/// <summary>
/// Represents a frequency band supported by an antenna.
/// </summary>
public class AntennaBand
{
    /// <summary>
    /// Center frequency in MHz.
    /// </summary>
    [JsonPropertyName("frequencyMHz")]
    public double FrequencyMHz { get; set; }

    /// <summary>
    /// Antenna gain at this frequency in dBi.
    /// </summary>
    [JsonPropertyName("gainDbi")]
    public double GainDbi { get; set; }

    /// <summary>
    /// Vertical radiation pattern - attenuation in dB at angles 0, 10, 20, ..., 90 degrees.
    /// 10 values total. Index 0 = 0 deg (horizon), Index 9 = 90 deg (straight up/down).
    /// </summary>
    [JsonPropertyName("pattern")]
    public double[] Pattern { get; set; } = new double[10];

    /// <summary>
    /// Gets the vertical attenuation at a specific angle (0-90 degrees).
    /// </summary>
    public double GetAttenuationAtAngle(double angleDegrees)
    {
        if (Pattern == null || Pattern.Length == 0) return 0;

        angleDegrees = Math.Abs(angleDegrees);
        if (angleDegrees > 90) angleDegrees = 180 - angleDegrees;

        int index = (int)Math.Round(angleDegrees / 10);
        index = Math.Clamp(index, 0, Pattern.Length - 1);
        return Pattern[index];
    }
}

/// <summary>
/// Represents an antenna with multi-band support and radiation patterns.
/// </summary>
public class Antenna
{
    /// <summary>
    /// Antenna manufacturer name.
    /// </summary>
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Antenna model/type name. Accepts both "model" and "type" from JSON.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Alias for Model - used in legacy JSON format.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type
    {
        get => null;
        set { if (!string.IsNullOrEmpty(value)) Model = value; }
    }

    /// <summary>
    /// Whether the antenna is horizontally rotatable.
    /// </summary>
    [JsonPropertyName("isRotatable")]
    public bool IsRotatable { get; set; }

    /// <summary>
    /// Frequency bands supported by this antenna with gain and pattern data.
    /// </summary>
    [JsonPropertyName("bands")]
    public List<AntennaBand> Bands { get; set; } = new();

    // Legacy single-band format support
    [JsonPropertyName("frequencyMHz")]
    public double? LegacyFrequencyMHz
    {
        get => null;
        set
        {
            if (value.HasValue && Bands.Count == 0)
            {
                Bands.Add(new AntennaBand { FrequencyMHz = value.Value });
            }
            else if (value.HasValue && Bands.Count > 0)
            {
                Bands[0].FrequencyMHz = value.Value;
            }
        }
    }

    [JsonPropertyName("gainDbi")]
    public double? LegacyGainDbi
    {
        get => null;
        set
        {
            if (value.HasValue)
            {
                if (Bands.Count == 0) Bands.Add(new AntennaBand());
                Bands[0].GainDbi = value.Value;
            }
        }
    }

    /// <summary>
    /// Display name combining manufacturer and model.
    /// </summary>
    [JsonIgnore]
    public string DisplayName => $"{Manufacturer} {Model}".Trim();

    /// <summary>
    /// Gets the band data for a specific frequency, or null if not supported.
    /// </summary>
    public AntennaBand? GetBand(double frequencyMHz)
    {
        return Bands.FirstOrDefault(b => Math.Abs(b.FrequencyMHz - frequencyMHz) < 0.5);
    }

    /// <summary>
    /// Gets all supported frequencies.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<double> SupportedFrequencies => Bands.Select(b => b.FrequencyMHz);
}
