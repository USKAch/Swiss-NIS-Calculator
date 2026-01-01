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
    /// Line 2: Radio/power, linear amplifier, cable
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
                   $"{Strings.Instance.Transmitter}: {RadioName}, {Strings.Instance.Amplifier}: {linearInfo}, {Strings.Instance.Cable}: {CableDescription}\n" +
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

        // Use amplifier power if set, otherwise use radio power
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

        // Use amplifier power if set, otherwise use radio power
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
        var lang = Localization.Strings.Instance.Language;

        var reportLabel = !string.IsNullOrWhiteSpace(_project?.Name)
            ? _project!.Name
            : _project?.Operator ?? "Project";

        // Localized strings
        var (titlePrefix, opLabel, addrLabel, dateLabel, senderLabel, cableLabel, antennaLabel,
             polLabel, rotLabel, linearLabel, okaLabel, modLabel, dampLabel,
             statusCompliant, statusNonCompliant) = lang switch
        {
            "de" => ("Immissionsberechnung für", "Betreiber", "Adresse", "Datum", "Sender", "Kabel", "Antenne",
                     "Polarisation", "Rotation", "Linear", "OKA", "Modulation", "Gebäudedämpfung",
                     "**Status: KONFORM** - Alle Frequenzen innerhalb der Grenzwerte",
                     "**Status: NICHT KONFORM** - Grenzwerte überschritten!"),
            "fr" => ("Calcul d'immission pour", "Opérateur", "Adresse", "Date", "Émetteur", "Câble", "Antenne",
                     "Polarisation", "Rotation", "Linéaire", "LSM", "Modulation", "Atténuation bâtiment",
                     "**Statut: CONFORME** - Toutes les fréquences dans les limites",
                     "**Statut: NON CONFORME** - Limites dépassées!"),
            "it" => ("Calcolo immissione per", "Operatore", "Indirizzo", "Data", "Trasmettitore", "Cavo", "Antenna",
                     "Polarizzazione", "Rotazione", "Lineare", "LSBD", "Modulazione", "Attenuazione edificio",
                     "**Stato: CONFORME** - Tutte le frequenze entro i limiti",
                     "**Stato: NON CONFORME** - Limiti superati!"),
            _ => ("Emission Calculation for", "Operator", "Address", "Date", "Transmitter", "Cable", "Antenna",
                  "Polarization", "Rotation", "Linear", "OKA", "Modulation", "Building Damping",
                  "**Status: COMPLIANT** - All frequencies within limits",
                  "**Status: NON-COMPLIANT** - Limits exceeded!")
        };

        sb.AppendLine($"# {titlePrefix} {reportLabel}");
        sb.AppendLine();
        sb.AppendLine($"**{opLabel}:** {_project?.Operator}");
        sb.AppendLine($"**{addrLabel}:** {_project?.Address}, {_project?.Location}");
        sb.AppendLine($"**{dateLabel}:** {DateTime.Now:dd.MM.yyyy}");
        sb.AppendLine();

        foreach (var result in Results)
        {
            sb.AppendLine($"## {result.ConfigurationName}");
            sb.AppendLine();

            // Configuration summary table
            sb.AppendLine("| | |");
            sb.AppendLine("|:------------------------|:-------------------------------------------------------|");
            sb.AppendLine($"| **{senderLabel}:**      | {result.PowerWatts}W                                   |");
            sb.AppendLine($"| **{cableLabel}:**       | {result.CableDescription}                              |");
            sb.AppendLine($"| **{antennaLabel}:**     | {result.AntennaName}                                   |");
            sb.AppendLine($"| **{polLabel}:**         | {(result.IsHorizontallyPolarized ? "Horizontal" : "Vertical")} |");
            sb.AppendLine($"| **{rotLabel}:**         | {(result.IsRotatable ? $"{result.HorizontalAngleDegrees}°" : "Fix")} |");
            sb.AppendLine($"| **{linearLabel}:**      | {result.LinearName}                                    |");
            sb.AppendLine($"| **{okaLabel}:**         | {result.OkaName} @ {result.OkaDistance:F1}m            |");
            sb.AppendLine($"| **{modLabel}:**         | {result.Modulation}                                    |");
            sb.AppendLine($"| **{dampLabel}:**        | {result.BuildingDampingDb:F2} dB                       |");
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
            AppendRow(sb, GetLabel(lang, "Frequency", "Frequenz", "Fréquence", "Frequenza"), "f", "MHz", result.BandResults, b => b.FrequencyMHz, "F0");
            AppendRow(sb, GetLabel(lang, "Distance to antenna", "Abstand zur Antenne", "Distance à l'antenne", "Distanza dall'antenna"), "d", "m", result.BandResults, _ => result.OkaDistance, "F1");
            AppendRow(sb, GetLabel(lang, "TX Power", "Senderleistung", "Puissance TX", "Potenza TX"), "P", "W", result.BandResults, b => b.TxPowerW, "F2");
            AppendRow(sb, GetLabel(lang, "Activity factor", "Aktivitätsfaktor", "Facteur d'activité", "Fattore di attività"), "AF", "-", result.BandResults, b => b.ActivityFactor, "F2");
            AppendRow(sb, GetLabel(lang, "Modulation factor", "Modulationsfaktor", "Facteur de modulation", "Fattore di modulazione"), "MF", "-", result.BandResults, b => b.ModulationFactor, "F2");
            AppendRow(sb, GetLabel(lang, "Mean power", "Mittlere Leistung", "Puissance moyenne", "Potenza media"), "Pm", "W", result.BandResults, b => b.MeanPowerW, "F2");
            AppendRow(sb, GetLabel(lang, "Cable attenuation", "Kabeldämpfung", "Atténuation câble", "Attenuazione cavo"), "a1", "dB", result.BandResults, b => b.CableLossDb, "F2");
            AppendRow(sb, GetLabel(lang, "Additional losses", "Übrige Dämpfung", "Pertes supplémentaires", "Perdite aggiuntive"), "a2", "dB", result.BandResults, b => b.AdditionalLossDb, "F2");
            AppendRow(sb, GetLabel(lang, "Total attenuation", "Gesamtdämpfung", "Atténuation totale", "Attenuazione totale"), "a", "dB", result.BandResults, b => b.TotalLossDb, "F2");
            AppendRow(sb, GetLabel(lang, "Attenuation factor", "Dämpfungsfaktor", "Facteur d'atténuation", "Fattore di attenuazione"), "A", "-", result.BandResults, b => b.AttenuationFactor, "F2");
            AppendRow(sb, GetLabel(lang, "Antenna gain", "Antennengewinn", "Gain d'antenne", "Guadagno antenna"), "g1", "dBi", result.BandResults, b => b.GainDbi, "F2");
            AppendRow(sb, GetLabel(lang, "Vertical angle attenuation", "Vertikale Winkeldämpfung", "Atténuation angle vertical", "Attenuazione angolo verticale"), "g2", "dB", result.BandResults, b => b.VerticalAttenuation, "F2");
            AppendRow(sb, GetLabel(lang, "Total antenna gain", "Totaler Antennengewinn", "Gain d'antenne total", "Guadagno antenna totale"), "g", "dB", result.BandResults, b => b.TotalGainDbi, "F2");
            AppendRow(sb, GetLabel(lang, "Gain factor", "Gewinnfaktor", "Facteur de gain", "Fattore di guadagno"), "G", "-", result.BandResults, b => b.GainFactor, "F2");
            AppendRow(sb, GetLabel(lang, "EIRP", "EIRP", "PIRE", "EIRP"), "Ps", "W", result.BandResults, b => b.Eirp, "F2");
            AppendRow(sb, GetLabel(lang, "ERP", "ERP", "PAR", "ERP"), "P's", "W", result.BandResults, b => b.Erp, "F2");
            AppendRow(sb, GetLabel(lang, "Building damping", "Gebäudedämpfung", "Atténuation bâtiment", "Attenuazione edificio"), "ag", "dB", result.BandResults, b => b.BuildingDampingDb, "F2");
            AppendRow(sb, GetLabel(lang, "Building damping factor", "Gebäudedämpfungsfaktor", "Facteur atténuation bât.", "Fattore attenuazione ed."), "AG", "-", result.BandResults, b => b.BuildingDampingFactor, "F2");
            AppendRow(sb, GetLabel(lang, "Ground reflection factor", "Bodenreflexionsfaktor", "Facteur réflexion sol", "Fattore riflessione suolo"), "kr", "-", result.BandResults, b => b.GroundReflectionFactor, "F2");
            AppendRow(sb, "**" + GetLabel(lang, "Field strength at OKA", "Feldstärke am OKA", "Champ au LSM", "Campo al LSBD") + "**", "E'", "V/m", result.BandResults, b => b.FieldStrength, "F2");
            AppendRow(sb, "**" + GetLabel(lang, "Limit", "Grenzwert", "Limite", "Limite") + "**", "EIGW", "V/m", result.BandResults, b => b.Limit, "F1");
            AppendRow(sb, "**" + GetLabel(lang, "Safety distance", "Sicherheitsabstand", "Distance de sécurité", "Distanza di sicurezza") + "**", "ds", "m", result.BandResults, b => b.SafetyDistance, "F2");

            sb.AppendLine();

            // Compliance status
            sb.AppendLine(result.IsCompliant ? statusCompliant : statusNonCompliant);
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();
        }

        // Column explanations section (FSD Section 5.2)
        AppendColumnExplanations(sb, lang);

        return sb.ToString();
    }

    private static string GetLabel(string lang, string en, string de, string fr, string it) => lang switch
    {
        "de" => de,
        "fr" => fr,
        "it" => it,
        _ => en
    };

    private static void AppendColumnExplanations(StringBuilder sb, string lang)
    {
        var title = lang switch
        {
            "de" => "## Spaltenlegende",
            "fr" => "## Légende des colonnes",
            "it" => "## Legenda delle colonne",
            _ => "## Column Explanations"
        };

        sb.AppendLine(title);
        sb.AppendLine();

        var explanations = lang switch
        {
            "de" => new[]
            {
                ("f", "Frequenz in MHz"),
                ("d", "Horizontaler Abstand vom OKA zur Antenne in Metern"),
                ("P", "Senderausgangsleistung in Watt"),
                ("AF", "Aktivitätsfaktor (typisch 0.5 = 50% Sendezeit)"),
                ("MF", "Modulationsfaktor (SSB=0.2, CW=0.4, FM/Digital=1.0)"),
                ("Pm", "Mittlere Leistung = P × AF × MF"),
                ("a1", "Kabeldämpfung in dB"),
                ("a2", "Zusätzliche Dämpfung (Stecker, Schalter) in dB"),
                ("a", "Gesamtdämpfung = a1 + a2"),
                ("A", "Dämpfungsfaktor = 10^(-a/10)"),
                ("g1", "Antennengewinn in dBi"),
                ("g2", "Vertikale Winkeldämpfung basierend auf Antennendiagramm in dB"),
                ("g", "Totaler Antennengewinn = g1 - g2"),
                ("G", "Gewinnfaktor = 10^(g/10)"),
                ("Ps", "EIRP (Equivalent Isotropic Radiated Power) = Pm × A × G"),
                ("P's", "ERP (Effective Radiated Power) = Ps / 1.64"),
                ("ag", "Gebäudedämpfung in dB (0 für Aussenbereich)"),
                ("AG", "Gebäudedämpfungsfaktor = 10^(-ag/10)"),
                ("kr", "Bodenreflexionsfaktor (1.6 gemäss NISV Anhang 2)"),
                ("E'", "Berechnete Feldstärke am OKA in V/m"),
                ("EIGW", "Immissions-Grenzwert gemäss NISV in V/m"),
                ("ds", "Sicherheitsabstand in Metern")
            },
            "fr" => new[]
            {
                ("f", "Fréquence en MHz"),
                ("d", "Distance horizontale du LSM à l'antenne en mètres"),
                ("P", "Puissance de sortie de l'émetteur en Watts"),
                ("AF", "Facteur d'activité (typique 0.5 = 50% du temps d'émission)"),
                ("MF", "Facteur de modulation (SSB=0.2, CW=0.4, FM/Digital=1.0)"),
                ("Pm", "Puissance moyenne = P × AF × MF"),
                ("a1", "Atténuation du câble en dB"),
                ("a2", "Pertes supplémentaires (connecteurs, commutateurs) en dB"),
                ("a", "Atténuation totale = a1 + a2"),
                ("A", "Facteur d'atténuation = 10^(-a/10)"),
                ("g1", "Gain d'antenne en dBi"),
                ("g2", "Atténuation angle vertical basée sur le diagramme d'antenne en dB"),
                ("g", "Gain d'antenne total = g1 - g2"),
                ("G", "Facteur de gain = 10^(g/10)"),
                ("Ps", "PIRE (Puissance Isotrope Rayonnée Équivalente) = Pm × A × G"),
                ("P's", "PAR (Puissance Apparente Rayonnée) = Ps / 1.64"),
                ("ag", "Atténuation du bâtiment en dB (0 pour l'extérieur)"),
                ("AG", "Facteur d'atténuation du bâtiment = 10^(-ag/10)"),
                ("kr", "Facteur de réflexion au sol (1.6 selon ORNI Annexe 2)"),
                ("E'", "Intensité de champ calculée au LSM en V/m"),
                ("EIGW", "Valeur limite d'immission selon ORNI en V/m"),
                ("ds", "Distance de sécurité en mètres")
            },
            "it" => new[]
            {
                ("f", "Frequenza in MHz"),
                ("d", "Distanza orizzontale dal LSBD all'antenna in metri"),
                ("P", "Potenza di uscita del trasmettitore in Watt"),
                ("AF", "Fattore di attività (tipico 0.5 = 50% del tempo di trasmissione)"),
                ("MF", "Fattore di modulazione (SSB=0.2, CW=0.4, FM/Digital=1.0)"),
                ("Pm", "Potenza media = P × AF × MF"),
                ("a1", "Attenuazione del cavo in dB"),
                ("a2", "Perdite aggiuntive (connettori, interruttori) in dB"),
                ("a", "Attenuazione totale = a1 + a2"),
                ("A", "Fattore di attenuazione = 10^(-a/10)"),
                ("g1", "Guadagno dell'antenna in dBi"),
                ("g2", "Attenuazione angolo verticale basata sul diagramma dell'antenna in dB"),
                ("g", "Guadagno totale dell'antenna = g1 - g2"),
                ("G", "Fattore di guadagno = 10^(g/10)"),
                ("Ps", "EIRP (Potenza Isotropa Irradiata Equivalente) = Pm × A × G"),
                ("P's", "ERP (Potenza Effettiva Irradiata) = Ps / 1.64"),
                ("ag", "Attenuazione dell'edificio in dB (0 per esterni)"),
                ("AG", "Fattore di attenuazione dell'edificio = 10^(-ag/10)"),
                ("kr", "Fattore di riflessione al suolo (1.6 secondo ORNI Allegato 2)"),
                ("E'", "Intensità di campo calcolata al LSBD in V/m"),
                ("EIGW", "Valore limite di immissione secondo ORNI in V/m"),
                ("ds", "Distanza di sicurezza in metri")
            },
            _ => new[]
            {
                ("f", "Frequency in MHz"),
                ("d", "Horizontal distance from OKA to antenna in meters"),
                ("P", "Transmitter output power in Watts"),
                ("AF", "Activity factor (typical 0.5 = 50% transmit time)"),
                ("MF", "Modulation factor (SSB=0.2, CW=0.4, FM/Digital=1.0)"),
                ("Pm", "Mean power = P × AF × MF"),
                ("a1", "Cable attenuation in dB"),
                ("a2", "Additional losses (connectors, switches) in dB"),
                ("a", "Total attenuation = a1 + a2"),
                ("A", "Attenuation factor = 10^(-a/10)"),
                ("g1", "Antenna gain in dBi"),
                ("g2", "Vertical angle attenuation based on antenna pattern in dB"),
                ("g", "Total antenna gain = g1 - g2"),
                ("G", "Gain factor = 10^(g/10)"),
                ("Ps", "EIRP (Equivalent Isotropic Radiated Power) = Pm × A × G"),
                ("P's", "ERP (Effective Radiated Power) = Ps / 1.64"),
                ("ag", "Building damping in dB (0 for outdoor)"),
                ("AG", "Building damping factor = 10^(-ag/10)"),
                ("kr", "Ground reflection factor (1.6 per NISV Annex 2)"),
                ("E'", "Calculated field strength at OKA in V/m"),
                ("EIGW", "Emission limit per NISV in V/m"),
                ("ds", "Safety distance in meters")
            }
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
