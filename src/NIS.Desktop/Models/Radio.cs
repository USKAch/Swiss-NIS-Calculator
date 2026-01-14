using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIS.Desktop.Models;

/// <summary>
/// Represents a frequency band supported by a radio with band-specific power.
/// </summary>
public class RadioBand
{
    /// <summary>
    /// Center frequency in MHz (matches standard amateur bands).
    /// </summary>
    [JsonPropertyName("frequencyMHz")]
    public double FrequencyMHz { get; set; }

    /// <summary>
    /// Maximum output power at this frequency in Watts.
    /// </summary>
    [JsonPropertyName("maxPowerWatts")]
    public double MaxPowerWatts { get; set; }
}

/// <summary>
/// Represents a radio transceiver.
/// </summary>
public class Radio
{
    /// <summary>
    /// Database primary key.
    /// </summary>
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// Radio manufacturer name.
    /// </summary>
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Radio model name.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Default/fallback maximum output power in Watts.
    /// Used when no band-specific power is defined.
    /// </summary>
    [JsonPropertyName("maxPowerWatts")]
    public double MaxPowerWatts { get; set; }

    /// <summary>
    /// Indicates whether this radio is user data (editable) or factory data (read-only).
    /// </summary>
    [JsonPropertyName("isUserData")]
    public bool IsUserData { get; set; }

    /// <summary>
    /// Band-specific power outputs. Optional - if empty, MaxPowerWatts applies to all bands.
    /// </summary>
    [JsonPropertyName("bands")]
    public List<RadioBand> Bands { get; set; } = new();

    /// <summary>
    /// Whether this radio has band-specific power defined.
    /// </summary>
    [JsonIgnore]
    public bool HasBandSpecificPower => Bands.Count > 0;

    /// <summary>
    /// Gets the maximum power for a specific frequency.
    /// Returns band-specific power if defined, otherwise fallback to MaxPowerWatts.
    /// </summary>
    /// <param name="frequencyMHz">The frequency in MHz to look up.</param>
    /// <returns>Power in Watts for the given frequency.</returns>
    public double GetPowerAtFrequency(double frequencyMHz)
    {
        var band = Bands.FirstOrDefault(b => Math.Abs(b.FrequencyMHz - frequencyMHz) < 0.5);
        return band?.MaxPowerWatts ?? MaxPowerWatts;
    }

    /// <summary>
    /// Gets all supported frequencies (from bands, or empty if using global power).
    /// </summary>
    [JsonIgnore]
    public IEnumerable<double> SupportedFrequencies => Bands.Select(b => b.FrequencyMHz);

    /// <summary>
    /// Display name combining manufacturer and model.
    /// </summary>
    [JsonIgnore]
    public string DisplayName => $"{Manufacturer} {Model}".Trim();

    /// <summary>
    /// Returns display name for text search in ComboBox.
    /// </summary>
    public override string ToString() => DisplayName;
}
