// C# Script to convert VB6 antenna and cable data to JSON
// Run with: dotnet script ConvertVb6Data.csx

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

// Antenna model
class AntennaData
{
    public string Manufacturer { get; set; } = "";
    public string Type { get; set; } = "";
    public List<FrequencyData> Frequencies { get; set; } = new();
}

class FrequencyData
{
    public double FrequencyMHz { get; set; }
    public double GainDbi { get; set; }
    public double[] Pattern { get; set; } = Array.Empty<double>(); // 0-90 degrees, 10 degree steps
}

// Cable model
class CableData
{
    public string Name { get; set; } = "";
    public Dictionary<double, double> AttenuationPer100m { get; set; } = new(); // freq -> dB/100m
}

var antDir = @"D:\Github\Swiss-NIS-Calculator\Example\ant_dat";
var outputDir = @"D:\Github\Swiss-NIS-Calculator\data";

// Parse all .ant files
var antennas = new List<AntennaData>();
var antFiles = Directory.GetFiles(antDir, "*.ant");

Console.WriteLine($"Found {antFiles.Length} antenna files");

foreach (var file in antFiles)
{
    try
    {
        var lines = File.ReadAllLines(file);
        if (lines.Length < 2) continue;

        var antenna = new AntennaData();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line == ":") continue;

            var parts = line.Split(':');
            if (parts.Length < 5) continue;

            // First data line has manufacturer and type
            if (string.IsNullOrEmpty(antenna.Manufacturer) && !string.IsNullOrEmpty(parts[2]))
            {
                antenna.Manufacturer = parts[2].Trim();
            }
            if (string.IsNullOrEmpty(antenna.Type) && !string.IsNullOrEmpty(parts[3]))
            {
                antenna.Type = parts[3].Trim();
            }

            // Parse frequency data
            if (parts.Length >= 6 && double.TryParse(parts[4], out double freq) && freq > 0)
            {
                var freqData = new FrequencyData { FrequencyMHz = freq };

                if (double.TryParse(parts[5], out double gain))
                {
                    freqData.GainDbi = gain;
                }

                // Pattern data (0-90 degrees in 10 degree steps)
                var pattern = new List<double>();
                for (int i = 6; i < Math.Min(parts.Length, 16); i++)
                {
                    if (double.TryParse(parts[i], out double val))
                        pattern.Add(val);
                }
                if (pattern.Count > 0)
                {
                    freqData.Pattern = pattern.ToArray();
                }

                if (freqData.GainDbi > 0 || freqData.Pattern.Length > 0)
                {
                    antenna.Frequencies.Add(freqData);
                }
            }
        }

        if (!string.IsNullOrEmpty(antenna.Type) && antenna.Frequencies.Count > 0)
        {
            antennas.Add(antenna);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error parsing {file}: {ex.Message}");
    }
}

Console.WriteLine($"Successfully parsed {antennas.Count} antennas");

// Parse cable data
var cables = new List<CableData>();
var cableFile = Path.Combine(antDir, "Kabelxdaten1.dat");

if (File.Exists(cableFile))
{
    var lines = File.ReadAllLines(cableFile);
    if (lines.Length > 1)
    {
        // First line is header with cable names
        var header = lines[0].Split(':');
        var cableNames = header.Skip(2).Where(n => !string.IsNullOrWhiteSpace(n)).ToList();

        // Initialize cables
        foreach (var name in cableNames)
        {
            cables.Add(new CableData { Name = name.Trim() });
        }

        // Parse frequency rows
        for (int i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split(':');
            if (parts.Length < 3) continue;

            if (double.TryParse(parts[1], out double freq) && freq > 0)
            {
                for (int j = 0; j < cableNames.Count && j + 2 < parts.Length; j++)
                {
                    if (double.TryParse(parts[j + 2], out double atten) && atten > 0)
                    {
                        cables[j].AttenuationPer100m[freq] = atten;
                    }
                }
            }
        }

        // Remove cables with no data
        cables = cables.Where(c => c.AttenuationPer100m.Count > 0).ToList();
    }
}

Console.WriteLine($"Successfully parsed {cables.Count} cables");

// Write JSON output
Directory.CreateDirectory(Path.Combine(outputDir, "antennas"));
Directory.CreateDirectory(Path.Combine(outputDir, "cables"));

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

// Write antennas
var antennasJson = JsonSerializer.Serialize(antennas, jsonOptions);
File.WriteAllText(Path.Combine(outputDir, "antennas", "antennas_full.json"), antennasJson);
Console.WriteLine($"Wrote antennas_full.json");

// Write cables
var cablesJson = JsonSerializer.Serialize(cables, jsonOptions);
File.WriteAllText(Path.Combine(outputDir, "cables", "cables_full.json"), cablesJson);
Console.WriteLine($"Wrote cables_full.json");

Console.WriteLine("Done!");
