using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Dapper;
using Microsoft.Data.Sqlite;
using NIS.Core.Models;

namespace NIS.Desktop.Services;

/// <summary>
/// SQLite database service - single source of truth for all master data.
/// </summary>
public class DatabaseService : IDisposable
{

    private readonly SqliteConnection _connection;
    private static DatabaseService? _instance;
    private static readonly object _lock = new();
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public static DatabaseService Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new DatabaseService();
                }
            }
            return _instance;
        }
    }

    private DatabaseService()
    {
        var connectionString = $"Data Source={AppPaths.DatabaseFile}";
        _connection = new SqliteConnection(connectionString);
        _connection.Open();

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        _connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Antennas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Manufacturer TEXT NOT NULL,
                Model TEXT NOT NULL,
                AntennaType TEXT NOT NULL DEFAULT 'other',
                IsHorizontallyPolarized INTEGER NOT NULL DEFAULT 1,
                IsRotatable INTEGER NOT NULL DEFAULT 0,
                HorizontalAngleDegrees REAL NOT NULL DEFAULT 360,
                IsProjectSpecific INTEGER NOT NULL DEFAULT 0,
                BandsJson TEXT NOT NULL DEFAULT '[]',
                UNIQUE(Manufacturer, Model)
            );

            CREATE TABLE IF NOT EXISTS Cables (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE,
                IsProjectSpecific INTEGER NOT NULL DEFAULT 0,
                AttenuationsJson TEXT NOT NULL DEFAULT '{}'
            );

            CREATE TABLE IF NOT EXISTS Radios (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Manufacturer TEXT NOT NULL,
                Model TEXT NOT NULL,
                MaxPowerWatts REAL NOT NULL DEFAULT 100,
                IsProjectSpecific INTEGER NOT NULL DEFAULT 0,
                UNIQUE(Manufacturer, Model)
            );

            CREATE TABLE IF NOT EXISTS Projects (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Language TEXT NOT NULL DEFAULT 'de',
                Callsign TEXT,
                OperatorName TEXT,
                Address TEXT,
                Location TEXT,
                FilePath TEXT,
                CreatedAt TEXT NOT NULL,
                ModifiedAt TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Okas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProjectId INTEGER NOT NULL,
                Number INTEGER NOT NULL,
                Name TEXT NOT NULL,
                DefaultDistanceMeters REAL NOT NULL DEFAULT 10,
                DefaultDampingDb REAL NOT NULL DEFAULT 0,
                FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
            );

            CREATE TABLE IF NOT EXISTS Configurations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProjectId INTEGER NOT NULL,
                ConfigNumber INTEGER NOT NULL,
                Name TEXT,
                PowerWatts REAL NOT NULL DEFAULT 100,
                RadioId INTEGER,
                HasLinear INTEGER NOT NULL DEFAULT 0,
                LinearId INTEGER,
                CableId INTEGER,
                CableLengthMeters REAL NOT NULL DEFAULT 10,
                AdditionalLossDb REAL NOT NULL DEFAULT 0,
                AdditionalLossDescription TEXT,
                AntennaId INTEGER,
                HeightMeters REAL NOT NULL DEFAULT 10,
                IsHorizontallyRotatable INTEGER NOT NULL DEFAULT 1,
                HorizontalAngleDegrees REAL NOT NULL DEFAULT 360,
                IsVerticallyRotatable INTEGER NOT NULL DEFAULT 0,
                ModulationFactor REAL NOT NULL DEFAULT 0.4,
                ActivityFactor REAL NOT NULL DEFAULT 0.5,
                OkaName TEXT,
                OkaDistanceMeters REAL NOT NULL DEFAULT 10,
                OkaBuildingDampingDb REAL NOT NULL DEFAULT 0,
                FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
                FOREIGN KEY (RadioId) REFERENCES Radios(Id) ON DELETE SET NULL,
                FOREIGN KEY (LinearId) REFERENCES Radios(Id) ON DELETE SET NULL,
                FOREIGN KEY (CableId) REFERENCES Cables(Id) ON DELETE SET NULL,
                FOREIGN KEY (AntennaId) REFERENCES Antennas(Id) ON DELETE SET NULL
            );

            CREATE TABLE IF NOT EXISTS SchemaVersion (
                Version INTEGER PRIMARY KEY
            );
        ");

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

    public void SaveAntenna(Antenna antenna, bool isAdminMode = false)
    {
        var existing = GetAntenna(antenna.Manufacturer, antenna.Model);
        var isProjectSpecific = isAdminMode ? false : (existing?.IsProjectSpecific ?? true);

        if (existing != null)
        {
            UpdateAntenna(antenna, isProjectSpecific);
        }
        else
        {
            InsertAntenna(antenna, isProjectSpecific);
        }
    }

    private void InsertAntenna(Antenna antenna, bool isProjectSpecific)
    {
        var bandsJson = JsonSerializer.Serialize(antenna.Bands.Select(b => new BandData
        {
            FrequencyMHz = b.FrequencyMHz,
            GainDbi = b.GainDbi,
            Pattern = b.Pattern
        }), JsonOptions);

        _connection.Execute(@"
            INSERT INTO Antennas (Manufacturer, Model, AntennaType, IsHorizontallyPolarized, IsRotatable, HorizontalAngleDegrees, IsProjectSpecific, BandsJson)
            VALUES (@Manufacturer, @Model, @AntennaType, @IsHorizontallyPolarized, @IsRotatable, @HorizontalAngleDegrees, @IsProjectSpecific, @BandsJson)",
            new
            {
                antenna.Manufacturer,
                antenna.Model,
                antenna.AntennaType,
                IsHorizontallyPolarized = antenna.IsHorizontallyPolarized ? 1 : 0,
                IsRotatable = antenna.IsRotatable ? 1 : 0,
                antenna.HorizontalAngleDegrees,
                IsProjectSpecific = isProjectSpecific ? 1 : 0,
                BandsJson = bandsJson
            });
    }

    private void UpdateAntenna(Antenna antenna, bool isProjectSpecific)
    {
        var bandsJson = JsonSerializer.Serialize(antenna.Bands.Select(b => new BandData
        {
            FrequencyMHz = b.FrequencyMHz,
            GainDbi = b.GainDbi,
            Pattern = b.Pattern
        }), JsonOptions);

        _connection.Execute(@"
            UPDATE Antennas SET
                AntennaType = @AntennaType,
                IsHorizontallyPolarized = @IsHorizontallyPolarized,
                IsRotatable = @IsRotatable,
                HorizontalAngleDegrees = @HorizontalAngleDegrees,
                IsProjectSpecific = @IsProjectSpecific,
                BandsJson = @BandsJson
            WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new
            {
                antenna.Manufacturer,
                antenna.Model,
                antenna.AntennaType,
                IsHorizontallyPolarized = antenna.IsHorizontallyPolarized ? 1 : 0,
                IsRotatable = antenna.IsRotatable ? 1 : 0,
                antenna.HorizontalAngleDegrees,
                IsProjectSpecific = isProjectSpecific ? 1 : 0,
                BandsJson = bandsJson
            });
    }

    public void DeleteAntenna(string manufacturer, string model)
    {
        _connection.Execute(
            "DELETE FROM Antennas WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new { Manufacturer = manufacturer, Model = model });
    }

    public bool AntennaExists(string manufacturer, string model)
    {
        return _connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM Antennas WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new { Manufacturer = manufacturer, Model = model }) > 0;
    }

    public int? GetAntennaId(string manufacturer, string model)
    {
        return _connection.QueryFirstOrDefault<int?>(
            "SELECT Id FROM Antennas WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new { Manufacturer = manufacturer, Model = model });
    }

    public Antenna? GetAntennaById(int id)
    {
        var row = _connection.QueryFirstOrDefault<AntennaRow>(
            "SELECT * FROM Antennas WHERE Id = @Id", new { Id = id });
        return row != null ? ToAntenna(row) : null;
    }

    private Antenna ToAntenna(AntennaRow row)
    {
        var bands = JsonSerializer.Deserialize<List<BandData>>(row.BandsJson, JsonOptions) ?? new List<BandData>();
        return new Antenna
        {
            Manufacturer = row.Manufacturer,
            Model = row.Model,
            AntennaType = row.AntennaType,
            IsHorizontallyPolarized = row.IsHorizontallyPolarized == 1,
            IsRotatable = row.IsRotatable == 1,
            HorizontalAngleDegrees = row.HorizontalAngleDegrees,
            IsProjectSpecific = row.IsProjectSpecific == 1,
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

    public void SaveCable(Cable cable, bool isAdminMode = false)
    {
        var existing = GetCable(cable.Name);
        var isProjectSpecific = isAdminMode ? false : (existing?.IsProjectSpecific ?? true);

        if (existing != null)
        {
            UpdateCable(cable, isProjectSpecific);
        }
        else
        {
            InsertCable(cable, isProjectSpecific);
        }
    }

    private void InsertCable(Cable cable, bool isProjectSpecific)
    {
        var attenuationsJson = JsonSerializer.Serialize(cable.AttenuationPer100m, JsonOptions);

        _connection.Execute(@"
            INSERT INTO Cables (Name, IsProjectSpecific, AttenuationsJson)
            VALUES (@Name, @IsProjectSpecific, @AttenuationsJson)",
            new
            {
                cable.Name,
                IsProjectSpecific = isProjectSpecific ? 1 : 0,
                AttenuationsJson = attenuationsJson
            });
    }

    private void UpdateCable(Cable cable, bool isProjectSpecific)
    {
        var attenuationsJson = JsonSerializer.Serialize(cable.AttenuationPer100m, JsonOptions);

        _connection.Execute(@"
            UPDATE Cables SET
                IsProjectSpecific = @IsProjectSpecific,
                AttenuationsJson = @AttenuationsJson
            WHERE Name = @Name",
            new
            {
                cable.Name,
                IsProjectSpecific = isProjectSpecific ? 1 : 0,
                AttenuationsJson = attenuationsJson
            });
    }

    public void DeleteCable(string name)
    {
        _connection.Execute("DELETE FROM Cables WHERE Name = @Name", new { Name = name });
    }

    public bool CableExists(string name)
    {
        return _connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM Cables WHERE Name = @Name", new { Name = name }) > 0;
    }

    public int? GetCableId(string name)
    {
        return _connection.QueryFirstOrDefault<int?>(
            "SELECT Id FROM Cables WHERE Name = @Name", new { Name = name });
    }

    public Cable? GetCableById(int id)
    {
        var row = _connection.QueryFirstOrDefault<CableRow>(
            "SELECT * FROM Cables WHERE Id = @Id", new { Id = id });
        return row != null ? ToCable(row) : null;
    }

    private Cable ToCable(CableRow row)
    {
        var attenuations = JsonSerializer.Deserialize<Dictionary<string, double>>(row.AttenuationsJson, JsonOptions)
            ?? new Dictionary<string, double>();
        return new Cable
        {
            Name = row.Name,
            IsProjectSpecific = row.IsProjectSpecific == 1,
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

    public void SaveRadio(Radio radio, bool isAdminMode = false)
    {
        var existing = GetRadio(radio.Manufacturer, radio.Model);
        var isProjectSpecific = isAdminMode ? false : (existing?.IsProjectSpecific ?? true);

        if (existing != null)
        {
            UpdateRadio(radio, isProjectSpecific);
        }
        else
        {
            InsertRadio(radio, isProjectSpecific);
        }
    }

    private void InsertRadio(Radio radio, bool isProjectSpecific)
    {
        _connection.Execute(@"
            INSERT INTO Radios (Manufacturer, Model, MaxPowerWatts, IsProjectSpecific)
            VALUES (@Manufacturer, @Model, @MaxPowerWatts, @IsProjectSpecific)",
            new
            {
                radio.Manufacturer,
                radio.Model,
                radio.MaxPowerWatts,
                IsProjectSpecific = isProjectSpecific ? 1 : 0
            });
    }

    private void UpdateRadio(Radio radio, bool isProjectSpecific)
    {
        _connection.Execute(@"
            UPDATE Radios SET
                MaxPowerWatts = @MaxPowerWatts,
                IsProjectSpecific = @IsProjectSpecific
            WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new
            {
                radio.Manufacturer,
                radio.Model,
                radio.MaxPowerWatts,
                IsProjectSpecific = isProjectSpecific ? 1 : 0
            });
    }

    public void DeleteRadio(string manufacturer, string model)
    {
        _connection.Execute(
            "DELETE FROM Radios WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new { Manufacturer = manufacturer, Model = model });
    }

    public bool RadioExists(string manufacturer, string model)
    {
        return _connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM Radios WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new { Manufacturer = manufacturer, Model = model }) > 0;
    }

    public int? GetRadioId(string manufacturer, string model)
    {
        return _connection.QueryFirstOrDefault<int?>(
            "SELECT Id FROM Radios WHERE Manufacturer = @Manufacturer AND Model = @Model",
            new { Manufacturer = manufacturer, Model = model });
    }

    public Radio? GetRadioById(int id)
    {
        var row = _connection.QueryFirstOrDefault<RadioRow>(
            "SELECT * FROM Radios WHERE Id = @Id", new { Id = id });
        return row != null ? ToRadio(row) : null;
    }

    private Radio ToRadio(RadioRow row)
    {
        return new Radio
        {
            Manufacturer = row.Manufacturer,
            Model = row.Model,
            MaxPowerWatts = row.MaxPowerWatts,
            IsProjectSpecific = row.IsProjectSpecific == 1
        };
    }

    #endregion

    #region Project Operations

    public int CreateProject(Project project)
    {
        var now = DateTime.UtcNow.ToString("o");
        var id = _connection.ExecuteScalar<int>(@"
            INSERT INTO Projects (Name, Language, Callsign, OperatorName, Address, Location, FilePath, CreatedAt, ModifiedAt)
            VALUES (@Name, @Language, @Callsign, @OperatorName, @Address, @Location, @FilePath, @CreatedAt, @ModifiedAt);
            SELECT last_insert_rowid();",
            new
            {
                Name = project.Station.Callsign ?? "New Project",
                project.Language,
                project.Station.Callsign,
                OperatorName = project.Station.Operator,
                project.Station.Address,
                project.Station.Location,
                FilePath = (string?)null,
                CreatedAt = now,
                ModifiedAt = now
            });

        // Save OKAs
        foreach (var oka in project.Okas)
        {
            SaveOka(id, oka);
        }

        // Save configurations
        int configNum = 1;
        foreach (var config in project.AntennaConfigurations)
        {
            SaveConfiguration(id, config, configNum++);
        }

        return id;
    }

    public void UpdateProject(int projectId, Project project)
    {
        var now = DateTime.UtcNow.ToString("o");
        _connection.Execute(@"
            UPDATE Projects SET
                Name = @Name,
                Language = @Language,
                Callsign = @Callsign,
                OperatorName = @OperatorName,
                Address = @Address,
                Location = @Location,
                ModifiedAt = @ModifiedAt
            WHERE Id = @Id",
            new
            {
                Id = projectId,
                Name = project.Station.Callsign ?? "Project",
                project.Language,
                project.Station.Callsign,
                OperatorName = project.Station.Operator,
                project.Station.Address,
                project.Station.Location,
                ModifiedAt = now
            });

        // Delete and recreate OKAs and configurations
        _connection.Execute("DELETE FROM Okas WHERE ProjectId = @ProjectId", new { ProjectId = projectId });
        _connection.Execute("DELETE FROM Configurations WHERE ProjectId = @ProjectId", new { ProjectId = projectId });

        foreach (var oka in project.Okas)
        {
            SaveOka(projectId, oka);
        }

        int configNum = 1;
        foreach (var config in project.AntennaConfigurations)
        {
            SaveConfiguration(projectId, config, configNum++);
        }
    }

    public Project? GetProject(int projectId)
    {
        var row = _connection.QueryFirstOrDefault<ProjectRow>(
            "SELECT * FROM Projects WHERE Id = @Id", new { Id = projectId });
        if (row == null) return null;

        var project = new Project
        {
            Language = row.Language,
            Station = new StationInfo
            {
                Callsign = row.Callsign ?? "",
                Operator = row.OperatorName ?? "",
                Address = row.Address ?? "",
                Location = row.Location ?? ""
            }
        };

        // Load OKAs
        var okaRows = _connection.Query<OkaRow>(
            "SELECT * FROM Okas WHERE ProjectId = @ProjectId ORDER BY Number", new { ProjectId = projectId });
        project.Okas = okaRows.Select(r => new Oka
        {
            Id = r.Number,
            Name = r.Name,
            DefaultDistanceMeters = r.DefaultDistanceMeters,
            DefaultDampingDb = r.DefaultDampingDb
        }).ToList();

        // Load configurations
        var configRows = _connection.Query<ConfigurationRow>(
            "SELECT * FROM Configurations WHERE ProjectId = @ProjectId ORDER BY ConfigNumber", new { ProjectId = projectId });
        project.AntennaConfigurations = configRows.Select(ToConfiguration).ToList();

        return project;
    }

    public List<(int Id, string Name, string? Callsign, DateTime ModifiedAt)> GetRecentProjects(int limit = 5)
    {
        return _connection.Query<(int Id, string Name, string? Callsign, string ModifiedAt)>(
            "SELECT Id, Name, Callsign, ModifiedAt FROM Projects ORDER BY ModifiedAt DESC LIMIT @Limit",
            new { Limit = limit })
            .Select(r => (r.Id, r.Name, r.Callsign, DateTime.Parse(r.ModifiedAt)))
            .ToList();
    }

    public void DeleteProject(int projectId)
    {
        _connection.Execute("DELETE FROM Projects WHERE Id = @Id", new { Id = projectId });
    }

    #endregion

    #region OKA Operations

    private void SaveOka(int projectId, Oka oka)
    {
        _connection.Execute(@"
            INSERT INTO Okas (ProjectId, Number, Name, DefaultDistanceMeters, DefaultDampingDb)
            VALUES (@ProjectId, @Number, @Name, @DefaultDistanceMeters, @DefaultDampingDb)",
            new
            {
                ProjectId = projectId,
                Number = oka.Id,
                oka.Name,
                oka.DefaultDistanceMeters,
                oka.DefaultDampingDb
            });
    }

    #endregion

    #region Configuration Operations

    private void SaveConfiguration(int projectId, AntennaConfiguration config, int configNumber)
    {
        // Look up IDs from names
        var radioId = GetRadioId(config.Radio.Manufacturer, config.Radio.Model);
        var linearId = config.Linear != null ? GetRadioId(config.Linear.Manufacturer, config.Linear.Model) : null;
        var cableId = GetCableId(config.Cable.Type);
        var antennaId = GetAntennaId(config.Antenna.Manufacturer, config.Antenna.Model);

        _connection.Execute(@"
            INSERT INTO Configurations (
                ProjectId, ConfigNumber, Name, PowerWatts,
                RadioId, HasLinear, LinearId,
                CableId, CableLengthMeters, AdditionalLossDb, AdditionalLossDescription,
                AntennaId, HeightMeters,
                IsHorizontallyRotatable, HorizontalAngleDegrees, IsVerticallyRotatable,
                ModulationFactor, ActivityFactor,
                OkaName, OkaDistanceMeters, OkaBuildingDampingDb
            ) VALUES (
                @ProjectId, @ConfigNumber, @Name, @PowerWatts,
                @RadioId, @HasLinear, @LinearId,
                @CableId, @CableLengthMeters, @AdditionalLossDb, @AdditionalLossDescription,
                @AntennaId, @HeightMeters,
                @IsHorizontallyRotatable, @HorizontalAngleDegrees, @IsVerticallyRotatable,
                @ModulationFactor, @ActivityFactor,
                @OkaName, @OkaDistanceMeters, @OkaBuildingDampingDb
            )",
            new
            {
                ProjectId = projectId,
                ConfigNumber = configNumber,
                config.Name,
                config.PowerWatts,
                RadioId = radioId,
                HasLinear = config.Linear != null ? 1 : 0,
                LinearId = linearId,
                CableId = cableId,
                config.Cable.LengthMeters,
                AdditionalLossDb = config.Cable.AdditionalLossDb,
                AdditionalLossDescription = config.Cable.AdditionalLossDescription,
                AntennaId = antennaId,
                HeightMeters = config.Antenna.HeightMeters,
                IsHorizontallyRotatable = config.Antenna.IsHorizontallyRotatable ? 1 : 0,
                HorizontalAngleDegrees = config.Antenna.HorizontalAngleDegrees,
                IsVerticallyRotatable = config.Antenna.IsVerticallyRotatable ? 1 : 0,
                config.ModulationFactor,
                config.ActivityFactor,
                config.OkaName,
                config.OkaDistanceMeters,
                config.OkaBuildingDampingDb
            });
    }

    private AntennaConfiguration ToConfiguration(ConfigurationRow row)
    {
        // Look up entities by ID
        var radio = row.RadioId.HasValue ? GetRadioById(row.RadioId.Value) : null;
        var linear = row.HasLinear == 1 && row.LinearId.HasValue ? GetRadioById(row.LinearId.Value) : null;
        var cable = row.CableId.HasValue ? GetCableById(row.CableId.Value) : null;
        var antenna = row.AntennaId.HasValue ? GetAntennaById(row.AntennaId.Value) : null;

        return new AntennaConfiguration
        {
            Name = row.Name ?? "",
            PowerWatts = row.PowerWatts,
            Radio = new RadioConfig
            {
                Manufacturer = radio?.Manufacturer ?? "",
                Model = radio?.Model ?? ""
            },
            Linear = row.HasLinear == 1 ? new LinearConfig
            {
                Manufacturer = linear?.Manufacturer ?? "",
                Model = linear?.Model ?? ""
            } : null,
            Cable = new CableConfig
            {
                Type = cable?.Name ?? "",
                LengthMeters = row.CableLengthMeters,
                AdditionalLossDb = row.AdditionalLossDb,
                AdditionalLossDescription = row.AdditionalLossDescription ?? ""
            },
            Antenna = new AntennaPlacement
            {
                Manufacturer = antenna?.Manufacturer ?? "",
                Model = antenna?.Model ?? "",
                HeightMeters = row.HeightMeters,
                IsHorizontallyRotatable = row.IsHorizontallyRotatable == 1,
                HorizontalAngleDegrees = row.HorizontalAngleDegrees,
                IsVerticallyRotatable = row.IsVerticallyRotatable == 1
            },
            ModulationFactor = row.ModulationFactor,
            ActivityFactor = row.ActivityFactor,
            OkaName = row.OkaName ?? "",
            OkaDistanceMeters = row.OkaDistanceMeters,
            OkaBuildingDampingDb = row.OkaBuildingDampingDb
        };
    }

    #endregion

    #region Export/Import Database (Factory Mode)

    public void ExportDatabase(string folderPath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // Export antennas
        var antennas = GetAllAntennas();
        var antennasJson = JsonSerializer.Serialize(antennas, options);
        File.WriteAllText(Path.Combine(folderPath, "antennas.json"), antennasJson);

        // Export cables
        var cables = GetAllCables();
        var cablesJson = JsonSerializer.Serialize(cables, options);
        File.WriteAllText(Path.Combine(folderPath, "cables.json"), cablesJson);

        // Export radios
        var radios = GetAllRadios();
        var radiosJson = JsonSerializer.Serialize(radios, options);
        File.WriteAllText(Path.Combine(folderPath, "radios.json"), radiosJson);
    }

    public void ImportDatabase(string folderPath)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        // Clear existing data
        _connection.Execute("DELETE FROM Antennas");
        _connection.Execute("DELETE FROM Cables");
        _connection.Execute("DELETE FROM Radios");

        // Import antennas
        var antennasFile = Path.Combine(folderPath, "antennas.json");
        if (File.Exists(antennasFile))
        {
            var json = File.ReadAllText(antennasFile);
            var antennas = JsonSerializer.Deserialize<List<Antenna>>(json, options) ?? new();
            foreach (var antenna in antennas)
            {
                InsertAntenna(antenna, antenna.IsProjectSpecific);
            }
        }

        // Import cables
        var cablesFile = Path.Combine(folderPath, "cables.json");
        if (File.Exists(cablesFile))
        {
            var json = File.ReadAllText(cablesFile);
            var cables = JsonSerializer.Deserialize<List<Cable>>(json, options) ?? new();
            foreach (var cable in cables)
            {
                InsertCable(cable, cable.IsProjectSpecific);
            }
        }

        // Import radios
        var radiosFile = Path.Combine(folderPath, "radios.json");
        if (File.Exists(radiosFile))
        {
            var json = File.ReadAllText(radiosFile);
            var radios = JsonSerializer.Deserialize<List<Radio>>(json, options) ?? new();
            foreach (var radio in radios)
            {
                InsertRadio(radio, radio.IsProjectSpecific);
            }
        }
    }

    #endregion

    public void Dispose()
    {
        _connection?.Dispose();
    }

    #region Row Classes (for Dapper mapping)

    private class AntennaRow
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = "";
        public string Model { get; set; } = "";
        public string AntennaType { get; set; } = "other";
        public int IsHorizontallyPolarized { get; set; }
        public int IsRotatable { get; set; }
        public double HorizontalAngleDegrees { get; set; }
        public int IsProjectSpecific { get; set; }
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
        public int IsProjectSpecific { get; set; }
        public string AttenuationsJson { get; set; } = "{}";
    }

    private class RadioRow
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = "";
        public string Model { get; set; } = "";
        public double MaxPowerWatts { get; set; }
        public int IsProjectSpecific { get; set; }
    }

    private class ProjectRow
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Language { get; set; } = "de";
        public string? Callsign { get; set; }
        public string? OperatorName { get; set; }
        public string? Address { get; set; }
        public string? Location { get; set; }
        public string? FilePath { get; set; }
        public string CreatedAt { get; set; } = "";
        public string ModifiedAt { get; set; } = "";
    }

    private class OkaRow
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; } = "";
        public double DefaultDistanceMeters { get; set; }
        public double DefaultDampingDb { get; set; }
    }

    private class ConfigurationRow
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int ConfigNumber { get; set; }
        public string? Name { get; set; }
        public double PowerWatts { get; set; }
        public int? RadioId { get; set; }
        public int HasLinear { get; set; }
        public int? LinearId { get; set; }
        public int? CableId { get; set; }
        public double CableLengthMeters { get; set; }
        public double AdditionalLossDb { get; set; }
        public string? AdditionalLossDescription { get; set; }
        public int? AntennaId { get; set; }
        public double HeightMeters { get; set; }
        public int IsHorizontallyRotatable { get; set; }
        public double HorizontalAngleDegrees { get; set; }
        public int IsVerticallyRotatable { get; set; }
        public double ModulationFactor { get; set; }
        public double ActivityFactor { get; set; }
        public string? OkaName { get; set; }
        public double OkaDistanceMeters { get; set; }
        public double OkaBuildingDampingDb { get; set; }
    }

    #endregion
}
