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
            return JsonSerializer.Deserialize<MasterDataFile>(json, JsonOptions) ?? CreateDefaultMasterData();
        }
        catch
        {
            return CreateDefaultMasterData();
        }
    }

    public static void Save(MasterDataFile data)
    {
        var dir = Path.GetDirectoryName(AppPaths.MasterDataFile);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var json = JsonSerializer.Serialize(data, JsonOptions);
        File.WriteAllText(AppPaths.MasterDataFile, json);
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
                new() { Name = "30m", FrequencyMHz = 10.1 },
                new() { Name = "20m", FrequencyMHz = 14.0 },
                new() { Name = "17m", FrequencyMHz = 18.1 },
                new() { Name = "15m", FrequencyMHz = 21.0 },
                new() { Name = "12m", FrequencyMHz = 24.9 },
                new() { Name = "10m", FrequencyMHz = 28.0 },
                new() { Name = "6m", FrequencyMHz = 50.0 },
                new() { Name = "2m", FrequencyMHz = 144.0 },
                new() { Name = "70cm", FrequencyMHz = 432.0 }
            },
            Constants = new MasterConstants()
        };
    }

    public static IReadOnlyList<double> GetBandFrequencies()
    {
        return Load().Bands.Select(b => b.FrequencyMHz).ToList();
    }
}
