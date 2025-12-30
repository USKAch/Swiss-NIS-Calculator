using System;
using System.Linq;

namespace NIS.Desktop.Calculations;

/// <summary>
/// Generates vertical radiation patterns based on antenna gain using the formulas
/// documented in NIS_fsd.md Section 11.4.
/// </summary>
public static class PatternGenerator
{
    /// <summary>
    /// Generates a vertical radiation pattern for directional antennas (Yagi, Quad, Log-Periodic).
    /// Uses the dual-segment curve fit model from FSD Section 11.4.1.
    /// </summary>
    /// <param name="gainDbi">Antenna gain in dBi</param>
    /// <returns>Array of 10 attenuation values (dB) for angles 0°, 10°, 20°... 90°</returns>
    public static double[] GenerateDirectionalPattern(double gainDbi)
    {
        // HPBW = 105° / √(G_linear)
        double gLinear = Math.Pow(10, gainDbi / 10.0);
        double hpbw = 105.0 / Math.Sqrt(gLinear);
        double thetaHp = hpbw / 2.0;

        // A_zenith = min(35, max(20, 20 + (G_dBi - 6)))
        double aZenith = Math.Min(35, Math.Max(20, 20 + (gainDbi - 6)));

        var pattern = new double[10];

        for (int i = 0; i < 10; i++)
        {
            double theta = i * 10.0; // 0°, 10°, 20°, ... 90°

            double attenuation;
            if (theta <= thetaHp)
            {
                // Inner beam: Attenuation(θ) = 3 × (θ / θ_hp)²
                attenuation = 3.0 * Math.Pow(theta / thetaHp, 2);
            }
            else
            {
                // Outer beam: Attenuation(θ) = 3 + ((θ - θ_hp) / (90 - θ_hp)) × (A_zenith - 3)
                attenuation = 3.0 + ((theta - thetaHp) / (90.0 - thetaHp)) * (aZenith - 3.0);
            }

            // Round to 1 decimal place for cleaner display
            pattern[i] = Math.Round(attenuation, 1);
        }

        return pattern;
    }

    /// <summary>
    /// Generates a vertical radiation pattern for omnidirectional vertical antennas.
    /// Uses the quadratic rolloff model with zenith cap from FSD Section 11.4.2.
    /// </summary>
    /// <param name="gainDbi">Antenna gain in dBi</param>
    /// <returns>Array of 10 attenuation values (dB) for angles 0°, 10°, 20°... 90°</returns>
    public static double[] GenerateVerticalPattern(double gainDbi)
    {
        // HPBW = 105° / √(G_linear)
        double gLinear = Math.Pow(10, gainDbi / 10.0);
        double hpbw = 105.0 / Math.Sqrt(gLinear);
        double thetaHp = hpbw / 2.0;

        // Rolloff = 3.0 (ensures 3 dB loss at half beamwidth)
        const double rolloff = 3.0;

        // A_zenith = 20 + (G_dBi × 1.5)
        double aZenith = 20 + (gainDbi * 1.5);

        var pattern = new double[10];

        for (int i = 0; i < 10; i++)
        {
            double theta = i * 10.0; // 0°, 10°, 20°, ... 90°

            // Attenuation(θ) = min(A_zenith, Rolloff × (θ / θ_hp)²)
            double attenuation = Math.Min(aZenith, rolloff * Math.Pow(theta / thetaHp, 2));

            // Round to 1 decimal place for cleaner display
            pattern[i] = Math.Round(attenuation, 1);
        }

        return pattern;
    }

    /// <summary>
    /// Generates a vertical radiation pattern based on antenna type.
    /// </summary>
    /// <param name="antennaType">Antenna type (from AntennaTypes class)</param>
    /// <param name="gainDbi">Antenna gain in dBi</param>
    /// <returns>Array of 10 attenuation values (dB) for angles 0°, 10°, 20°... 90°</returns>
    public static double[] GeneratePattern(string antennaType, double gainDbi)
    {
        return antennaType switch
        {
            Models.AntennaTypes.Yagi => GenerateDirectionalPattern(gainDbi),
            Models.AntennaTypes.Quad => GenerateDirectionalPattern(gainDbi),
            Models.AntennaTypes.LogPeriodic => GenerateDirectionalPattern(gainDbi),
            Models.AntennaTypes.Vertical => GenerateVerticalPattern(gainDbi),
            // For wire, loop, and other types - use vertical (omnidirectional) as safer default
            _ => GenerateVerticalPattern(gainDbi)
        };
    }

    /// <summary>
    /// Determines if the antenna type uses directional pattern formulas.
    /// </summary>
    public static bool IsDirectional(string antennaType)
    {
        return antennaType is Models.AntennaTypes.Yagi
            or Models.AntennaTypes.Quad
            or Models.AntennaTypes.LogPeriodic;
    }
}
