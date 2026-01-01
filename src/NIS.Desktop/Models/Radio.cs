using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIS.Desktop.Models;

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
    /// Maximum output power in Watts.
    /// </summary>
    [JsonPropertyName("maxPowerWatts")]
    public double MaxPowerWatts { get; set; }

    /// <summary>
    /// Indicates whether this radio is user data (editable) or factory data (read-only).
    /// </summary>
    [JsonPropertyName("isUserData")]
    public bool IsUserData { get; set; }

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
