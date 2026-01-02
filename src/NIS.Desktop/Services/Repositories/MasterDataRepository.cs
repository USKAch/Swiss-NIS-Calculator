using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using Dapper;
using NIS.Desktop.Models;

namespace NIS.Desktop.Services.Repositories;

/// <summary>
/// Repository for master data CRUD operations (Antennas, Cables, Radios, OKAs, Modulations).
/// Uses the same schema as DatabaseService.
/// </summary>
public class MasterDataRepository
{
    private readonly IDbConnection _connection;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public MasterDataRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    #region Antenna Operations

    public List<Antenna> GetAllAntennas()
    {
        var rows = _connection.Query<AntennaRow>(
            "SELECT * FROM Antennas ORDER BY Manufacturer, Model").ToList();
        return rows.Select(ToAntenna).ToList();
    }

    public Antenna? GetAntenna(string manufacturer, string model)
    {
        var row = _connection.QueryFirstOrDefault<AntennaRow>(
            "SELECT * FROM Antennas WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new { Manufacturer = manufacturer, Model = model });
        return row != null ? ToAntenna(row) : null;
    }

    public Antenna? GetAntennaById(int id)
    {
        var row = _connection.QueryFirstOrDefault<AntennaRow>(
            "SELECT * FROM Antennas WHERE Id = @Id", new { Id = id });
        return row != null ? ToAntenna(row) : null;
    }

    public int? GetAntennaId(string manufacturer, string model)
    {
        return _connection.QueryFirstOrDefault<int?>(
            "SELECT Id FROM Antennas WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new { Manufacturer = manufacturer, Model = model });
    }

    public bool AntennaExists(string manufacturer, string model)
    {
        return _connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM Antennas WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new { Manufacturer = manufacturer, Model = model }) > 0;
    }

    public void SaveAntenna(Antenna antenna, bool isAdminMode = false)
    {
        if (antenna.Id > 0)
        {
            var existing = GetAntennaById(antenna.Id);
            var isUserData = existing?.IsUserData ?? !isAdminMode;
            UpdateAntenna(antenna, isUserData);
        }
        else
        {
            var isUserData = !isAdminMode;
            InsertAntenna(antenna, isUserData);
        }
    }

    public void InsertAntenna(Antenna antenna, bool isUserData)
    {
        var bandsJson = JsonSerializer.Serialize(antenna.Bands.Select(b => new BandData
        {
            FrequencyMHz = b.FrequencyMHz,
            GainDbi = b.GainDbi,
            Pattern = b.Pattern
        }), JsonOptions);

        _connection.Execute(
            @"INSERT INTO Antennas (Manufacturer, Model, AntennaType, IsHorizontallyPolarized, IsUserData, BandsJson)
              VALUES (@Manufacturer, @Model, @AntennaType, @IsHorizontallyPolarized, @IsUserData, @BandsJson)",
            new
            {
                antenna.Manufacturer,
                antenna.Model,
                antenna.AntennaType,
                IsHorizontallyPolarized = antenna.IsHorizontallyPolarized ? 1 : 0,
                IsUserData = isUserData ? 1 : 0,
                BandsJson = bandsJson
            });
    }

    private void UpdateAntenna(Antenna antenna, bool isUserData)
    {
        var bandsJson = JsonSerializer.Serialize(antenna.Bands.Select(b => new BandData
        {
            FrequencyMHz = b.FrequencyMHz,
            GainDbi = b.GainDbi,
            Pattern = b.Pattern
        }), JsonOptions);

        _connection.Execute(@"
            UPDATE Antennas SET
                Manufacturer = @Manufacturer,
                Model = @Model,
                AntennaType = @AntennaType,
                IsHorizontallyPolarized = @IsHorizontallyPolarized,
                IsUserData = @IsUserData,
                BandsJson = @BandsJson
            WHERE Id = @Id",
            new
            {
                antenna.Id,
                antenna.Manufacturer,
                antenna.Model,
                antenna.AntennaType,
                IsHorizontallyPolarized = antenna.IsHorizontallyPolarized ? 1 : 0,
                IsUserData = isUserData ? 1 : 0,
                BandsJson = bandsJson
            });
    }

    public void DeleteAntenna(int id)
    {
        _connection.Execute("DELETE FROM Antennas WHERE Id = @Id", new { Id = id });
    }

    public List<EntityUsage> GetAntennaUsage(int antennaId)
    {
        return _connection.Query<EntityUsage>(@"
            SELECT p.Name as ProjectName, c.Name as ConfigurationName, c.ConfigNumber
            FROM Configurations c
            JOIN Projects p ON c.ProjectId = p.Id
            WHERE c.AntennaId = @AntennaId
            ORDER BY p.Name, c.ConfigNumber",
            new { AntennaId = antennaId }).ToList();
    }

    private Antenna ToAntenna(AntennaRow row)
    {
        var bands = JsonSerializer.Deserialize<List<BandData>>(row.BandsJson, JsonOptions) ?? new List<BandData>();
        return new Antenna
        {
            Id = row.Id,
            Manufacturer = row.Manufacturer,
            Model = row.Model,
            AntennaType = row.AntennaType,
            IsHorizontallyPolarized = row.IsHorizontallyPolarized == 1,
            IsUserData = row.IsUserData == 1,
            Bands = bands.Select(b => new AntennaBand
            {
                FrequencyMHz = b.FrequencyMHz,
                GainDbi = b.GainDbi,
                Pattern = b.Pattern ?? new double[10]
            }).ToList()
        };
    }

    #endregion

    #region Cable Operations

    public List<Cable> GetAllCables()
    {
        var rows = _connection.Query<CableRow>(
            "SELECT * FROM Cables ORDER BY Name").ToList();
        return rows.Select(ToCable).ToList();
    }

    public Cable? GetCable(string name)
    {
        var row = _connection.QueryFirstOrDefault<CableRow>(
            "SELECT * FROM Cables WHERE Name = @Name", new { Name = name });
        return row != null ? ToCable(row) : null;
    }

    public Cable? GetCableById(int id)
    {
        var row = _connection.QueryFirstOrDefault<CableRow>(
            "SELECT * FROM Cables WHERE Id = @Id", new { Id = id });
        return row != null ? ToCable(row) : null;
    }

    public int? GetCableId(string name)
    {
        return _connection.QueryFirstOrDefault<int?>(
            "SELECT Id FROM Cables WHERE Name = @Name", new { Name = name });
    }

    public bool CableExists(string name)
    {
        return _connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM Cables WHERE Name = @Name", new { Name = name }) > 0;
    }

    public void SaveCable(Cable cable, bool isAdminMode = false)
    {
        if (cable.Id > 0)
        {
            var existing = GetCableById(cable.Id);
            var isUserData = existing?.IsUserData ?? !isAdminMode;
            UpdateCable(cable, isUserData);
        }
        else
        {
            var isUserData = !isAdminMode;
            InsertCable(cable, isUserData);
        }
    }

    public void InsertCable(Cable cable, bool isUserData)
    {
        var attenuationsJson = JsonSerializer.Serialize(cable.AttenuationPer100m, JsonOptions);

        _connection.Execute(
            @"INSERT INTO Cables (Name, IsUserData, AttenuationsJson)
              VALUES (@Name, @IsUserData, @AttenuationsJson)",
            new
            {
                cable.Name,
                IsUserData = isUserData ? 1 : 0,
                AttenuationsJson = attenuationsJson
            });
    }

    private void UpdateCable(Cable cable, bool isUserData)
    {
        var attenuationsJson = JsonSerializer.Serialize(cable.AttenuationPer100m, JsonOptions);

        _connection.Execute(@"
            UPDATE Cables SET
                Name = @Name,
                IsUserData = @IsUserData,
                AttenuationsJson = @AttenuationsJson
            WHERE Id = @Id",
            new
            {
                cable.Id,
                cable.Name,
                IsUserData = isUserData ? 1 : 0,
                AttenuationsJson = attenuationsJson
            });
    }

    public void DeleteCable(int id)
    {
        _connection.Execute("DELETE FROM Cables WHERE Id = @Id", new { Id = id });
    }

    public List<EntityUsage> GetCableUsage(int cableId)
    {
        return _connection.Query<EntityUsage>(@"
            SELECT p.Name as ProjectName, c.Name as ConfigurationName, c.ConfigNumber
            FROM Configurations c
            JOIN Projects p ON c.ProjectId = p.Id
            WHERE c.CableId = @CableId
            ORDER BY p.Name, c.ConfigNumber",
            new { CableId = cableId }).ToList();
    }

    private Cable ToCable(CableRow row)
    {
        var attenuations = JsonSerializer.Deserialize<Dictionary<string, double>>(row.AttenuationsJson, JsonOptions)
            ?? new Dictionary<string, double>();
        return new Cable
        {
            Id = row.Id,
            Name = row.Name,
            IsUserData = row.IsUserData == 1,
            AttenuationPer100m = attenuations
        };
    }

    #endregion

    #region Radio Operations

    public List<Radio> GetAllRadios()
    {
        var rows = _connection.Query<RadioRow>(
            "SELECT * FROM Radios ORDER BY Manufacturer, Model").ToList();
        return rows.Select(ToRadio).ToList();
    }

    public Radio? GetRadio(string manufacturer, string model)
    {
        var row = _connection.QueryFirstOrDefault<RadioRow>(
            "SELECT * FROM Radios WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new { Manufacturer = manufacturer, Model = model });
        return row != null ? ToRadio(row) : null;
    }

    public Radio? GetRadioById(int id)
    {
        var row = _connection.QueryFirstOrDefault<RadioRow>(
            "SELECT * FROM Radios WHERE Id = @Id", new { Id = id });
        return row != null ? ToRadio(row) : null;
    }

    public int? GetRadioId(string manufacturer, string model)
    {
        return _connection.QueryFirstOrDefault<int?>(
            "SELECT Id FROM Radios WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new { Manufacturer = manufacturer, Model = model });
    }

    public bool RadioExists(string manufacturer, string model)
    {
        return _connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM Radios WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new { Manufacturer = manufacturer, Model = model }) > 0;
    }

    public void SaveRadio(Radio radio, bool isAdminMode = false)
    {
        if (radio.Id > 0)
        {
            var existing = GetRadioById(radio.Id);
            var isUserData = existing?.IsUserData ?? !isAdminMode;
            UpdateRadio(radio, isUserData);
        }
        else
        {
            var isUserData = !isAdminMode;
            InsertRadio(radio, isUserData);
        }
    }

    public void InsertRadio(Radio radio, bool isUserData)
    {
        _connection.Execute(
            @"INSERT INTO Radios (Manufacturer, Model, MaxPowerWatts, IsUserData)
              VALUES (@Manufacturer, @Model, @MaxPowerWatts, @IsUserData)",
            new
            {
                radio.Manufacturer,
                radio.Model,
                radio.MaxPowerWatts,
                IsUserData = isUserData ? 1 : 0
            });
    }

    private void UpdateRadio(Radio radio, bool isUserData)
    {
        _connection.Execute(@"
            UPDATE Radios SET
                Manufacturer = @Manufacturer,
                Model = @Model,
                MaxPowerWatts = @MaxPowerWatts,
                IsUserData = @IsUserData
            WHERE Id = @Id",
            new
            {
                radio.Id,
                radio.Manufacturer,
                radio.Model,
                radio.MaxPowerWatts,
                IsUserData = isUserData ? 1 : 0
            });
    }

    public void DeleteRadio(int id)
    {
        _connection.Execute("DELETE FROM Radios WHERE Id = @Id", new { Id = id });
    }

    public List<EntityUsage> GetRadioUsage(int radioId)
    {
        return _connection.Query<EntityUsage>(@"
            SELECT p.Name as ProjectName, c.Name as ConfigurationName, c.ConfigNumber
            FROM Configurations c
            JOIN Projects p ON c.ProjectId = p.Id
            WHERE c.RadioId = @RadioId
            ORDER BY p.Name, c.ConfigNumber",
            new { RadioId = radioId }).ToList();
    }

    private Radio ToRadio(RadioRow row)
    {
        return new Radio
        {
            Id = row.Id,
            Manufacturer = row.Manufacturer,
            Model = row.Model,
            MaxPowerWatts = row.MaxPowerWatts,
            IsUserData = row.IsUserData == 1
        };
    }

    #endregion

    #region OKA Operations

    public List<Oka> GetAllOkas()
    {
        return _connection.Query<Oka>(
            "SELECT Id, Name, DefaultDistanceMeters, DefaultDampingDb, IsUserData FROM Okas ORDER BY Name").ToList();
    }

    public Oka? GetOka(string name)
    {
        return _connection.QueryFirstOrDefault<Oka>(
            "SELECT Id, Name, DefaultDistanceMeters, DefaultDampingDb, IsUserData FROM Okas WHERE Name = @Name",
            new { Name = name });
    }

    public Oka? GetOkaById(int id)
    {
        return _connection.QueryFirstOrDefault<Oka>(
            "SELECT Id, Name, DefaultDistanceMeters, DefaultDampingDb, IsUserData FROM Okas WHERE Id = @Id",
            new { Id = id });
    }

    public int? GetOkaId(string name)
    {
        return _connection.QueryFirstOrDefault<int?>(
            "SELECT Id FROM Okas WHERE Name = @Name", new { Name = name });
    }

    public bool OkaExists(string name)
    {
        return _connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM Okas WHERE Name = @Name", new { Name = name }) > 0;
    }

    public void SaveOka(Oka oka, bool isAdminMode = false)
    {
        if (oka.Id > 0)
        {
            var existing = GetOkaById(oka.Id);
            var isUserData = existing?.IsUserData ?? oka.IsUserData;
            _connection.Execute(@"
                UPDATE Okas SET
                    Name = @Name,
                    DefaultDistanceMeters = @DefaultDistanceMeters,
                    DefaultDampingDb = @DefaultDampingDb,
                    IsUserData = @IsUserData
                WHERE Id = @Id",
                new { oka.Id, oka.Name, oka.DefaultDistanceMeters, oka.DefaultDampingDb, IsUserData = isUserData ? 1 : 0 });
        }
        else
        {
            var isUserData = !isAdminMode;
            _connection.Execute(@"
                INSERT INTO Okas (Name, DefaultDistanceMeters, DefaultDampingDb, IsUserData)
                VALUES (@Name, @DefaultDistanceMeters, @DefaultDampingDb, @IsUserData)",
                new { oka.Name, oka.DefaultDistanceMeters, oka.DefaultDampingDb, IsUserData = isUserData ? 1 : 0 });
        }
    }

    public void DeleteOka(int id)
    {
        _connection.Execute("DELETE FROM Okas WHERE Id = @Id", new { Id = id });
    }

    public List<EntityUsage> GetOkaUsage(int okaId)
    {
        return _connection.Query<EntityUsage>(@"
            SELECT p.Name as ProjectName, c.Name as ConfigurationName, c.ConfigNumber
            FROM Configurations c
            JOIN Projects p ON c.ProjectId = p.Id
            WHERE c.OkaId = @OkaId
            ORDER BY p.Name, c.ConfigNumber",
            new { OkaId = okaId }).ToList();
    }

    #endregion

    #region Modulation Operations

    public List<Modulation> GetAllModulations()
    {
        return _connection.Query<Modulation>(
            "SELECT Id, Name, Factor, IsUserData FROM Modulations ORDER BY Name").ToList();
    }

    public Modulation? GetModulationById(int id)
    {
        return _connection.QueryFirstOrDefault<Modulation>(
            "SELECT Id, Name, Factor, IsUserData FROM Modulations WHERE Id = @Id",
            new { Id = id });
    }

    public Modulation? GetModulationByName(string name)
    {
        return _connection.QueryFirstOrDefault<Modulation>(
            "SELECT Id, Name, Factor, IsUserData FROM Modulations WHERE Name = @Name",
            new { Name = name });
    }

    public int? GetModulationId(string name)
    {
        return _connection.QueryFirstOrDefault<int?>(
            "SELECT Id FROM Modulations WHERE Name = @Name", new { Name = name });
    }

    public void SaveModulation(Modulation modulation, bool isAdminMode = false)
    {
        var existing = GetModulationByName(modulation.Name);
        var isUserData = existing?.IsUserData ?? !isAdminMode;

        if (existing != null)
        {
            _connection.Execute(@"
                UPDATE Modulations SET
                    Factor = @Factor,
                    IsUserData = @IsUserData
                WHERE Name = @Name",
                new
                {
                    modulation.Name,
                    modulation.Factor,
                    IsUserData = isUserData ? 1 : 0
                });
        }
        else
        {
            InsertModulation(new Modulation
            {
                Name = modulation.Name,
                Factor = modulation.Factor,
                IsUserData = isUserData
            });
        }
    }

    public void InsertModulation(Modulation modulation)
    {
        _connection.Execute(@"
            INSERT INTO Modulations (Name, Factor, IsUserData)
            VALUES (@Name, @Factor, @IsUserData)",
            new
            {
                modulation.Name,
                modulation.Factor,
                IsUserData = modulation.IsUserData ? 1 : 0
            });
    }

    public void DeleteModulation(int id)
    {
        _connection.Execute("DELETE FROM Modulations WHERE Id = @Id", new { Id = id });
    }

    #endregion

    #region Row Classes

    private class AntennaRow
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = "";
        public string Model { get; set; } = "";
        public string AntennaType { get; set; } = "other";
        public int IsHorizontallyPolarized { get; set; }
        public int IsUserData { get; set; }
        public string BandsJson { get; set; } = "[]";
    }

    private class BandData
    {
        public double FrequencyMHz { get; set; }
        public double GainDbi { get; set; }
        public double[]? Pattern { get; set; }
    }

    private class CableRow
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int IsUserData { get; set; }
        public string AttenuationsJson { get; set; } = "{}";
    }

    private class RadioRow
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = "";
        public string Model { get; set; } = "";
        public double MaxPowerWatts { get; set; }
        public int IsUserData { get; set; }
    }

    #endregion
}
