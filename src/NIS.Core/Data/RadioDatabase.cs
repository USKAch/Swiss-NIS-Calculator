using System.Text.Json;
using NIS.Core.Models;

namespace NIS.Core.Data;

/// <summary>
/// Manages radio data loading and access.
/// </summary>
public class RadioDatabase
{
    private readonly List<Radio> _radios = [];

    /// <summary>
    /// All loaded radios.
    /// </summary>
    public IReadOnlyList<Radio> Radios => _radios;

    /// <summary>
    /// Loads radio data from a JSON file.
    /// </summary>
    public async Task LoadFromJsonAsync(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        LoadFromJson(json);
    }

    /// <summary>
    /// Loads radio data from a JSON string.
    /// </summary>
    public void LoadFromJson(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var data = JsonSerializer.Deserialize<RadioDataFile>(json, options);
        if (data?.Radios == null) return;

        _radios.Clear();
        _radios.AddRange(data.Radios);
    }

    /// <summary>
    /// Loads default radio data embedded in the application.
    /// </summary>
    public void LoadDefaults()
    {
        var defaultJson = GetDefaultRadioJson();
        LoadFromJson(defaultJson);
    }

    /// <summary>
    /// Gets a radio by manufacturer and model.
    /// </summary>
    public Radio? GetByName(string manufacturer, string model)
    {
        return _radios.FirstOrDefault(r =>
            r.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase) &&
            r.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets a radio by display name.
    /// </summary>
    public Radio? GetByDisplayName(string displayName)
    {
        return _radios.FirstOrDefault(r =>
            r.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase));
    }

    private static string GetDefaultRadioJson()
    {
        var assembly = typeof(RadioDatabase).Assembly;
        using var stream = assembly.GetManifestResourceStream("NIS.Core.Resources.radios.json");
        if (stream == null)
        {
            // Fallback to minimal default if resource not found
            return """{"radios": [{"manufacturer": "Generic", "model": "HF Transceiver", "maxPowerWatts": 100}]}""";
        }
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private class RadioDataFile
    {
        public List<Radio>? Radios { get; set; }
    }
}
