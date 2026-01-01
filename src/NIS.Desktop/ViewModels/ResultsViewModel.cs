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
using NIS.Desktop.Localization;
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
    public required string RadioName { get; init; }
    public required double PowerWatts { get; init; }
    public required string Modulation { get; init; }
    public required double OkaDistance { get; init; }
    public required double AntennaHeight { get; init; }
    public required string OkaName { get; init; }
    public required double BuildingDampingDb { get; init; }
    public required string CableDescription { get; init; }
    public required string LinearName { get; init; }
    public required bool IsRotatable { get; init; }
    public required int HorizontalAngleDegrees { get; init; }
    public required bool IsHorizontallyPolarized { get; init; }

    /// <summary>
    /// Real 3D distance from antenna to OKA = √(horizontal² + height²)
    /// </summary>
    public double RealDistance => Math.Sqrt(OkaDistance * OkaDistance + AntennaHeight * AntennaHeight);

    /// <summary>
    /// Formatted configuration description with localized labels on 3 lines.
    /// Line 1: Antenna info (name, height)
    /// Line 2: Radio/power, linear, cable
    /// Line 3: OKA info (name, horizontal distance, real distance)
    /// </summary>
    public string ConfigDescription
    {
        get
        {
            var linearInfo = string.IsNullOrEmpty(LinearName)
                ? Strings.Instance.None
                : LinearName;

            return $"{Strings.Instance.Antenna}: {AntennaName}, {AntennaHeight:F0}m {Strings.Instance.AboveOka}\n" +
                   $"{Strings.Instance.Transmitter}: {RadioName}, {Strings.Instance.Linear}: {linearInfo}, {Strings.Instance.Cable}: {CableDescription}\n" +
                   $"{Strings.Instance.OkaFullName}: {OkaName}, {OkaDistance:F0}m {Strings.Instance.HorizDistToMast}, {Strings.Instance.DistanceAntennaOka}: {RealDistance:F1}m";
        }
    }

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
    public double TxPowerW { get; init; }
    public double ActivityFactor { get; init; }
    public double ModulationFactor { get; init; }
    public double MeanPowerW { get; init; }
    public double CableLossDb { get; init; }
    public double AdditionalLossDb { get; init; }
    public double TotalLossDb { get; init; }
    public double AttenuationFactor { get; init; }
    public double GainDbi { get; init; }
    public double VerticalAttenuation { get; init; }
    public double TotalGainDbi { get; init; }
    public double GainFactor { get; init; }
    public double Eirp { get; init; }
    public double Erp { get; init; }
    public double BuildingDampingDb { get; init; }
    public double BuildingDampingFactor { get; init; }
    public double GroundReflectionFactor { get; init; }
    public double FieldStrength { get; init; }
    public double Limit { get; init; }
    public double SafetyDistance { get; init; }
    public double OkaDistance { get; init; }
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

        // Use linear power if set, otherwise use radio power
        double effectivePower = (config.Linear != null && config.Linear.PowerWatts > 0)
            ? config.Linear.PowerWatts
            : config.PowerWatts;

        var result = new ConfigurationResult
        {
            ConfigurationName = config.Name,
            AntennaName = config.Antenna.DisplayName,
            RadioName = config.Radio.DisplayName,
            PowerWatts = effectivePower,
            Modulation = config.ModulationDisplay,
            OkaDistance = config.OkaDistanceMeters,
            AntennaHeight = config.Antenna.HeightMeters,
            OkaName = config.OkaName,
            BuildingDampingDb = config.OkaBuildingDampingDb,
            CableDescription = $"{config.Cable.LengthMeters:F1}m {config.Cable.Type}",
            LinearName = config.Linear?.Name ?? "",
            IsRotatable = config.Antenna.IsRotatable,
            HorizontalAngleDegrees = (int)config.Antenna.HorizontalAngleDegrees,
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
        double horizontalDistance = Math.Max(config.OkaDistanceMeters, 0.001); // Guard against division by zero
        double antennaHeight = config.Antenna.HeightMeters;
        double frequencyMHz = band.FrequencyMHz;

        // Calculate real 3D distance from antenna to OKA: √(horizontal² + height²)
        double realDistance = Math.Sqrt(horizontalDistance * horizontalDistance + antennaHeight * antennaHeight);

        // Cable loss calculation
        double cableLossDb = cable?.CalculateLoss(config.Cable.LengthMeters, frequencyMHz) ?? 0;

        // Calculate vertical angle to OKA, then look up vertical attenuation from antenna pattern
        double verticalAngle = Math.Atan(antennaHeight / horizontalDistance) * 180 / Math.PI;
        double verticalAttenuation = band.GetAttenuationAtAngle(verticalAngle);

        double modulationFactor = modulation?.Factor ?? 0.4;

        // Use linear power if set, otherwise use radio power
        double effectivePower = (config.Linear != null && config.Linear.PowerWatts > 0)
            ? config.Linear.PowerWatts
            : config.PowerWatts;

        // Use Core calculator for field strength computation
        // Use real 3D distance for field strength calculation
        var input = new CalculationInput
        {
            FrequencyMHz = frequencyMHz,
            DistanceMeters = realDistance,
            TxPowerWatts = effectivePower,
            ActivityFactor = config.ActivityFactor,
            ModulationFactor = modulationFactor,
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
            TxPowerW = effectivePower,
            ActivityFactor = config.ActivityFactor,
            ModulationFactor = modulationFactor,
            MeanPowerW = result.MeanPowerWatts,
            CableLossDb = cableLossDb,
            AdditionalLossDb = config.Cable.AdditionalLossDb,
            TotalLossDb = result.TotalAttenuationDb,
            AttenuationFactor = result.AttenuationFactor,
            GainDbi = band.GainDbi,
            VerticalAttenuation = verticalAttenuation,
            TotalGainDbi = result.TotalAntennaGainDb,
            GainFactor = result.AntennaGainFactor,
            Eirp = result.EirpWatts,
            Erp = result.ErpWatts,
            BuildingDampingDb = config.OkaBuildingDampingDb,
            BuildingDampingFactor = result.BuildingDampingFactor,
            GroundReflectionFactor = groundReflectionFactor,
            FieldStrength = result.FieldStrengthVm,
            Limit = result.NisLimitVm,
            SafetyDistance = result.SafetyDistanceMeters,
            OkaDistance = config.OkaDistanceMeters
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

    [RelayCommand]
    private async Task ExportPdf()
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
            Title = "Export Results as PDF",
            SuggestedStartLocation = startFolder,
            SuggestedFileName = $"{reportLabel}_{DateTime.Now:yyyyMMdd}.pdf",
            DefaultExtension = ".pdf",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("PDF") { Patterns = new[] { "*.pdf" } }
            }
        });

        if (file != null && _project != null)
        {
            try
            {
                var pdfGenerator = new PdfReportGenerator();
                pdfGenerator.GenerateReport(_project, Results.ToList(), file.Path.LocalPath);
                StatusMessage = $"PDF exported to {file.Name}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"PDF export failed: {ex.Message}";
            }
        }
    }

    private string GenerateMarkdown()
    {
        var sb = new StringBuilder();
        var s = Localization.Strings.Instance;

        var reportLabel = !string.IsNullOrWhiteSpace(_project?.Name)
            ? _project!.Name
            : _project?.Operator ?? "Project";

        sb.AppendLine($"# {s.CalcTitlePrefix} {reportLabel}");
        sb.AppendLine();
        sb.AppendLine($"**{s.CalcOperator}:** {_project?.Operator}");
        sb.AppendLine($"**{s.CalcAddress}:** {_project?.Address}, {_project?.Location}");
        sb.AppendLine($"**{s.CalcDate}:** {DateTime.Now:dd.MM.yyyy}");
        sb.AppendLine();

        foreach (var result in Results)
        {
            sb.AppendLine($"## {result.ConfigurationName}");
            sb.AppendLine();

            // Configuration summary table
            sb.AppendLine("| | |");
            sb.AppendLine("|:------------------------|:-------------------------------------------------------|");
            sb.AppendLine($"| **{s.CalcTransmitter}:**      | {result.PowerWatts}W                                   |");
            sb.AppendLine($"| **{s.CalcCable}:**       | {result.CableDescription}                              |");
            sb.AppendLine($"| **{s.CalcAntenna}:**     | {result.AntennaName}                                   |");
            sb.AppendLine($"| **{s.CalcPolarization}:**         | {(result.IsHorizontallyPolarized ? s.CalcHorizontal : s.CalcVertical)} |");
            sb.AppendLine($"| **{s.CalcRotation}:**         | {(result.IsRotatable ? $"{result.HorizontalAngleDegrees}°" : s.CalcFixed)} |");
            sb.AppendLine($"| **{s.CalcLinear}:**      | {result.LinearName}                                    |");
            sb.AppendLine($"| **{s.CalcOka}:**         | {result.OkaName} @ {result.OkaDistance:F1}m            |");
            sb.AppendLine($"| **{s.CalcModulation}:**         | {result.Modulation}                                    |");
            sb.AppendLine($"| **{s.CalcBuildingDamping}:**        | {result.BuildingDampingDb:F2} dB                       |");
            sb.AppendLine();

            // Band results header - FSD Section 5 columns
            sb.Append("| Parameter                            | Sym  | Unit   |");
            foreach (var band in result.BandResults)
            {
                sb.Append($" {band.FrequencyMHz,7:F0} |");
            }
            sb.AppendLine();

            sb.Append("|:-------------------------------------|:----:|:------:|");
            foreach (var _ in result.BandResults)
            {
                sb.Append("-------:|");
            }
            sb.AppendLine();

            // FSD Section 5 required rows
            AppendRow(sb, s.CalcFrequency, "f", "MHz", result.BandResults, b => b.FrequencyMHz, "F0");
            AppendRow(sb, s.CalcDistanceToAntenna, "d", "m", result.BandResults, _ => result.OkaDistance, "F1");
            AppendRow(sb, s.CalcTxPower, "P", "W", result.BandResults, b => b.TxPowerW, "F2");
            AppendRow(sb, s.CalcActivityFactor, "AF", "-", result.BandResults, b => b.ActivityFactor, "F2");
            AppendRow(sb, s.CalcModulationFactor, "MF", "-", result.BandResults, b => b.ModulationFactor, "F2");
            AppendRow(sb, s.CalcMeanPower, s.PmittelLbl, "W", result.BandResults, b => b.MeanPowerW, "F2");
            AppendRow(sb, s.CalcCableAttenuation, "a1", "dB", result.BandResults, b => b.CableLossDb, "F2");
            AppendRow(sb, s.CalcAdditionalLosses, "a2", "dB", result.BandResults, b => b.AdditionalLossDb, "F2");
            AppendRow(sb, s.CalcTotalAttenuation, "a", "dB", result.BandResults, b => b.TotalLossDb, "F2");
            AppendRow(sb, s.CalcAttenuationFactor, "A", "-", result.BandResults, b => b.AttenuationFactor, "F2");
            AppendRow(sb, s.CalcAntennaGain, "g1", "dBi", result.BandResults, b => b.GainDbi, "F2");
            AppendRow(sb, s.CalcVerticalAttenuation, "g2", "dB", result.BandResults, b => b.VerticalAttenuation, "F2");
            AppendRow(sb, s.CalcTotalAntennaGain, "g", "dB", result.BandResults, b => b.TotalGainDbi, "F2");
            AppendRow(sb, s.CalcGainFactor, "G", "-", result.BandResults, b => b.GainFactor, "F2");
            AppendRow(sb, s.CalcEirp, "Ps", "W", result.BandResults, b => b.Eirp, "F2");
            AppendRow(sb, s.CalcErp, "P's", "W", result.BandResults, b => b.Erp, "F2");
            AppendRow(sb, s.CalcBuildingDampingRow, "ag", "dB", result.BandResults, b => b.BuildingDampingDb, "F2");
            AppendRow(sb, s.CalcBuildingDampingFactor, "AG", "-", result.BandResults, b => b.BuildingDampingFactor, "F2");
            AppendRow(sb, s.CalcGroundReflection, "kr", "-", result.BandResults, b => b.GroundReflectionFactor, "F2");
            AppendRow(sb, "**" + s.CalcFieldStrength + "**", "E'", "V/m", result.BandResults, b => b.FieldStrength, "F2");
            AppendRow(sb, "**" + s.CalcLimit + "**", "EIGW", "V/m", result.BandResults, b => b.Limit, "F1");
            AppendRow(sb, "**" + s.CalcMinSafetyDistance + "**", "ds", "m", result.BandResults, b => b.SafetyDistance, "F2");
            AppendRow(sb, "**" + s.CalcOkaDistance + "**", "d", "m", result.BandResults, _ => result.OkaDistance, "F1");

            sb.AppendLine();

            // Compliance status
            sb.AppendLine(result.IsCompliant ? s.CalcStatusCompliant : s.CalcStatusNonCompliant);
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();
        }

        // Column explanations section (FSD Section 5.2)
        AppendColumnExplanations(sb);

        return sb.ToString();
    }

    private static void AppendColumnExplanations(StringBuilder sb)
    {
        var s = Localization.Strings.Instance;

        sb.AppendLine(s.CalcColumnExplanations);
        sb.AppendLine();

        var explanations = new[]
        {
            ("f", s.CalcExplainF),
            ("d", s.CalcExplainD),
            ("P", s.CalcExplainP),
            ("AF", s.CalcExplainAF),
            ("MF", s.CalcExplainMF),
            (s.PmittelLbl, s.CalcExplainPm),
            ("a1", s.CalcExplainA1),
            ("a2", s.CalcExplainA2),
            ("a", s.CalcExplainA),
            ("A", s.CalcExplainAFactor),
            ("g1", s.CalcExplainG1),
            ("g2", s.CalcExplainG2),
            ("g", s.CalcExplainG),
            ("G", s.CalcExplainGFactor),
            ("Ps", s.CalcExplainPs),
            ("P's", s.CalcExplainPsPrime),
            ("ag", s.CalcExplainAg),
            ("AG", s.CalcExplainAG),
            ("kr", s.CalcExplainKr),
            ("E'", s.CalcExplainE),
            ("EIGW", s.CalcExplainEigw),
            ("ds", s.CalcExplainDs)
        };

        foreach (var (symbol, description) in explanations)
        {
            sb.AppendLine($"- **{symbol}**: {description}");
        }
        sb.AppendLine();
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
