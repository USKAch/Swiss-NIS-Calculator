using System.Text.Json.Serialization;

namespace NIS.Core.Models;

/// <summary>
/// Represents a radio transceiver.
/// </summary>
public class Radio
{
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
    /// Maximum output power in Watts.
    /// </summary>
    [JsonPropertyName("maxPowerWatts")]
    public double MaxPowerWatts { get; set; }

    /// <summary>
    /// Indicates whether this radio is project-specific (editable) or from master data (read-only).
    /// </summary>
    [JsonPropertyName("isProjectSpecific")]
    public bool IsProjectSpecific { get; set; }

    /// <summary>
    /// Display name combining manufacturer and model.
    /// </summary>
    [JsonIgnore]
    public string DisplayName => $"{Manufacturer} {Model}".Trim();

    /// <summary>
    /// Returns display name with power for text search in ComboBox.
    /// </summary>
    public override string ToString() => $"{Manufacturer} {Model} ({MaxPowerWatts}W)";
}
