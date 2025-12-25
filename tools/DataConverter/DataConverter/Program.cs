using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

// Set culture for parsing decimal numbers
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

var baseDir = @"D:\Github\Swiss-NIS-Calculator";
var antDir = Path.Combine(baseDir, "Example", "ant_dat");
var outputDir = Path.Combine(baseDir, "data");

Console.WriteLine("VB6 Data Converter");
Console.WriteLine("==================");

// Parse all .ant files
var antennas = new List<AntennaEntry>();
var antFiles = Directory.GetFiles(antDir, "*.ant");

Console.WriteLine($"Found {antFiles.Length} antenna files");

foreach (var file in antFiles)
{
    try
    {
        var lines = File.ReadAllLines(file);
        if (lines.Length < 2) continue;

        string manufacturer = "";
        string type = "";
        var frequencies = new List<FrequencyEntry>();
        bool headerSkipped = false;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line == ":") continue;

            var parts = line.Split(':');
            if (parts.Length < 5) continue;

            // Skip header row (contains "Hersteller" or "f(MHz)")
            if (!headerSkipped && (line.Contains("Hersteller") || line.Contains("f(MHz)")))
            {
                headerSkipped = true;
                continue;
            }

            // First data line has manufacturer and type
            if (string.IsNullOrEmpty(manufacturer) && parts.Length > 2 && !string.IsNullOrEmpty(parts[2]))
            {
                manufacturer = parts[2].Trim();
            }
            if (string.IsNullOrEmpty(type) && parts.Length > 3 && !string.IsNullOrEmpty(parts[3]))
            {
                type = parts[3].Trim();
            }

            // Parse frequency data (column 4 = frequency, column 5 = gain, columns 6-15 = pattern)
            if (parts.Length >= 6 && double.TryParse(parts[4], NumberStyles.Any, CultureInfo.InvariantCulture, out double freq) && freq > 0)
            {
                var freqEntry = new FrequencyEntry { FrequencyMHz = freq };

                if (parts.Length > 5 && double.TryParse(parts[5], NumberStyles.Any, CultureInfo.InvariantCulture, out double gain))
                {
                    freqEntry.GainDbi = gain;
                }

                // Pattern data (0-90 degrees in 10 degree steps)
                var pattern = new List<double>();
                for (int i = 6; i < Math.Min(parts.Length, 16); i++)
                {
                    if (double.TryParse(parts[i], NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                        pattern.Add(val);
                    else if (!string.IsNullOrWhiteSpace(parts[i]))
                        break; // Stop if we hit non-numeric data
                }

                if (pattern.Count > 0)
                {
                    freqEntry.PatternDegrees = pattern.ToArray();
                }

                if (freqEntry.GainDbi > 0 || (freqEntry.PatternDegrees?.Length ?? 0) > 0)
                {
                    frequencies.Add(freqEntry);
                }
            }
        }

        if (!string.IsNullOrEmpty(type) && frequencies.Count > 0)
        {
            antennas.Add(new AntennaEntry
            {
                Manufacturer = manufacturer,
                Name = type,
                Frequencies = frequencies
            });
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error parsing {Path.GetFileName(file)}: {ex.Message}");
    }
}

Console.WriteLine($"Successfully parsed {antennas.Count} antennas");

// Parse cable data
var cables = new List<CableEntry>();
var cableFile = Path.Combine(antDir, "Kabelxdaten1.dat");

if (File.Exists(cableFile))
{
    var lines = File.ReadAllLines(cableFile);
    if (lines.Length > 1)
    {
        // First line is header with cable names
        var header = lines[0].Split(':');
        var cableNames = header.Skip(2).Where(n => !string.IsNullOrWhiteSpace(n)).ToList();

        Console.WriteLine($"Found {cableNames.Count} cable types in header");

        // Initialize cables
        foreach (var name in cableNames)
        {
            cables.Add(new CableEntry { Name = name.Trim() });
        }

        // Parse frequency rows
        for (int i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split(':');
            if (parts.Length < 3) continue;

            if (double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double freq) && freq > 0)
            {
                for (int j = 0; j < cableNames.Count && j + 2 < parts.Length; j++)
                {
                    if (double.TryParse(parts[j + 2], NumberStyles.Any, CultureInfo.InvariantCulture, out double atten) && atten > 0)
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

Console.WriteLine($"Successfully parsed {cables.Count} cables with data");

// Add "No Cable" option
cables.Insert(0, new CableEntry { Name = "No Cable" });

// Write JSON output
Directory.CreateDirectory(Path.Combine(outputDir, "antennas"));
Directory.CreateDirectory(Path.Combine(outputDir, "cables"));

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

// Convert to multi-band antenna format (matching NIS.Core.Models.Antenna)
var multiBandAntennas = new List<MultiBandAntennaEntry>();
foreach (var ant in antennas)
{
    var bands = new List<BandEntry>();
    foreach (var freq in ant.Frequencies)
    {
        // Keep pattern as 0-90° (10 values) - vertical attenuation
        // 0° = horizon, 90° = straight down toward OKA
        double[]? pattern = null;
        if (freq.PatternDegrees != null && freq.PatternDegrees.Length > 0)
        {
            pattern = new double[10];
            for (int i = 0; i < 10; i++)
            {
                pattern[i] = i < freq.PatternDegrees.Length ? freq.PatternDegrees[i] : 0;
            }
        }
        else
        {
            // Default flat pattern (no attenuation at any angle)
            pattern = new double[10];
        }

        bands.Add(new BandEntry
        {
            FrequencyMHz = freq.FrequencyMHz,
            GainDbi = freq.GainDbi,
            Pattern = pattern
        });
    }

    multiBandAntennas.Add(new MultiBandAntennaEntry
    {
        Manufacturer = ant.Manufacturer,
        Model = ant.Name,
        IsRotatable = true, // Assume rotatable for directional antennas
        Bands = bands
    });
}

// Write antennas in multi-band format
var antennaOutput = new { Antennas = multiBandAntennas };
var antennasJson = JsonSerializer.Serialize(antennaOutput, jsonOptions);
var antennasPath = Path.Combine(outputDir, "antennas", "antennas.json");
File.WriteAllText(antennasPath, antennasJson);
Console.WriteLine($"Wrote {multiBandAntennas.Count} antennas with {multiBandAntennas.Sum(a => a.Bands.Count)} total bands to {antennasPath}");

// Write cables in expected format (convert double keys to string keys)
var cableOutput = new
{
    Cables = cables.Select(c => new
    {
        Name = c.Name,
        AttenuationPer100m = c.AttenuationPer100m.ToDictionary(
            kvp => kvp.Key.ToString(CultureInfo.InvariantCulture),
            kvp => kvp.Value)
    }).ToList()
};
var cablesJson = JsonSerializer.Serialize(cableOutput, jsonOptions);
var cablesPath = Path.Combine(outputDir, "cables", "cables.json");
File.WriteAllText(cablesPath, cablesJson);
Console.WriteLine($"Wrote {cables.Count} cables to {cablesPath}");

// Print some stats
Console.WriteLine("\nTop 10 manufacturers by antenna count:");
var byManufacturer = antennas.GroupBy(a => a.Manufacturer)
    .OrderByDescending(g => g.Count())
    .Take(10);
foreach (var g in byManufacturer)
{
    Console.WriteLine($"  {g.Key}: {g.Count()} antennas");
}

Console.WriteLine("\nCables:");
foreach (var cable in cables.Take(15))
{
    Console.WriteLine($"  {cable.Name}: {cable.AttenuationPer100m.Count} frequency points");
}

Console.WriteLine("\nDone!");

// Models
class AntennaEntry
{
    public string Manufacturer { get; set; } = "";
    public string Name { get; set; } = "";
    public List<FrequencyEntry> Frequencies { get; set; } = new();
}

class FrequencyEntry
{
    public double FrequencyMHz { get; set; }
    public double GainDbi { get; set; }
    public double[]? PatternDegrees { get; set; }
}

class CableEntry
{
    public string Name { get; set; } = "";
    public Dictionary<double, double> AttenuationPer100m { get; set; } = new();
}

class MultiBandAntennaEntry
{
    public string Manufacturer { get; set; } = "";
    public string Model { get; set; } = "";
    public bool IsRotatable { get; set; }
    public List<BandEntry> Bands { get; set; } = new();
}

class BandEntry
{
    public double FrequencyMHz { get; set; }
    public double GainDbi { get; set; }
    public double[] Pattern { get; set; } = new double[10]; // 0-90° in 10° steps
}
