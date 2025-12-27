using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NIS.Core.Models;

namespace NIS.Desktop.Services;

/// <summary>
/// Singleton service for persisting OKAs to a local JSON file.
/// Shared across all ViewModels for consistent OKA data.
/// </summary>
public class OkaStorageService
{
    private static OkaStorageService? _instance;
    private static readonly object _lock = new();

    public static OkaStorageService Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new OkaStorageService();
                }
            }
            return _instance;
        }
    }

    private readonly string _filePath;
    private List<Oka> _okas = new();

    private OkaStorageService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "SwissNISCalculator");
        Directory.CreateDirectory(appFolder);
        _filePath = Path.Combine(appFolder, "okas.json");
        Load();
    }

    public IReadOnlyList<Oka> Okas => _okas;

    public int NextId => _okas.Count > 0 ? _okas.Max(o => o.Id) + 1 : 1;

    public void Load()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                var data = JsonSerializer.Deserialize<OkaDataFile>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                _okas = data?.Okas ?? new List<Oka>();
            }
        }
        catch
        {
            _okas = new List<Oka>();
        }
    }

    public void Save()
    {
        try
        {
            var data = new OkaDataFile { Okas = _okas };
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_filePath, json);
        }
        catch
        {
            // Ignore save errors
        }
    }

    public void AddOrUpdate(Oka oka)
    {
        var existing = _okas.FirstOrDefault(o => o.Id == oka.Id);
        if (existing != null)
        {
            var index = _okas.IndexOf(existing);
            _okas[index] = oka;
        }
        else
        {
            // Assign next ID if not set
            if (oka.Id <= 0)
            {
                oka.Id = NextId;
            }
            _okas.Add(oka);
        }
        Save();
    }

    public void Remove(int id)
    {
        var oka = _okas.FirstOrDefault(o => o.Id == id);
        if (oka != null)
        {
            _okas.Remove(oka);
            Save();
        }
    }

    public Oka? GetById(int id) => _okas.FirstOrDefault(o => o.Id == id);

    public Oka? GetByName(string name) => _okas.FirstOrDefault(o =>
        o.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    private class OkaDataFile
    {
        public List<Oka>? Okas { get; set; }
    }
}
