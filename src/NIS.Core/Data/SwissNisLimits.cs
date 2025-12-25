namespace NIS.Core.Data;

/// <summary>
/// Swiss NIS (Non-Ionizing Radiation) field strength limits by frequency.
/// Values from Swiss NISV regulations (Anhang 2, Ziffer 12).
/// </summary>
public static class SwissNisLimits
{
    /// <summary>
    /// Frequency bands and their corresponding field strength limits in V/m.
    /// Per Swiss NISV Anhang 2.
    /// </summary>
    private static readonly (double MinMHz, double MaxMHz, double LimitVm)[] Limits =
    [
        (0.0, 9.999, 87),           // < 10 MHz: 87 V/m
        (10.0, 399.999, 28),        // 10-400 MHz: 28 V/m
        (400.0, 1999.999, 28.5),    // 400-2000 MHz: ~28.5 V/m
        (2000.0, double.MaxValue, 61) // >= 2000 MHz: 61 V/m
    ];

    /// <summary>
    /// Standard amateur radio frequencies with their limits per NISV.
    /// </summary>
    public static readonly Dictionary<double, double> StandardFrequencyLimits = new()
    {
        { 1.8, 87 },     // 160m band
        { 3.5, 87 },     // 80m band
        { 7.0, 87 },     // 40m band
        { 10.0, 28 },    // 30m band
        { 14.0, 28 },    // 20m band
        { 18.0, 28 },    // 17m band
        { 21.0, 28 },    // 15m band
        { 24.0, 28 },    // 12m band
        { 28.0, 28 },    // 10m band
        { 50.0, 28 },    // 6m band
        { 144.0, 28 },   // 2m band
        { 432.0, 28.5 }, // 70cm band
        { 1240.0, 48.5 },// 23cm band
        { 2400.0, 61 },  // 13cm band
        { 5650.0, 61 },  // 6cm band
        { 10000.0, 61 }  // 3cm band
    };

    /// <summary>
    /// Gets the field strength limit in V/m for a given frequency.
    /// </summary>
    /// <param name="frequencyMHz">Frequency in MHz</param>
    /// <returns>Field strength limit in V/m</returns>
    public static double GetLimitVm(double frequencyMHz)
    {
        foreach (var (minMHz, maxMHz, limitVm) in Limits)
        {
            if (frequencyMHz >= minMHz && frequencyMHz < maxMHz)
                return limitVm;
        }

        // Default to highest limit for very high frequencies
        return 61;
    }

    /// <summary>
    /// Gets all standard amateur radio frequencies.
    /// </summary>
    public static IReadOnlyList<double> StandardFrequencies =>
        StandardFrequencyLimits.Keys.OrderBy(f => f).ToList();
}
