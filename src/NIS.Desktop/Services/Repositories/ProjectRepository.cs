using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using NIS.Desktop.Models;

namespace NIS.Desktop.Services.Repositories;

/// <summary>
/// Repository for project and configuration operations.
/// </summary>
public class ProjectRepository
{
    private readonly IDbConnection _connection;
    private readonly MasterDataRepository _masterData;

    public ProjectRepository(IDbConnection connection, MasterDataRepository masterData)
    {
        _connection = connection;
        _masterData = masterData;
    }

    #region Project Operations

    public int CreateProject(Project project)
    {
        using var transaction = _connection.BeginTransaction();
        try
        {
            var now = DateTime.UtcNow.ToString("o");
            var id = _connection.ExecuteScalar<int>(@"
                INSERT INTO Projects (Name, OperatorName, Callsign, Address, Location, CreatedAt, ModifiedAt)
                VALUES (@Name, @OperatorName, @Callsign, @Address, @Location, @CreatedAt, @ModifiedAt);
                SELECT last_insert_rowid();",
                new
                {
                    Name = string.IsNullOrWhiteSpace(project.Name) ? "New Project" : project.Name,
                    OperatorName = project.Operator,
                    project.Callsign,
                    project.Address,
                    project.Location,
                    CreatedAt = now,
                    ModifiedAt = now
                },
                transaction);

            int configNum = 1;
            foreach (var config in project.AntennaConfigurations)
            {
                SaveConfiguration(id, config, configNum++, transaction);
            }

            transaction.Commit();
            return id;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
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
                    Callsign = @Callsign,
                    Address = @Address,
                    Location = @Location,
                    ModifiedAt = @ModifiedAt
                WHERE Id = @Id",
                new
                {
                    Id = projectId,
                    Name = string.IsNullOrWhiteSpace(project.Name) ? "Project" : project.Name,
                    OperatorName = project.Operator,
                    project.Callsign,
                    project.Address,
                    project.Location,
                    ModifiedAt = now
                },
                transaction);

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
            Callsign = row.Callsign ?? "",
            Address = row.Address ?? "",
            Location = row.Location ?? ""
        };

        var configRows = _connection.Query<ConfigurationRow>(
            "SELECT * FROM Configurations WHERE ProjectId = @ProjectId ORDER BY ConfigNumber", new { ProjectId = projectId });
        project.AntennaConfigurations = configRows.Select(ToConfiguration).ToList();

