using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace NIS.Desktop.Models;

/// <summary>
/// Radio configuration within an antenna configuration.
/// </summary>
public class RadioConfig
{
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; } = string.Empty;

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonIgnore]
    public string DisplayName => $"{Manufacturer} {Model}".Trim();
}

/// <summary>
/// Linear configuration (optional).
/// Contains name and output power.
/// </summary>
public class LinearConfig
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("powerWatts")]
    public double PowerWatts { get; set; }

    [JsonIgnore]
    public string DisplayName => string.IsNullOrEmpty(Name) ? "" : $"{Name} ({PowerWatts}W)";
}

/// <summary>
/// Cable configuration within an antenna configuration.
/// </summary>
public class CableConfig
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("lengthMeters")]
    public double LengthMeters { get; set; }

    [JsonPropertyName("additionalLossDb")]
    public double AdditionalLossDb { get; set; }

    [JsonPropertyName("additionalLossDescription")]
    public string AdditionalLossDescription { get; set; } = string.Empty;
}

/// <summary>
/// Antenna placement configuration.
/// </summary>
public class AntennaPlacement
{
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; } = string.Empty;

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("heightMeters")]
    public double HeightMeters { get; set; }

    /// <summary>
    /// Whether the antenna is rotatable (can be pointed in different directions).
    /// </summary>
    [JsonPropertyName("isRotatable")]
    public bool IsRotatable { get; set; }

    /// <summary>
    /// Horizontal rotation angle in degrees (0-360). Only relevant if IsRotatable is true.
    /// </summary>
    [JsonPropertyName("horizontalAngleDegrees")]
    public double HorizontalAngleDegrees { get; set; } = 360;

    [JsonIgnore]
    public string DisplayName => $"{Manufacturer} {Model}".Trim();
}

/// <summary>
/// Complete antenna configuration (radio -> cable -> antenna signal path).
/// All master data references use IDs for lookups; names are for display only.
/// </summary>
public class AntennaConfiguration
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    // Radio reference (ID for lookup, config for display)
    [JsonPropertyName("radioId")]
    public int? RadioId { get; set; }

    [JsonPropertyName("radio")]
    public RadioConfig Radio { get; set; } = new();

    [JsonPropertyName("linear")]
    public LinearConfig? Linear { get; set; }

    /// <summary>
    /// Output power in Watts (at radio output, or linear output if present).
    /// </summary>
    [JsonPropertyName("powerWatts")]
    public double PowerWatts { get; set; }

    // Cable reference (ID for lookup, config for display/settings)
    [JsonPropertyName("cableId")]
    public int? CableId { get; set; }

    [JsonPropertyName("cable")]
    public CableConfig Cable { get; set; } = new();

    // Antenna reference (ID for lookup, placement for display/settings)
    [JsonPropertyName("antennaId")]
    public int? AntennaId { get; set; }

    [JsonPropertyName("antenna")]
    public AntennaPlacement Antenna { get; set; } = new();

    // Modulation reference (ID for lookup, name for display)
    [JsonPropertyName("modulationId")]
    public int? ModulationId { get; set; }

    [JsonPropertyName("modulation")]
    public string Modulation { get; set; } = "CW";

    /// <summary>
    /// Activity factor for this configuration (default 0.5).
    /// </summary>
    [JsonPropertyName("activityFactor")]
    public double ActivityFactor { get; set; } = 0.5;

    // OKA reference (ID for lookup, name for display cache)
    [JsonPropertyName("okaId")]
    public int? OkaId { get; set; }

    [JsonPropertyName("okaName")]
    public string OkaName { get; set; } = string.Empty;

    [JsonIgnore]
    public string ModulationDisplay => Modulation;
}

/// <summary>
/// NIS calculation project file (.nisproj).
/// Project fields: Name, Operator (callsign), Address, Location.
/// All master data is referenced by ID, with names stored for display purposes.
/// </summary>
public class Project
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "New Project";


    [JsonPropertyName("operator")]
    public string Operator { get; set; } = string.Empty;

    [JsonPropertyName("callsign")]
    public string Callsign { get; set; } = string.Empty;

    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;

    [JsonPropertyName("configurations")]
    public List<AntennaConfiguration> AntennaConfigurations { get; set; } = new();
}
