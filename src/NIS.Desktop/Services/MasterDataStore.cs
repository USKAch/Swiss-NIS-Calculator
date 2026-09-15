using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NIS.Desktop.Models;

namespace NIS.Desktop.Services;

public static class MasterDataStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    // Band list cache for display lookups (GetBandName is called from bindings/converters).
    private static IReadOnlyList<BandDefinition>? _cachedBands;

    /// <summary>
    /// Band definitions as last loaded/saved; loads the file on first access.
    /// </summary>
    public static IReadOnlyList<BandDefinition> Bands => _cachedBands ?? Load().Bands;

    public static MasterDataFile Load()
    {
        if (!File.Exists(AppPaths.MasterDataFile))
        {
            var defaults = CreateDefaultMasterData();
            Save(defaults);
            return defaults;
        }

        try
        {
            var json = File.ReadAllText(AppPaths.MasterDataFile);
            var data = JsonSerializer.Deserialize<MasterDataFile>(json, JsonOptions) ?? CreateDefaultMasterData();
            if (MergeMissingDefaultBands(data))
            {
                Save(data);
            }
            _cachedBands = data.Bands.ToList();
            return data;
        }
        catch
        {
            var defaults = CreateDefaultMasterData();
            _cachedBands = defaults.Bands.ToList();
            return defaults;
        }
    }

    /// <summary>
    /// Adds default bands (e.g. 4m added in 0.9) that are missing from an existing
    /// masterdata.json, so older installations pick up new bands without a reset.
    /// Returns true if anything was added.
    /// </summary>
    internal static bool MergeMissingDefaultBands(MasterDataFile data)
    {
        data.Bands ??= new List<BandDefinition>();
        var added = false;
        foreach (var def in CreateDefaultMasterData().Bands)
        {
            if (!data.Bands.Any(b => Math.Abs(b.FrequencyMHz - def.FrequencyMHz) < 0.01))
            {
                data.Bands.Add(def);
                added = true;
            }
        }
        if (added)
        {
            data.Bands = data.Bands.OrderBy(b => b.FrequencyMHz).ToList();
        }
        return added;
    }

    /// <summary>
    /// Returns the band name (e.g. "40m") for a frequency, or "{f} MHz" if no band matches.
    /// Bands come from master data so there is a single list to maintain.
    /// </summary>
    public static string GetBandName(double frequencyMHz) => GetBandName(Bands, frequencyMHz);

    /// <summary>
    /// Same as <see cref="GetBandName(double)"/> but with a pre-loaded band list (avoids re-reading the file in loops).
    /// </summary>
    public static string GetBandName(IEnumerable<BandDefinition> bands, double frequencyMHz)
    {
        var band = bands
            .OrderBy(b => Math.Abs(b.FrequencyMHz - frequencyMHz))
            .FirstOrDefault(b => Math.Abs(b.FrequencyMHz - frequencyMHz) <= BandMatchToleranceMHz(b.FrequencyMHz));
        return band?.Name ?? $"{frequencyMHz:0.###} MHz";
    }

    // Antenna bands may be entered slightly off the nominal band frequency
    // (e.g. 10.1 for 30m, 24.9 for 12m, 1296 for 23cm); tolerate 5% but at least 1 MHz.
    // The nearest band is picked first, so neighbouring bands never overlap in practice.
    private static double BandMatchToleranceMHz(double nominal) => Math.Max(1.0, nominal * 0.05);

    public static void Save(MasterDataFile data)
    {
        var dir = Path.GetDirectoryName(AppPaths.MasterDataFile);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var json = JsonSerializer.Serialize(data, JsonOptions);
        File.WriteAllText(AppPaths.MasterDataFile, json);
        _cachedBands = data.Bands.ToList();
    }

    public static MasterDataFile CreateDefaultMasterData()
    {
        return new MasterDataFile
        {
            Bands = new List<BandDefinition>
            {
                new() { Name = "160m", FrequencyMHz = 1.8 },
                new() { Name = "80m", FrequencyMHz = 3.5 },
                new() { Name = "40m", FrequencyMHz = 7.0 },
                new() { Name = "30m", FrequencyMHz = 10.0 },
                new() { Name = "20m", FrequencyMHz = 14.0 },
                new() { Name = "17m", FrequencyMHz = 18.0 },
                new() { Name = "15m", FrequencyMHz = 21.0 },
                new() { Name = "12m", FrequencyMHz = 24.0 },
                new() { Name = "10m", FrequencyMHz = 28.0 },
                new() { Name = "6m", FrequencyMHz = 50.0 },
                new() { Name = "4m", FrequencyMHz = 70.0 },
                new() { Name = "2m", FrequencyMHz = 144.0 },
                new() { Name = "70cm", FrequencyMHz = 430.0 }
            },
            Constants = new MasterConstants()
        };
    }

    public static IReadOnlyList<double> GetBandFrequencies()
    {
        return Load().Bands.Select(b => b.FrequencyMHz).ToList();
    }
}
