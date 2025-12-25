namespace NIS.Core.Models;

/// <summary>
/// Input parameters for field strength calculation.
/// </summary>
public class CalculationInput
{
    /// <summary>
    /// Operator callsign or name.
    /// </summary>
    public string Callsign { get; set; } = string.Empty;

    /// <summary>
    /// Distance to the exposure point in meters.
    /// </summary>
    public double DistanceMeters { get; set; }

    /// <summary>
    /// Evaluation height in meters.
    /// </summary>
    public double EvaluationHeightMeters { get; set; } = 1.5;

    /// <summary>
    /// Operating frequency in MHz.
    /// </summary>
    public double FrequencyMHz { get; set; }

    /// <summary>
    /// Transmitter power in Watts.
    /// </summary>
    public double TxPowerWatts { get; set; }

    /// <summary>
    /// Activity factor (0.0 to 1.0). Default is 0.5.
    /// Represents the fraction of time the transmitter is active.
    /// </summary>
    public double ActivityFactor { get; set; } = 0.5;

    /// <summary>
    /// Modulation factor (0.0 to 1.0).
    /// SSB=0.2, CW=0.4, FM/RTTY/PSK31=1.0.
    /// </summary>
    public double ModulationFactor { get; set; } = 0.4;

    /// <summary>
    /// Antenna gain in dBi.
    /// </summary>
    public double AntennaGainDbi { get; set; }

    /// <summary>
    /// Angle-dependent attenuation in dB (from radiation pattern).
    /// </summary>
    public double AngleAttenuationDb { get; set; }

    /// <summary>
    /// Total cable loss in dB (sum of all cable sections).
    /// </summary>
    public double TotalCableLossDb { get; set; }

    /// <summary>
    /// Additional losses in dB (connectors, filters, etc.).
    /// </summary>
    public double AdditionalLossDb { get; set; }

    /// <summary>
    /// Building damping factor in dB.
    /// </summary>
    public double BuildingDampingDb { get; set; }
}
