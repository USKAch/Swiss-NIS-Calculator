namespace NIS.Core.Data;

/// <summary>
/// Swiss NIS (Non-Ionizing Radiation) field strength limits by frequency.
/// Values from Swiss NISV regulations (Anhang 2, Ziffer 12).
/// </summary>
public static class SwissNisLimits
{
    /// <summary>
    /// Standard amateur radio frequencies with their limits per NISV.
    /// Values from VB6 legacy code (HB9ZS original implementation).
    /// </summary>
    public static readonly Dictionary<double, double> StandardFrequencyLimits = new()
    {
        { 1.8, 64.7 },   // 160m band
        { 3.5, 46.5 },   // 80m band
        { 7.0, 32.4 },   // 40m band
        { 10.0, 28 },    // 30m band
        { 14.0, 28 },    // 20m band
        { 18.0, 28 },    // 17m band
        { 21.0, 28 },    // 15m band
        { 24.0, 28 },    // 12m band
        { 28.0, 28 },    // 10m band
        { 50.0, 28 },    // 6m band
        { 144.0, 28 },   // 2m band
        { 432.0, 28.6 }, // 70cm band
        { 1240.0, 48.5 },// 23cm band
        { 2400.0, 61 },  // 13cm band
        { 5650.0, 61 },  // 6cm band
        { 10000.0, 61 }  // 3cm band
    };

    /// <summary>
    /// Gets the field strength limit in V/m for a given frequency.
    /// Uses VB6 legacy frequency bands for amateur radio compliance.
    /// </summary>
    /// <param name="frequencyMHz">Frequency in MHz</param>
    /// <returns>Field strength limit in V/m</returns>
    public static double GetLimitVm(double frequencyMHz)
    {
        // Swiss NISV limits per frequency band (from VB6 legacy code)
        return frequencyMHz switch
        {
            < 2.5 => 64.7,    // 1.8 MHz (160m)
            < 5 => 46.5,      // 3.5 MHz (80m)
            < 9 => 32.4,      // 7 MHz (40m)
            < 40 => 28,       // 10-28 MHz (30m-10m)
            < 100 => 28,      // 50 MHz (6m)
            < 300 => 28,      // 144 MHz (2m)
            < 800 => 28.6,    // 432 MHz (70cm)
            < 2000 => 48.5,   // 1240 MHz (23cm)
            _ => 61           // 2300+ MHz (13cm and higher)
        };
    }

    /// <summary>
    /// Gets all standard amateur radio frequencies.
    /// </summary>
    public static IReadOnlyList<double> StandardFrequencies =>
        StandardFrequencyLimits.Keys.OrderBy(f => f).ToList();
}
