using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIS.Desktop.Models;

/// <summary>
/// Antenna type classification for vertical pattern determination.
/// </summary>
public static class AntennaTypes
{
    public const string LogPeriodic = "Log-Periodic";
    public const string Loop = "Loop";
    public const string Other = "Other";
    public const string Quad = "Quad";
    public const string Vertical = "Vertical";
    public const string Wire = "Wire";
    public const string Yagi = "Yagi";

    public static readonly string[] All = { Yagi, Vertical, Wire, Loop, LogPeriodic, Quad, Other };

    /// <summary>
    /// Normalize antenna type to proper case (handles legacy lowercase values).
    /// </summary>
    public static string Normalize(string? type)
    {
        if (string.IsNullOrEmpty(type)) return Other;
        return type.ToLowerInvariant() switch
        {
            "log-periodic" => LogPeriodic,
            "loop" => Loop,
            "quad" => Quad,
            "vertical" => Vertical,
            "wire" => Wire,
            "yagi" => Yagi,
            _ => string.IsNullOrEmpty(type) ? Other :
                 char.ToUpper(type[0]) + type.Substring(1).ToLowerInvariant()
        };
    }
}

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
    /// Uses linear interpolation between pattern data points.
    /// </summary>
    public double GetAttenuationAtAngle(double angleDegrees)
    {
        if (Pattern == null || Pattern.Length == 0) return 0;

        angleDegrees = Math.Abs(angleDegrees);
        if (angleDegrees > 90) angleDegrees = 180 - angleDegrees;

        // Calculate position in the pattern array (0-9 for 0-90 degrees)
        double position = angleDegrees / 10.0;
        int lowerIndex = (int)Math.Floor(position);
        int upperIndex = (int)Math.Ceiling(position);

        lowerIndex = Math.Clamp(lowerIndex, 0, Pattern.Length - 1);
        upperIndex = Math.Clamp(upperIndex, 0, Pattern.Length - 1);

        // If same index (exact match or at boundary), return directly
        if (lowerIndex == upperIndex)
            return Pattern[lowerIndex];

        // Linear interpolation between the two nearest pattern values
        double fraction = position - lowerIndex;
        return Pattern[lowerIndex] + fraction * (Pattern[upperIndex] - Pattern[lowerIndex]);
    }
}

/// <summary>
/// Represents an antenna with multi-band support and radiation patterns.
/// </summary>
public class Antenna
{
    /// <summary>
    /// Database primary key.
    /// </summary>
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// Antenna manufacturer name.
    /// </summary>
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Antenna model name.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Antenna type classification. See <see cref="AntennaTypes"/> for valid values.
    /// Used to determine if vertical pattern formulas apply.
    /// </summary>
    [JsonPropertyName("antennaType")]
    public string AntennaType { get; set; } = AntennaTypes.Other;

    /// <summary>
    /// Whether the antenna is horizontally polarized.
    /// </summary>
    [JsonPropertyName("isHorizontallyPolarized")]
    public bool IsHorizontallyPolarized { get; set; } = true;

    /// <summary>
    /// Frequency bands supported by this antenna with gain and pattern data.
    /// </summary>
    [JsonPropertyName("bands")]
    public List<AntennaBand> Bands { get; set; } = new();

    /// <summary>
    /// Indicates whether this antenna is user data (editable) or factory data (read-only).
    /// </summary>
    [JsonPropertyName("isUserData")]
    public bool IsUserData { get; set; }

    /// <summary>
    /// Display name combining manufacturer and model.
    /// </summary>
    [JsonIgnore]
    public string DisplayName => $"{Manufacturer} {Model}".Trim();

    /// <summary>
    /// Antenna type with proper capitalization for display.
    /// </summary>
    [JsonIgnore]
    public string AntennaTypeDisplay => AntennaTypes.Normalize(AntennaType);

    /// <summary>
    /// Returns DisplayName for text search in ComboBox.
    /// </summary>
    public override string ToString() => DisplayName;

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
