using System;
using System.Linq;
using System.Collections.Generic;
namespace NIS.Desktop.Models;

/// <summary>
/// Represents an antenna radiation pattern with attenuation values at different angles.
/// Pattern is measured in 10-degree increments from 0° to 350° (36 points).
/// </summary>
public class RadiationPattern
{
    /// <summary>
    /// Attenuation values in dB for each 10-degree increment.
    /// Index 0 = 0°, Index 1 = 10°, ..., Index 35 = 350°.
    /// </summary>
    public double[] AttenuationDegrees { get; set; } = new double[36];

    /// <summary>
    /// Gets the attenuation at a specific angle (0-360 degrees).
    /// </summary>
    public double GetAttenuationAtAngle(double angleDegrees)
    {
        // Normalize angle to 0-360
        angleDegrees = ((angleDegrees % 360) + 360) % 360;

        int index = (int)Math.Round(angleDegrees / 10) % 36;
        return AttenuationDegrees[index];
    }

    /// <summary>
    /// Gets the maximum attenuation value in the pattern.
    /// </summary>
    public double MaxAttenuation => AttenuationDegrees.Length > 0 ? AttenuationDegrees.Max() : 0;
}
