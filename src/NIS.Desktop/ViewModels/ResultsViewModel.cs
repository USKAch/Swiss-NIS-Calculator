using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NIS.Desktop.Calculations;
using NIS.Desktop.Models;
using NIS.Desktop.Services;

namespace NIS.Desktop.ViewModels;

// Navigation callback for ResultsViewModel
public partial class ResultsViewModel
{
    public Action? NavigateBack { get; set; }
}

/// <summary>
/// Result for a single configuration calculation.
/// </summary>
public class ConfigurationResult
{
    public required string ConfigurationName { get; init; }
    public required string AntennaName { get; init; }
    public required double PowerWatts { get; init; }
    public required string Modulation { get; init; }
    public required double OkaDistance { get; init; }
    public required string OkaName { get; init; }
    public required double BuildingDampingDb { get; init; }
    public required string CableDescription { get; init; }
    public required string LinearName { get; init; }
    public required bool IsRotatable { get; init; }
    public required int HorizontalAngleDegrees { get; init; }
    public required bool IsHorizontallyPolarized { get; init; }

    public ObservableCollection<BandResult> BandResults { get; } = new();

    public bool IsCompliant => BandResults.Count > 0 &&
        System.Linq.Enumerable.All(BandResults, b => b.IsCompliant);

    public double MaxFieldStrength => BandResults.Count > 0
        ? System.Linq.Enumerable.Max(BandResults, b => b.FieldStrength)
        : 0;
}

/// <summary>
/// Result for a single band within a configuration.
/// </summary>
public class BandResult
{
    public double FrequencyMHz { get; init; }
    public double GainDbi { get; init; }
    public double VerticalAttenuation { get; init; }
    public double TotalGainDbi { get; init; }
    public double CableLossDb { get; init; }
    public double AdditionalLossDb { get; init; }
    public double TotalLossDb { get; init; }
    public double MeanPowerW { get; init; }
    public double Eirp { get; init; }
    public double Erp { get; init; }
    public double FieldStrength { get; init; }
    public double Limit { get; init; }
    public double SafetyDistance { get; init; }
    public bool IsCompliant => FieldStrength <= Limit;
}

/// <summary>
/// ViewModel for the Results view.
/// </summary>
public partial class ResultsViewModel : ViewModelBase
{
    private readonly FieldStrengthCalculator _calculator = new();
    private Project? _project;

    // Storage provider for file dialogs (set by view)
    public IStorageProvider? StorageProvider { get; set; }

    public ObservableCollection<ConfigurationResult> Results { get; } = new();

    [ObservableProperty]
    private bool _isCalculating;

    [ObservableProperty]
    private string _statusMessage = "Ready";

    public bool HasResults => Results.Count > 0;

    public bool AllCompliant => Results.Count > 0 &&
        System.Linq.Enumerable.All(Results, r => r.IsCompliant);

    public string ComplianceSummary => AllCompliant
        ? "All configurations comply with NISV limits"
        : "Some configurations exceed NISV limits";

    public ResultsViewModel()
    {
    }

    /// <summary>
    /// Calculate results for all configurations in the project.
    /// </summary>
    public async Task CalculateAsync(Project project)
    {
        _project = project;
        IsCalculating = true;
        StatusMessage = "Calculating...";
        Results.Clear();

        await Task.Run(() =>
        {
            foreach (var config in project.AntennaConfigurations)
            {
                var validationError = ValidateConfiguration(config);
                if (!string.IsNullOrEmpty(validationError))
                {
                    Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                    {
                        Results.Clear();
                        StatusMessage = validationError;
                    });
                    return;
                }

                var result = CalculateConfiguration(config);
                Avalonia.Threading.Dispatcher.UIThread.Post(() => Results.Add(result));
            }
        });

