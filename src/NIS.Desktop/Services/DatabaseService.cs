using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Dapper;
using Microsoft.Data.Sqlite;
using NIS.Desktop.Models;

namespace NIS.Desktop.Services;

/// <summary>
/// Project list item for display in project selection UI.
/// </summary>
public class ProjectListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? Operator { get; set; }
    public string? Address { get; set; }
    public string? Location { get; set; }
    public int ConfigCount { get; set; }
    public string ModifiedAt { get; set; } = "";

    public string DisplayName => string.IsNullOrEmpty(Operator) ? Name : $"{Operator} - {Name}";
}

/// <summary>
/// SQLite database service - single source of truth for all master data.
/// </summary>
public class DatabaseService : IDisposable
{
    /// <summary>
    /// IDs below this threshold are reserved for factory (shipped) master data.
    /// Project-specific items use IDs >= this value to prevent overwrites during factory imports.
    /// </summary>
    public const int ProjectSpecificIdStart = 1_000_000;

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

            CREATE TABLE IF NOT EXISTS Okas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE,
                DefaultDistanceMeters REAL NOT NULL DEFAULT 10,
                DefaultDampingDb REAL NOT NULL DEFAULT 0
            );

            CREATE TABLE IF NOT EXISTS Projects (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                OperatorName TEXT,
                Address TEXT,
                Location TEXT,
                CreatedAt TEXT NOT NULL,
                ModifiedAt TEXT NOT NULL
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
                OkaId INTEGER,
                OkaDistanceMeters REAL NOT NULL DEFAULT 10,
                OkaBuildingDampingDb REAL NOT NULL DEFAULT 0,
                FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
                FOREIGN KEY (RadioId) REFERENCES Radios(Id) ON DELETE SET NULL,
                FOREIGN KEY (LinearId) REFERENCES Radios(Id) ON DELETE SET NULL,
                FOREIGN KEY (CableId) REFERENCES Cables(Id) ON DELETE SET NULL,
                FOREIGN KEY (AntennaId) REFERENCES Antennas(Id) ON DELETE SET NULL,
                FOREIGN KEY (OkaId) REFERENCES Okas(Id) ON DELETE SET NULL
            );

            CREATE TABLE IF NOT EXISTS SchemaVersion (
                Version INTEGER PRIMARY KEY
            );
        ");

        // Migrate old databases that don't have OkaId column
        EnsureConfigurationsSchema();

        // Auto-import factory data if database is empty
        if (GetAllAntennas().Count == 0)
        {
            ImportDatabase(AppPaths.DataFolder);
        }
    }

    private void EnsureConfigurationsSchema()
    {
        var columns = _connection.Query("PRAGMA table_info(Configurations)")
            .Select(row => (string)row.name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (columns.Count == 0) return; // Table doesn't exist yet

        if (!columns.Contains("OkaId"))
            _connection.Execute("ALTER TABLE Configurations ADD COLUMN OkaId INTEGER");
        if (!columns.Contains("OkaDistanceMeters"))
            _connection.Execute("ALTER TABLE Configurations ADD COLUMN OkaDistanceMeters REAL NOT NULL DEFAULT 10");
        if (!columns.Contains("OkaBuildingDampingDb"))
            _connection.Execute("ALTER TABLE Configurations ADD COLUMN OkaBuildingDampingDb REAL NOT NULL DEFAULT 0");
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

        // Project-specific items get IDs >= ProjectSpecificIdStart to avoid factory import conflicts
        int? explicitId = isProjectSpecific ? GetNextProjectAntennaId() : null;

        var sql = isProjectSpecific
            ? @"INSERT INTO Antennas (Id, Manufacturer, Model, AntennaType, IsHorizontallyPolarized, IsRotatable, HorizontalAngleDegrees, IsProjectSpecific, BandsJson)
               VALUES (@Id, @Manufacturer, @Model, @AntennaType, @IsHorizontallyPolarized, @IsRotatable, @HorizontalAngleDegrees, @IsProjectSpecific, @BandsJson)"
            : @"INSERT INTO Antennas (Manufacturer, Model, AntennaType, IsHorizontallyPolarized, IsRotatable, HorizontalAngleDegrees, IsProjectSpecific, BandsJson)
               VALUES (@Manufacturer, @Model, @AntennaType, @IsHorizontallyPolarized, @IsRotatable, @HorizontalAngleDegrees, @IsProjectSpecific, @BandsJson)";

        _connection.Execute(sql,
            new
            {
                Id = explicitId,
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

    /// <summary>
    /// Gets the next available ID for project-specific antennas (>= ProjectSpecificIdStart).
    /// </summary>
    private int GetNextProjectAntennaId()
    {
        var maxId = _connection.ExecuteScalar<int?>(
            "SELECT MAX(Id) FROM Antennas WHERE Id >= @Threshold",
            new { Threshold = ProjectSpecificIdStart });
        return maxId.HasValue ? maxId.Value + 1 : ProjectSpecificIdStart;
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

        // Project-specific items get IDs >= ProjectSpecificIdStart to avoid factory import conflicts
        int? explicitId = isProjectSpecific ? GetNextProjectCableId() : null;

        var sql = isProjectSpecific
            ? @"INSERT INTO Cables (Id, Name, IsProjectSpecific, AttenuationsJson)
               VALUES (@Id, @Name, @IsProjectSpecific, @AttenuationsJson)"
            : @"INSERT INTO Cables (Name, IsProjectSpecific, AttenuationsJson)
               VALUES (@Name, @IsProjectSpecific, @AttenuationsJson)";

        _connection.Execute(sql,
            new
            {
                Id = explicitId,
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

    /// <summary>
    /// Gets the next available ID for project-specific cables (>= ProjectSpecificIdStart).
    /// </summary>
    private int GetNextProjectCableId()
    {
        var maxId = _connection.ExecuteScalar<int?>(
            "SELECT MAX(Id) FROM Cables WHERE Id >= @Threshold",
            new { Threshold = ProjectSpecificIdStart });
        return maxId.HasValue ? maxId.Value + 1 : ProjectSpecificIdStart;
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
        // Project-specific items get IDs >= ProjectSpecificIdStart to avoid factory import conflicts
        int? explicitId = isProjectSpecific ? GetNextProjectRadioId() : null;

        var sql = isProjectSpecific
            ? @"INSERT INTO Radios (Id, Manufacturer, Model, MaxPowerWatts, IsProjectSpecific)
               VALUES (@Id, @Manufacturer, @Model, @MaxPowerWatts, @IsProjectSpecific)"
            : @"INSERT INTO Radios (Manufacturer, Model, MaxPowerWatts, IsProjectSpecific)
               VALUES (@Manufacturer, @Model, @MaxPowerWatts, @IsProjectSpecific)";

        _connection.Execute(sql,
            new
            {
                Id = explicitId,
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

    /// <summary>
    /// Gets the next available ID for project-specific radios (>= ProjectSpecificIdStart).
    /// </summary>
    private int GetNextProjectRadioId()
    {
        var maxId = _connection.ExecuteScalar<int?>(
            "SELECT MAX(Id) FROM Radios WHERE Id >= @Threshold",
            new { Threshold = ProjectSpecificIdStart });
        return maxId.HasValue ? maxId.Value + 1 : ProjectSpecificIdStart;
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

    #region OKA Operations

    public List<Oka> GetAllOkas()
    {
        return _connection.Query<Oka>(
            "SELECT Id, Name, DefaultDistanceMeters, DefaultDampingDb FROM Okas ORDER BY Name").ToList();
    }

    public Oka? GetOka(string name)
    {
        return _connection.QueryFirstOrDefault<Oka>(
            "SELECT Id, Name, DefaultDistanceMeters, DefaultDampingDb FROM Okas WHERE Name = @Name",
            new { Name = name });
    }

    public Oka? GetOkaById(int id)
    {
        return _connection.QueryFirstOrDefault<Oka>(
            "SELECT Id, Name, DefaultDistanceMeters, DefaultDampingDb FROM Okas WHERE Id = @Id",
            new { Id = id });
    }

    public int? GetOkaId(string name)
    {
        return _connection.QueryFirstOrDefault<int?>(
            "SELECT Id FROM Okas WHERE Name = @Name", new { Name = name });
    }

    public void SaveOka(Oka oka)
    {
        var existing = GetOka(oka.Name);
        if (existing != null)
        {
            _connection.Execute(@"
                UPDATE Okas SET
                    DefaultDistanceMeters = @DefaultDistanceMeters,
                    DefaultDampingDb = @DefaultDampingDb
                WHERE Name = @Name",
                new { oka.Name, oka.DefaultDistanceMeters, oka.DefaultDampingDb });
        }
        else
        {
            _connection.Execute(@"
                INSERT INTO Okas (Name, DefaultDistanceMeters, DefaultDampingDb)
                VALUES (@Name, @DefaultDistanceMeters, @DefaultDampingDb)",
                new { oka.Name, oka.DefaultDistanceMeters, oka.DefaultDampingDb });
        }
    }

    public void DeleteOka(string name)
    {
        _connection.Execute("DELETE FROM Okas WHERE Name = @Name", new { Name = name });
    }

    public bool OkaExists(string name)
    {
        return _connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM Okas WHERE Name = @Name", new { Name = name }) > 0;
    }

    #endregion

    #region Project Operations

    public int CreateProject(Project project)
    {
        var now = DateTime.UtcNow.ToString("o");
        var id = _connection.ExecuteScalar<int>(@"
            INSERT INTO Projects (Name, OperatorName, Address, Location, CreatedAt, ModifiedAt)
            VALUES (@Name, @OperatorName, @Address, @Location, @CreatedAt, @ModifiedAt);
            SELECT last_insert_rowid();",
            new
            {
                Name = string.IsNullOrWhiteSpace(project.Name) ? "New Project" : project.Name,
                OperatorName = project.Operator,
                project.Address,
                project.Location,
                CreatedAt = now,
                ModifiedAt = now
            });

        // Save/update OKAs from the project
        foreach (var oka in project.Okas)
        {
            if (!OkaExists(oka.Name))
            {
                SaveOka(oka);
            }
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
        using var transaction = _connection.BeginTransaction();
        try
        {
            var now = DateTime.UtcNow.ToString("o");
            _connection.Execute(@"
                UPDATE Projects SET
                    Name = @Name,
                    OperatorName = @OperatorName,
                    Address = @Address,
                    Location = @Location,
                    ModifiedAt = @ModifiedAt
                WHERE Id = @Id",
                new
                {
                    Id = projectId,
                    Name = string.IsNullOrWhiteSpace(project.Name) ? "Project" : project.Name,
                    OperatorName = project.Operator,
                    project.Address,
                    project.Location,
                    ModifiedAt = now
                },
                transaction);

            // Save/update OKAs from the project
            foreach (var oka in project.Okas)
            {
                if (!OkaExists(oka.Name))
                {
                    SaveOka(oka);
                }
            }

            // Delete and recreate configurations
            _connection.Execute("DELETE FROM Configurations WHERE ProjectId = @ProjectId",
                new { ProjectId = projectId }, transaction);

            int configNum = 1;
            foreach (var config in project.AntennaConfigurations)
            {
                SaveConfiguration(projectId, config, configNum++, transaction);
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    public Project? GetProject(int projectId)
    {
        var row = _connection.QueryFirstOrDefault<ProjectRow>(
            "SELECT * FROM Projects WHERE Id = @Id", new { Id = projectId });
        if (row == null) return null;

        var project = new Project
        {
            Name = row.Name,
            Operator = row.OperatorName ?? "",
            Address = row.Address ?? "",
            Location = row.Location ?? ""
        };

        // Load configurations
        var configRows = _connection.Query<ConfigurationRow>(
            "SELECT * FROM Configurations WHERE ProjectId = @ProjectId ORDER BY ConfigNumber", new { ProjectId = projectId });
        project.AntennaConfigurations = configRows.Select(ToConfiguration).ToList();

        // Load OKAs referenced by this project configurations
        var okaIds = configRows
            .Where(c => c.OkaId.HasValue)
            .Select(c => c.OkaId!.Value)
            .Distinct()
            .ToList();

        if (okaIds.Count > 0)
        {
            project.Okas = _connection.Query<Oka>(
                "SELECT Id, Name, DefaultDistanceMeters, DefaultDampingDb FROM Okas WHERE Id IN @Ids",
                new { Ids = okaIds }).ToList();
        }

        return project;
    }
    public void DeleteProject(int projectId)
    {
        // Configurations are deleted automatically due to ON DELETE CASCADE
        _connection.Execute("DELETE FROM Projects WHERE Id = @Id", new { Id = projectId });
    }

    /// <summary>
    /// Deletes all projects from the database.
    /// </summary>
    public void ClearAllProjects()
    {
        _connection.Execute("DELETE FROM Configurations");
        _connection.Execute("DELETE FROM Projects");
    }

    /// <summary>
    /// Imports the demo project from the bundled sample file.
    /// Returns the project ID if successful, 0 if demo not found.
    /// </summary>
    public int ImportDemoProject()
    {
        var demoPath = System.IO.Path.Combine(AppPaths.DataFolder, "samples", "demo.nisproj");
        if (!System.IO.File.Exists(demoPath)) return 0;

        try
        {
            var json = System.IO.File.ReadAllText(demoPath);
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
            var project = System.Text.Json.JsonSerializer.Deserialize<Models.Project>(json, options);
            if (project == null) return 0;

            return CreateProject(project);
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Gets a list of all projects with basic info (for project list display).
    /// </summary>
    public List<ProjectListItem> GetProjectList()
    {
        return _connection.Query<ProjectListItem>(@"
            SELECT p.Id, p.Name, p.OperatorName as Operator, p.Address, p.Location,
                   strftime('%d.%m.%Y %H:%M', p.ModifiedAt) as ModifiedAt,
                   (SELECT COUNT(*) FROM Configurations WHERE ProjectId = p.Id) as ConfigCount
            FROM Projects p
            ORDER BY p.ModifiedAt DESC").ToList();
    }

    public List<(int Id, string Name, DateTime ModifiedAt)> GetRecentProjects(int limit = 5)
    {
        return _connection.Query<(int Id, string Name, string ModifiedAt)>(
            "SELECT Id, Name, ModifiedAt FROM Projects ORDER BY ModifiedAt DESC LIMIT @Limit",
            new { Limit = limit })
            .Select(r => (r.Id, r.Name, DateTime.Parse(r.ModifiedAt)))
            .ToList();
    }

    #endregion

    #region Configuration Operations

    private void SaveConfiguration(int projectId, AntennaConfiguration config, int configNumber, System.Data.IDbTransaction? transaction = null)
    {
        // Look up IDs from names
        var radioId = GetRadioId(config.Radio.Manufacturer, config.Radio.Model);
        var linearId = config.Linear != null ? GetRadioId(config.Linear.Manufacturer, config.Linear.Model) : null;
        var cableId = GetCableId(config.Cable.Type);
        var antennaId = GetAntennaId(config.Antenna.Manufacturer, config.Antenna.Model);
        var okaId = EnsureOkaForConfig(config);

        _connection.Execute(@"
            INSERT INTO Configurations (
                ProjectId, ConfigNumber, Name, PowerWatts,
                RadioId, HasLinear, LinearId,
                CableId, CableLengthMeters, AdditionalLossDb, AdditionalLossDescription,
                AntennaId, HeightMeters,
                IsHorizontallyRotatable, HorizontalAngleDegrees, IsVerticallyRotatable,
                ModulationFactor, ActivityFactor,
                OkaId, OkaDistanceMeters, OkaBuildingDampingDb
            ) VALUES (
                @ProjectId, @ConfigNumber, @Name, @PowerWatts,
                @RadioId, @HasLinear, @LinearId,
                @CableId, @CableLengthMeters, @AdditionalLossDb, @AdditionalLossDescription,
                @AntennaId, @HeightMeters,
                @IsHorizontallyRotatable, @HorizontalAngleDegrees, @IsVerticallyRotatable,
                @ModulationFactor, @ActivityFactor,
                @OkaId, @OkaDistanceMeters, @OkaBuildingDampingDb
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
                CableLengthMeters = config.Cable.LengthMeters,
                AdditionalLossDb = config.Cable.AdditionalLossDb,
                AdditionalLossDescription = config.Cable.AdditionalLossDescription,
                AntennaId = antennaId,
                HeightMeters = config.Antenna.HeightMeters,
                IsHorizontallyRotatable = config.Antenna.IsHorizontallyRotatable ? 1 : 0,
                HorizontalAngleDegrees = config.Antenna.HorizontalAngleDegrees,
                IsVerticallyRotatable = config.Antenna.IsVerticallyRotatable ? 1 : 0,
                config.ModulationFactor,
                config.ActivityFactor,
                OkaId = okaId,
                config.OkaDistanceMeters,
                config.OkaBuildingDampingDb,
            },
            transaction);
    }

    private int? EnsureOkaForConfig(AntennaConfiguration config)
    {
        if (string.IsNullOrWhiteSpace(config.OkaName))
        {
            return null;
        }

        var okaId = GetOkaId(config.OkaName);
        if (okaId.HasValue)
        {
            return okaId;
        }

        SaveOka(new Oka
        {
            Name = config.OkaName,
            DefaultDistanceMeters = config.OkaDistanceMeters,
            DefaultDampingDb = config.OkaBuildingDampingDb
        });

        return GetOkaId(config.OkaName);
    }

    private AntennaConfiguration ToConfiguration(ConfigurationRow row)
    {
        // Look up entities by ID
        var radio = row.RadioId.HasValue ? GetRadioById(row.RadioId.Value) : null;
        var linear = row.HasLinear == 1 && row.LinearId.HasValue ? GetRadioById(row.LinearId.Value) : null;
        var cable = row.CableId.HasValue ? GetCableById(row.CableId.Value) : null;
        var antenna = row.AntennaId.HasValue ? GetAntennaById(row.AntennaId.Value) : null;
        var oka = row.OkaId.HasValue ? GetOkaById(row.OkaId.Value) : null;

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
            OkaName = oka?.Name ?? "",
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

        // Only delete factory data (ID < ProjectSpecificIdStart), preserve project-specific items
        _connection.Execute("DELETE FROM Antennas WHERE Id < @Threshold", new { Threshold = ProjectSpecificIdStart });
        _connection.Execute("DELETE FROM Cables WHERE Id < @Threshold", new { Threshold = ProjectSpecificIdStart });
        _connection.Execute("DELETE FROM Radios WHERE Id < @Threshold", new { Threshold = ProjectSpecificIdStart });

        // Import antennas (consolidate multiband entries with same manufacturer/model)
        var antennasFile = Path.Combine(folderPath, "antennas.json");
        if (File.Exists(antennasFile))
        {
            var json = File.ReadAllText(antennasFile);
            var wrapper = JsonSerializer.Deserialize<AntennaFileWrapper>(json, options);
            var rawAntennas = wrapper?.Antennas ?? new();

            // Group by manufacturer/model and merge bands
            var consolidated = rawAntennas
                .GroupBy(a => (a.Manufacturer, a.Model))
                .Select(g =>
                {
                    var first = g.First();
                    var merged = new Antenna
                    {
                        Manufacturer = first.Manufacturer,
                        Model = first.Model,
                        AntennaType = first.AntennaType,
                        IsRotatable = first.IsRotatable,
                        IsHorizontallyPolarized = first.IsHorizontallyPolarized,
                        HorizontalAngleDegrees = first.HorizontalAngleDegrees,
                        IsProjectSpecific = first.IsProjectSpecific
                    };
                    // Merge all bands from all entries
                    foreach (var ant in g)
                    {
                        merged.Bands.AddRange(ant.Bands);
                    }
                    return merged;
                });

            foreach (var antenna in consolidated)
            {
                InsertAntenna(antenna, antenna.IsProjectSpecific);
            }
        }

        // Import cables
        var cablesFile = Path.Combine(folderPath, "cables.json");
        if (File.Exists(cablesFile))
        {
            var json = File.ReadAllText(cablesFile);
            var wrapper = JsonSerializer.Deserialize<CableFileWrapper>(json, options);
            var cables = wrapper?.Cables ?? new();
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
            var wrapper = JsonSerializer.Deserialize<RadioFileWrapper>(json, options);
            var radios = wrapper?.Radios ?? new();
            foreach (var radio in radios)
            {
                InsertRadio(radio, radio.IsProjectSpecific);
            }
        }
    }

    private class AntennaFileWrapper { public List<Antenna>? Antennas { get; set; } }
    private class CableFileWrapper { public List<Cable>? Cables { get; set; } }
    private class RadioFileWrapper { public List<Radio>? Radios { get; set; } }

    #endregion

    #region Export/Import User Data

    /// <summary>
    /// Export all user data (projects, OKAs, user-specific master data) to a JSON file.
    /// </summary>
    public void ExportUserData(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var export = new UserDataExport
        {
            ExportDate = DateTime.UtcNow.ToString("o"),
            Projects = GetAllProjects(),
            Okas = GetAllOkas(),
            UserAntennas = GetAllAntennas().Where(a => a.IsProjectSpecific).ToList(),
            UserCables = GetAllCables().Where(c => c.IsProjectSpecific).ToList(),
            UserRadios = GetAllRadios().Where(r => r.IsProjectSpecific).ToList()
        };

        var json = JsonSerializer.Serialize(export, options);
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Import user data from a JSON file.
    /// </summary>
    public void ImportUserData(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        var json = File.ReadAllText(filePath);
        var import = JsonSerializer.Deserialize<UserDataExport>(json, options);
        if (import == null) return;

        // Import OKAs first (projects may reference them)
        foreach (var oka in import.Okas ?? new List<Oka>())
        {
            if (!OkaExists(oka.Name))
            {
                SaveOka(oka);
            }
        }

        // Import user master data
        foreach (var antenna in import.UserAntennas ?? new List<Antenna>())
        {
            if (!AntennaExists(antenna.Manufacturer, antenna.Model))
            {
                InsertAntenna(antenna, isProjectSpecific: true);
            }
        }

        foreach (var cable in import.UserCables ?? new List<Cable>())
        {
            if (!CableExists(cable.Name))
            {
                InsertCable(cable, isProjectSpecific: true);
            }
        }

        foreach (var radio in import.UserRadios ?? new List<Radio>())
        {
            if (!RadioExists(radio.Manufacturer, radio.Model))
            {
                InsertRadio(radio, isProjectSpecific: true);
            }
        }

        // Import projects
        foreach (var project in import.Projects ?? new List<Project>())
        {
            CreateProject(project);
        }
    }

    /// <summary>
    /// Get all projects from the database.
    /// </summary>
    public List<Project> GetAllProjects()
    {
        var projectRows = _connection.Query<ProjectRow>(
            "SELECT * FROM Projects ORDER BY ModifiedAt DESC").ToList();

        return projectRows.Select(row =>
        {
            var project = new Project
            {
                Name = row.Name,
                Operator = row.OperatorName ?? "",
                Address = row.Address ?? "",
                Location = row.Location ?? ""
            };

            // Load configurations
            var configRows = _connection.Query<ConfigurationRow>(
                "SELECT * FROM Configurations WHERE ProjectId = @ProjectId ORDER BY ConfigNumber",
                new { ProjectId = row.Id });
            project.AntennaConfigurations = configRows.Select(ToConfiguration).ToList();

            return project;
        }).ToList();
    }

    private class UserDataExport
    {
        public string? ExportDate { get; set; }
        public List<Project>? Projects { get; set; }
        public List<Oka>? Okas { get; set; }
        public List<Antenna>? UserAntennas { get; set; }
        public List<Cable>? UserCables { get; set; }
        public List<Radio>? UserRadios { get; set; }
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
        public string? OperatorName { get; set; }
        public string? Address { get; set; }
        public string? Location { get; set; }
        public string CreatedAt { get; set; } = "";
        public string ModifiedAt { get; set; } = "";
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
        public int? OkaId { get; set; }
        public double OkaDistanceMeters { get; set; }
        public double OkaBuildingDampingDb { get; set; }
    }

    #endregion
}
