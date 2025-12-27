using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NIS.Desktop.Localization;

/// <summary>
/// Provides localized strings for the application.
/// All UI text should be accessed through this class.
/// </summary>
public class Strings : INotifyPropertyChanged
{
    private static Strings? _instance;
    public static Strings Instance => _instance ??= new Strings();

    private string _language = "de";

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Language
    {
        get => _language;
        set
        {
            if (_language != value)
            {
                _language = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }
    }

    private string Get(string key) =>
        TranslationData.TryGetValue(key, out var t) && t.TryGetValue(_language, out var v) ? v :
        TranslationData.TryGetValue(key, out var t2) && t2.TryGetValue("de", out var fallback) ? fallback : $"[{key}]";

    // ============================================================
    // COMMON / BUTTONS
    // ============================================================
    public string Save => Get("Save");
    public string SaveAs => Get("SaveAs");
    public string Cancel => Get("Cancel");
    public string Edit => Get("Edit");
    public string New => Get("New");
    public string Delete => Get("Delete");
    public string Add => Get("Add");
    public string Remove => Get("Remove");
    public string Back => Get("Back");
    public string Close => Get("Close");
    public string Calculate => Get("Calculate");
    public string Export => Get("Export");
    public string Yes => Get("Yes");
    public string No => Get("No");

    // ============================================================
    // WELCOME SCREEN
    // ============================================================
    public string AppTitle => Get("AppTitle");
    public string AppSubtitle => Get("AppSubtitle");
    public string SelectLanguage => Get("SelectLanguage");
    public string Theme => Get("Theme");
    public string ThemeLight => Get("ThemeLight");
    public string ThemeDark => Get("ThemeDark");
    public string NewProject => Get("NewProject");
    public string OpenProject => Get("OpenProject");
    public string MasterData => Get("MasterData");
    public string NisvCompliance => Get("NisvCompliance");

    // ============================================================
    // PROJECT INFO
    // ============================================================
    public string StationInfo => Get("StationInfo");
    public string Callsign => Get("Callsign");
    public string Address => Get("Address");
    public string CreateProject => Get("CreateProject");
    public string EditStationInfo => Get("EditStationInfo");
    public string CallsignExample => Get("CallsignExample");
    public string AddressExample => Get("AddressExample");

    // ============================================================
    // PROJECT OVERVIEW
    // ============================================================
    public string Configurations => Get("Configurations");
    public string AddConfiguration => Get("AddConfiguration");
    public string CalculateAll => Get("CalculateAll");
    public string ExportReport => Get("ExportReport");
    public string NoConfigurations => Get("NoConfigurations");

    // ============================================================
    // CONFIGURATION EDITOR
    // ============================================================
    public string Configuration => Get("Configuration");
    public string Antenna => Get("Antenna");
    public string Transmitter => Get("Transmitter");
    public string FeedLine => Get("FeedLine");
    public string OperatingParameters => Get("OperatingParameters");
    public string EvaluationPoint => Get("EvaluationPoint");

    // Antenna section
    public string SelectAntenna => Get("SelectAntenna");
    public string Height => Get("Height");
    public string HeightUnit => Get("HeightUnit");
    public string FrequencyBands => Get("FrequencyBands");

    // Transmitter section
    public string Radio => Get("Radio");
    public string SelectRadio => Get("SelectRadio");
    public string UseLinearAmplifier => Get("UseLinearAmplifier");
    public string Linear => Get("Linear");
    public string Power => Get("Power");
    public string PowerUnit => Get("PowerUnit");

    // Feed line section
    public string Cable => Get("Cable");
    public string SelectCable => Get("SelectCable");
    public string CableLength => Get("CableLength");
    public string AdditionalLoss => Get("AdditionalLoss");
    public string LossDescription => Get("LossDescription");

    // Operating parameters
    public string Modulation => Get("Modulation");
    public string ActivityFactor => Get("ActivityFactor");
    public string ActivityFactorHint => Get("ActivityFactorHint");
    public string ParametersApplyToAllBands => Get("ParametersApplyToAllBands");

    // OKA section
    public string OkaName => Get("OkaName");
    public string OkaDistance => Get("OkaDistance");
    public string OkaDamping => Get("OkaDamping");
    public string OkaDampingHint => Get("OkaDampingHint");
    public string OkaExplanation => Get("OkaExplanation");
    public string AdditionalLossExample => Get("AdditionalLossExample");
    public string OkaNameExample => Get("OkaNameExample");

    // ============================================================
    // ANTENNA EDITOR
    // ============================================================
    public string AntennaDetails => Get("AntennaDetails");
    public string Manufacturer => Get("Manufacturer");
    public string Model => Get("Model");
    public string Polarization => Get("Polarization");
    public string Horizontal => Get("Horizontal");
    public string Vertical => Get("Vertical");
    public string Rotatable => Get("Rotatable");
    public string RotationAngle => Get("RotationAngle");
    public string DegreeInfoOnly => Get("DegreeInfoOnly");
    public string Frequency => Get("Frequency");
    public string Gain => Get("Gain");
    public string GainRange => Get("GainRange");
    public string AddBand => Get("AddBand");
    public string VerticalRadiationPattern => Get("VerticalRadiationPattern");
    public string PatternExplanation => Get("PatternExplanation");
    public string AddAtLeastOneBand => Get("AddAtLeastOneBand");
    public string AddNewAntenna => Get("AddNewAntenna");
    public string EditAntenna => Get("EditAntenna");
    public string AntennaRepository => Get("AntennaRepository");
    public string SelectAntennaPrompt => Get("SelectAntennaPrompt");
    public string TypeToSearch => Get("TypeToSearch");
    public string HorizontallyRotatable => Get("HorizontallyRotatable");
    public string TypeToSearchDescription => Get("TypeToSearchDescription");
    public string ManufacturerExample => Get("ManufacturerExample");
    public string ModelExample => Get("ModelExample");
    public string SaveAntenna => Get("SaveAntenna");
    public string AntennaMfgExample => Get("AntennaMfgExample");
    public string AntennaModelExample => Get("AntennaModelExample");

    // ============================================================
    // CABLE EDITOR
    // ============================================================
    public string CableDetails => Get("CableDetails");
    public string CableName => Get("CableName");
    public string AttenuationData => Get("AttenuationData");
    public string AttenuationHint => Get("AttenuationHint");
    public string HfBands => Get("HfBands");
    public string VhfUhfShfBands => Get("VhfUhfShfBands");
    public string AddNewCable => Get("AddNewCable");
    public string EditCable => Get("EditCable");
    public string CableNameExample => Get("CableNameExample");

    // ============================================================
    // RADIO EDITOR
    // ============================================================
    public string RadioDetails => Get("RadioDetails");
    public string MaxPower => Get("MaxPower");
    public string MaxPowerHint => Get("MaxPowerHint");
    public string RadioNote => Get("RadioNote");
    public string AddNewRadio => Get("AddNewRadio");
    public string EditRadio => Get("EditRadio");
    public string RadioManufacturerExample => Get("RadioManufacturerExample");
    public string RadioModelExample => Get("RadioModelExample");
    public string Note => Get("Note");

    // ============================================================
    // MASTER DATA MANAGER
    // ============================================================
    public string Antennas => Get("Antennas");
    public string Cables => Get("Cables");
    public string Radios => Get("Radios");
    public string Search => Get("Search");
    public string Translations => Get("Translations");
    public string MasterDataManager => Get("MasterDataManager");
    public string Clear => Get("Clear");
    public string SearchAntennas => Get("SearchAntennas");
    public string SearchCables => Get("SearchCables");
    public string SearchRadios => Get("SearchRadios");
    public string SearchTranslations => Get("SearchTranslations");
    public string AddAntennaButton => Get("AddAntennaButton");
    public string AddCableButton => Get("AddCableButton");
    public string AddRadioButton => Get("AddRadioButton");
    public string BackToWelcome => Get("BackToWelcome");
    public string SaveChanges => Get("SaveChanges");
    public string TranslationEditorInfo => Get("TranslationEditorInfo");
    public string ColumnKey => Get("ColumnKey");
    public string ColumnCategory => Get("ColumnCategory");
    public string ColumnGerman => Get("ColumnGerman");
    public string ColumnFrench => Get("ColumnFrench");
    public string ColumnItalian => Get("ColumnItalian");
    public string ColumnEnglish => Get("ColumnEnglish");
    public string BandsCount => Get("BandsCount");
    public string FrequencyPoints => Get("FrequencyPoints");
    public string MaxWatts => Get("MaxWatts");

    // ============================================================
    // RESULTS
    // ============================================================
    public string Results => Get("Results");
    public string CalculationResults => Get("CalculationResults");
    public string Pass => Get("Pass");
    public string Fail => Get("Fail");
    public string FieldStrength => Get("FieldStrength");
    public string SafetyDistance => Get("SafetyDistance");
    public string Limit => Get("Limit");
    public string ExportMarkdown => Get("ExportMarkdown");
    public string FreqHeader => Get("FreqHeader");
    public string GainHeader => Get("GainHeader");
    public string PmHeader => Get("PmHeader");
    public string FieldHeader => Get("FieldHeader");
    public string SafetyHeader => Get("SafetyHeader");
    public string StatusHeader => Get("StatusHeader");
    public string DampingLabel => Get("DampingLabel");

    // ============================================================
    // VALIDATION MESSAGES
    // ============================================================
    public string ValidationRequired => Get("ValidationRequired");
    public string ValidationManufacturer => Get("ValidationManufacturer");
    public string ValidationModel => Get("ValidationModel");
    public string ValidationCableName => Get("ValidationCableName");
    public string ValidationGainRange => Get("ValidationGainRange");
    public string ValidationPatternRange => Get("ValidationPatternRange");
    public string ValidationRotationRange => Get("ValidationRotationRange");
    public string ValidationPowerRange => Get("ValidationPowerRange");
    public string ValidationAtLeastOneBand => Get("ValidationAtLeastOneBand");

    // ============================================================
    // DIALOGS
    // ============================================================
    public string UnsavedChanges => Get("UnsavedChanges");
    public string SaveChangesPrompt => Get("SaveChangesPrompt");
    public string DiscardChanges => Get("DiscardChanges");
    public string DiscardChangesPrompt => Get("DiscardChangesPrompt");
    public string ChangesDiscarded => Get("ChangesDiscarded");

    // ============================================================
    // UNITS (not translated, but centralized)
    // ============================================================
    public string UnitMeter => "m";
    public string UnitWatt => "W";
    public string UnitDb => "dB";
    public string UnitDbi => "dBi";
    public string UnitMhz => "MHz";
    public string UnitDegree => "°";

    // ============================================================
    // RUNTIME TRANSLATION API
    // ============================================================

    /// <summary>
    /// Gets all translations with their categories for the translation editor.
    /// </summary>
    public static IEnumerable<(string Key, string Category, Dictionary<string, string> Values)> GetAllTranslations()
    {
        return TranslationData.Select(kvp => (
            Key: kvp.Key,
            Category: Categories.GetValueOrDefault(kvp.Key, "Other"),
            Values: new Dictionary<string, string>(kvp.Value)
        ));
    }

    /// <summary>
    /// Updates a translation value at runtime.
    /// </summary>
    public static void UpdateTranslation(string key, string language, string value)
    {
        if (TranslationData.TryGetValue(key, out var translations))
        {
            translations[language] = value;
        }
        else
        {
            TranslationData[key] = new Dictionary<string, string> { [language] = value };
        }
        // Notify UI to refresh
        Instance.PropertyChanged?.Invoke(Instance, new PropertyChangedEventArgs(null));
    }

    /// <summary>
    /// Adds a new translation key.
    /// </summary>
    public static void AddTranslation(string key, string category, Dictionary<string, string> values)
    {
        TranslationData[key] = new Dictionary<string, string>(values);
        Categories[key] = category;
        Instance.PropertyChanged?.Invoke(Instance, new PropertyChangedEventArgs(null));
    }

    // ============================================================
    // CATEGORIES MAPPING
    // ============================================================
    private static readonly Dictionary<string, string> Categories = new()
    {
        // Common
        ["Save"] = "Common", ["SaveAs"] = "Common", ["Cancel"] = "Common", ["Edit"] = "Common", ["New"] = "Common",
        ["Delete"] = "Common", ["Add"] = "Common", ["Remove"] = "Common", ["Back"] = "Common",
        ["Close"] = "Common", ["Calculate"] = "Common", ["Export"] = "Common", ["Yes"] = "Common", ["No"] = "Common",

        // Welcome
        ["AppTitle"] = "Welcome", ["AppSubtitle"] = "Welcome", ["SelectLanguage"] = "Welcome",
        ["Theme"] = "Welcome", ["ThemeLight"] = "Welcome", ["ThemeDark"] = "Welcome",
        ["NewProject"] = "Welcome", ["OpenProject"] = "Welcome", ["MasterData"] = "Welcome", ["NisvCompliance"] = "Welcome",

        // ProjectInfo
        ["StationInfo"] = "ProjectInfo", ["Callsign"] = "ProjectInfo", ["Address"] = "ProjectInfo",
        ["CreateProject"] = "ProjectInfo", ["EditStationInfo"] = "ProjectInfo",
        ["CallsignExample"] = "ProjectInfo", ["AddressExample"] = "ProjectInfo",

        // ProjectOverview
        ["Configurations"] = "ProjectOverview", ["AddConfiguration"] = "ProjectOverview",
        ["CalculateAll"] = "ProjectOverview", ["ExportReport"] = "ProjectOverview", ["NoConfigurations"] = "ProjectOverview",

        // ConfigEditor
        ["Configuration"] = "ConfigEditor", ["Antenna"] = "ConfigEditor", ["Transmitter"] = "ConfigEditor",
        ["FeedLine"] = "ConfigEditor", ["OperatingParameters"] = "ConfigEditor", ["EvaluationPoint"] = "ConfigEditor",
        ["SelectAntenna"] = "ConfigEditor", ["Height"] = "ConfigEditor", ["HeightUnit"] = "ConfigEditor", ["FrequencyBands"] = "ConfigEditor",
        ["Radio"] = "ConfigEditor", ["SelectRadio"] = "ConfigEditor", ["UseLinearAmplifier"] = "ConfigEditor",
        ["Linear"] = "ConfigEditor", ["Power"] = "ConfigEditor", ["PowerUnit"] = "ConfigEditor",
        ["Cable"] = "ConfigEditor", ["SelectCable"] = "ConfigEditor", ["CableLength"] = "ConfigEditor",
        ["AdditionalLoss"] = "ConfigEditor", ["LossDescription"] = "ConfigEditor",
        ["Modulation"] = "ConfigEditor", ["ActivityFactor"] = "ConfigEditor", ["ActivityFactorHint"] = "ConfigEditor",
        ["ParametersApplyToAllBands"] = "ConfigEditor",
        ["OkaName"] = "ConfigEditor", ["OkaDistance"] = "ConfigEditor", ["OkaDamping"] = "ConfigEditor",
        ["OkaDampingHint"] = "ConfigEditor", ["OkaExplanation"] = "ConfigEditor",
        ["AdditionalLossExample"] = "ConfigEditor", ["OkaNameExample"] = "ConfigEditor",

        // AntennaEditor
        ["AntennaDetails"] = "AntennaEditor", ["Manufacturer"] = "AntennaEditor", ["Model"] = "AntennaEditor",
        ["Polarization"] = "AntennaEditor", ["Horizontal"] = "AntennaEditor", ["Vertical"] = "AntennaEditor",
        ["Rotatable"] = "AntennaEditor", ["RotationAngle"] = "AntennaEditor", ["DegreeInfoOnly"] = "AntennaEditor",
        ["Frequency"] = "AntennaEditor", ["Gain"] = "AntennaEditor", ["GainRange"] = "AntennaEditor",
        ["AddBand"] = "AntennaEditor", ["VerticalRadiationPattern"] = "AntennaEditor", ["PatternExplanation"] = "AntennaEditor",
        ["AddAtLeastOneBand"] = "AntennaEditor", ["AddNewAntenna"] = "AntennaEditor", ["EditAntenna"] = "AntennaEditor",
        ["AntennaRepository"] = "AntennaEditor", ["SelectAntennaPrompt"] = "AntennaEditor", ["TypeToSearch"] = "AntennaEditor",
        ["HorizontallyRotatable"] = "AntennaEditor", ["TypeToSearchDescription"] = "AntennaEditor",
        ["ManufacturerExample"] = "AntennaEditor", ["ModelExample"] = "AntennaEditor", ["SaveAntenna"] = "AntennaEditor",
        ["AntennaMfgExample"] = "AntennaEditor", ["AntennaModelExample"] = "AntennaEditor",

        // CableEditor
        ["CableDetails"] = "CableEditor", ["CableName"] = "CableEditor", ["AttenuationData"] = "CableEditor",
        ["AttenuationHint"] = "CableEditor", ["HfBands"] = "CableEditor", ["VhfUhfShfBands"] = "CableEditor",
        ["AddNewCable"] = "CableEditor", ["EditCable"] = "CableEditor", ["CableNameExample"] = "CableEditor",

        // RadioEditor
        ["RadioDetails"] = "RadioEditor", ["MaxPower"] = "RadioEditor", ["MaxPowerHint"] = "RadioEditor",
        ["RadioNote"] = "RadioEditor", ["AddNewRadio"] = "RadioEditor", ["EditRadio"] = "RadioEditor",
        ["RadioManufacturerExample"] = "RadioEditor", ["RadioModelExample"] = "RadioEditor", ["Note"] = "RadioEditor",

        // MasterData
        ["Antennas"] = "MasterData", ["Cables"] = "MasterData", ["Radios"] = "MasterData", ["Search"] = "MasterData",
        ["Translations"] = "MasterData", ["MasterDataManager"] = "MasterData", ["Clear"] = "MasterData",
        ["SearchAntennas"] = "MasterData", ["SearchCables"] = "MasterData", ["SearchRadios"] = "MasterData",
        ["SearchTranslations"] = "MasterData", ["AddAntennaButton"] = "MasterData", ["AddCableButton"] = "MasterData",
        ["AddRadioButton"] = "MasterData", ["BackToWelcome"] = "MasterData", ["SaveChanges"] = "MasterData",
        ["TranslationEditorInfo"] = "MasterData",

        // Results
        ["Results"] = "Results", ["CalculationResults"] = "Results", ["Pass"] = "Results", ["Fail"] = "Results",
        ["FieldStrength"] = "Results", ["SafetyDistance"] = "Results", ["Limit"] = "Results", ["ExportMarkdown"] = "Results",
        ["FreqHeader"] = "Results", ["GainHeader"] = "Results", ["PmHeader"] = "Results", ["FieldHeader"] = "Results",
        ["SafetyHeader"] = "Results", ["StatusHeader"] = "Results", ["DampingLabel"] = "Results",

        // Validation
        ["ValidationRequired"] = "Validation", ["ValidationManufacturer"] = "Validation", ["ValidationModel"] = "Validation",
        ["ValidationCableName"] = "Validation", ["ValidationGainRange"] = "Validation", ["ValidationPatternRange"] = "Validation",
        ["ValidationRotationRange"] = "Validation", ["ValidationPowerRange"] = "Validation", ["ValidationAtLeastOneBand"] = "Validation",

        // Dialogs
        ["UnsavedChanges"] = "Dialogs", ["SaveChangesPrompt"] = "Dialogs",
    };

    // ============================================================
    // TRANSLATIONS DICTIONARY
    // ============================================================
    private static readonly Dictionary<string, Dictionary<string, string>> TranslationData = new()
    {
        // Common
        ["Save"] = new() { ["de"] = "Speichern", ["fr"] = "Enregistrer", ["it"] = "Salva", ["en"] = "Save" },
        ["SaveAs"] = new() { ["de"] = "Speichern unter...", ["fr"] = "Enregistrer sous...", ["it"] = "Salva con nome...", ["en"] = "Save As..." },
        ["Cancel"] = new() { ["de"] = "Abbrechen", ["fr"] = "Annuler", ["it"] = "Annulla", ["en"] = "Cancel" },
        ["Edit"] = new() { ["de"] = "Bearbeiten", ["fr"] = "Modifier", ["it"] = "Modifica", ["en"] = "Edit" },
        ["New"] = new() { ["de"] = "Neu", ["fr"] = "Nouveau", ["it"] = "Nuovo", ["en"] = "New" },
        ["Delete"] = new() { ["de"] = "Löschen", ["fr"] = "Supprimer", ["it"] = "Elimina", ["en"] = "Delete" },
        ["Add"] = new() { ["de"] = "Hinzufügen", ["fr"] = "Ajouter", ["it"] = "Aggiungi", ["en"] = "Add" },
        ["Remove"] = new() { ["de"] = "Entfernen", ["fr"] = "Retirer", ["it"] = "Rimuovi", ["en"] = "Remove" },
        ["Back"] = new() { ["de"] = "Zurück", ["fr"] = "Retour", ["it"] = "Indietro", ["en"] = "Back" },
        ["Close"] = new() { ["de"] = "Schliessen", ["fr"] = "Fermer", ["it"] = "Chiudi", ["en"] = "Close" },
        ["Calculate"] = new() { ["de"] = "Berechnen", ["fr"] = "Calculer", ["it"] = "Calcola", ["en"] = "Calculate" },
        ["Export"] = new() { ["de"] = "Exportieren", ["fr"] = "Exporter", ["it"] = "Esporta", ["en"] = "Export" },
        ["Yes"] = new() { ["de"] = "Ja", ["fr"] = "Oui", ["it"] = "Sì", ["en"] = "Yes" },
        ["No"] = new() { ["de"] = "Nein", ["fr"] = "Non", ["it"] = "No", ["en"] = "No" },

        // Welcome
        ["AppTitle"] = new() { ["de"] = "Swiss NIS Calculator", ["fr"] = "Swiss NIS Calculator", ["it"] = "Swiss NIS Calculator", ["en"] = "Swiss NIS Calculator" },
        ["AppSubtitle"] = new() { ["de"] = "Feldstärkerechner für Amateurfunk", ["fr"] = "Calculateur de champ RF pour radioamateurs", ["it"] = "Calcolatore campo RF per radioamatori", ["en"] = "RF Field Strength Calculator for Amateur Radio" },
        ["SelectLanguage"] = new() { ["de"] = "Sprache wählen", ["fr"] = "Choisir la langue", ["it"] = "Seleziona lingua", ["en"] = "Select Language" },
        ["Theme"] = new() { ["de"] = "Design", ["fr"] = "Thème", ["it"] = "Tema", ["en"] = "Theme" },
        ["ThemeLight"] = new() { ["de"] = "Hell", ["fr"] = "Clair", ["it"] = "Chiaro", ["en"] = "Light" },
        ["ThemeDark"] = new() { ["de"] = "Dunkel", ["fr"] = "Sombre", ["it"] = "Scuro", ["en"] = "Dark" },
        ["NewProject"] = new() { ["de"] = "Neues Projekt", ["fr"] = "Nouveau projet", ["it"] = "Nuovo progetto", ["en"] = "New Project" },
        ["OpenProject"] = new() { ["de"] = "Projekt öffnen...", ["fr"] = "Ouvrir projet...", ["it"] = "Apri progetto...", ["en"] = "Open Project..." },
        ["MasterData"] = new() { ["de"] = "Stammdaten", ["fr"] = "Données de base", ["it"] = "Dati master", ["en"] = "Master Data" },
        ["NisvCompliance"] = new() { ["de"] = "NISV-Konformitätsrechner für Schweizer Amateurfunkstationen", ["fr"] = "Calculateur de conformité ORNI pour stations radioamateurs suisses", ["it"] = "Calcolatore conformità ORNI per stazioni radioamatoriali svizzere", ["en"] = "NISV Compliance Calculator for Swiss Amateur Radio Stations" },

        // Project Info
        ["StationInfo"] = new() { ["de"] = "Stationsinformationen", ["fr"] = "Informations station", ["it"] = "Informazioni stazione", ["en"] = "Station Information" },
        ["Callsign"] = new() { ["de"] = "Rufzeichen", ["fr"] = "Indicatif", ["it"] = "Nominativo", ["en"] = "Callsign" },
        ["Address"] = new() { ["de"] = "Adresse", ["fr"] = "Adresse", ["it"] = "Indirizzo", ["en"] = "Address" },
        ["CreateProject"] = new() { ["de"] = "Projekt erstellen", ["fr"] = "Créer projet", ["it"] = "Crea progetto", ["en"] = "Create Project" },
        ["EditStationInfo"] = new() { ["de"] = "Stationsinfo bearbeiten", ["fr"] = "Modifier info station", ["it"] = "Modifica info stazione", ["en"] = "Edit Station Info" },
        ["CallsignExample"] = new() { ["de"] = "z.B. HB9FS", ["fr"] = "ex. HB9FS", ["it"] = "es. HB9FS", ["en"] = "e.g. HB9FS" },
        ["AddressExample"] = new() { ["de"] = "z.B. Musterstrasse 1, 8000 Zürich", ["fr"] = "ex. Rue Exemple 1, 1000 Lausanne", ["it"] = "es. Via Esempio 1, 6500 Bellinzona", ["en"] = "e.g. Example Street 1, 8000 Zurich" },

        // Project Overview
        ["Configurations"] = new() { ["de"] = "Konfigurationen", ["fr"] = "Configurations", ["it"] = "Configurazioni", ["en"] = "Configurations" },
        ["AddConfiguration"] = new() { ["de"] = "+ Konfiguration hinzufügen", ["fr"] = "+ Ajouter configuration", ["it"] = "+ Aggiungi configurazione", ["en"] = "+ Add Configuration" },
        ["CalculateAll"] = new() { ["de"] = "Alle berechnen", ["fr"] = "Tout calculer", ["it"] = "Calcola tutto", ["en"] = "Calculate All" },
        ["ExportReport"] = new() { ["de"] = "Bericht exportieren", ["fr"] = "Exporter rapport", ["it"] = "Esporta rapporto", ["en"] = "Export Report" },
        ["NoConfigurations"] = new() { ["de"] = "Keine Konfigurationen. Klicken Sie auf '+ Konfiguration hinzufügen'.", ["fr"] = "Aucune configuration. Cliquez sur '+ Ajouter configuration'.", ["it"] = "Nessuna configurazione. Clicca su '+ Aggiungi configurazione'.", ["en"] = "No configurations. Click '+ Add Configuration' to get started." },

        // Configuration Editor
        ["Configuration"] = new() { ["de"] = "Konfiguration", ["fr"] = "Configuration", ["it"] = "Configurazione", ["en"] = "Configuration" },
        ["Antenna"] = new() { ["de"] = "Antenne", ["fr"] = "Antenne", ["it"] = "Antenna", ["en"] = "Antenna" },
        ["Transmitter"] = new() { ["de"] = "Sender", ["fr"] = "Émetteur", ["it"] = "Trasmettitore", ["en"] = "Transmitter" },
        ["FeedLine"] = new() { ["de"] = "Speiseleitung", ["fr"] = "Ligne d'alimentation", ["it"] = "Linea di alimentazione", ["en"] = "Feed Line" },
        ["OperatingParameters"] = new() { ["de"] = "Betriebsparameter", ["fr"] = "Paramètres d'exploitation", ["it"] = "Parametri operativi", ["en"] = "Operating Parameters" },
        ["EvaluationPoint"] = new() { ["de"] = "Beurteilungspunkt (OKA)", ["fr"] = "Point d'évaluation (OKA)", ["it"] = "Punto di valutazione (OKA)", ["en"] = "Evaluation Point (OKA)" },

        ["SelectAntenna"] = new() { ["de"] = "-- Antenne wählen --", ["fr"] = "-- Sélectionner antenne --", ["it"] = "-- Seleziona antenna --", ["en"] = "-- Select Antenna --" },
        ["Height"] = new() { ["de"] = "Höhe", ["fr"] = "Hauteur", ["it"] = "Altezza", ["en"] = "Height" },
        ["HeightUnit"] = new() { ["de"] = "m", ["fr"] = "m", ["it"] = "m", ["en"] = "m" },
        ["FrequencyBands"] = new() { ["de"] = "Frequenzbänder", ["fr"] = "Bandes de fréquence", ["it"] = "Bande di frequenza", ["en"] = "Frequency Bands" },

        ["Radio"] = new() { ["de"] = "Transceiver", ["fr"] = "Émetteur-récepteur", ["it"] = "Ricetrasmettitore", ["en"] = "Radio" },
        ["SelectRadio"] = new() { ["de"] = "-- Transceiver wählen --", ["fr"] = "-- Sélectionner émetteur --", ["it"] = "-- Seleziona ricetrasmettitore --", ["en"] = "-- Select Radio --" },
        ["UseLinearAmplifier"] = new() { ["de"] = "Linearendstufe verwenden", ["fr"] = "Utiliser amplificateur linéaire", ["it"] = "Usa amplificatore lineare", ["en"] = "Use Linear Amplifier" },
        ["Linear"] = new() { ["de"] = "Linear", ["fr"] = "Linéaire", ["it"] = "Lineare", ["en"] = "Linear" },
        ["Power"] = new() { ["de"] = "Leistung", ["fr"] = "Puissance", ["it"] = "Potenza", ["en"] = "Power" },
        ["PowerUnit"] = new() { ["de"] = "W", ["fr"] = "W", ["it"] = "W", ["en"] = "W" },

        ["Cable"] = new() { ["de"] = "Kabel", ["fr"] = "Câble", ["it"] = "Cavo", ["en"] = "Cable" },
        ["SelectCable"] = new() { ["de"] = "-- Kabel wählen --", ["fr"] = "-- Sélectionner câble --", ["it"] = "-- Seleziona cavo --", ["en"] = "-- Select Cable --" },
        ["CableLength"] = new() { ["de"] = "Kabellänge", ["fr"] = "Longueur câble", ["it"] = "Lunghezza cavo", ["en"] = "Cable Length" },
        ["AdditionalLoss"] = new() { ["de"] = "Zusätzliche Dämpfung", ["fr"] = "Pertes supplémentaires", ["it"] = "Perdite aggiuntive", ["en"] = "Additional Loss" },
        ["LossDescription"] = new() { ["de"] = "Beschreibung", ["fr"] = "Description", ["it"] = "Descrizione", ["en"] = "Description" },

        ["Modulation"] = new() { ["de"] = "Modulation", ["fr"] = "Modulation", ["it"] = "Modulazione", ["en"] = "Modulation" },
        ["ActivityFactor"] = new() { ["de"] = "Aktivitätsfaktor", ["fr"] = "Facteur d'activité", ["it"] = "Fattore di attività", ["en"] = "Activity Factor" },
        ["ActivityFactorHint"] = new() { ["de"] = "(Standard 0.5)", ["fr"] = "(défaut 0.5)", ["it"] = "(default 0.5)", ["en"] = "(default 0.5)" },
        ["ParametersApplyToAllBands"] = new() { ["de"] = "Diese Parameter gelten für alle Bänder der gewählten Antenne.", ["fr"] = "Ces paramètres s'appliquent à toutes les bandes de l'antenne sélectionnée.", ["it"] = "Questi parametri si applicano a tutte le bande dell'antenna selezionata.", ["en"] = "These parameters apply to all bands of the selected antenna." },

        ["OkaName"] = new() { ["de"] = "Name", ["fr"] = "Nom", ["it"] = "Nome", ["en"] = "Name" },
        ["OkaDistance"] = new() { ["de"] = "Distanz", ["fr"] = "Distance", ["it"] = "Distanza", ["en"] = "Distance" },
        ["OkaDamping"] = new() { ["de"] = "Gebäudedämpfung", ["fr"] = "Atténuation bâtiment", ["it"] = "Attenuazione edificio", ["en"] = "Building Damping" },
        ["OkaDampingHint"] = new() { ["de"] = "dB (Gebäudedämpfung)", ["fr"] = "dB (atténuation bâtiment)", ["it"] = "dB (attenuazione edificio)", ["en"] = "dB (building attenuation)" },
        ["OkaExplanation"] = new() { ["de"] = "OKA = Ort des kurzfristigen Aufenthalts", ["fr"] = "OKA = Lieu de séjour de courte durée", ["it"] = "OKA = Luogo di soggiorno breve", ["en"] = "OKA = Place of short-term stay" },
        ["AdditionalLossExample"] = new() { ["de"] = "z.B. Stecker, Schalter", ["fr"] = "ex. Connecteurs, commutateur", ["it"] = "es. Connettori, interruttore", ["en"] = "e.g. Connectors, switch" },
        ["OkaNameExample"] = new() { ["de"] = "z.B. Balkon des Nachbarn", ["fr"] = "ex. Balcon du voisin", ["it"] = "es. Balcone del vicino", ["en"] = "e.g. Neighbor's balcony" },

        // Antenna Editor
        ["AntennaDetails"] = new() { ["de"] = "Antennendetails", ["fr"] = "Détails antenne", ["it"] = "Dettagli antenna", ["en"] = "Antenna Details" },
        ["Manufacturer"] = new() { ["de"] = "Hersteller", ["fr"] = "Fabricant", ["it"] = "Produttore", ["en"] = "Manufacturer" },
        ["Model"] = new() { ["de"] = "Modell", ["fr"] = "Modèle", ["it"] = "Modello", ["en"] = "Model" },
        ["Polarization"] = new() { ["de"] = "Polarisation", ["fr"] = "Polarisation", ["it"] = "Polarizzazione", ["en"] = "Polarization" },
        ["Horizontal"] = new() { ["de"] = "Horizontal", ["fr"] = "Horizontal", ["it"] = "Orizzontale", ["en"] = "Horizontal" },
        ["Vertical"] = new() { ["de"] = "Vertikal", ["fr"] = "Vertical", ["it"] = "Verticale", ["en"] = "Vertical" },
        ["Rotatable"] = new() { ["de"] = "Drehbar", ["fr"] = "Rotatif", ["it"] = "Rotabile", ["en"] = "Rotatable" },
        ["RotationAngle"] = new() { ["de"] = "Drehwinkel", ["fr"] = "Angle de rotation", ["it"] = "Angolo di rotazione", ["en"] = "Rotation Angle" },
        ["DegreeInfoOnly"] = new() { ["de"] = "Grad (nur Info)", ["fr"] = "degrés (info seulement)", ["it"] = "gradi (solo info)", ["en"] = "deg (info only)" },
        ["Frequency"] = new() { ["de"] = "Frequenz", ["fr"] = "Fréquence", ["it"] = "Frequenza", ["en"] = "Frequency" },
        ["Gain"] = new() { ["de"] = "Gewinn", ["fr"] = "Gain", ["it"] = "Guadagno", ["en"] = "Gain" },
        ["GainRange"] = new() { ["de"] = "dBi (-20 bis 50)", ["fr"] = "dBi (-20 à 50)", ["it"] = "dBi (-20 a 50)", ["en"] = "dBi (-20 to 50)" },
        ["AddBand"] = new() { ["de"] = "+ Band hinzufügen", ["fr"] = "+ Ajouter bande", ["it"] = "+ Aggiungi banda", ["en"] = "+ Add Band" },
        ["VerticalRadiationPattern"] = new() { ["de"] = "Vertikales Strahlungsdiagramm (Dämpfung in dB je Winkel vom Horizont)", ["fr"] = "Diagramme de rayonnement vertical (atténuation en dB par angle depuis l'horizon)", ["it"] = "Diagramma di radiazione verticale (attenuazione in dB per angolo dall'orizzonte)", ["en"] = "Vertical Radiation Pattern (attenuation in dB at each angle from horizon)" },
        ["PatternExplanation"] = new() { ["de"] = "0° = Horizont (max. Abstrahlung), 90° = Senkrecht nach oben. Werte: 0-60 dB Dämpfung.", ["fr"] = "0° = Horizon (rayonnement max), 90° = Verticalement. Valeurs: 0-60 dB d'atténuation.", ["it"] = "0° = Orizzonte (radiazione max), 90° = Verticalmente. Valori: 0-60 dB attenuazione.", ["en"] = "0° = Horizon (max radiation), 90° = Straight up. Values: 0-60 dB attenuation." },
        ["AddAtLeastOneBand"] = new() { ["de"] = "Fügen Sie mindestens ein Frequenzband mit Gewinn und vertikalem Strahlungsdiagramm hinzu.", ["fr"] = "Ajoutez au moins une bande de fréquence avec gain et diagramme de rayonnement vertical.", ["it"] = "Aggiungi almeno una banda di frequenza con guadagno e diagramma di radiazione verticale.", ["en"] = "Add at least one frequency band with gain and vertical radiation pattern." },
        ["AddNewAntenna"] = new() { ["de"] = "Neue Antenne hinzufügen", ["fr"] = "Ajouter nouvelle antenne", ["it"] = "Aggiungi nuova antenna", ["en"] = "Add New Antenna" },
        ["EditAntenna"] = new() { ["de"] = "Antenne bearbeiten", ["fr"] = "Modifier antenne", ["it"] = "Modifica antenna", ["en"] = "Edit Antenna" },
        ["AntennaRepository"] = new() { ["de"] = "Antennen-Datenbank", ["fr"] = "Base de données antennes", ["it"] = "Database antenne", ["en"] = "Antenna Repository" },
        ["SelectAntennaPrompt"] = new() { ["de"] = "Antenne auswählen", ["fr"] = "Sélectionner antenne", ["it"] = "Seleziona antenna", ["en"] = "Select Antenna" },
        ["TypeToSearch"] = new() { ["de"] = "Eingabe zum Suchen...", ["fr"] = "Tapez pour rechercher...", ["it"] = "Digita per cercare...", ["en"] = "Type to search..." },
        ["HorizontallyRotatable"] = new() { ["de"] = "Horizontal drehbar", ["fr"] = "Rotatif horizontalement", ["it"] = "Rotabile orizzontalmente", ["en"] = "Horizontally rotatable" },
        ["TypeToSearchDescription"] = new() { ["de"] = "Suche nach Hersteller oder Modell:", ["fr"] = "Rechercher par fabricant ou modèle:", ["it"] = "Cerca per produttore o modello:", ["en"] = "Type to search by manufacturer or model:" },
        ["ManufacturerExample"] = new() { ["de"] = "z.B. Cushcraft", ["fr"] = "ex. Cushcraft", ["it"] = "es. Cushcraft", ["en"] = "e.g. Cushcraft" },
        ["ModelExample"] = new() { ["de"] = "z.B. A3S", ["fr"] = "ex. A3S", ["it"] = "es. A3S", ["en"] = "e.g. A3S" },
        ["SaveAntenna"] = new() { ["de"] = "Antenne speichern", ["fr"] = "Enregistrer antenne", ["it"] = "Salva antenna", ["en"] = "Save Antenna" },
        ["AntennaMfgExample"] = new() { ["de"] = "z.B. Fritzel", ["fr"] = "ex. Fritzel", ["it"] = "es. Fritzel", ["en"] = "e.g. Fritzel" },
        ["AntennaModelExample"] = new() { ["de"] = "z.B. FB-Do450", ["fr"] = "ex. FB-Do450", ["it"] = "es. FB-Do450", ["en"] = "e.g. FB-Do450" },

        // Cable Editor
        ["CableDetails"] = new() { ["de"] = "Kabeldetails", ["fr"] = "Détails câble", ["it"] = "Dettagli cavo", ["en"] = "Cable Details" },
        ["CableName"] = new() { ["de"] = "Kabelname", ["fr"] = "Nom du câble", ["it"] = "Nome cavo", ["en"] = "Cable Name" },
        ["AttenuationData"] = new() { ["de"] = "Dämpfungsdaten (dB pro 100m)", ["fr"] = "Données d'atténuation (dB par 100m)", ["it"] = "Dati attenuazione (dB per 100m)", ["en"] = "Attenuation Data (dB per 100m)" },
        ["AttenuationHint"] = new() { ["de"] = "Geben Sie die Dämpfungswerte für die verfügbaren Frequenzen ein. Leer lassen wenn unbekannt.", ["fr"] = "Entrez les valeurs d'atténuation pour les fréquences disponibles. Laissez vide si inconnu.", ["it"] = "Inserisci i valori di attenuazione per le frequenze disponibili. Lascia vuoto se sconosciuto.", ["en"] = "Enter attenuation values for the frequencies you have data for. Leave empty if unknown." },
        ["HfBands"] = new() { ["de"] = "KW-Bänder", ["fr"] = "Bandes HF", ["it"] = "Bande HF", ["en"] = "HF Bands" },
        ["VhfUhfShfBands"] = new() { ["de"] = "VHF / UHF / SHF Bänder", ["fr"] = "Bandes VHF / UHF / SHF", ["it"] = "Bande VHF / UHF / SHF", ["en"] = "VHF / UHF / SHF Bands" },
        ["AddNewCable"] = new() { ["de"] = "Neues Kabel hinzufügen", ["fr"] = "Ajouter nouveau câble", ["it"] = "Aggiungi nuovo cavo", ["en"] = "Add New Cable" },
        ["EditCable"] = new() { ["de"] = "Kabel bearbeiten", ["fr"] = "Modifier câble", ["it"] = "Modifica cavo", ["en"] = "Edit Cable" },
        ["CableNameExample"] = new() { ["de"] = "z.B. EcoFlex10, RG-213", ["fr"] = "ex. EcoFlex10, RG-213", ["it"] = "es. EcoFlex10, RG-213", ["en"] = "e.g. EcoFlex10, RG-213" },

        // Radio Editor
        ["RadioDetails"] = new() { ["de"] = "Transceiver-Details", ["fr"] = "Détails émetteur-récepteur", ["it"] = "Dettagli ricetrasmettitore", ["en"] = "Radio / Transceiver Details" },
        ["MaxPower"] = new() { ["de"] = "Max. Leistung", ["fr"] = "Puissance max.", ["it"] = "Potenza max.", ["en"] = "Max Power" },
        ["MaxPowerHint"] = new() { ["de"] = "W (maximale Ausgangsleistung)", ["fr"] = "W (puissance de sortie maximale)", ["it"] = "W (potenza di uscita massima)", ["en"] = "W (maximum output power)" },
        ["RadioNote"] = new() { ["de"] = "Der Transceiver dient nur zur Dokumentation im NIS-Bericht. Die effektive Ausgangsleistung wird in jeder Antennenkonfiguration angegeben.", ["fr"] = "L'émetteur-récepteur sert uniquement à la documentation dans le rapport NIS. La puissance de sortie effective est spécifiée dans chaque configuration d'antenne.", ["it"] = "Il ricetrasmettitore serve solo per la documentazione nel rapporto NIS. La potenza di uscita effettiva è specificata in ogni configurazione antenna.", ["en"] = "The radio is used for documentation purposes in the NIS report. The actual output power is specified in each antenna configuration." },
        ["AddNewRadio"] = new() { ["de"] = "Neuen Transceiver hinzufügen", ["fr"] = "Ajouter nouvel émetteur-récepteur", ["it"] = "Aggiungi nuovo ricetrasmettitore", ["en"] = "Add New Radio" },
        ["EditRadio"] = new() { ["de"] = "Transceiver bearbeiten", ["fr"] = "Modifier émetteur-récepteur", ["it"] = "Modifica ricetrasmettitore", ["en"] = "Edit Radio" },
        ["RadioManufacturerExample"] = new() { ["de"] = "z.B. Yaesu, Icom, Kenwood", ["fr"] = "ex. Yaesu, Icom, Kenwood", ["it"] = "es. Yaesu, Icom, Kenwood", ["en"] = "e.g. Yaesu, Icom, Kenwood" },
        ["RadioModelExample"] = new() { ["de"] = "z.B. FT-1000, IC-7300", ["fr"] = "ex. FT-1000, IC-7300", ["it"] = "es. FT-1000, IC-7300", ["en"] = "e.g. FT-1000, IC-7300" },
        ["Note"] = new() { ["de"] = "Hinweis", ["fr"] = "Note", ["it"] = "Nota", ["en"] = "Note" },

        // Master Data Manager
        ["Antennas"] = new() { ["de"] = "Antennen", ["fr"] = "Antennes", ["it"] = "Antenne", ["en"] = "Antennas" },
        ["Cables"] = new() { ["de"] = "Kabel", ["fr"] = "Câbles", ["it"] = "Cavi", ["en"] = "Cables" },
        ["Radios"] = new() { ["de"] = "Transceiver", ["fr"] = "Émetteurs-récepteurs", ["it"] = "Ricetrasmettitori", ["en"] = "Radios" },
        ["Search"] = new() { ["de"] = "Suchen...", ["fr"] = "Rechercher...", ["it"] = "Cerca...", ["en"] = "Search..." },
        ["Translations"] = new() { ["de"] = "Übersetzungen", ["fr"] = "Traductions", ["it"] = "Traduzioni", ["en"] = "Translations" },
        ["MasterDataManager"] = new() { ["de"] = "Stammdaten-Verwaltung", ["fr"] = "Gestion des données de base", ["it"] = "Gestione dati master", ["en"] = "Master Data Manager" },
        ["Clear"] = new() { ["de"] = "Löschen", ["fr"] = "Effacer", ["it"] = "Cancella", ["en"] = "Clear" },
        ["SearchAntennas"] = new() { ["de"] = "Hersteller oder Modell suchen...", ["fr"] = "Rechercher fabricant ou modèle...", ["it"] = "Cerca produttore o modello...", ["en"] = "Search by manufacturer or model..." },
        ["SearchCables"] = new() { ["de"] = "Kabelname suchen...", ["fr"] = "Rechercher nom du câble...", ["it"] = "Cerca nome cavo...", ["en"] = "Search by cable name..." },
        ["SearchRadios"] = new() { ["de"] = "Hersteller oder Modell suchen...", ["fr"] = "Rechercher fabricant ou modèle...", ["it"] = "Cerca produttore o modello...", ["en"] = "Search by manufacturer or model..." },
        ["SearchTranslations"] = new() { ["de"] = "Übersetzungen suchen...", ["fr"] = "Rechercher traductions...", ["it"] = "Cerca traduzioni...", ["en"] = "Search translations..." },
        ["AddAntennaButton"] = new() { ["de"] = "+ Antenne hinzufügen", ["fr"] = "+ Ajouter antenne", ["it"] = "+ Aggiungi antenna", ["en"] = "+ Add Antenna" },
        ["AddCableButton"] = new() { ["de"] = "+ Kabel hinzufügen", ["fr"] = "+ Ajouter câble", ["it"] = "+ Aggiungi cavo", ["en"] = "+ Add Cable" },
        ["AddRadioButton"] = new() { ["de"] = "+ Transceiver hinzufügen", ["fr"] = "+ Ajouter émetteur", ["it"] = "+ Aggiungi ricetrasmettitore", ["en"] = "+ Add Radio" },
        ["BackToWelcome"] = new() { ["de"] = "Zurück zur Startseite", ["fr"] = "Retour à l'accueil", ["it"] = "Torna alla home", ["en"] = "Back to Welcome" },
        ["SaveChanges"] = new() { ["de"] = "Änderungen speichern", ["fr"] = "Enregistrer les modifications", ["it"] = "Salva modifiche", ["en"] = "Save Changes" },
        ["TranslationEditorInfo"] = new() { ["de"] = "Übersetzungen direkt bearbeiten. Änderungen werden sofort wirksam. Klicken Sie auf Speichern, um die Änderungen dauerhaft zu speichern.", ["fr"] = "Modifiez les traductions directement. Les modifications prennent effet immédiatement. Cliquez sur Enregistrer pour conserver les modifications.", ["it"] = "Modifica le traduzioni direttamente. Le modifiche hanno effetto immediato. Clicca su Salva per rendere permanenti le modifiche.", ["en"] = "Edit translations directly. Changes take effect immediately. Click Save to persist changes across restarts." },
        ["ColumnKey"] = new() { ["de"] = "Schlüssel", ["fr"] = "Clé", ["it"] = "Chiave", ["en"] = "Key" },
        ["ColumnCategory"] = new() { ["de"] = "Kategorie", ["fr"] = "Catégorie", ["it"] = "Categoria", ["en"] = "Category" },
        ["ColumnGerman"] = new() { ["de"] = "Deutsch (DE)", ["fr"] = "Allemand (DE)", ["it"] = "Tedesco (DE)", ["en"] = "German (DE)" },
        ["ColumnFrench"] = new() { ["de"] = "Französisch (FR)", ["fr"] = "Français (FR)", ["it"] = "Francese (FR)", ["en"] = "French (FR)" },
        ["ColumnItalian"] = new() { ["de"] = "Italienisch (IT)", ["fr"] = "Italien (IT)", ["it"] = "Italiano (IT)", ["en"] = "Italian (IT)" },
        ["ColumnEnglish"] = new() { ["de"] = "Englisch (EN)", ["fr"] = "Anglais (EN)", ["it"] = "Inglese (EN)", ["en"] = "English (EN)" },
        ["BandsCount"] = new() { ["de"] = "{0} Bänder", ["fr"] = "{0} bandes", ["it"] = "{0} bande", ["en"] = "{0} bands" },
        ["FrequencyPoints"] = new() { ["de"] = "{0} Frequenzpunkte", ["fr"] = "{0} points de fréquence", ["it"] = "{0} punti di frequenza", ["en"] = "{0} frequency points" },
        ["MaxWatts"] = new() { ["de"] = "{0}W max", ["fr"] = "{0}W max", ["it"] = "{0}W max", ["en"] = "{0}W max" },

        // Results
        ["Results"] = new() { ["de"] = "Ergebnisse", ["fr"] = "Résultats", ["it"] = "Risultati", ["en"] = "Results" },
        ["CalculationResults"] = new() { ["de"] = "Berechnungsergebnisse", ["fr"] = "Résultats du calcul", ["it"] = "Risultati del calcolo", ["en"] = "Calculation Results" },
        ["Pass"] = new() { ["de"] = "Bestanden", ["fr"] = "Réussi", ["it"] = "Superato", ["en"] = "Pass" },
        ["Fail"] = new() { ["de"] = "Nicht bestanden", ["fr"] = "Échoué", ["it"] = "Non superato", ["en"] = "Fail" },
        ["FieldStrength"] = new() { ["de"] = "Feldstärke", ["fr"] = "Intensité du champ", ["it"] = "Intensità di campo", ["en"] = "Field Strength" },
        ["SafetyDistance"] = new() { ["de"] = "Sicherheitsabstand", ["fr"] = "Distance de sécurité", ["it"] = "Distanza di sicurezza", ["en"] = "Safety Distance" },
        ["Limit"] = new() { ["de"] = "Grenzwert", ["fr"] = "Limite", ["it"] = "Limite", ["en"] = "Limit" },
        ["ExportMarkdown"] = new() { ["de"] = "Markdown exportieren", ["fr"] = "Exporter Markdown", ["it"] = "Esporta Markdown", ["en"] = "Export Markdown" },
        ["FreqHeader"] = new() { ["de"] = "Freq", ["fr"] = "Fréq", ["it"] = "Freq", ["en"] = "Freq" },
        ["GainHeader"] = new() { ["de"] = "Gewinn", ["fr"] = "Gain", ["it"] = "Guad", ["en"] = "Gain" },
        ["PmHeader"] = new() { ["de"] = "Pm", ["fr"] = "Pm", ["it"] = "Pm", ["en"] = "Pm" },
        ["FieldHeader"] = new() { ["de"] = "Feld (V/m)", ["fr"] = "Champ (V/m)", ["it"] = "Campo (V/m)", ["en"] = "Field (V/m)" },
        ["SafetyHeader"] = new() { ["de"] = "Sicherh.", ["fr"] = "Sécu.", ["it"] = "Sicur.", ["en"] = "Safety" },
        ["StatusHeader"] = new() { ["de"] = "Status", ["fr"] = "Statut", ["it"] = "Stato", ["en"] = "Status" },
        ["DampingLabel"] = new() { ["de"] = "Dämpfung", ["fr"] = "Atténuation", ["it"] = "Attenuazione", ["en"] = "Damping" },

        // Validation
        ["ValidationRequired"] = new() { ["de"] = "Dieses Feld ist erforderlich.", ["fr"] = "Ce champ est requis.", ["it"] = "Questo campo è obbligatorio.", ["en"] = "This field is required." },
        ["ValidationManufacturer"] = new() { ["de"] = "Bitte geben Sie einen Hersteller ein.", ["fr"] = "Veuillez entrer un fabricant.", ["it"] = "Inserisci un produttore.", ["en"] = "Please enter a manufacturer." },
        ["ValidationModel"] = new() { ["de"] = "Bitte geben Sie ein Modell ein.", ["fr"] = "Veuillez entrer un modèle.", ["it"] = "Inserisci un modello.", ["en"] = "Please enter a model." },
        ["ValidationCableName"] = new() { ["de"] = "Bitte geben Sie einen Kabelnamen ein.", ["fr"] = "Veuillez entrer un nom de câble.", ["it"] = "Inserisci un nome per il cavo.", ["en"] = "Please enter a cable name." },
        ["ValidationGainRange"] = new() { ["de"] = "Gewinn muss zwischen -20 und 50 dBi liegen.", ["fr"] = "Le gain doit être entre -20 et 50 dBi.", ["it"] = "Il guadagno deve essere tra -20 e 50 dBi.", ["en"] = "Gain must be between -20 and 50 dBi." },
        ["ValidationPatternRange"] = new() { ["de"] = "Diagrammwerte müssen zwischen 0 und 60 dB liegen.", ["fr"] = "Les valeurs du diagramme doivent être entre 0 et 60 dB.", ["it"] = "I valori del diagramma devono essere tra 0 e 60 dB.", ["en"] = "Pattern values must be between 0 and 60 dB." },
        ["ValidationRotationRange"] = new() { ["de"] = "Drehwinkel muss zwischen 0 und 360 Grad liegen.", ["fr"] = "L'angle de rotation doit être entre 0 et 360 degrés.", ["it"] = "L'angolo di rotazione deve essere tra 0 e 360 gradi.", ["en"] = "Rotation angle must be between 0 and 360 degrees." },
        ["ValidationPowerRange"] = new() { ["de"] = "Leistung muss zwischen 1 und 10000 W liegen.", ["fr"] = "La puissance doit être entre 1 et 10000 W.", ["it"] = "La potenza deve essere tra 1 e 10000 W.", ["en"] = "Power must be between 1 and 10000 W." },
        ["ValidationAtLeastOneBand"] = new() { ["de"] = "Bitte fügen Sie mindestens ein Frequenzband hinzu.", ["fr"] = "Veuillez ajouter au moins une bande de fréquence.", ["it"] = "Aggiungi almeno una banda di frequenza.", ["en"] = "Please add at least one frequency band." },

        // Dialogs
        ["UnsavedChanges"] = new() { ["de"] = "Ungespeicherte Änderungen", ["fr"] = "Modifications non enregistrées", ["it"] = "Modifiche non salvate", ["en"] = "Unsaved Changes" },
        ["SaveChangesPrompt"] = new() { ["de"] = "Möchten Sie Ihre Änderungen vor dem Schliessen speichern?", ["fr"] = "Voulez-vous enregistrer vos modifications avant de fermer?", ["it"] = "Vuoi salvare le modifiche prima di chiudere?", ["en"] = "Do you want to save your changes before closing?" },
        ["DiscardChanges"] = new() { ["de"] = "Änderungen verwerfen?", ["fr"] = "Annuler les modifications?", ["it"] = "Annullare le modifiche?", ["en"] = "Discard Changes?" },
        ["DiscardChangesPrompt"] = new() { ["de"] = "Sie haben ungespeicherte Änderungen. Möchten Sie diese verwerfen?", ["fr"] = "Vous avez des modifications non enregistrées. Voulez-vous les annuler?", ["it"] = "Hai modifiche non salvate. Vuoi annullarle?", ["en"] = "You have unsaved changes. Do you want to discard them?" },
        ["ChangesDiscarded"] = new() { ["de"] = "Änderungen verworfen.", ["fr"] = "Modifications annulées.", ["it"] = "Modifiche annullate.", ["en"] = "Changes discarded." },
    };
}
