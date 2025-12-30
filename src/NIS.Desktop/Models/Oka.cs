using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIS.Desktop.Models;

/// <summary>
/// Represents an OKA (Ort des kurzfristigen Aufenthalts / Short-term stay location).
/// Master data entry for evaluation points.
/// </summary>
public class Oka
{
    /// <summary>
    /// Unique identifier/number for this OKA.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Name/description of the OKA location (e.g., "Neighbor's balcony", "Garden").
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Default building damping in dB (0 for outdoor locations).
    /// </summary>
    [JsonPropertyName("defaultDampingDb")]
    public double DefaultDampingDb { get; set; }

    /// <summary>
    /// Default horizontal distance from antenna in meters.
    /// </summary>
    [JsonPropertyName("defaultDistanceMeters")]
    public double DefaultDistanceMeters { get; set; } = 10;

    /// <summary>
    /// Display name combining ID and name.
    /// </summary>
    [JsonIgnore]
    public string DisplayName => $"{Id}. {Name}";

    /// <summary>
    /// Returns DisplayName for text search in ComboBox.
    /// </summary>
    public override string ToString() => DisplayName;
}
