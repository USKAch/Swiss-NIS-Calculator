using System.Text.Json;
using NIS.Core.Models;

namespace NIS.Core.Data;

/// <summary>
/// Manages cable data loading and access.
/// </summary>
public class CableDatabase
{
    private readonly List<Cable> _cables = [];

    /// <summary>
    /// All loaded cables.
    /// </summary>
    public IReadOnlyList<Cable> Cables => _cables;

    /// <summary>
    /// Loads cable data from a JSON file.
    /// </summary>
    public async Task LoadFromJsonAsync(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        LoadFromJson(json);
    }

    /// <summary>
    /// Loads cable data from a JSON string.
    /// Expected format matches FSD cables.json structure.
    /// </summary>
    public void LoadFromJson(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var data = JsonSerializer.Deserialize<CableDataFile>(json, options);
        if (data?.Cables == null) return;

        _cables.Clear();
        _cables.AddRange(data.Cables);
    }

    /// <summary>
    /// Loads default cable data embedded in the application.
    /// </summary>
    public void LoadDefaults()
    {
        var defaultJson = GetDefaultCableJson();
        LoadFromJson(defaultJson);
    }

    /// <summary>
    /// Gets a cable by name.
    /// </summary>
    public Cable? GetByName(string name)
    {
        return _cables.FirstOrDefault(c =>
            c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private static string GetDefaultCableJson()
    {
        var assembly = typeof(CableDatabase).Assembly;
        using var stream = assembly.GetManifestResourceStream("NIS.Core.Resources.cables.json");
        if (stream == null)
        {
            // Fallback to minimal default if resource not found
            return """{"cables": [{"name": "No Cable", "attenuationPer100m": {}}]}""";
        }
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private class CableDataFile
    {
        public List<Cable>? Cables { get; set; }
    }
}
