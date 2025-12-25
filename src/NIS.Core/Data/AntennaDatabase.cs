using System.Text.Json;
using NIS.Core.Models;

namespace NIS.Core.Data;

/// <summary>
/// Manages antenna data loading and access.
/// </summary>
public class AntennaDatabase
{
    private readonly List<Antenna> _antennas = [];

    /// <summary>
    /// All loaded antennas.
    /// </summary>
    public IReadOnlyList<Antenna> Antennas => _antennas;

    /// <summary>
    /// Loads antenna data from a JSON file.
    /// </summary>
    public async Task LoadFromJsonAsync(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        LoadFromJson(json);
    }

    /// <summary>
    /// Loads antenna data from a JSON string.
    /// Expected format matches FSD antennas.json structure.
    /// Consolidates multiband antennas (same manufacturer/model) into single entries.
    /// </summary>
    public void LoadFromJson(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var data = JsonSerializer.Deserialize<AntennaDataFile>(json, options);
        if (data?.Antennas == null) return;

        _antennas.Clear();

        // Consolidate antennas with same manufacturer/model into multiband entries
        var grouped = data.Antennas
            .GroupBy(a => new { a.Manufacturer, a.Model })
            .ToList();

        foreach (var group in grouped)
        {
            var first = group.First();
            var consolidated = new Antenna
            {
                Manufacturer = first.Manufacturer,
                Model = first.Model,
                IsRotatable = first.IsRotatable
            };

            // Merge all bands from all entries in the group
            foreach (var antenna in group)
            {
                foreach (var band in antenna.Bands)
                {
                    // Only add if not already present
                    if (!consolidated.Bands.Any(b => Math.Abs(b.FrequencyMHz - band.FrequencyMHz) < 0.5))
                    {
                        consolidated.Bands.Add(band);
                    }
                }
            }

            // Sort bands by frequency
            consolidated.Bands = consolidated.Bands.OrderBy(b => b.FrequencyMHz).ToList();

            _antennas.Add(consolidated);
        }
    }

    /// <summary>
    /// Loads default antenna data embedded in the application.
    /// </summary>
    public void LoadDefaults()
    {
        var defaultJson = GetDefaultAntennaJson();
        LoadFromJson(defaultJson);
    }

    /// <summary>
    /// Gets antennas that support a specific frequency band.
    /// </summary>
    public IEnumerable<Antenna> GetForFrequency(double frequencyMHz)
    {
        return _antennas.Where(a => a.GetBand(frequencyMHz) != null);
    }

    /// <summary>
    /// Gets all unique manufacturers.
    /// </summary>
    public IEnumerable<string> GetManufacturers()
    {
        return _antennas.Select(a => a.Manufacturer).Distinct().OrderBy(m => m);
    }

    /// <summary>
    /// Gets antennas by manufacturer.
    /// </summary>
    public IEnumerable<Antenna> GetByManufacturer(string manufacturer)
    {
        return _antennas.Where(a =>
            a.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets an antenna by manufacturer and model.
    /// </summary>
    public Antenna? GetByName(string manufacturer, string model)
    {
        return _antennas.FirstOrDefault(a =>
            a.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase) &&
            a.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
    }

    private static string GetDefaultAntennaJson()
    {
        var assembly = typeof(AntennaDatabase).Assembly;
        using var stream = assembly.GetManifestResourceStream("NIS.Core.Resources.antennas.json");
        if (stream == null)
        {
            // Fallback to minimal default if resource not found
            return """{"antennas": [{"manufacturer": "Custom", "model": "User Defined", "isRotatable": false, "bands": []}]}""";
        }
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private class AntennaDataFile
    {
        public List<Antenna>? Antennas { get; set; }
    }
}
