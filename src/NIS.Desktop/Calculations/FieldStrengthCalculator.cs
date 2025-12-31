using System;
using System.Linq;

using NIS.Desktop.Models;

namespace NIS.Desktop.Calculations;

/// <summary>
/// Calculates electromagnetic field strength according to Swiss NIS regulations.
/// Formulas per NISV Anhang 2 and ported from original VB6 NIS_berechnen().
/// </summary>
public class FieldStrengthCalculator
{
    /// <summary>
    /// Default ground reflection factor (kr).
    /// </summary>
    private const double DefaultGroundReflectionFactor = 1.6;

    /// <summary>
    /// Free space constant for field strength calculation.
    /// E = sqrt(30 * P * G) / r where 30 = (120π)/4π.
    /// </summary>
    private const double FreeSpaceConstant = 30.0;

    /// <summary>
    /// Conversion factor from EIRP to ERP (dipole reference).
    /// ERP = EIRP / 1.64
    /// </summary>
    private const double EirpToErpFactor = 1.64;

    /// <summary>
    /// Calculates field strength and all intermediate values per Swiss NIS formulas.
    ///
    /// Formulas:
    /// Pm = P × AF × MF                    (Mean power)
    /// a = a1 + a2                         (Total attenuation dB)
    /// A = 10^(-a/10)                      (Attenuation factor)
    /// g = g1 - g2                         (Total antenna gain dB)
    /// G = 10^(g/10)                       (Gain factor)
    /// Ps = Pm × A × G                     (EIRP)
    /// P's = Ps / 1.64                     (ERP)
    /// AG = 10^(-ag/10)                    (Building factor)
    /// E' = kr × sqrt(30 × Pm × A × G × AG) / d  (Field strength)
    /// </summary>
    public CalculationResult Calculate(CalculationInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var distance = input.DistanceMeters <= 0 ? 0.001 : input.DistanceMeters;
        var groundReflectionFactor = input.GroundReflectionFactor > 0
            ? input.GroundReflectionFactor
            : DefaultGroundReflectionFactor;

        // Step 1: Mean power (Pm = P × AF × MF)
        double meanPower = input.TxPowerWatts <= 0 || input.ActivityFactor <= 0 || input.ModulationFactor <= 0
            ? 0
            : input.TxPowerWatts * input.ActivityFactor * input.ModulationFactor;

        // Step 2: Cable/line attenuation
        double totalAttenuationDb = input.TotalCableLossDb + input.AdditionalLossDb;
        double attenuationFactor = Math.Pow(10, -totalAttenuationDb / 10);

        // Step 3: Antenna gain
        double totalAntennaGainDb = input.AntennaGainDbi - input.AngleAttenuationDb;
        double antennaGainFactor = Math.Pow(10, totalAntennaGainDb / 10);

        // Step 4: EIRP and ERP
        double eirp = meanPower * attenuationFactor * antennaGainFactor;
        double erp = eirp / EirpToErpFactor;

        // Step 5: Building damping
        double buildingDampingFactor = Math.Pow(10, -input.BuildingDampingDb / 10);

        // Step 6: Field strength (V/m)
        // E' = kr × sqrt(30 × Pm × A × G × AG) / d
        double fieldStrength = groundReflectionFactor *
            Math.Sqrt(FreeSpaceConstant * meanPower * attenuationFactor * antennaGainFactor * buildingDampingFactor)
            / distance;

        // Step 7: Power density (W/m²) - optional reference
        // S = E² / 377
        double powerDensity = (fieldStrength * fieldStrength) / 377.0;

        // Step 8: NIS limit and safety distance
        double nisLimit = SwissNisLimits.GetLimitVm(input.FrequencyMHz);

        // Safety distance: solve E' = EIGW for d
        // d = kr × sqrt(30 × Pm × A × G × AG) / EIGW
        double safetyDistance = groundReflectionFactor *
            Math.Sqrt(FreeSpaceConstant * meanPower * attenuationFactor * antennaGainFactor * buildingDampingFactor)
            / nisLimit;

        return new CalculationResult
        {
            Input = input,

            // Input echo
            FrequencyMHz = input.FrequencyMHz,
            DistanceMeters = distance,
            TxPowerWatts = input.TxPowerWatts,
            ActivityFactor = input.ActivityFactor,
            ModulationFactor = input.ModulationFactor,

            // Intermediate values
            MeanPowerWatts = meanPower,
            CableAttenuationDb = input.TotalCableLossDb,
            AdditionalLossDb = input.AdditionalLossDb,
            TotalAttenuationDb = totalAttenuationDb,
            AttenuationFactor = attenuationFactor,
            AntennaGainDbi = input.AntennaGainDbi,
            VerticalAttenuationDb = input.AngleAttenuationDb,
            TotalAntennaGainDb = totalAntennaGainDb,
            AntennaGainFactor = antennaGainFactor,
            EirpWatts = eirp,
            ErpWatts = erp,
            BuildingDampingDb = input.BuildingDampingDb,
            BuildingDampingFactor = buildingDampingFactor,
            GroundReflectionFactor = groundReflectionFactor,

            // Final results
            FieldStrengthVm = fieldStrength,
            NisLimitVm = nisLimit,
            SafetyDistanceMeters = safetyDistance,
            PowerDensityWm2 = powerDensity
        };
    }

    /// <summary>
    /// Calculates field strength for multiple distances.
    /// </summary>
    public CalculationResult[] CalculateForDistances(CalculationInput input, double[] distances)
    {
        var results = new CalculationResult[distances.Length];

        for (int i = 0; i < distances.Length; i++)
        {
            var modifiedInput = CloneInput(input);
            modifiedInput.DistanceMeters = distances[i];
            results[i] = Calculate(modifiedInput);
        }

        return results;
    }

    /// <summary>
    /// Finds the minimum safe distance for a given set of parameters.
    /// </summary>
    public double FindMinimumSafeDistance(CalculationInput input)
    {
        // Use the Calculate method which now includes safety distance
        var tempInput = CloneInput(input);
        tempInput.DistanceMeters = 1.0; // Dummy value, we only need safety distance
        var result = Calculate(tempInput);
        return result.SafetyDistanceMeters;
    }

    private static CalculationInput CloneInput(CalculationInput input)
    {
        return new CalculationInput
        {
            OperatorName = input.OperatorName,
            DistanceMeters = input.DistanceMeters,
            EvaluationHeightMeters = input.EvaluationHeightMeters,
            FrequencyMHz = input.FrequencyMHz,
            TxPowerWatts = input.TxPowerWatts,
            ActivityFactor = input.ActivityFactor,
            ModulationFactor = input.ModulationFactor,
            AntennaGainDbi = input.AntennaGainDbi,
            AngleAttenuationDb = input.AngleAttenuationDb,
            TotalCableLossDb = input.TotalCableLossDb,
            AdditionalLossDb = input.AdditionalLossDb,
            BuildingDampingDb = input.BuildingDampingDb
        };
    }
}