        return project;
    }

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
                Callsign = row.Callsign ?? "",
                Address = row.Address ?? "",
                Location = row.Location ?? ""
            };

            var configRows = _connection.Query<ConfigurationRow>(
                "SELECT * FROM Configurations WHERE ProjectId = @ProjectId ORDER BY ConfigNumber",
                new { ProjectId = row.Id });
            project.AntennaConfigurations = configRows.Select(ToConfiguration).ToList();

            return project;
        }).ToList();
    }

    public void DeleteProject(int projectId)
    {
        // Configurations are deleted automatically due to ON DELETE CASCADE
        _connection.Execute("DELETE FROM Projects WHERE Id = @Id", new { Id = projectId });
    }

    public void ClearAllProjects()
    {
        _connection.Execute("DELETE FROM Configurations");
        _connection.Execute("DELETE FROM Projects");
    }

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

    /// <summary>
    /// Validates FK integrity for all configurations and returns a list of issues.
    /// </summary>
    public List<string> ValidateConfigurationIntegrity()
    {
        var issues = new List<string>();

        var configs = _connection.Query<ConfigurationRow>(
            @"SELECT c.*, p.Name as ProjectName FROM Configurations c
              JOIN Projects p ON c.ProjectId = p.Id");

        foreach (var config in configs)
        {
            var configName = !string.IsNullOrEmpty(config.Name) ? config.Name : $"Config {config.ConfigNumber}";
            var projectName = config.ProjectName ?? $"Project {config.ProjectId}";
            var prefix = $"{projectName} / {configName}";

            if (config.RadioId.HasValue && _masterData.GetRadioById(config.RadioId.Value) == null)
                issues.Add($"{prefix}: Radio ID {config.RadioId} not found");

            if (config.CableId.HasValue && _masterData.GetCableById(config.CableId.Value) == null)
                issues.Add($"{prefix}: Cable ID {config.CableId} not found");

            if (config.AntennaId.HasValue && _masterData.GetAntennaById(config.AntennaId.Value) == null)
                issues.Add($"{prefix}: Antenna ID {config.AntennaId} not found");

            if (config.ModulationId.HasValue && _masterData.GetModulationById(config.ModulationId.Value) == null)
                issues.Add($"{prefix}: Modulation ID {config.ModulationId} not found");

            if (config.OkaId.HasValue && _masterData.GetOkaById(config.OkaId.Value) == null)
                issues.Add($"{prefix}: OKA ID {config.OkaId} not found");
        }

        return issues;
    }

    #endregion

    #region Configuration Operations

    private void SaveConfiguration(int projectId, AntennaConfiguration config, int configNumber, IDbTransaction? transaction = null)
    {
        var radioId = config.RadioId;
        var cableId = config.CableId;
        var antennaId = config.AntennaId;
        var modulationId = config.ModulationId;
        var okaId = config.OkaId;

        // Get OKA distance/damping from master data
        var oka = okaId.HasValue ? _masterData.GetOkaById(okaId.Value) : null;
        var okaDistance = oka?.DefaultDistanceMeters ?? 10;
        var okaDamping = oka?.DefaultDampingDb ?? 0;

        _connection.Execute(@"
            INSERT INTO Configurations (
                ProjectId, ConfigNumber, Name, PowerWatts,
                RadioId, LinearName, LinearPowerWatts,
                CableId, CableLengthMeters, AdditionalLossDb, AdditionalLossDescription,
                AntennaId, HeightMeters, IsRotatable, HorizontalAngleDegrees,
                ModulationId, ActivityFactor,
                OkaId, OkaDistanceMeters, OkaBuildingDampingDb
            ) VALUES (
                @ProjectId, @ConfigNumber, @Name, @PowerWatts,
                @RadioId, @LinearName, @LinearPowerWatts,
                @CableId, @CableLengthMeters, @AdditionalLossDb, @AdditionalLossDescription,
                @AntennaId, @HeightMeters, @IsRotatable, @HorizontalAngleDegrees,
                @ModulationId, @ActivityFactor,
                @OkaId, @OkaDistanceMeters, @OkaBuildingDampingDb
            )",
            new
            {
                ProjectId = projectId,
                ConfigNumber = configNumber,
                config.Name,
                config.PowerWatts,
                RadioId = radioId,
                LinearName = config.Linear?.Name,
                LinearPowerWatts = config.Linear?.PowerWatts ?? 0,
                CableId = cableId,
                CableLengthMeters = config.Cable.LengthMeters,
                AdditionalLossDb = config.Cable.AdditionalLossDb,
                AdditionalLossDescription = config.Cable.AdditionalLossDescription,
                AntennaId = antennaId,
                HeightMeters = config.Antenna.HeightMeters,
                IsRotatable = config.Antenna.IsRotatable ? 1 : 0,
                HorizontalAngleDegrees = config.Antenna.HorizontalAngleDegrees,
                ModulationId = modulationId,
                config.ActivityFactor,
                OkaId = okaId,
                OkaDistanceMeters = okaDistance,
                OkaBuildingDampingDb = okaDamping,
            },
            transaction);
    }

    private AntennaConfiguration ToConfiguration(ConfigurationRow row)
    {
        var radio = row.RadioId.HasValue ? _masterData.GetRadioById(row.RadioId.Value) : null;
        var cable = row.CableId.HasValue ? _masterData.GetCableById(row.CableId.Value) : null;
        var antenna = row.AntennaId.HasValue ? _masterData.GetAntennaById(row.AntennaId.Value) : null;
        var modulation = row.ModulationId.HasValue ? _masterData.GetModulationById(row.ModulationId.Value) : null;
        var oka = row.OkaId.HasValue ? _masterData.GetOkaById(row.OkaId.Value) : null;

        LinearConfig? linearConfig = null;
        if (!string.IsNullOrEmpty(row.LinearName) || row.LinearPowerWatts > 0)
        {
            linearConfig = new LinearConfig
            {
                Name = row.LinearName ?? "",
                PowerWatts = row.LinearPowerWatts
            };
        }

        return new AntennaConfiguration
        {
            Name = row.Name ?? "",
            PowerWatts = row.PowerWatts,
            RadioId = row.RadioId,
            Radio = new RadioConfig
            {
                Manufacturer = radio?.Manufacturer ?? "",
                Model = radio?.Model ?? ""
            },
            Linear = linearConfig,
            CableId = row.CableId,
            Cable = new CableConfig
            {
                Type = cable?.Name ?? "",
                LengthMeters = row.CableLengthMeters,
                AdditionalLossDb = row.AdditionalLossDb,
                AdditionalLossDescription = row.AdditionalLossDescription ?? ""
            },
            AntennaId = row.AntennaId,
            Antenna = new AntennaPlacement
            {
                Manufacturer = antenna?.Manufacturer ?? "",
                Model = antenna?.Model ?? "",
                HeightMeters = row.HeightMeters,
                IsRotatable = row.IsRotatable == 1,
                HorizontalAngleDegrees = row.HorizontalAngleDegrees
            },
            ModulationId = row.ModulationId,
            Modulation = modulation?.Name ?? "CW",
            ActivityFactor = row.ActivityFactor,
            OkaId = row.OkaId,
            OkaName = oka?.Name ?? ""
        };
    }

    #endregion

    #region Row Classes

    private class ProjectRow
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? OperatorName { get; set; }
        public string? Callsign { get; set; }
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
        public string? LinearName { get; set; }
        public double LinearPowerWatts { get; set; }
        public int? CableId { get; set; }
        public double CableLengthMeters { get; set; }
        public double AdditionalLossDb { get; set; }
        public string? AdditionalLossDescription { get; set; }
        public int? AntennaId { get; set; }
        public double HeightMeters { get; set; }
        public int IsRotatable { get; set; }
        public double HorizontalAngleDegrees { get; set; }
        public int? ModulationId { get; set; }
        public double ActivityFactor { get; set; }
        public int? OkaId { get; set; }
        public double OkaDistanceMeters { get; set; }
        public double OkaBuildingDampingDb { get; set; }
        // For join queries
        public string? ProjectName { get; set; }
    }

    #endregion
}

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
