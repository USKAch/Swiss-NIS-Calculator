using System;
using System.Linq;
using System.Collections.Generic;
namespace NIS.Desktop.Models;

/// <summary>
/// Result of field strength calculation with all intermediate values.
/// Names match Swiss NIS calculation documentation.
/// </summary>
public class CalculationResult
{
    /// <summary>
    /// The input parameters used for this calculation.
    /// </summary>
    public CalculationInput? Input { get; set; }

    // === Input Echo Values ===

    /// <summary>
    /// Frequency in MHz (f).
    /// </summary>
    public double FrequencyMHz { get; set; }

    /// <summary>
    /// Distance to antenna in meters (d).
    /// </summary>
    public double DistanceMeters { get; set; }

    /// <summary>
    /// Transmitter output power in Watts (P).
    /// </summary>
    public double TxPowerWatts { get; set; }

    /// <summary>
    /// Activity factor (AF).
    /// </summary>
    public double ActivityFactor { get; set; }

    /// <summary>
    /// Modulation factor (MF).
    /// </summary>
    public double ModulationFactor { get; set; }

    // === Calculated Intermediate Values ===

    /// <summary>
    /// Mean power at transmitter output in Watts (Pm = P × AF × MF).
    /// </summary>
    public double MeanPowerWatts { get; set; }

    /// <summary>
    /// Cable attenuation in dB (a1).
    /// </summary>
    public double CableAttenuationDb { get; set; }

    /// <summary>
    /// Additional losses in dB - connectors, switches, etc. (a2).
    /// </summary>
    public double AdditionalLossDb { get; set; }

    /// <summary>
    /// Total attenuation in dB (a = a1 + a2).
    /// </summary>
    public double TotalAttenuationDb { get; set; }

    /// <summary>
    /// Attenuation factor linear (A = 10^(-a/10)).
    /// </summary>
    public double AttenuationFactor { get; set; }

    /// <summary>
    /// Antenna gain in dBi (g1).
    /// </summary>
    public double AntennaGainDbi { get; set; }

    /// <summary>
    /// Vertical angle attenuation in dB (g2).
    /// </summary>
    public double VerticalAttenuationDb { get; set; }

    /// <summary>
    /// Total antenna gain in dB (g = g1 - g2).
    /// </summary>
    public double TotalAntennaGainDb { get; set; }

    /// <summary>
    /// Antenna gain factor linear (G = 10^(g/10)).
    /// </summary>
    public double AntennaGainFactor { get; set; }

    /// <summary>
    /// EIRP - Equivalent Isotropic Radiated Power in Watts (Ps = Pm × A × G).
    /// </summary>
    public double EirpWatts { get; set; }

    /// <summary>
    /// ERP - Equivalent Radiated Power in Watts (P's = Ps / 1.64).
    /// Referenced to a dipole instead of isotropic radiator.
    /// </summary>
    public double ErpWatts { get; set; }

    /// <summary>
    /// Building damping in dB (ag).
    /// </summary>
    public double BuildingDampingDb { get; set; }

    /// <summary>
    /// Building damping factor linear (AG = 10^(-ag/10)).
    /// </summary>
    public double BuildingDampingFactor { get; set; }

    /// <summary>
    /// Ground reflection factor (kr = 1.6 constant).
    /// </summary>
    public double GroundReflectionFactor { get; set; } = 1.6;

    // === Final Results ===

    /// <summary>
    /// Calculated field strength in V/m (E').
    /// E' = kr × sqrt(30 × Pm × A × G × AG) / d
    /// </summary>
    public double FieldStrengthVm { get; set; }

    /// <summary>
    /// Swiss NIS limit for the operating frequency in V/m (EIGW).
    /// </summary>
    public double NisLimitVm { get; set; }

    /// <summary>
    /// Safety distance in meters (ds).
    /// Distance where field strength equals NIS limit.
    /// </summary>
    public double SafetyDistanceMeters { get; set; }

    /// <summary>
    /// Calculated power density in W/m² (optional, for reference).
    /// </summary>
    public double PowerDensityWm2 { get; set; }

    // === Derived Properties ===

    /// <summary>
    /// Percentage of the NIS limit used (0-100+).
    /// </summary>
    public double NisLimitPercentage => NisLimitVm > 0 ? (FieldStrengthVm / NisLimitVm) * 100 : 0;

    /// <summary>
    /// Whether the calculated field strength is within Swiss NIS limits.
    /// </summary>
    public bool IsWithinLimits => FieldStrengthVm <= NisLimitVm;
}
