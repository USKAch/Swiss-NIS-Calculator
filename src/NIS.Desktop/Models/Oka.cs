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
    /// Unique identifier for this OKA. Also used as the OKA number on the site plan.
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
    /// Indicates whether this OKA is user data (editable) or factory data (read-only).
    /// </summary>
    [JsonPropertyName("isUserData")]
    public bool IsUserData { get; set; } = true;

    /// <summary>
    /// Display name for the OKA including the number (e.g., "1 - Neighbor's balcony").
    /// </summary>
    [JsonIgnore]
    public string DisplayName => $"{Id} - {Name}";

    /// <summary>
    /// Returns DisplayName for text search in ComboBox.
    /// </summary>
    public override string ToString() => DisplayName;
}
