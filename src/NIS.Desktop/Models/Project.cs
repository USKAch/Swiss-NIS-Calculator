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
/// Same structure as RadioConfig.
/// </summary>
public class LinearConfig
{
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; } = string.Empty;

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonIgnore]
    public string DisplayName => $"{Manufacturer} {Model}".Trim();
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

    [JsonPropertyName("isHorizontallyRotatable")]
    public bool IsHorizontallyRotatable { get; set; }

    [JsonPropertyName("horizontalAngleDegrees")]
    public double HorizontalAngleDegrees { get; set; } = 360;

    [JsonPropertyName("isVerticallyRotatable")]
    public bool IsVerticallyRotatable { get; set; }

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
    /// Modulation factor for this configuration.
    /// SSB=0.2, CW=0.4, FM/Digital=1.0
    /// </summary>
    [JsonPropertyName("modulationFactor")]
    public double ModulationFactor { get; set; } = 0.4;

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
    public string ModulationDisplay => ModulationFactor switch
    {
        0.2 => "SSB",
        0.4 => "CW",
        1.0 => "FM",
        _ => $"{ModulationFactor}"
    };

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
                Address = value.Address;
                Location = value.Location;
            }
        }
    }

    [JsonPropertyName("antennaConfigurations")]
    public List<AntennaConfiguration> AntennaConfigurations { get; set; } = new();

    [JsonPropertyName("customAntennas")]
    public List<Antenna> CustomAntennas { get; set; } = new();

    [JsonPropertyName("customCables")]
    public List<Cable> CustomCables { get; set; } = new();

    [JsonPropertyName("customRadios")]
    public List<Radio> CustomRadios { get; set; } = new();

    [JsonPropertyName("okas")]
    public List<Oka> Okas { get; set; } = new();
}
