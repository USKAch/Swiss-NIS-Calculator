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
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
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
    public required double RadioPowerWatts { get; init; }
    public required double LinearPowerWatts { get; init; }
    public required string Modulation { get; init; }
    public required double OkaDistance { get; init; }
    public required double AntennaHeight { get; init; }
    public required string OkaName { get; init; }
    public required string OkaNumber { get; init; }
    public required double BuildingDampingDb { get; init; }
    public required string CableDescription { get; init; }
    public required string LinearName { get; init; }
    public required bool IsRotatable { get; init; }
    public required int HorizontalAngleDegrees { get; init; }
    public required bool IsHorizontallyPolarized { get; init; }

    public bool HasLinear => !string.IsNullOrWhiteSpace(LinearName);

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
    private string _statusMessage = "";

    public ResultsViewModel()
    {
        _statusMessage = Strings.Instance.StatusReady;
    }

    public bool HasResults => Results.Count > 0;

    public bool AllCompliant => Results.Count > 0 &&
        System.Linq.Enumerable.All(Results, r => r.IsCompliant);

    public string ComplianceSummary => AllCompliant
        ? Strings.Instance.CalcAllCompliant
        : Strings.Instance.CalcNonCompliantDetected;

    /// <summary>
    /// Calculate results for all configurations in the project.
    /// </summary>
    public async Task CalculateAsync(Project project)
    {
        _project = project;
        IsCalculating = true;
        StatusMessage = Strings.Instance.Calculating;
        Results.Clear();

        // Validate all configurations first
        if (project.AntennaConfigurations.Count == 0)
        {
            IsCalculating = false;
            StatusMessage = Strings.Instance.NoConfigurationsToCalculate;
            await MessageBoxManager.GetMessageBoxStandard(
                Strings.Instance.Error,
                Strings.Instance.NoConfigurationsToCalculate,
                ButtonEnum.Ok,
                Icon.Warning).ShowAsync();
            return;
        }

        var validationErrors = new List<string>();
        foreach (var config in project.AntennaConfigurations)
        {
            var error = ValidateConfiguration(config);
            if (!string.IsNullOrEmpty(error))
                validationErrors.Add(error);
        }

        if (validationErrors.Count > 0)
        {
            IsCalculating = false;
            var errorList = string.Join("\n", validationErrors);
            StatusMessage = Strings.Instance.ConfigurationIncomplete;
            await MessageBoxManager.GetMessageBoxStandard(
                Strings.Instance.ConfigurationIncomplete,
                $"{Strings.Instance.FixErrorsBeforeCalculating}\n\n{errorList}",
                ButtonEnum.Ok,
                Icon.Warning).ShowAsync();
            return;
        }

        // All validations passed, proceed with calculation
        await Task.Run(() =>
        {
            try
            {
                foreach (var config in project.AntennaConfigurations)
                {
                    var result = CalculateConfiguration(config);
                    Avalonia.Threading.Dispatcher.UIThread.Post(() => Results.Add(result));
                }
            }
            catch (Exception ex)
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    StatusMessage = $"{Strings.Instance.Error}: {ex.Message}";
                });
            }
        });

        IsCalculating = false;
        StatusMessage = $"{Strings.Instance.CalculationComplete} {Results.Count} {Strings.Instance.ConfigurationsAnalyzed}";
        OnPropertyChanged(nameof(HasResults));
        OnPropertyChanged(nameof(AllCompliant));
        OnPropertyChanged(nameof(ComplianceSummary));
    }

    private ConfigurationResult CalculateConfiguration(AntennaConfiguration config)
    {
        // All lookups use IDs only
        var antenna = config.AntennaId.HasValue
            ? DatabaseService.Instance.GetAntennaById(config.AntennaId.Value)
            : null;
        var cable = config.CableId.HasValue
            ? DatabaseService.Instance.GetCableById(config.CableId.Value)
            : null;
        var modulation = config.ModulationId.HasValue
            ? DatabaseService.Instance.GetModulationById(config.ModulationId.Value)
            : null;
        var oka = config.OkaId.HasValue
            ? DatabaseService.Instance.GetOkaById(config.OkaId.Value)
            : null;
        var constants = MasterDataStore.Load().Constants;

        // Get OKA values from master data (single source of truth)
        double okaDistance = oka?.DefaultDistanceMeters ?? 10;
        double okaDamping = oka?.DefaultDampingDb ?? 0;

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
            RadioPowerWatts = config.PowerWatts,
            LinearPowerWatts = config.Linear?.PowerWatts ?? 0,
            Modulation = config.ModulationDisplay,
            OkaDistance = okaDistance,
            AntennaHeight = config.Antenna.HeightMeters,
            OkaName = config.OkaName,
            OkaNumber = oka?.Id.ToString() ?? "-",
            BuildingDampingDb = okaDamping,
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
            var bandResult = CalculateBand(config, band, cable, modulation, constants.GroundReflectionFactor, okaDistance, okaDamping);
            result.BandResults.Add(bandResult);
        }

        return result;
    }

    /// <summary>
    /// Validates all configurations in a project and returns a list of error messages.
    /// Returns empty list if all configurations are valid.
    /// </summary>
    public static List<string> ValidateAllConfigurations(Project project)
    {
        var errors = new List<string>();

        if (project.AntennaConfigurations.Count == 0)
        {
            errors.Add(Strings.Instance.NoConfigurationsToCalculate);
            return errors;
        }

        foreach (var config in project.AntennaConfigurations)
        {
            var error = ValidateConfiguration(config);
            if (!string.IsNullOrEmpty(error))
                errors.Add(error);
        }

        return errors;
    }

    /// <summary>
    /// Validates a configuration and returns an error message if invalid, or null if valid.
    /// All lookups use IDs only.
    /// </summary>
    public static string? ValidateConfiguration(AntennaConfiguration config)
    {
        var configName = !string.IsNullOrEmpty(config.Name) ? config.Name : config.Antenna.DisplayName;

        // Check antenna is selected
        if (!config.AntennaId.HasValue)
        {
            return $"{configName}: {Strings.Instance.NoAntennaSelected}";
        }

        var antenna = DatabaseService.Instance.GetAntennaById(config.AntennaId.Value);
        if (antenna == null)
        {
            return $"{configName}: {Strings.Instance.AntennaNotFound} ({config.Antenna.DisplayName})";
        }

        if (antenna.Bands.Count == 0)
        {
            return $"{configName}: {Strings.Instance.AntennaNoBands} ({config.Antenna.DisplayName})";
        }

        // Check cable is selected
        if (!config.CableId.HasValue)
        {
            return $"{configName}: {Strings.Instance.NoCableSelected}";
        }

        var cable = DatabaseService.Instance.GetCableById(config.CableId.Value);
        if (cable == null)
        {
            return $"{configName}: {Strings.Instance.CableNotFound} ({config.Cable.Type})";
        }

        // Check modulation is selected
        if (!config.ModulationId.HasValue)
        {
            return $"{configName}: {Strings.Instance.NoModulationSelected}";
        }

        var modulation = DatabaseService.Instance.GetModulationById(config.ModulationId.Value);
        if (modulation == null)
        {
            return $"{configName}: {Strings.Instance.ModulationNotFound} ({config.Modulation})";
        }

        // Check OKA is configured
        if (!config.OkaId.HasValue)
        {
            return $"{configName}: {Strings.Instance.NoOkaSelected}";
        }

        var oka = DatabaseService.Instance.GetOkaById(config.OkaId.Value);
        if (oka == null)
        {
            return $"{configName}: {Strings.Instance.OkaNotFound} ({config.OkaName})";
        }

        if (oka.DefaultDistanceMeters <= 0)
        {
            return $"{configName}: {Strings.Instance.OkaDistanceInvalid}";
        }

        return null;
    }

    private BandResult CalculateBand(AntennaConfiguration config, AntennaBand band, Cable? cable, Modulation? modulation, double groundReflectionFactor, double okaDistance, double okaDamping)
    {
        double horizontalDistance = Math.Max(okaDistance, 0.001); // Guard against division by zero
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
            BuildingDampingDb = okaDamping,
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
            BuildingDampingDb = okaDamping,
            BuildingDampingFactor = result.BuildingDampingFactor,
            GroundReflectionFactor = groundReflectionFactor,
            FieldStrength = result.FieldStrengthVm,
            Limit = result.NisLimitVm,
            SafetyDistance = result.SafetyDistanceMeters,
            OkaDistance = okaDistance
        };
    }

    [RelayCommand]
    private async Task ExportMarkdown()
    {
        if (StorageProvider == null || _project == null)
        {
            StatusMessage = Strings.Instance.CannotExportNoProject;
            return;
        }

        var startFolder = await StorageProvider.TryGetFolderFromPathAsync(AppPaths.ExportFolder);
        var reportLabel = !string.IsNullOrWhiteSpace(_project?.Name)
            ? _project!.Name
            : _project?.Operator ?? "Project";
        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = Strings.Instance.ExportMarkdownTitle,
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
            StatusMessage = string.Format(Strings.Instance.ExportedToFile, file.Name);
        }
    }

    [RelayCommand]
    private async Task ExportPdf()
    {
        if (StorageProvider == null || _project == null)
        {
            StatusMessage = Strings.Instance.CannotExportNoProject;
            return;
        }

        var startFolder = await StorageProvider.TryGetFolderFromPathAsync(AppPaths.ExportFolder);
        var reportLabel = !string.IsNullOrWhiteSpace(_project?.Name)
            ? _project!.Name
            : _project?.Operator ?? "Project";
        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = Strings.Instance.ExportPdfTitle,
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
                StatusMessage = string.Format(Strings.Instance.ExportedToFile, file.Name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format(Strings.Instance.PdfExportError, ex.Message);
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

        for (int i = 0; i < Results.Count; i++)
        {
            var result = Results[i];
            var configName = !string.IsNullOrWhiteSpace(result.ConfigurationName)
                ? result.ConfigurationName
                : $"{s.Configuration} #{i + 1}";

            // Configuration summary table (config name as header)
            sb.AppendLine($"| **{configName}** | |");
            sb.AppendLine("|:--|:--|");
            if (result.HasLinear)
            {
                sb.AppendLine($"| **{s.CalcTransmitter}:** | {result.RadioName} |");
                sb.AppendLine($"| **{s.CalcLinear}:** | {result.LinearName}, {result.LinearPowerWatts:F0}W |");
            }
            else
            {
                sb.AppendLine($"| **{s.CalcTransmitter}:** | {result.RadioName}, {result.RadioPowerWatts:F0}W |");
            }
            sb.AppendLine($"| **{s.CalcCable}:** | {result.CableDescription} |");
            sb.AppendLine($"| **{s.CalcAntenna}:** | {result.AntennaName} |");
            sb.AppendLine($"| **{s.CalcPolarization}:** | {(result.IsHorizontallyPolarized ? s.CalcHorizontal : s.CalcVertical)} |");
            sb.AppendLine($"| **{s.CalcRotation}:** | {(result.IsRotatable ? $"{result.HorizontalAngleDegrees}°" : s.CalcFixed)} |");
            sb.AppendLine($"| **{s.OkaFullName} ({s.CalcOka}):** | Nr. {result.OkaNumber}: {result.OkaName} @ {result.OkaDistance:F1}m |");
            sb.AppendLine($"| **{s.CalcModulation}:** | {result.Modulation} |");
            sb.AppendLine($"| **{s.CalcBuildingDamping}:** | {result.BuildingDampingDb:F2} dB |");
            sb.AppendLine();

            // Band results table - markdown format (frequencies shown in first data row)
            sb.Append("| Parameter | Sym | Unit |");
            foreach (var _ in result.BandResults)
            {
                sb.Append(" |");
            }
            sb.AppendLine();

            // Table separator
            sb.Append("|:---|:---:|:---:|");
            foreach (var _ in result.BandResults)
            {
                sb.Append("---:|");
            }
            sb.AppendLine();

            // FSD Section 5 required rows (22 rows)
            foreach (var row in CalculationTableDefinition.Rows)
            {
                var label = row.GetLabel(s);
                var symbol = row.GetSymbol(s);

                if (row.IsBold)
                {
                    label = $"**{label}**";
                    symbol = $"**{symbol}**";
                }

                if (row.IsConstantString)
                {
                    AppendRow(sb, label, symbol, row.Unit, result.BandResults, row.StringValueGetter!(result));
                }
                else if (row.IsConstantNumeric)
                {
                    AppendRow(sb, label, symbol, row.Unit, result.BandResults, row.ConfigValueGetter!(result).ToString(row.Format));
                }
                else
                {
                    AppendRow(sb, label, symbol, row.Unit, result.BandResults, row.BandValueGetter!, row.Format);
                }
            }
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

        // Explanations table (parameter label + explanation)
        var title = s.CalcColumnExplanations.Replace("## ", "");
        sb.AppendLine($"| **{title}** | |");
        sb.AppendLine("|:--|:--|");

        var rows = CalculationTableDefinition.Rows;
        var explanations = CalculationTableDefinition.Explanations;
        for (int i = 0; i < rows.Count && i < explanations.Count; i++)
        {
            var label = rows[i].GetLabel(s);
            var explanation = s.Get(explanations[i].ExplanationKey);
            sb.AppendLine($"| {label} | {explanation} |");
        }
        sb.AppendLine();
    }

    private void AppendRow(StringBuilder sb, string param, string sym, string unit,
        ObservableCollection<BandResult> bands, Func<BandResult, double> getValue, string format)
    {
        sb.Append($"| {param} | {sym} | {unit} |");
        foreach (var band in bands)
        {
            sb.Append($" {getValue(band).ToString(format)} |");
        }
        sb.AppendLine();
    }

    private void AppendRow(StringBuilder sb, string param, string sym, string unit,
        ObservableCollection<BandResult> bands, string value)
    {
        sb.Append($"| {param} | {sym} | {unit} |");
        foreach (var _ in bands)
        {
            sb.Append($" {value} |");
        }
        sb.AppendLine();
    }

    [RelayCommand]
    private void Close()
    {
        NavigateBack?.Invoke();
    }
}
