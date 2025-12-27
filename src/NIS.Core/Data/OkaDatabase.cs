using System.Text.Json;
using NIS.Core.Models;

namespace NIS.Core.Data;

/// <summary>
/// Manages OKA (evaluation point) data loading and access.
/// Unlike other databases, OKA has no default entries - all are user-defined.
/// </summary>
public class OkaDatabase
{
    private readonly List<Oka> _okas = [];
    private int _nextId = 1;

    /// <summary>
    /// All loaded OKAs.
    /// </summary>
    public IReadOnlyList<Oka> Okas => _okas;

    /// <summary>
    /// Loads OKA data from a JSON file.
    /// </summary>
    public async Task LoadFromJsonAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            LoadDefaults();
            return;
        }

        var json = await File.ReadAllTextAsync(filePath);
        LoadFromJson(json);
    }

    /// <summary>
    /// Loads OKA data from a JSON string.
    /// </summary>
    public void LoadFromJson(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var data = JsonSerializer.Deserialize<OkaDataFile>(json, options);
        if (data?.Okas == null) return;

        _okas.Clear();
        _okas.AddRange(data.Okas);

        // Update next ID
        if (_okas.Count > 0)
        {
            _nextId = _okas.Max(o => o.Id) + 1;
        }
    }

    /// <summary>
    /// Saves OKA data to a JSON file.
    /// </summary>
    public async Task SaveToJsonAsync(string filePath)
    {
        var data = new OkaDataFile { Okas = _okas };
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(data, options);

        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(filePath, json);
    }

    /// <summary>
    /// Loads default OKA data - empty list since all OKAs are user-defined.
    /// </summary>
    public void LoadDefaults()
    {
        _okas.Clear();
        _nextId = 1;
    }

    /// <summary>
    /// Gets an OKA by ID.
    /// </summary>
    public Oka? GetById(int id)
    {
        return _okas.FirstOrDefault(o => o.Id == id);
    }

    /// <summary>
    /// Gets an OKA by name.
    /// </summary>
    public Oka? GetByName(string name)
    {
        return _okas.FirstOrDefault(o =>
            o.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Adds a new OKA and assigns it the next available ID.
    /// </summary>
    public Oka Add(string name, double defaultDampingDb = 0)
    {
        var oka = new Oka
        {
            Id = _nextId++,
            Name = name,
            DefaultDampingDb = defaultDampingDb
        };
        _okas.Add(oka);
        return oka;
    }

    /// <summary>
    /// Adds an existing OKA (preserving its ID).
    /// </summary>
    public void Add(Oka oka)
    {
        // Ensure unique ID
        if (_okas.Any(o => o.Id == oka.Id))
        {
            oka.Id = _nextId++;
        }
        _okas.Add(oka);
        if (oka.Id >= _nextId)
        {
            _nextId = oka.Id + 1;
        }
    }

    /// <summary>
    /// Removes an OKA by ID.
    /// </summary>
    public bool Remove(int id)
    {
        var oka = GetById(id);
        if (oka != null)
        {
            return _okas.Remove(oka);
        }
        return false;
    }

    private class OkaDataFile
    {
        public List<Oka>? Okas { get; set; }
    }
}