        IsCalculating = false;
        StatusMessage = $"Calculation complete. {Results.Count} configuration(s) analyzed.";
        OnPropertyChanged(nameof(HasResults));
        OnPropertyChanged(nameof(AllCompliant));
        OnPropertyChanged(nameof(ComplianceSummary));
    }

    private ConfigurationResult CalculateConfiguration(AntennaConfiguration config)
    {
        var antenna = DatabaseService.Instance.GetAntenna(config.Antenna.Manufacturer, config.Antenna.Model);
        var cable = DatabaseService.Instance.GetCable(config.Cable.Type);
        var modulation = DatabaseService.Instance.GetModulationByName(config.Modulation);
        var constants = MasterDataStore.Load().Constants;

        var result = new ConfigurationResult
        {
            ConfigurationName = config.Name,
            AntennaName = config.Antenna.DisplayName,
            PowerWatts = config.PowerWatts,
            Modulation = config.ModulationDisplay,
            OkaDistance = config.OkaDistanceMeters,
            OkaName = config.OkaName,
            BuildingDampingDb = config.OkaBuildingDampingDb,
            CableDescription = $"{config.Cable.LengthMeters:F1}m {config.Cable.Type}",
            LinearName = config.Linear?.DisplayName ?? "None",
            IsRotatable = antenna?.IsRotatable ?? false,
            HorizontalAngleDegrees = (int)(antenna?.HorizontalAngleDegrees ?? 360),
            IsHorizontallyPolarized = antenna?.IsHorizontallyPolarized ?? true
        };

        // Get bands from antenna or use default frequencies
        var bands = antenna?.Bands ?? new List<AntennaBand>();

        foreach (var band in bands)
        {
            var bandResult = CalculateBand(config, band, cable, modulation, constants.GroundReflectionFactor);
            result.BandResults.Add(bandResult);
        }

        return result;
    }

    private string? ValidateConfiguration(AntennaConfiguration config)
    {
        if (config.OkaDistanceMeters <= 0)
        {
            return $"OKA distance must be greater than 0 for {config.Antenna.DisplayName}.";
        }

        var antenna = DatabaseService.Instance.GetAntenna(config.Antenna.Manufacturer, config.Antenna.Model);
        if (antenna == null || antenna.Bands.Count == 0)
        {
            return $"Antenna bands missing for {config.Antenna.DisplayName}.";
        }

        var modulation = DatabaseService.Instance.GetModulationByName(config.Modulation);
        if (modulation == null)
        {
            return $"Unknown modulation '{config.Modulation}' for {config.Antenna.DisplayName}.";
        }

        var cable = DatabaseService.Instance.GetCable(config.Cable.Type);
        if (cable == null)
        {
            return $"Cable '{config.Cable.Type}' not found for {config.Antenna.DisplayName}.";
        }

        return null;
    }

    private BandResult CalculateBand(AntennaConfiguration config, AntennaBand band, Cable? cable, Modulation? modulation, double groundReflectionFactor)
    {
        double distance = Math.Max(config.OkaDistanceMeters, 0.001); // Guard against division by zero
        double frequencyMHz = band.FrequencyMHz;

        // Cable loss calculation
        double cableLossDb = cable?.CalculateLoss(config.Cable.LengthMeters, frequencyMHz) ?? 0;

        // Calculate vertical angle to OKA based on antenna height and distance
        double antennaHeight = config.Antenna.HeightMeters;
        double verticalAngle = Math.Atan(antennaHeight / distance) * 180 / Math.PI;
        double verticalAttenuation = band.GetAttenuationAtAngle(verticalAngle);

        // Use Core calculator for field strength computation
        var input = new CalculationInput
        {
            FrequencyMHz = frequencyMHz,
            DistanceMeters = distance,
            TxPowerWatts = config.PowerWatts,
            ActivityFactor = config.ActivityFactor,
            ModulationFactor = modulation?.Factor ?? 0.4,
            AntennaGainDbi = band.GainDbi,
            AngleAttenuationDb = verticalAttenuation,
            TotalCableLossDb = cableLossDb,
            AdditionalLossDb = config.Cable.AdditionalLossDb,
            BuildingDampingDb = config.OkaBuildingDampingDb,
            GroundReflectionFactor = groundReflectionFactor
        };

        var result = _calculator.Calculate(input);

        return new BandResult
        {
            FrequencyMHz = frequencyMHz,
            GainDbi = band.GainDbi,
            VerticalAttenuation = verticalAttenuation,
            TotalGainDbi = result.TotalAntennaGainDb,
            CableLossDb = cableLossDb,
            AdditionalLossDb = config.Cable.AdditionalLossDb,
            TotalLossDb = result.TotalAttenuationDb,
            MeanPowerW = result.MeanPowerWatts,
            Eirp = result.EirpWatts,
            Erp = result.ErpWatts,
            FieldStrength = result.FieldStrengthVm,
            Limit = result.NisLimitVm,
            SafetyDistance = result.SafetyDistanceMeters
        };
    }

    [RelayCommand]
    private async Task ExportMarkdown()
    {
        if (StorageProvider == null || _project == null)
        {
            StatusMessage = "Cannot export. Please save the project first.";
            return;
        }

        var startFolder = await StorageProvider.TryGetFolderFromPathAsync(AppPaths.ExportFolder);
        var reportLabel = !string.IsNullOrWhiteSpace(_project?.Name)
            ? _project!.Name
            : _project?.Operator ?? "Project";
        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Export Results as Markdown",
            SuggestedStartLocation = startFolder,
            SuggestedFileName = $"{reportLabel}_NIS_Report.md",
            DefaultExtension = ".md",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("Markdown") { Patterns = new[] { "*.md" } }
            }
        });

        if (file != null)
        {
            var markdown = GenerateMarkdown();
            await File.WriteAllTextAsync(file.Path.LocalPath, markdown);
            StatusMessage = $"Exported to {file.Name}";
        }
    }

    private string GenerateMarkdown()
    {
        var sb = new StringBuilder();

        var reportLabel = !string.IsNullOrWhiteSpace(_project?.Name)
            ? _project!.Name
            : _project?.Operator ?? "Project";
        sb.AppendLine($"# Immissionsberechnung für {reportLabel}");
        sb.AppendLine();
        sb.AppendLine($"**Operator:** {_project?.Operator}");
        sb.AppendLine($"**Address:** {_project?.Address}, {_project?.Location}");
        sb.AppendLine($"**Date:** {DateTime.Now:dd.MM.yyyy}");
        sb.AppendLine();

        foreach (var result in Results)
        {
            sb.AppendLine($"## {result.ConfigurationName}");
            sb.AppendLine();

            // Configuration summary table
            sb.AppendLine("| | |");
            sb.AppendLine("|:------------------------|:-------------------------------------------------------|");
            sb.AppendLine($"| **Sender:**             | {result.PowerWatts}W                                   |");
            sb.AppendLine($"| **Kabel:**              | {result.CableDescription}                              |");
            sb.AppendLine($"| **Antenne:**            | {result.AntennaName}                                   |");
            sb.AppendLine($"| **Polarisation:**       | {(result.IsHorizontallyPolarized ? "Horizontal" : "Vertical")} |");
            sb.AppendLine($"| **Rotation:**           | {(result.IsRotatable ? $"{result.HorizontalAngleDegrees} Grad" : "Fix")} |");
            sb.AppendLine($"| **Linear:**             | {result.LinearName}                                    |");
            sb.AppendLine($"| **OKA:**                | {result.OkaName} @ {result.OkaDistance:F1}m            |");
            sb.AppendLine($"| **Modulation:**         | {result.Modulation}                                    |");
            sb.AppendLine($"| **Gebäudedämpfung:**    | {result.BuildingDampingDb:F1} dB                       |");
            sb.AppendLine();

            // Band results header
            sb.Append("| Parameter                          | Sym  | Unit   |");
            foreach (var band in result.BandResults)
            {
                sb.Append($" {band.FrequencyMHz,6:F0} |");
            }
            sb.AppendLine();

            sb.Append("|:-----------------------------------|:----:|:------:|");
            foreach (var _ in result.BandResults)
            {
                sb.Append("------:|");
            }
            sb.AppendLine();

            // Data rows
            AppendRow(sb, "Frequenz", "f", "MHz", result.BandResults, b => b.FrequencyMHz, "F0");
            AppendRow(sb, "Abstand OKA zur Antenne", "d", "m", result.BandResults, _ => result.OkaDistance, "F1");
            AppendRow(sb, "Mittl. Leistung am Senderausgang", "Pm", "W", result.BandResults, b => b.MeanPowerW, "F2");
            AppendRow(sb, "Kabeldämpfung", "a1", "dB", result.BandResults, b => b.CableLossDb, "F2");
            AppendRow(sb, "Übrige Dämpfung", "a2", "dB", result.BandResults, b => b.AdditionalLossDb, "F2");
            AppendRow(sb, "Summe der Dämpfung", "a", "dB", result.BandResults, b => b.TotalLossDb, "F2");
            AppendRow(sb, "Antennengewinn", "g1", "dBi", result.BandResults, b => b.GainDbi, "F2");
            AppendRow(sb, "Vertikale Winkeldämpfung", "g2", "dB", result.BandResults, b => b.VerticalAttenuation, "F2");
            AppendRow(sb, "Totaler Antennengewinn", "g", "dB", result.BandResults, b => b.TotalGainDbi, "F2");
            AppendRow(sb, "Massgebende Sendeleistung (EIRP)", "Ps", "W", result.BandResults, b => b.Eirp, "F2");
            AppendRow(sb, "Massgebende Sendeleistung (ERP)", "P's", "W", result.BandResults, b => b.Erp, "F2");
            AppendRow(sb, "Gebäudedämpfung", "ag", "dB", result.BandResults, _ => result.BuildingDampingDb, "F2");
            AppendRow(sb, "**Massgebende Feldstärke am OKA**", "E'", "V/m", result.BandResults, b => b.FieldStrength, "F2");
            AppendRow(sb, "**Immissions-Grenzwert**", "EIGW", "V/m", result.BandResults, b => b.Limit, "F2");
            AppendRow(sb, "**Sicherheitsabstand**", "ds", "m", result.BandResults, b => b.SafetyDistance, "F2");

            sb.AppendLine();

            // Compliance status
            sb.AppendLine(result.IsCompliant
                ? "**Status: KONFORM** - Alle Frequenzen innerhalb der Grenzwerte"
                : "**Status: NICHT KONFORM** - Grenzwerte überschritten!");
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();
        }

        return sb.ToString();
    }

    private void AppendRow(StringBuilder sb, string param, string sym, string unit,
        ObservableCollection<BandResult> bands, Func<BandResult, double> getValue, string format)
    {
        sb.Append($"| {param,-36} | {sym,-4} | {unit,-6} |");
        foreach (var band in bands)
        {
            sb.Append($" {getValue(band).ToString(format),6} |");
        }
        sb.AppendLine();
    }

    [RelayCommand]
    private void Close()
    {
        NavigateBack?.Invoke();
    }
}
