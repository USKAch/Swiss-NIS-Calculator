using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace NIS.Desktop.Models;

/// <summary>
/// Station information for the project.
/// </summary>
public class StationInfo
{
    [JsonPropertyName("operator")]
    public string Operator { get; set; } = string.Empty;

    [JsonPropertyName("callsign")]
    public string Callsign { get; set; } = string.Empty;

    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;
}

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
/// Linear amplifier configuration (optional).
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

    [JsonIgnore]
    public string DisplayName => $"{Manufacturer} {Model}".Trim();
}

/// <summary>
/// Complete antenna configuration (radio -> cable -> antenna signal path).
/// </summary>
public class AntennaConfiguration
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("radio")]
    public RadioConfig Radio { get; set; } = new();

    [JsonPropertyName("linear")]
    public LinearConfig? Linear { get; set; }

    /// <summary>
    /// Output power in Watts (at radio output, or linear output if present).
    /// </summary>
    [JsonPropertyName("powerWatts")]
    public double PowerWatts { get; set; }

    [JsonPropertyName("cable")]
    public CableConfig Cable { get; set; } = new();

    [JsonPropertyName("antenna")]
    public AntennaPlacement Antenna { get; set; } = new();

    /// <summary>
    /// Modulation name for this configuration (e.g., SSB, CW, FM).
    /// </summary>
    [JsonPropertyName("modulation")]
    public string Modulation { get; set; } = "CW";

    /// <summary>
    /// Activity factor for this configuration (default 0.5).
    /// </summary>
    [JsonPropertyName("activityFactor")]
    public double ActivityFactor { get; set; } = 0.5;

    /// <summary>
    /// OKA name/identifier (e.g., "Neighbor's balcony").
    /// </summary>
    [JsonPropertyName("okaName")]
    public string OkaName { get; set; } = string.Empty;

    /// <summary>
    /// Distance to OKA in meters.
    /// </summary>
    [JsonPropertyName("okaDistanceMeters")]
    public double OkaDistanceMeters { get; set; }

    /// <summary>
    /// Building damping at OKA in dB.
    /// </summary>
    [JsonPropertyName("okaBuildingDampingDb")]
    public double OkaBuildingDampingDb { get; set; }

    [JsonIgnore]
    public string ModulationDisplay => Modulation;

    [JsonIgnore]
    public string OkaSummary => OkaDistanceMeters > 0
        ? $"{OkaDistanceMeters}m"
        : "No OKA";
}

/// <summary>
/// Evaluation point (OKA - Ort des kurzfristigen Aufenthalts).
/// Now belongs to an AntennaConfiguration with a single distance.
/// </summary>
public class EvaluationPoint
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("distanceMeters")]
    public double DistanceMeters { get; set; }

    [JsonPropertyName("buildingDampingDb")]
    public double BuildingDampingDb { get; set; }

    [JsonIgnore]
    public string Summary => $"{Id}: {DistanceMeters}m, {BuildingDampingDb}dB";
}

/// <summary>
/// NIS calculation project file (.nisproj).
/// Project fields: Name, Operator (callsign), Address, Location.
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

    // Legacy: for backward compatibility with old JSON files
    [JsonPropertyName("station")]
    public StationInfo? Station
    {
        get => null; // Don't serialize
        set
        {
            // Import from old format
            if (value != null)
            {
                if (!string.IsNullOrWhiteSpace(value.Operator))
                {
                    Operator = value.Operator;
                }
                if (!string.IsNullOrWhiteSpace(value.Callsign))
                {
                    Callsign = value.Callsign;
                }
                Address = value.Address;
                Location = value.Location;
            }
        }
    }

    [JsonPropertyName("configurations")]
    public List<AntennaConfiguration> AntennaConfigurations { get; set; } = new();

    // Legacy: for backward compatibility with old JSON files
    [JsonPropertyName("antennaConfigurations")]
    public List<AntennaConfiguration>? LegacyAntennaConfigurations
    {
        get => null;
        set
        {
            if (value != null && value.Count > 0)
            {
                AntennaConfigurations = value;
            }
        }
    }

    // Master data is stored globally in the database; projects reference it by name/model.
}
