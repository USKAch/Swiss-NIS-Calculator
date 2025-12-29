using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NIS.Core.Data;
using NIS.Core.Models;

namespace NIS.Desktop.New.Services;

/// <summary>
/// Single source of truth for all master data (antennas, cables, radios).
/// On first launch, copies embedded data to external JSON files.
/// User-created items are appended with IsProjectSpecific = true.
/// Data is stored next to the executable for portability.
/// </summary>
public class MasterDataStore
{
    private static readonly string DataFolder = Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().Location) ?? ".";

    private static readonly string AntennasFile = Path.Combine(DataFolder, "antennas.json");
    private static readonly string CablesFile = Path.Combine(DataFolder, "cables.json");
    private static readonly string RadiosFile = Path.Combine(DataFolder, "radios.json");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public List<Antenna> Antennas { get; private set; } = new();
    public List<Cable> Cables { get; private set; } = new();
    public List<Radio> Radios { get; private set; } = new();

    /// <summary>
    /// Singleton instance
    /// </summary>
    public static MasterDataStore Instance { get; } = new();

    private MasterDataStore()
    {
        Load();
    }

    /// <summary>
    /// Load master data from disk. If files don't exist, initialize from embedded resources.
    /// </summary>
    public void Load()
    {
        EnsureDataFolder();

        // Load antennas (or initialize from embedded)
        if (File.Exists(AntennasFile))
        {
            Antennas = LoadList<Antenna>(AntennasFile);
        }
        else
        {
            InitializeAntennasFromEmbedded();
        }

        // Load cables (or initialize from embedded)
        if (File.Exists(CablesFile))
        {
            Cables = LoadList<Cable>(CablesFile);
        }
        else
        {
            InitializeCablesFromEmbedded();
        }

        // Load radios (or initialize from embedded)
        if (File.Exists(RadiosFile))
        {
            Radios = LoadList<Radio>(RadiosFile);
        }
        else
        {
            InitializeRadiosFromEmbedded();
        }
    }

    private void InitializeAntennasFromEmbedded()
    {
        var db = new AntennaDatabase();
        db.LoadDefaults();
        Antennas = db.Antennas.ToList();
        // Mark as shipped (not user-created)
        foreach (var a in Antennas) a.IsProjectSpecific = false;
        SaveAntennas();
    }

    private void InitializeCablesFromEmbedded()
    {
        var db = new CableDatabase();
        db.LoadDefaults();
        Cables = db.Cables.ToList();
        // Mark as shipped (not user-created)
        foreach (var c in Cables) c.IsProjectSpecific = false;
        SaveCables();
    }

    private void InitializeRadiosFromEmbedded()
    {
        var db = new RadioDatabase();
        db.LoadDefaults();
        Radios = db.Radios.ToList();
        // Mark as shipped (not user-created)
        foreach (var r in Radios) r.IsProjectSpecific = false;
        SaveRadios();
    }

    /// <summary>
    /// Add or update an antenna and save immediately. Always sorted alphabetically.
    /// </summary>
    public void SaveAntenna(Antenna antenna)
    {
        antenna.IsProjectSpecific = true;

        var existingIndex = Antennas.FindIndex(a =>
            a.Manufacturer.Equals(antenna.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
            a.Model.Equals(antenna.Model, StringComparison.OrdinalIgnoreCase));

        if (existingIndex >= 0)
            Antennas[existingIndex] = antenna;
        else
            Antennas.Add(antenna);

        SaveAntennas();
    }

    /// <summary>
    /// Add or update a cable and save immediately. Always sorted alphabetically.
    /// </summary>
    public void SaveCable(Cable cable)
    {
        cable.IsProjectSpecific = true;

        var existingIndex = Cables.FindIndex(c =>
            c.Name.Equals(cable.Name, StringComparison.OrdinalIgnoreCase));

        if (existingIndex >= 0)
            Cables[existingIndex] = cable;
        else
            Cables.Add(cable);

        SaveCables();
    }

    /// <summary>
    /// Add or update a radio and save immediately. Always sorted alphabetically.
    /// </summary>
    public void SaveRadio(Radio radio)
    {
        radio.IsProjectSpecific = true;

        var existingIndex = Radios.FindIndex(r =>
            r.Manufacturer.Equals(radio.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
            r.Model.Equals(radio.Model, StringComparison.OrdinalIgnoreCase));

        if (existingIndex >= 0)
            Radios[existingIndex] = radio;
        else
            Radios.Add(radio);

        SaveRadios();
    }

    /// <summary>
    /// Delete an antenna and save immediately.
    /// </summary>
    public void DeleteAntenna(Antenna antenna)
    {
        Antennas.RemoveAll(a =>
            a.Manufacturer.Equals(antenna.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
            a.Model.Equals(antenna.Model, StringComparison.OrdinalIgnoreCase));
        SaveAntennas();
    }

    /// <summary>
    /// Delete a cable and save immediately.
    /// </summary>
    public void DeleteCable(Cable cable)
    {
        Cables.RemoveAll(c => c.Name.Equals(cable.Name, StringComparison.OrdinalIgnoreCase));
        SaveCables();
    }

    /// <summary>
    /// Delete a radio and save immediately.
    /// </summary>
    public void DeleteRadio(Radio radio)
    {
        Radios.RemoveAll(r =>
            r.Manufacturer.Equals(radio.Manufacturer, StringComparison.OrdinalIgnoreCase) &&
            r.Model.Equals(radio.Model, StringComparison.OrdinalIgnoreCase));
        SaveRadios();
    }

    /// <summary>
    /// Check if an antenna with the same name exists.
    /// </summary>
    public bool AntennaExists(string manufacturer, string model, Antenna? exclude = null)
    {
        return Antennas.Any(a =>
            a != exclude &&
            a.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase) &&
            a.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Check if a cable with the same name exists.
    /// </summary>
    public bool CableExists(string name, Cable? exclude = null)
    {
        return Cables.Any(c =>
            c != exclude &&
            c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Check if a radio with the same name exists.
    /// </summary>
    public bool RadioExists(string manufacturer, string model, Radio? exclude = null)
    {
        return Radios.Any(r =>
            r != exclude &&
            r.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase) &&
            r.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
    }

    private void SaveAntennas()
    {
        // Sort alphabetically before saving
        Antennas = Antennas
            .OrderBy(a => a.Manufacturer, StringComparer.OrdinalIgnoreCase)
            .ThenBy(a => a.Model, StringComparer.OrdinalIgnoreCase)
            .ToList();
        SaveList(AntennasFile, Antennas);
    }

    private void SaveCables()
    {
        // Sort alphabetically before saving
        Cables = Cables
            .OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
        SaveList(CablesFile, Cables);
    }

    private void SaveRadios()
    {
        // Sort alphabetically before saving
        Radios = Radios
            .OrderBy(r => r.Manufacturer, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.Model, StringComparer.OrdinalIgnoreCase)
            .ToList();
        SaveList(RadiosFile, Radios);
    }

    private static void EnsureDataFolder()
    {
        if (!Directory.Exists(DataFolder))
            Directory.CreateDirectory(DataFolder);
    }

    private static List<T> LoadList<T>(string filePath)
    {
        try
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? new List<T>();
        }
        catch (Exception)
        {
            return new List<T>();
        }
    }

    private static void SaveList<T>(string filePath, List<T> items)
    {
        try
        {
            var json = JsonSerializer.Serialize(items, JsonOptions);
            File.WriteAllText(filePath, json);
        }
        catch (Exception)
        {
            // Ignore save errors
        }
    }
}
