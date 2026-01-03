using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Dapper;
using Microsoft.Data.Sqlite;
using NIS.Desktop.Models;
using NIS.Desktop.Services.Repositories;

namespace NIS.Desktop.Services;

/// <summary>
/// SQLite database service - facade over repositories.
/// Provides singleton access and handles schema initialization.
/// </summary>
public class DatabaseService : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly MasterDataRepository _masterData;
    private readonly ProjectRepository _projects;
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

        // Enable foreign key enforcement
        _connection.Execute("PRAGMA foreign_keys = ON");

        if (!IsSchemaCompatible())
        {
            // Show warning - user data will be lost
            var result = MsBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandard(
                    Localization.Strings.Instance.DatabaseResetRequired,
                    Localization.Strings.Instance.DatabaseResetMessage,
                    MsBox.Avalonia.Enums.ButtonEnum.YesNo,
                    MsBox.Avalonia.Enums.Icon.Warning)
                .ShowAsync()
                .GetAwaiter()
                .GetResult();

            if (result != MsBox.Avalonia.Enums.ButtonResult.Yes)
            {
                Environment.Exit(0);
            }

            ResetDatabaseSchema();
        }

        // Initialize repositories before InitializeDatabase (which needs them)
        _masterData = new MasterDataRepository(_connection);
        _projects = new ProjectRepository(_connection, _masterData);

        InitializeDatabase();
    }

    #region Schema Management

    private bool IsSchemaCompatible()
    {
        if (!TableExists("Antennas") || !HasColumn("Antennas", "IsUserData")) return false;
        if (!TableExists("Cables") || !HasColumn("Cables", "IsUserData")) return false;
        if (!TableExists("Radios") || !HasColumn("Radios", "IsUserData")) return false;
        if (!TableExists("Okas") || !HasColumn("Okas", "IsUserData")) return false;
        if (!TableExists("Modulations")) return false;
        if (!TableExists("Projects") || !HasColumn("Projects", "Callsign")) return false;
        if (!TableExists("Configurations")) return false;
        if (!IndexExists("IX_Configurations_ProjectId_ConfigNumber")) return false;
        return true;
    }

    private bool IndexExists(string indexName)
    {
        var result = _connection.ExecuteScalar<string>(
            "SELECT name FROM sqlite_master WHERE type='index' AND name=@Name",
            new { Name = indexName });
        return !string.IsNullOrEmpty(result);
    }

    private bool TableExists(string tableName)
    {
        var result = _connection.ExecuteScalar<string>(
            "SELECT name FROM sqlite_master WHERE type='table' AND name=@Name",
            new { Name = tableName });
        return !string.IsNullOrEmpty(result);
    }

    private bool HasColumn(string tableName, string columnName)
    {
        var columns = _connection.Query("PRAGMA table_info(" + tableName + ")")
            .Select(row => (string)row.name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        return columns.Contains(columnName);
    }

    private void ResetDatabaseSchema()
    {
        _connection.Execute("DROP TABLE IF EXISTS Configurations");
        _connection.Execute("DROP TABLE IF EXISTS Projects");
        _connection.Execute("DROP TABLE IF EXISTS Antennas");
        _connection.Execute("DROP TABLE IF EXISTS Cables");
        _connection.Execute("DROP TABLE IF EXISTS Radios");
        _connection.Execute("DROP TABLE IF EXISTS Okas");
        _connection.Execute("DROP TABLE IF EXISTS Modulations");
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
                IsUserData INTEGER NOT NULL DEFAULT 0,
                BandsJson TEXT NOT NULL DEFAULT '[]',
                UNIQUE(Manufacturer, Model)
            );

            CREATE TABLE IF NOT EXISTS Cables (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE,
                IsUserData INTEGER NOT NULL DEFAULT 0,
                AttenuationsJson TEXT NOT NULL DEFAULT '{}'
            );

            CREATE TABLE IF NOT EXISTS Radios (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Manufacturer TEXT NOT NULL,
                Model TEXT NOT NULL,
                MaxPowerWatts REAL NOT NULL DEFAULT 100 CHECK (MaxPowerWatts > 0),
                IsUserData INTEGER NOT NULL DEFAULT 0,
                UNIQUE(Manufacturer, Model)
            );

            CREATE TABLE IF NOT EXISTS Okas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE,
                DefaultDistanceMeters REAL NOT NULL DEFAULT 10 CHECK (DefaultDistanceMeters > 0),
                DefaultDampingDb REAL NOT NULL DEFAULT 0 CHECK (DefaultDampingDb >= 0),
                IsUserData INTEGER NOT NULL DEFAULT 1
            );

            CREATE TABLE IF NOT EXISTS Modulations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE,
                Factor REAL NOT NULL CHECK (Factor > 0 AND Factor <= 1),
                IsUserData INTEGER NOT NULL DEFAULT 0
            );

            CREATE TABLE IF NOT EXISTS Projects (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Operator TEXT,
                Callsign TEXT,
                Address TEXT,
                Location TEXT,
                CreatedAt TEXT NOT NULL,
                ModifiedAt TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Configurations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProjectId INTEGER NOT NULL,
                ConfigNumber INTEGER NOT NULL CHECK (ConfigNumber > 0),
                Name TEXT,
                PowerWatts REAL NOT NULL DEFAULT 100 CHECK (PowerWatts > 0),
                RadioId INTEGER,
                LinearName TEXT,
                LinearPowerWatts REAL NOT NULL DEFAULT 0 CHECK (LinearPowerWatts >= 0),
                CableId INTEGER,
                CableLengthMeters REAL NOT NULL DEFAULT 10 CHECK (CableLengthMeters >= 0),
                AdditionalLossDb REAL NOT NULL DEFAULT 0 CHECK (AdditionalLossDb >= 0),
                AdditionalLossDescription TEXT,
                AntennaId INTEGER,
                HeightMeters REAL NOT NULL DEFAULT 10 CHECK (HeightMeters > 0),
                IsRotatable INTEGER NOT NULL DEFAULT 0,
                HorizontalAngleDegrees REAL NOT NULL DEFAULT 360 CHECK (HorizontalAngleDegrees >= 0 AND HorizontalAngleDegrees <= 360),
                ModulationId INTEGER,
                ActivityFactor REAL NOT NULL DEFAULT 0.5 CHECK (ActivityFactor > 0 AND ActivityFactor <= 1),
                OkaId INTEGER,
                FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
                FOREIGN KEY (RadioId) REFERENCES Radios(Id) ON DELETE RESTRICT,
                FOREIGN KEY (CableId) REFERENCES Cables(Id) ON DELETE RESTRICT,
                FOREIGN KEY (AntennaId) REFERENCES Antennas(Id) ON DELETE RESTRICT,
                FOREIGN KEY (ModulationId) REFERENCES Modulations(Id) ON DELETE RESTRICT,
                FOREIGN KEY (OkaId) REFERENCES Okas(Id) ON DELETE RESTRICT
            );

            CREATE INDEX IF NOT EXISTS IX_Configurations_ProjectId ON Configurations(ProjectId);
            CREATE INDEX IF NOT EXISTS IX_Configurations_RadioId ON Configurations(RadioId);
            CREATE INDEX IF NOT EXISTS IX_Configurations_CableId ON Configurations(CableId);
            CREATE INDEX IF NOT EXISTS IX_Configurations_AntennaId ON Configurations(AntennaId);
            CREATE INDEX IF NOT EXISTS IX_Configurations_ModulationId ON Configurations(ModulationId);
            CREATE INDEX IF NOT EXISTS IX_Configurations_OkaId ON Configurations(OkaId);
            CREATE UNIQUE INDEX IF NOT EXISTS IX_Configurations_ProjectId_ConfigNumber
                ON Configurations(ProjectId, ConfigNumber);
        ");

        EnsureDefaultModulations();

        // Auto-import bundled factory data if database is empty
        if (GetAllAntennas().Count == 0)
        {
            ImportFactoryDataFromBundled();
        }
    }

    private void EnsureDefaultModulations()
    {
        var existing = GetAllModulations();
        if (existing.Count > 0) return;

        _masterData?.InsertModulation(new Modulation { Name = "SSB", Factor = 0.2, IsUserData = false });
        _masterData?.InsertModulation(new Modulation { Name = "CW", Factor = 0.4, IsUserData = false });
        _masterData?.InsertModulation(new Modulation { Name = "FM", Factor = 1.0, IsUserData = false });
    }

    #endregion

    #region Antenna Operations (delegates to MasterDataRepository)

    public List<Antenna> GetAllAntennas() => _masterData.GetAllAntennas();
    public Antenna? GetAntenna(string manufacturer, string model) => _masterData.GetAntenna(manufacturer, model);
    public Antenna? GetAntennaById(int id) => _masterData.GetAntennaById(id);
    public int? GetAntennaId(string manufacturer, string model) => _masterData.GetAntennaId(manufacturer, model);
    public bool AntennaExists(string manufacturer, string model) => _masterData.AntennaExists(manufacturer, model);
    public void SaveAntenna(Antenna antenna, bool isAdminMode = false) => _masterData.SaveAntenna(antenna, isAdminMode);
    public void DeleteAntenna(int id) => _masterData.DeleteAntenna(id);
    public List<EntityUsage> GetAntennaUsage(int antennaId) => _masterData.GetAntennaUsage(antennaId);

    #endregion

    #region Cable Operations (delegates to MasterDataRepository)

    public List<Cable> GetAllCables() => _masterData.GetAllCables();
    public Cable? GetCable(string name) => _masterData.GetCable(name);
    public Cable? GetCableById(int id) => _masterData.GetCableById(id);
    public int? GetCableId(string name) => _masterData.GetCableId(name);
    public bool CableExists(string name) => _masterData.CableExists(name);
    public void SaveCable(Cable cable, bool isAdminMode = false) => _masterData.SaveCable(cable, isAdminMode);
    public void DeleteCable(int id) => _masterData.DeleteCable(id);
    public List<EntityUsage> GetCableUsage(int cableId) => _masterData.GetCableUsage(cableId);

    #endregion

    #region Radio Operations (delegates to MasterDataRepository)

    public List<Radio> GetAllRadios() => _masterData.GetAllRadios();
    public Radio? GetRadio(string manufacturer, string model) => _masterData.GetRadio(manufacturer, model);
    public Radio? GetRadioById(int id) => _masterData.GetRadioById(id);
    public int? GetRadioId(string manufacturer, string model) => _masterData.GetRadioId(manufacturer, model);
    public bool RadioExists(string manufacturer, string model) => _masterData.RadioExists(manufacturer, model);
    public void SaveRadio(Radio radio, bool isAdminMode = false) => _masterData.SaveRadio(radio, isAdminMode);
    public void DeleteRadio(int id) => _masterData.DeleteRadio(id);
    public List<EntityUsage> GetRadioUsage(int radioId) => _masterData.GetRadioUsage(radioId);

    #endregion

    #region OKA Operations (delegates to MasterDataRepository)

    public List<Oka> GetAllOkas() => _masterData.GetAllOkas();
    public Oka? GetOka(string name) => _masterData.GetOka(name);
    public Oka? GetOkaById(int id) => _masterData.GetOkaById(id);
    public int? GetOkaId(string name) => _masterData.GetOkaId(name);
    public bool OkaExists(string name) => _masterData.OkaExists(name);
    public void SaveOka(Oka oka, bool isAdminMode = false) => _masterData.SaveOka(oka, isAdminMode);
    public void DeleteOka(int id) => _masterData.DeleteOka(id);
    public List<EntityUsage> GetOkaUsage(int okaId) => _masterData.GetOkaUsage(okaId);

    #endregion

    #region Modulation Operations (delegates to MasterDataRepository)

    public List<Modulation> GetAllModulations() => _masterData.GetAllModulations();
    public Modulation? GetModulationById(int id) => _masterData.GetModulationById(id);
    public Modulation? GetModulationByName(string name) => _masterData.GetModulationByName(name);
    public int? GetModulationId(string name) => _masterData.GetModulationId(name);
    public void SaveModulation(Modulation modulation, bool isAdminMode = false) => _masterData.SaveModulation(modulation, isAdminMode);
    public void DeleteModulation(int id) => _masterData.DeleteModulation(id);

    #endregion

    #region Project Operations (delegates to ProjectRepository)

    public int CreateProject(Project project) => _projects.CreateProject(project);
    public void UpdateProject(int projectId, Project project) => _projects.UpdateProject(projectId, project);
    public Project? GetProject(int projectId) => _projects.GetProject(projectId);
    public List<Project> GetAllProjects() => _projects.GetAllProjects();
    public void DeleteProject(int projectId) => _projects.DeleteProject(projectId);
    public void ClearAllProjects() => _projects.ClearAllProjects();
    public List<ProjectListItem> GetProjectList() => _projects.GetProjectList();
    public List<(int Id, string Name, DateTime ModifiedAt)> GetRecentProjects(int limit = 5) => _projects.GetRecentProjects(limit);
    public List<string> ValidateConfigurationIntegrity() => _projects.ValidateConfigurationIntegrity();

    #endregion

    #region Data Management

    public void ClearAllData()
    {
        _connection.Execute("DELETE FROM Configurations");
        _connection.Execute("DELETE FROM Projects");
        _connection.Execute("DELETE FROM Antennas");
        _connection.Execute("DELETE FROM Cables");
        _connection.Execute("DELETE FROM Radios");
        _connection.Execute("DELETE FROM Okas");
        _connection.Execute("DELETE FROM Modulations");
    }

    #endregion

    #region Export/Import Factory Data

    public void ExportFactoryData(string filePath)
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
            UserAntennas = GetAllAntennas(),
            UserCables = GetAllCables(),
            UserRadios = GetAllRadios()
        };

        var json = JsonSerializer.Serialize(export, options);
        File.WriteAllText(filePath, json);
    }

    public void ImportFactoryData(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        var json = File.ReadAllText(filePath);
        var import = JsonSerializer.Deserialize<UserDataExport>(json, options);
        if (import == null) return;

        using var transaction = _connection.BeginTransaction();
        try
        {
            ClearAllData();
            EnsureDefaultModulations();
            ImportUserDataInternal(import, forceIsUserData: false);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private void ImportFactoryDataFromBundled()
    {
        var folderPath = AppPaths.DataFolder;
        if (!Directory.Exists(folderPath)) return;

        ImportFactoryDataFromFolder(folderPath);
    }

    private void ImportFactoryDataFromFolder(string folderPath)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        // Import antennas (consolidate multiband entries with same manufacturer/model)
        var antennasFile = Path.Combine(folderPath, "antennas.json");
        if (File.Exists(antennasFile))
        {
            var json = File.ReadAllText(antennasFile);
            var wrapper = JsonSerializer.Deserialize<AntennaFileWrapper>(json, options);
            var rawAntennas = wrapper?.Antennas ?? new();

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
                        IsHorizontallyPolarized = first.IsHorizontallyPolarized,
                        IsUserData = false
                    };
                    foreach (var ant in g)
                    {
                        merged.Bands.AddRange(ant.Bands);
                    }
                    return merged;
                });

            foreach (var antenna in consolidated)
            {
                _masterData.InsertAntenna(antenna, isUserData: false);
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
                cable.IsUserData = false;
                _masterData.InsertCable(cable, isUserData: false);
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
                radio.IsUserData = false;
                _masterData.InsertRadio(radio, isUserData: false);
            }
        }
    }

    private class AntennaFileWrapper { public List<Antenna>? Antennas { get; set; } }
    private class CableFileWrapper { public List<Cable>? Cables { get; set; } }
    private class RadioFileWrapper { public List<Radio>? Radios { get; set; } }

    #endregion

    #region Export/Import User Data

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
            UserAntennas = GetAllAntennas().Where(a => a.IsUserData).ToList(),
            UserCables = GetAllCables().Where(c => c.IsUserData).ToList(),
            UserRadios = GetAllRadios().Where(r => r.IsUserData).ToList()
        };

        var json = JsonSerializer.Serialize(export, options);
        File.WriteAllText(filePath, json);
    }

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

        using var transaction = _connection.BeginTransaction();
        try
        {
            ClearAllData();
            EnsureDefaultModulations();
            ImportUserDataInternal(import, forceIsUserData: true);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private void ImportUserDataInternal(UserDataExport import, bool forceIsUserData)
    {
        // Import OKAs first (projects may reference them)
        foreach (var oka in import.Okas ?? new List<Oka>())
        {
            oka.IsUserData = forceIsUserData;
            if (!OkaExists(oka.Name))
            {
                SaveOka(oka, isAdminMode: !forceIsUserData);
            }
        }

        // Import master data
        foreach (var antenna in import.UserAntennas ?? new List<Antenna>())
        {
            antenna.IsUserData = forceIsUserData;
            if (!AntennaExists(antenna.Manufacturer, antenna.Model))
            {
                _masterData.InsertAntenna(antenna, isUserData: forceIsUserData);
            }
        }

        foreach (var cable in import.UserCables ?? new List<Cable>())
        {
            cable.IsUserData = forceIsUserData;
            if (!CableExists(cable.Name))
            {
                _masterData.InsertCable(cable, isUserData: forceIsUserData);
            }
        }

        foreach (var radio in import.UserRadios ?? new List<Radio>())
        {
            radio.IsUserData = forceIsUserData;
            if (!RadioExists(radio.Manufacturer, radio.Model))
            {
                _masterData.InsertRadio(radio, isUserData: forceIsUserData);
            }
        }

        // Import projects
        foreach (var project in import.Projects ?? new List<Project>())
        {
            CreateProject(project);
        }
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
}

/// <summary>
/// Represents usage of a master data entity in a project configuration.
/// </summary>
public class EntityUsage
{
    public string ProjectName { get; set; } = "";
    public string? ConfigurationName { get; set; }
    public int ConfigNumber { get; set; }

    public string DisplayName => string.IsNullOrEmpty(ConfigurationName)
        ? $"{ProjectName} - Config {ConfigNumber}"
        : $"{ProjectName} - {ConfigurationName}";
}
