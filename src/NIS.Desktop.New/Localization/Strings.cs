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
                // Notify all properties have changed - raise for each bound property
                RaiseAllPropertiesChanged();
            }
        }
    }

    /// <summary>
    /// Gets the localized display name for a language code.
    /// </summary>
    public string GetLanguageName(string code) => code switch
    {
        "de" => LanguageGerman,
        "fr" => LanguageFrench,
        "it" => LanguageItalian,
        "en" => LanguageEnglish,
        _ => code
    };

    private void RaiseAllPropertiesChanged()
    {
        // Raise PropertyChanged for all localized string properties
        // This ensures Avalonia bindings update correctly
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Language)));

        // Common
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Save)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cancel)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(View)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProjectSpecific)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DuplicateNameError)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(New)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Delete)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Remove)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Back)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Close)));

        // Welcome
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AppTitle)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AppSubtitle)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectLanguage)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Theme)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ThemeLight)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ThemeDark)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ThemeSystem)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsAppearance)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsThemeDescription)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsLanguage)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsLanguageDescription)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsDisplayLanguage)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LanguageGerman)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LanguageEnglish)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LanguageFrench)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LanguageItalian)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsAbout)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsAboutVersion)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsAboutDescription)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsAboutCredits1)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsAboutCredits2)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewProject)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentProject)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OpenProject)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Continue)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GetStarted)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecentProjects)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MasterData)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NisvCompliance)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Home)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(File)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Project)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Settings)));

        // Project Info
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StationInfo)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Callsign)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Operator)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Location)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreateProject)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditStationInfo)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CallsignExample)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddressExample)));

        // Project Overview
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Configurations)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddConfiguration)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalculateAll)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExportReport)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoConfigurations)));

        // Configuration Editor
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Antenna)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Transmitter)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FeedLine)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OperatingParameters)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EvaluationPoint)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectAntenna)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FrequencyBands)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectRadio)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UseLinearAmplifier)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Power)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cable)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectCable)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CableLength)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdditionalLoss)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Modulation)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActivityFactor)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OkaDistance)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OkaDamping)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OkaExplanation)));

        // Master Data
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Antennas)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cables)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Radios)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MasterDataManager)));

        // Results
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalculationResults)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Limit)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExportMarkdown)));
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
    public string View => Get("View");
    public string ProjectSpecific => Get("ProjectSpecific");
    public string DuplicateNameError => Get("DuplicateNameError");
    public string New => Get("New");
    public string Delete => Get("Delete");
    public string Actions => Get("Actions");
    public string Remove => Get("Remove");
    public string Back => Get("Back");
    public string Close => Get("Close");

    // ============================================================
    // WELCOME SCREEN
    // ============================================================
    public string AppTitle => Get("AppTitle");
    public string AppSubtitle => Get("AppSubtitle");
    public string SelectLanguage => Get("SelectLanguage");
    public string Theme => Get("Theme");
    public string ThemeLight => Get("ThemeLight");
    public string ThemeDark => Get("ThemeDark");
    public string ThemeSystem => Get("ThemeSystem");
    public string SettingsAppearance => Get("SettingsAppearance");
    public string SettingsThemeDescription => Get("SettingsThemeDescription");
    public string SettingsLanguage => Get("SettingsLanguage");
    public string SettingsLanguageDescription => Get("SettingsLanguageDescription");
    public string SettingsDisplayLanguage => Get("SettingsDisplayLanguage");
    public string LanguageGerman => Get("LanguageGerman");
    public string LanguageEnglish => Get("LanguageEnglish");
    public string LanguageFrench => Get("LanguageFrench");
    public string LanguageItalian => Get("LanguageItalian");
    public string SettingsAbout => Get("SettingsAbout");
    public string SettingsAboutVersion => Get("SettingsAboutVersion");
    public string SettingsAboutDescription => Get("SettingsAboutDescription");
    public string SettingsAboutCredits1 => Get("SettingsAboutCredits1");
    public string SettingsAboutCredits2 => Get("SettingsAboutCredits2");
    public string NewProject => Get("NewProject");
    public string CurrentProject => Get("CurrentProject");
    public string OpenProject => Get("OpenProject");
    public string Continue => Get("Continue");
    public string GetStarted => Get("GetStarted");
    public string RecentProjects => Get("RecentProjects");
    public string MasterData => Get("MasterData");
    public string NisvCompliance => Get("NisvCompliance");
    public string Home => Get("Home");
    public string File => Get("File");
    public string Project => Get("Project");
    public string Settings => Get("Settings");

    // ============================================================
    // PROJECT INFO
    // ============================================================
    public string StationInfo => Get("StationInfo");
    public string Callsign => Get("Callsign");
    public string Operator => Get("Operator");
    public string Address => Get("Address");
    public string Location => Get("Location");
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
    public string Antenna => Get("Antenna");
    public string Transmitter => Get("Transmitter");
    public string FeedLine => Get("FeedLine");
    public string OperatingParameters => Get("OperatingParameters");
    public string EvaluationPoint => Get("EvaluationPoint");

    // Antenna section
    public string SelectAntenna => Get("SelectAntenna");
    public string Height => Get("Height");
    public string FrequencyBands => Get("FrequencyBands");
    public string Bands => Get("Bands");
    public string Distance => Get("OkaDistance");

    // Transmitter section
    public string SelectRadio => Get("SelectRadio");
    public string UseLinearAmplifier => Get("UseLinearAmplifier");
    public string Power => Get("Power");

    // Feed line section
    public string Cable => Get("Cable");
    public string SelectCable => Get("SelectCable");
    public string CableLength => Get("CableLength");
    public string AdditionalLoss => Get("AdditionalLoss");

    // Operating parameters
    public string Modulation => Get("Modulation");
    public string ActivityFactor => Get("ActivityFactor");
    public string ParametersApplyToAllBands => Get("ParametersApplyToAllBands");

    // OKA section
    public string OkaDistance => Get("OkaDistance");
    public string OkaDamping => Get("OkaDamping");
    public string OkaExplanation => Get("OkaExplanation");
    public string AdditionalLossExample => Get("AdditionalLossExample");
    public string OkaNameExample => Get("OkaNameExample");

    // OKA Master Editor
    public string OkaDetails => Get("OkaDetails");
    public string OkaNumber => Get("OkaNumber");
    public string OkaNameLabel => Get("OkaNameLabel");
    public string OkaNameRequired => Get("OkaNameRequired");
    public string DefaultDamping => Get("DefaultDamping");
    public string DefaultDampingHint => Get("DefaultDampingHint");
    public string DefaultDistance => Get("DefaultDistance");
    public string DefaultDistanceHint => Get("DefaultDistanceHint");
    public string AddOka => Get("AddOka");
    public string EditOka => Get("EditOka");
    public string SelectOka => Get("SelectOka");

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
    public string DegreeInfoOnly => Get("DegreeInfoOnly");
    public string Frequency => Get("Frequency");
    public string Gain => Get("Gain");
    public string GainRange => Get("GainRange");
    public string AddBand => Get("AddBand");
    public string VerticalRadiationPattern => Get("VerticalRadiationPattern");
    public string PatternExplanation => Get("PatternExplanation");
    public string AddAtLeastOneBand => Get("AddAtLeastOneBand");
    public string AddNewAntenna => Get("AddNewAntenna");
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
    public string AntennaTypeLabel => Get("AntennaTypeLabel");
    public string GenerateFromGain => Get("GenerateFromGain");
    public string GenerateAllPatterns => Get("GenerateAllPatterns");

    // ============================================================
    // CABLE EDITOR
    // ============================================================
    public string CableName => Get("CableName");
    public string AttenuationData => Get("AttenuationData");
    public string AttenuationHint => Get("AttenuationHint");
    public string HfBands => Get("HfBands");
    public string VhfUhfShfBands => Get("VhfUhfShfBands");
    public string CableNameExample => Get("CableNameExample");

    // ============================================================
    // RADIO EDITOR
    // ============================================================
    public string RadioDetails => Get("RadioDetails");
    public string MaxPower => Get("MaxPower");
    public string MaxPowerHint => Get("MaxPowerHint");
    public string RadioManufacturerExample => Get("RadioManufacturerExample");
    public string RadioModelExample => Get("RadioModelExample");

    // ============================================================
    // MASTER DATA MANAGER
    // ============================================================
    public string Antennas => Get("Antennas");
    public string Cables => Get("Cables");
    public string Radios => Get("Radios");
    public string Translations => Get("Translations");
    public string MasterDataManager => Get("MasterDataManager");
    public string Clear => Get("Clear");
    public string SearchAntennas => Get("SearchAntennas");
    public string SearchCables => Get("SearchCables");
    public string SearchRadios => Get("SearchRadios");
    public string SearchOkas => Get("SearchOkas");
    public string SearchTranslations => Get("SearchTranslations");
    public string AddAntennaButton => Get("AddAntennaButton");
    public string AddCableButton => Get("AddCableButton");
    public string AddRadioButton => Get("AddRadioButton");
    public string SaveChanges => Get("SaveChanges");
    public string TranslationEditorInfo => Get("TranslationEditorInfo");
    public string ColumnKey => Get("ColumnKey");
    public string ColumnCategory => Get("ColumnCategory");
    public string ColumnGerman => Get("ColumnGerman");
    public string ColumnFrench => Get("ColumnFrench");
    public string ColumnItalian => Get("ColumnItalian");
    public string ColumnEnglish => Get("ColumnEnglish");
    public string Database => Get("Database");
    public string DatabaseTabInfo => Get("DatabaseTabInfo");
    public string ExportDatabase => Get("ExportDatabase");
    public string ExportDatabaseDesc => Get("ExportDatabaseDesc");
    public string ImportDatabase => Get("ImportDatabase");
    public string ImportDatabaseDesc => Get("ImportDatabaseDesc");

    // ============================================================
    // RESULTS
    // ============================================================
    public string CalculationResults => Get("CalculationResults");
    public string Limit => Get("Limit");
    public string ExportMarkdown => Get("ExportMarkdown");
    public string FreqHeader => Get("FreqHeader");
    public string GainHeader => Get("GainHeader");
    public string PmHeader => Get("PmHeader");
    public string FieldHeader => Get("FieldHeader");
    public string SafetyHeader => Get("SafetyHeader");
    public string StatusHeader => Get("StatusHeader");

    // ============================================================
    // VALIDATION MESSAGES
    // ============================================================

    // ============================================================
    // DIALOGS
    // ============================================================
    public string UnsavedChanges => Get("UnsavedChanges");
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
        Instance.RaiseAllPropertiesChanged();
    }

    /// <summary>
    /// Adds a new translation key.
    /// </summary>
    public static void AddTranslation(string key, string category, Dictionary<string, string> values)
    {
        TranslationData[key] = new Dictionary<string, string>(values);
        Categories[key] = category;
        Instance.RaiseAllPropertiesChanged();
    }

    // ============================================================
    // CATEGORIES MAPPING
    // ============================================================
    private static readonly Dictionary<string, string> Categories = new()
    {
        // Common
        ["Save"] = "Common",
        ["Cancel"] = "Common",
        ["Edit"] = "Common",
        ["New"] = "Common",
        ["Delete"] = "Common",
        ["Remove"] = "Common",
        ["Back"] = "Common",
        ["Close"] = "Common",
        // Welcome
        ["AppTitle"] = "Welcome",
        ["AppSubtitle"] = "Welcome",
        ["SelectLanguage"] = "Welcome",
        ["Theme"] = "Welcome",
        ["ThemeLight"] = "Welcome",
        ["ThemeDark"] = "Welcome",
        ["ThemeSystem"] = "Welcome",
        ["SettingsAppearance"] = "Settings",
        ["SettingsThemeDescription"] = "Settings",
        ["SettingsLanguage"] = "Settings",
        ["SettingsLanguageDescription"] = "Settings",
        ["SettingsDisplayLanguage"] = "Settings",
        ["SettingsAbout"] = "Settings",
        ["SettingsAboutVersion"] = "Settings",
        ["SettingsAboutDescription"] = "Settings",
        ["SettingsAboutCredits1"] = "Settings",
        ["SettingsAboutCredits2"] = "Settings",
        ["LanguageGerman"] = "Settings",
        ["LanguageEnglish"] = "Settings",
        ["LanguageFrench"] = "Settings",
        ["LanguageItalian"] = "Settings",
        ["NewProject"] = "Welcome",
        ["CurrentProject"] = "Welcome",
        ["OpenProject"] = "Welcome",
        ["MasterData"] = "Welcome",
        ["NisvCompliance"] = "Welcome",
        // ProjectInfo
        ["StationInfo"] = "ProjectInfo",
        ["Callsign"] = "ProjectInfo",
        ["Operator"] = "ProjectInfo",
        ["Address"] = "ProjectInfo",
        ["Location"] = "ProjectInfo",
        ["CreateProject"] = "ProjectInfo",
        ["EditStationInfo"] = "ProjectInfo",
        ["CallsignExample"] = "ProjectInfo",
        ["AddressExample"] = "ProjectInfo",
        // ProjectOverview
        ["Configurations"] = "ProjectOverview",
        ["AddConfiguration"] = "ProjectOverview",
        ["CalculateAll"] = "ProjectOverview",
        ["ExportReport"] = "ProjectOverview",
        ["NoConfigurations"] = "ProjectOverview",
        // ConfigEditor
        ["Antenna"] = "ConfigEditor",
        ["Transmitter"] = "ConfigEditor",
        ["FeedLine"] = "ConfigEditor",
        ["OperatingParameters"] = "ConfigEditor",
        ["EvaluationPoint"] = "ConfigEditor",
        ["SelectAntenna"] = "ConfigEditor",
        ["Height"] = "ConfigEditor",
        ["FrequencyBands"] = "ConfigEditor",
        ["SelectRadio"] = "ConfigEditor",
        ["UseLinearAmplifier"] = "ConfigEditor",
        ["Power"] = "ConfigEditor",
        ["Cable"] = "ConfigEditor",
        ["SelectCable"] = "ConfigEditor",
        ["CableLength"] = "ConfigEditor",
        ["AdditionalLoss"] = "ConfigEditor",
        ["Modulation"] = "ConfigEditor",
        ["ActivityFactor"] = "ConfigEditor",
        ["ParametersApplyToAllBands"] = "ConfigEditor",
        ["OkaDistance"] = "ConfigEditor",
        ["OkaDamping"] = "ConfigEditor",
        ["OkaExplanation"] = "ConfigEditor",
        ["AdditionalLossExample"] = "ConfigEditor",
        ["OkaNameExample"] = "ConfigEditor",
        // AntennaEditor
        ["AntennaDetails"] = "AntennaEditor",
        ["Manufacturer"] = "AntennaEditor",
        ["Model"] = "AntennaEditor",
        ["Polarization"] = "AntennaEditor",
        ["Horizontal"] = "AntennaEditor",
        ["Vertical"] = "AntennaEditor",
        ["Rotatable"] = "AntennaEditor",
        ["DegreeInfoOnly"] = "AntennaEditor",
        ["Frequency"] = "AntennaEditor",
        ["Gain"] = "AntennaEditor",
        ["GainRange"] = "AntennaEditor",
        ["AddBand"] = "AntennaEditor",
        ["VerticalRadiationPattern"] = "AntennaEditor",
        ["PatternExplanation"] = "AntennaEditor",
        ["AddAtLeastOneBand"] = "AntennaEditor",
        ["AddNewAntenna"] = "AntennaEditor",
        ["AntennaRepository"] = "AntennaEditor",
        ["SelectAntennaPrompt"] = "AntennaEditor",
        ["TypeToSearch"] = "AntennaEditor",
        ["HorizontallyRotatable"] = "AntennaEditor",
        ["TypeToSearchDescription"] = "AntennaEditor",
        ["ManufacturerExample"] = "AntennaEditor",
        ["ModelExample"] = "AntennaEditor",
        ["SaveAntenna"] = "AntennaEditor",
        ["AntennaMfgExample"] = "AntennaEditor",
        ["AntennaModelExample"] = "AntennaEditor",
        ["AntennaTypeLabel"] = "AntennaEditor",
        ["GenerateFromGain"] = "AntennaEditor",
        ["GenerateAllPatterns"] = "AntennaEditor",
        // CableEditor
        ["CableName"] = "CableEditor",
        ["AttenuationData"] = "CableEditor",
        ["AttenuationHint"] = "CableEditor",
        ["HfBands"] = "CableEditor",
        ["VhfUhfShfBands"] = "CableEditor",
        ["CableNameExample"] = "CableEditor",
        // RadioEditor
        ["RadioDetails"] = "RadioEditor",
        ["MaxPower"] = "RadioEditor",
        ["MaxPowerHint"] = "RadioEditor",
        ["RadioManufacturerExample"] = "RadioEditor",
        ["RadioModelExample"] = "RadioEditor",
        // MasterData
        ["Antennas"] = "MasterData",
        ["Cables"] = "MasterData",
        ["Radios"] = "MasterData",
        ["Translations"] = "MasterData",
        ["MasterDataManager"] = "MasterData",
        ["Clear"] = "MasterData",
        ["SearchAntennas"] = "MasterData",
        ["SearchCables"] = "MasterData",
        ["SearchRadios"] = "MasterData",
        ["SearchTranslations"] = "MasterData",
        ["AddAntennaButton"] = "MasterData",
        ["AddCableButton"] = "MasterData",
        ["AddRadioButton"] = "MasterData",
        ["SaveChanges"] = "MasterData",
        ["TranslationEditorInfo"] = "MasterData",
        ["Database"] = "MasterData",
        ["DatabaseTabInfo"] = "MasterData",
        ["ExportDatabase"] = "MasterData",
        ["ExportDatabaseDesc"] = "MasterData",
        ["ImportDatabase"] = "MasterData",
        ["ImportDatabaseDesc"] = "MasterData",
        // Results
        ["CalculationResults"] = "Results",
        ["Limit"] = "Results",
        ["ExportMarkdown"] = "Results",
        ["FreqHeader"] = "Results",
        ["GainHeader"] = "Results",
        ["PmHeader"] = "Results",
        ["FieldHeader"] = "Results",
        ["SafetyHeader"] = "Results",
        ["StatusHeader"] = "Results",
        // Dialogs
        ["UnsavedChanges"] = "Dialogs",
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
        ["View"] = new() { ["de"] = "Anzeigen", ["fr"] = "Afficher", ["it"] = "Visualizza", ["en"] = "View" },
        ["ProjectSpecific"] = new() { ["de"] = "[Projekt]", ["fr"] = "[Projet]", ["it"] = "[Progetto]", ["en"] = "[Project]" },
        ["DuplicateNameError"] = new() { ["de"] = "Ein Eintrag mit diesem Namen existiert bereits", ["fr"] = "Une entrée avec ce nom existe déjà", ["it"] = "Esiste già una voce con questo nome", ["en"] = "An item with this name already exists" },
        ["New"] = new() { ["de"] = "Neu", ["fr"] = "Nouveau", ["it"] = "Nuovo", ["en"] = "New" },
        ["Delete"] = new() { ["de"] = "Löschen", ["fr"] = "Supprimer", ["it"] = "Elimina", ["en"] = "Delete" },
        ["Actions"] = new() { ["de"] = "Aktionen", ["fr"] = "Actions", ["it"] = "Azioni", ["en"] = "Actions" },
        ["Remove"] = new() { ["de"] = "Entfernen", ["fr"] = "Retirer", ["it"] = "Rimuovi", ["en"] = "Remove" },
        ["Back"] = new() { ["de"] = "Zurück", ["fr"] = "Retour", ["it"] = "Indietro", ["en"] = "Back" },
        ["Close"] = new() { ["de"] = "Schliessen", ["fr"] = "Fermer", ["it"] = "Chiudi", ["en"] = "Close" },

        // Welcome
        ["AppTitle"] = new() { ["de"] = "Swiss NIS Calculator", ["fr"] = "Swiss NIS Calculator", ["it"] = "Swiss NIS Calculator", ["en"] = "Swiss NIS Calculator" },
        ["AppSubtitle"] = new() { ["de"] = "Feldstärkerechner für Amateurfunk", ["fr"] = "Calculateur de champ RF pour radioamateurs", ["it"] = "Calcolatore campo RF per radioamatori", ["en"] = "RF Field Strength Calculator for Amateur Radio" },
        ["SelectLanguage"] = new() { ["de"] = "Sprache wählen", ["fr"] = "Choisir la langue", ["it"] = "Seleziona lingua", ["en"] = "Select Language" },
        ["Theme"] = new() { ["de"] = "Design", ["fr"] = "Thème", ["it"] = "Tema", ["en"] = "Theme" },
        ["ThemeLight"] = new() { ["de"] = "Hell", ["fr"] = "Clair", ["it"] = "Chiaro", ["en"] = "Light" },
        ["ThemeDark"] = new() { ["de"] = "Dunkel", ["fr"] = "Sombre", ["it"] = "Scuro", ["en"] = "Dark" },
        ["ThemeSystem"] = new() { ["de"] = "System", ["fr"] = "Système", ["it"] = "Sistema", ["en"] = "System" },
        ["SettingsAppearance"] = new() { ["de"] = "Erscheinungsbild", ["fr"] = "Apparence", ["it"] = "Aspetto", ["en"] = "Appearance" },
        ["SettingsThemeDescription"] = new() { ["de"] = "Hell, Dunkel oder Systemdesign wählen", ["fr"] = "Choisir clair, sombre ou système", ["it"] = "Scegli tema chiaro, scuro o di sistema", ["en"] = "Select light, dark, or system theme" },
        ["SettingsLanguage"] = new() { ["de"] = "Sprache", ["fr"] = "Langue", ["it"] = "Lingua", ["en"] = "Language" },
        ["SettingsLanguageDescription"] = new() { ["de"] = "Bevorzugte Sprache wählen", ["fr"] = "Choisissez votre langue préférée", ["it"] = "Scegli la lingua preferita", ["en"] = "Choose your preferred language" },
        ["SettingsDisplayLanguage"] = new() { ["de"] = "Anzeigesprache", ["fr"] = "Langue d'affichage", ["it"] = "Lingua di visualizzazione", ["en"] = "Display Language" },
        ["LanguageGerman"] = new() { ["de"] = "Deutsch", ["fr"] = "Allemand", ["it"] = "Tedesco", ["en"] = "German" },
        ["LanguageEnglish"] = new() { ["de"] = "Englisch", ["fr"] = "Anglais", ["it"] = "Inglese", ["en"] = "English" },
        ["LanguageFrench"] = new() { ["de"] = "Französisch", ["fr"] = "Français", ["it"] = "Francese", ["en"] = "French" },
        ["LanguageItalian"] = new() { ["de"] = "Italienisch", ["fr"] = "Italien", ["it"] = "Italiano", ["en"] = "Italian" },
        ["SettingsAbout"] = new() { ["de"] = "Über", ["fr"] = "À propos", ["it"] = "Informazioni", ["en"] = "About" },
        ["SettingsAboutVersion"] = new() { ["de"] = "Version 2.0", ["fr"] = "Version 2.0", ["it"] = "Versione 2.0", ["en"] = "Version 2.0" },
        ["SettingsAboutDescription"] = new() { ["de"] = "NISV-konformer Feldstärkerechner für Schweizer Funkamateure.", ["fr"] = "Calculateur de champ conforme à l'ORNI pour les radioamateurs en Suisse.", ["it"] = "Calcolatore di intensità di campo conforme all'ORNI per radioamatori in Svizzera.", ["en"] = "NISV-compliant field strength calculator for amateur radio operators in Switzerland." },
        ["SettingsAboutCredits1"] = new() { ["de"] = "Ursprünglich entwickelt von HB9ZS", ["fr"] = "Développé à l'origine par HB9ZS", ["it"] = "Sviluppato originariamente da HB9ZS", ["en"] = "Originally developed by HB9ZS" },
        ["SettingsAboutCredits2"] = new() { ["de"] = "Neu geschrieben mit Avalonia UI und FluentAvalonia", ["fr"] = "Réécrit avec Avalonia UI et FluentAvalonia", ["it"] = "Riscritto con Avalonia UI e FluentAvalonia", ["en"] = "Rewritten with Avalonia UI and FluentAvalonia" },
        ["NewProject"] = new() { ["de"] = "Neues Projekt", ["fr"] = "Nouveau projet", ["it"] = "Nuovo progetto", ["en"] = "New Project" },
        ["CurrentProject"] = new() { ["de"] = "Aktuelles Projekt", ["fr"] = "Projet actuel", ["it"] = "Progetto corrente", ["en"] = "Current Project" },
        ["OpenProject"] = new() { ["de"] = "Projekt öffnen...", ["fr"] = "Ouvrir projet...", ["it"] = "Apri progetto...", ["en"] = "Open Project..." },
        ["Continue"] = new() { ["de"] = "Fortsetzen", ["fr"] = "Continuer", ["it"] = "Continua", ["en"] = "Continue" },
        ["GetStarted"] = new() { ["de"] = "Projekt", ["fr"] = "Projet", ["it"] = "Progetto", ["en"] = "Project" },
        ["RecentProjects"] = new() { ["de"] = "Zuletzt geöffnet", ["fr"] = "Projets récents", ["it"] = "Progetti recenti", ["en"] = "Recent Projects" },
        ["MasterData"] = new() { ["de"] = "Stammdaten", ["fr"] = "Données de base", ["it"] = "Dati master", ["en"] = "Master Data" },
        ["NisvCompliance"] = new() { ["de"] = "NISV-Konformitätsrechner für Schweizer Amateurfunkstationen", ["fr"] = "Calculateur de conformité ORNI pour stations radioamateurs suisses", ["it"] = "Calcolatore conformità ORNI per stazioni radioamatoriali svizzere", ["en"] = "NISV Compliance Calculator for Swiss Amateur Radio Stations" },
        ["Home"] = new() { ["de"] = "Start", ["fr"] = "Accueil", ["it"] = "Home", ["en"] = "Home" },
        ["File"] = new() { ["de"] = "Datei", ["fr"] = "Fichier", ["it"] = "File", ["en"] = "File" },
        ["Project"] = new() { ["de"] = "Projekt", ["fr"] = "Projet", ["it"] = "Progetto", ["en"] = "Project" },
        ["Settings"] = new() { ["de"] = "Einstellungen", ["fr"] = "Paramètres", ["it"] = "Impostazioni", ["en"] = "Settings" },

        // Project Info
        ["StationInfo"] = new() { ["de"] = "Stationsinformationen", ["fr"] = "Informations station", ["it"] = "Informazioni stazione", ["en"] = "Station Information" },
        ["Callsign"] = new() { ["de"] = "Rufzeichen", ["fr"] = "Indicatif", ["it"] = "Nominativo", ["en"] = "Callsign" },
        ["Operator"] = new() { ["de"] = "Betreiber", ["fr"] = "Opérateur", ["it"] = "Operatore", ["en"] = "Operator" },
        ["Address"] = new() { ["de"] = "Adresse", ["fr"] = "Adresse", ["it"] = "Indirizzo", ["en"] = "Address" },
        ["Location"] = new() { ["de"] = "Ort", ["fr"] = "Localité", ["it"] = "Località", ["en"] = "Location" },
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
        ["Antenna"] = new() { ["de"] = "Antenne", ["fr"] = "Antenne", ["it"] = "Antenna", ["en"] = "Antenna" },
        ["Transmitter"] = new() { ["de"] = "Sender", ["fr"] = "Émetteur", ["it"] = "Trasmettitore", ["en"] = "Transmitter" },
        ["FeedLine"] = new() { ["de"] = "Speiseleitung", ["fr"] = "Ligne d'alimentation", ["it"] = "Linea di alimentazione", ["en"] = "Feed Line" },
        ["OperatingParameters"] = new() { ["de"] = "Betriebsparameter", ["fr"] = "Paramètres d'exploitation", ["it"] = "Parametri operativi", ["en"] = "Operating Parameters" },
        ["EvaluationPoint"] = new() { ["de"] = "Beurteilungspunkt (OKA)", ["fr"] = "Point d'évaluation (OKA)", ["it"] = "Punto di valutazione (OKA)", ["en"] = "Evaluation Point (OKA)" },

        ["SelectAntenna"] = new() { ["de"] = "-- Antenne wählen --", ["fr"] = "-- Sélectionner antenne --", ["it"] = "-- Seleziona antenna --", ["en"] = "-- Select Antenna --" },
        ["Height"] = new() { ["de"] = "Höhe", ["fr"] = "Hauteur", ["it"] = "Altezza", ["en"] = "Height" },
        ["FrequencyBands"] = new() { ["de"] = "Frequenzbänder", ["fr"] = "Bandes de fréquence", ["it"] = "Bande di frequenza", ["en"] = "Frequency Bands" },
        ["Bands"] = new() { ["de"] = "Bänder", ["fr"] = "Bandes", ["it"] = "Bande", ["en"] = "Bands" },

        ["SelectRadio"] = new() { ["de"] = "-- Transceiver wählen --", ["fr"] = "-- Sélectionner émetteur --", ["it"] = "-- Seleziona ricetrasmettitore --", ["en"] = "-- Select Radio --" },
        ["UseLinearAmplifier"] = new() { ["de"] = "Linearendstufe verwenden", ["fr"] = "Utiliser amplificateur linéaire", ["it"] = "Usa amplificatore lineare", ["en"] = "Use Linear Amplifier" },
        ["Power"] = new() { ["de"] = "Leistung", ["fr"] = "Puissance", ["it"] = "Potenza", ["en"] = "Power" },

        ["Cable"] = new() { ["de"] = "Kabel", ["fr"] = "Câble", ["it"] = "Cavo", ["en"] = "Cable" },
        ["SelectCable"] = new() { ["de"] = "-- Kabel wählen --", ["fr"] = "-- Sélectionner câble --", ["it"] = "-- Seleziona cavo --", ["en"] = "-- Select Cable --" },
        ["CableLength"] = new() { ["de"] = "Kabellänge", ["fr"] = "Longueur câble", ["it"] = "Lunghezza cavo", ["en"] = "Cable Length" },
        ["AdditionalLoss"] = new() { ["de"] = "Zusätzliche Dämpfung", ["fr"] = "Pertes supplémentaires", ["it"] = "Perdite aggiuntive", ["en"] = "Additional Loss" },

        ["Modulation"] = new() { ["de"] = "Modulation", ["fr"] = "Modulation", ["it"] = "Modulazione", ["en"] = "Modulation" },
        ["ActivityFactor"] = new() { ["de"] = "Aktivitätsfaktor", ["fr"] = "Facteur d'activité", ["it"] = "Fattore di attività", ["en"] = "Activity Factor" },
        ["ParametersApplyToAllBands"] = new() { ["de"] = "Diese Parameter gelten für alle Bänder der gewählten Antenne.", ["fr"] = "Ces paramètres s'appliquent à toutes les bandes de l'antenne sélectionnée.", ["it"] = "Questi parametri si applicano a tutte le bande dell'antenna selezionata.", ["en"] = "These parameters apply to all bands of the selected antenna." },

        ["OkaDistance"] = new() { ["de"] = "Distanz", ["fr"] = "Distance", ["it"] = "Distanza", ["en"] = "Distance" },
        ["OkaDamping"] = new() { ["de"] = "Gebäudedämpfung", ["fr"] = "Atténuation bâtiment", ["it"] = "Attenuazione edificio", ["en"] = "Building Damping" },
        ["OkaExplanation"] = new() { ["de"] = "OKA = Ort des kurzfristigen Aufenthalts", ["fr"] = "OKA = Lieu de séjour de courte durée", ["it"] = "OKA = Luogo di soggiorno breve", ["en"] = "OKA = Place of short-term stay" },
        ["AdditionalLossExample"] = new() { ["de"] = "z.B. Stecker, Schalter", ["fr"] = "ex. Connecteurs, commutateur", ["it"] = "es. Connettori, interruttore", ["en"] = "e.g. Connectors, switch" },
        ["OkaNameExample"] = new() { ["de"] = "z.B. Balkon des Nachbarn", ["fr"] = "ex. Balcon du voisin", ["it"] = "es. Balcone del vicino", ["en"] = "e.g. Neighbor's balcony" },

        // OKA Master Editor
        ["OkaDetails"] = new() { ["de"] = "OKA Details", ["fr"] = "Détails OKA", ["it"] = "Dettagli OKA", ["en"] = "OKA Details" },
        ["OkaNumber"] = new() { ["de"] = "Nummer", ["fr"] = "Numéro", ["it"] = "Numero", ["en"] = "Number" },
        ["OkaNameLabel"] = new() { ["de"] = "Bezeichnung", ["fr"] = "Désignation", ["it"] = "Designazione", ["en"] = "Name" },
        ["OkaNameRequired"] = new() { ["de"] = "Bitte eine Bezeichnung eingeben.", ["fr"] = "Veuillez entrer une désignation.", ["it"] = "Inserire una designazione.", ["en"] = "Please enter a name." },
        ["DefaultDamping"] = new() { ["de"] = "Standard-Dämpfung", ["fr"] = "Atténuation par défaut", ["it"] = "Attenuazione predefinita", ["en"] = "Default Damping" },
        ["DefaultDampingHint"] = new() { ["de"] = "0 dB für Aussenbereich, typisch 6-12 dB für Innenräume", ["fr"] = "0 dB pour l'extérieur, typiquement 6-12 dB pour l'intérieur", ["it"] = "0 dB per l'esterno, tipicamente 6-12 dB per interni", ["en"] = "0 dB for outdoor, typically 6-12 dB for indoor" },
        ["DefaultDistance"] = new() { ["de"] = "Horizontale Distanz", ["fr"] = "Distance horizontale", ["it"] = "Distanza orizzontale", ["en"] = "Horizontal Distance" },
        ["DefaultDistanceHint"] = new() { ["de"] = "Distanz von der Antenne", ["fr"] = "Distance de l'antenne", ["it"] = "Distanza dall'antenna", ["en"] = "Distance from antenna" },
        ["AddOka"] = new() { ["de"] = "OKA hinzufügen", ["fr"] = "Ajouter OKA", ["it"] = "Aggiungi OKA", ["en"] = "Add OKA" },
        ["EditOka"] = new() { ["de"] = "OKA bearbeiten", ["fr"] = "Modifier OKA", ["it"] = "Modifica OKA", ["en"] = "Edit OKA" },
        ["SelectOka"] = new() { ["de"] = "-- OKA wählen --", ["fr"] = "-- Sélectionner OKA --", ["it"] = "-- Seleziona OKA --", ["en"] = "-- Select OKA --" },

        // Antenna Editor
        ["AntennaDetails"] = new() { ["de"] = "Antennendetails", ["fr"] = "Détails antenne", ["it"] = "Dettagli antenna", ["en"] = "Antenna Details" },
        ["Manufacturer"] = new() { ["de"] = "Hersteller", ["fr"] = "Fabricant", ["it"] = "Produttore", ["en"] = "Manufacturer" },
        ["Model"] = new() { ["de"] = "Modell", ["fr"] = "Modèle", ["it"] = "Modello", ["en"] = "Model" },
        ["Polarization"] = new() { ["de"] = "Polarisation", ["fr"] = "Polarisation", ["it"] = "Polarizzazione", ["en"] = "Polarization" },
        ["Horizontal"] = new() { ["de"] = "Horizontal", ["fr"] = "Horizontal", ["it"] = "Orizzontale", ["en"] = "Horizontal" },
        ["Vertical"] = new() { ["de"] = "Vertikal", ["fr"] = "Vertical", ["it"] = "Verticale", ["en"] = "Vertical" },
        ["Rotatable"] = new() { ["de"] = "Drehbar", ["fr"] = "Rotatif", ["it"] = "Rotabile", ["en"] = "Rotatable" },
        ["DegreeInfoOnly"] = new() { ["de"] = "Grad (nur Info)", ["fr"] = "degrés (info seulement)", ["it"] = "gradi (solo info)", ["en"] = "deg (info only)" },
        ["Frequency"] = new() { ["de"] = "Frequenz", ["fr"] = "Fréquence", ["it"] = "Frequenza", ["en"] = "Frequency" },
        ["Gain"] = new() { ["de"] = "Gewinn", ["fr"] = "Gain", ["it"] = "Guadagno", ["en"] = "Gain" },
        ["GainRange"] = new() { ["de"] = "dBi (-20 bis 50)", ["fr"] = "dBi (-20 à 50)", ["it"] = "dBi (-20 a 50)", ["en"] = "dBi (-20 to 50)" },
        ["AddBand"] = new() { ["de"] = "+ Band hinzufügen", ["fr"] = "+ Ajouter bande", ["it"] = "+ Aggiungi banda", ["en"] = "+ Add Band" },
        ["VerticalRadiationPattern"] = new() { ["de"] = "Vertikales Strahlungsdiagramm (Dämpfung in dB je Winkel vom Horizont)", ["fr"] = "Diagramme de rayonnement vertical (atténuation en dB par angle depuis l'horizon)", ["it"] = "Diagramma di radiazione verticale (attenuazione in dB per angolo dall'orizzonte)", ["en"] = "Vertical Radiation Pattern (attenuation in dB at each angle from horizon)" },
        ["PatternExplanation"] = new() { ["de"] = "0° = Horizont (max. Abstrahlung), 90° = Senkrecht nach unten (Richtung OKA). Werte: 0-60 dB Dämpfung.", ["fr"] = "0° = Horizon (rayonnement max), 90° = Verticalement vers le bas (direction OKA). Valeurs: 0-60 dB d'atténuation.", ["it"] = "0° = Orizzonte (radiazione max), 90° = Verticalmente verso il basso (direzione OKA). Valori: 0-60 dB attenuazione.", ["en"] = "0° = Horizon (max radiation), 90° = Straight down (toward OKA). Values: 0-60 dB attenuation." },
        ["AddAtLeastOneBand"] = new() { ["de"] = "Fügen Sie mindestens ein Frequenzband mit Gewinn und vertikalem Strahlungsdiagramm hinzu.", ["fr"] = "Ajoutez au moins une bande de fréquence avec gain et diagramme de rayonnement vertical.", ["it"] = "Aggiungi almeno una banda di frequenza con guadagno e diagramma di radiazione verticale.", ["en"] = "Add at least one frequency band with gain and vertical radiation pattern." },
        ["AddNewAntenna"] = new() { ["de"] = "Neue Antenne hinzufügen", ["fr"] = "Ajouter nouvelle antenne", ["it"] = "Aggiungi nuova antenna", ["en"] = "Add New Antenna" },
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
        ["AntennaTypeLabel"] = new() { ["de"] = "Antennentyp", ["fr"] = "Type d'antenne", ["it"] = "Tipo di antenna", ["en"] = "Antenna Type" },
        ["GenerateFromGain"] = new() { ["de"] = "Automatisch berechnen", ["fr"] = "Calculer automatiquement", ["it"] = "Calcola automaticamente", ["en"] = "Auto-calculate" },
        ["GenerateAllPatterns"] = new() { ["de"] = "Strahlungsdiagramme aus Gewinn berechnen", ["fr"] = "Calculer diagrammes depuis gain", ["it"] = "Calcola diagrammi da guadagno", ["en"] = "Generate Patterns from Gain" },

        // Cable Editor
        ["CableName"] = new() { ["de"] = "Kabelname", ["fr"] = "Nom du câble", ["it"] = "Nome cavo", ["en"] = "Cable Name" },
        ["AttenuationData"] = new() { ["de"] = "Dämpfungsdaten (dB pro 100m)", ["fr"] = "Données d'atténuation (dB par 100m)", ["it"] = "Dati attenuazione (dB per 100m)", ["en"] = "Attenuation Data (dB per 100m)" },
        ["AttenuationHint"] = new() { ["de"] = "Geben Sie die Dämpfungswerte für die verfügbaren Frequenzen ein. Leer lassen wenn unbekannt.", ["fr"] = "Entrez les valeurs d'atténuation pour les fréquences disponibles. Laissez vide si inconnu.", ["it"] = "Inserisci i valori di attenuazione per le frequenze disponibili. Lascia vuoto se sconosciuto.", ["en"] = "Enter attenuation values for the frequencies you have data for. Leave empty if unknown." },
        ["HfBands"] = new() { ["de"] = "KW-Bänder", ["fr"] = "Bandes HF", ["it"] = "Bande HF", ["en"] = "HF Bands" },
        ["VhfUhfShfBands"] = new() { ["de"] = "VHF / UHF / SHF Bänder", ["fr"] = "Bandes VHF / UHF / SHF", ["it"] = "Bande VHF / UHF / SHF", ["en"] = "VHF / UHF / SHF Bands" },
        ["CableNameExample"] = new() { ["de"] = "z.B. EcoFlex10, RG-213", ["fr"] = "ex. EcoFlex10, RG-213", ["it"] = "es. EcoFlex10, RG-213", ["en"] = "e.g. EcoFlex10, RG-213" },

        // Radio Editor
        ["RadioDetails"] = new() { ["de"] = "Transceiver-Details", ["fr"] = "Détails émetteur-récepteur", ["it"] = "Dettagli ricetrasmettitore", ["en"] = "Radio / Transceiver Details" },
        ["MaxPower"] = new() { ["de"] = "Max. Leistung", ["fr"] = "Puissance max.", ["it"] = "Potenza max.", ["en"] = "Max Power" },
        ["MaxPowerHint"] = new() { ["de"] = "W (maximale Ausgangsleistung)", ["fr"] = "W (puissance de sortie maximale)", ["it"] = "W (potenza di uscita massima)", ["en"] = "W (maximum output power)" },
        ["RadioManufacturerExample"] = new() { ["de"] = "z.B. Yaesu, Icom, Kenwood", ["fr"] = "ex. Yaesu, Icom, Kenwood", ["it"] = "es. Yaesu, Icom, Kenwood", ["en"] = "e.g. Yaesu, Icom, Kenwood" },
        ["RadioModelExample"] = new() { ["de"] = "z.B. FT-1000, IC-7300", ["fr"] = "ex. FT-1000, IC-7300", ["it"] = "es. FT-1000, IC-7300", ["en"] = "e.g. FT-1000, IC-7300" },

        // Master Data Manager
        ["Antennas"] = new() { ["de"] = "Antennen", ["fr"] = "Antennes", ["it"] = "Antenne", ["en"] = "Antennas" },
        ["Cables"] = new() { ["de"] = "Kabel", ["fr"] = "Câbles", ["it"] = "Cavi", ["en"] = "Cables" },
        ["Radios"] = new() { ["de"] = "Transceiver", ["fr"] = "Émetteurs-récepteurs", ["it"] = "Ricetrasmettitori", ["en"] = "Radios" },
        ["Translations"] = new() { ["de"] = "Übersetzungen", ["fr"] = "Traductions", ["it"] = "Traduzioni", ["en"] = "Translations" },
        ["MasterDataManager"] = new() { ["de"] = "Stammdaten-Verwaltung", ["fr"] = "Gestion des données de base", ["it"] = "Gestione dati master", ["en"] = "Master Data Manager" },
        ["Clear"] = new() { ["de"] = "Löschen", ["fr"] = "Effacer", ["it"] = "Cancella", ["en"] = "Clear" },
        ["SearchAntennas"] = new() { ["de"] = "Hersteller oder Modell suchen...", ["fr"] = "Rechercher fabricant ou modèle...", ["it"] = "Cerca produttore o modello...", ["en"] = "Search by manufacturer or model..." },
        ["SearchCables"] = new() { ["de"] = "Kabelname suchen...", ["fr"] = "Rechercher nom du câble...", ["it"] = "Cerca nome cavo...", ["en"] = "Search by cable name..." },
        ["SearchRadios"] = new() { ["de"] = "Hersteller oder Modell suchen...", ["fr"] = "Rechercher fabricant ou modèle...", ["it"] = "Cerca produttore o modello...", ["en"] = "Search by manufacturer or model..." },
        ["SearchOkas"] = new() { ["de"] = "OKA suchen...", ["fr"] = "Rechercher OKA...", ["it"] = "Cerca OKA...", ["en"] = "Search OKA..." },
        ["SearchTranslations"] = new() { ["de"] = "Übersetzungen suchen...", ["fr"] = "Rechercher traductions...", ["it"] = "Cerca traduzioni...", ["en"] = "Search translations..." },
        ["AddAntennaButton"] = new() { ["de"] = "+ Antenne hinzufügen", ["fr"] = "+ Ajouter antenne", ["it"] = "+ Aggiungi antenna", ["en"] = "+ Add Antenna" },
        ["AddCableButton"] = new() { ["de"] = "+ Kabel hinzufügen", ["fr"] = "+ Ajouter câble", ["it"] = "+ Aggiungi cavo", ["en"] = "+ Add Cable" },
        ["AddRadioButton"] = new() { ["de"] = "+ Transceiver hinzufügen", ["fr"] = "+ Ajouter émetteur", ["it"] = "+ Aggiungi ricetrasmettitore", ["en"] = "+ Add Radio" },
        ["SaveChanges"] = new() { ["de"] = "Änderungen speichern", ["fr"] = "Enregistrer les modifications", ["it"] = "Salva modifiche", ["en"] = "Save Changes" },
        ["TranslationEditorInfo"] = new() { ["de"] = "Übersetzungen direkt bearbeiten. Änderungen werden sofort wirksam. Klicken Sie auf Speichern, um die Änderungen dauerhaft zu speichern.", ["fr"] = "Modifiez les traductions directement. Les modifications prennent effet immédiatement. Cliquez sur Enregistrer pour conserver les modifications.", ["it"] = "Modifica le traduzioni direttamente. Le modifiche hanno effetto immediato. Clicca su Salva per rendere permanenti le modifiche.", ["en"] = "Edit translations directly. Changes take effect immediately. Click Save to persist changes across restarts." },
        ["ColumnKey"] = new() { ["de"] = "Schlüssel", ["fr"] = "Clé", ["it"] = "Chiave", ["en"] = "Key" },
        ["ColumnCategory"] = new() { ["de"] = "Kategorie", ["fr"] = "Catégorie", ["it"] = "Categoria", ["en"] = "Category" },
        ["ColumnGerman"] = new() { ["de"] = "Deutsch (DE)", ["fr"] = "Allemand (DE)", ["it"] = "Tedesco (DE)", ["en"] = "German (DE)" },
        ["ColumnFrench"] = new() { ["de"] = "Französisch (FR)", ["fr"] = "Français (FR)", ["it"] = "Francese (FR)", ["en"] = "French (FR)" },
        ["ColumnItalian"] = new() { ["de"] = "Italienisch (IT)", ["fr"] = "Italien (IT)", ["it"] = "Italiano (IT)", ["en"] = "Italian (IT)" },
        ["ColumnEnglish"] = new() { ["de"] = "Englisch (EN)", ["fr"] = "Anglais (EN)", ["it"] = "Inglese (EN)", ["en"] = "English (EN)" },

        // Database (Factory Mode)
        ["Database"] = new() { ["de"] = "Datenbank", ["fr"] = "Base de données", ["it"] = "Database", ["en"] = "Database" },
        ["DatabaseTabInfo"] = new() { ["de"] = "Änderungen an der Datenbank betreffen alle Benutzer. Nur für Wartung und Updates verwenden.", ["fr"] = "Les modifications de la base de données affectent tous les utilisateurs. Utiliser uniquement pour la maintenance.", ["it"] = "Le modifiche al database influenzano tutti gli utenti. Usare solo per manutenzione.", ["en"] = "Database changes affect all users. Use only for maintenance and updates." },
        ["ExportDatabase"] = new() { ["de"] = "Datenbank exportieren", ["fr"] = "Exporter la base de données", ["it"] = "Esporta database", ["en"] = "Export Database" },
        ["ExportDatabaseDesc"] = new() { ["de"] = "Exportiert alle Stammdaten (Antennen, Kabel, Radios) in JSON-Dateien für Versionskontrolle.", ["fr"] = "Exporte toutes les données de base en fichiers JSON pour le contrôle de version.", ["it"] = "Esporta tutti i dati master in file JSON per il controllo versione.", ["en"] = "Export all master data (antennas, cables, radios) to JSON files for version control." },
        ["ImportDatabase"] = new() { ["de"] = "Datenbank importieren", ["fr"] = "Importer la base de données", ["it"] = "Importa database", ["en"] = "Import Database" },
        ["ImportDatabaseDesc"] = new() { ["de"] = "Ersetzt alle Stammdaten mit Daten aus JSON-Dateien. ACHTUNG: Alle bestehenden Daten werden überschrieben!", ["fr"] = "Remplace toutes les données de base par des fichiers JSON. ATTENTION: Toutes les données existantes seront écrasées!", ["it"] = "Sostituisce tutti i dati master con file JSON. ATTENZIONE: Tutti i dati esistenti verranno sovrascritti!", ["en"] = "Replace all master data from JSON files. WARNING: All existing data will be overwritten!" },

        // Results
        ["CalculationResults"] = new() { ["de"] = "Berechnungsergebnisse", ["fr"] = "Résultats du calcul", ["it"] = "Risultati del calcolo", ["en"] = "Calculation Results" },
        ["Limit"] = new() { ["de"] = "Grenzwert", ["fr"] = "Limite", ["it"] = "Limite", ["en"] = "Limit" },
        ["ExportMarkdown"] = new() { ["de"] = "Markdown exportieren", ["fr"] = "Exporter Markdown", ["it"] = "Esporta Markdown", ["en"] = "Export Markdown" },
        ["FreqHeader"] = new() { ["de"] = "Freq", ["fr"] = "Fréq", ["it"] = "Freq", ["en"] = "Freq" },
        ["GainHeader"] = new() { ["de"] = "Gewinn", ["fr"] = "Gain", ["it"] = "Guad", ["en"] = "Gain" },
        ["PmHeader"] = new() { ["de"] = "Pm", ["fr"] = "Pm", ["it"] = "Pm", ["en"] = "Pm" },
        ["FieldHeader"] = new() { ["de"] = "Feld (V/m)", ["fr"] = "Champ (V/m)", ["it"] = "Campo (V/m)", ["en"] = "Field (V/m)" },
        ["SafetyHeader"] = new() { ["de"] = "Sicherh.", ["fr"] = "Sécu.", ["it"] = "Sicur.", ["en"] = "Safety" },
        ["StatusHeader"] = new() { ["de"] = "Status", ["fr"] = "Statut", ["it"] = "Stato", ["en"] = "Status" },

        // Validation

        // Dialogs
        ["UnsavedChanges"] = new() { ["de"] = "Ungespeicherte Änderungen", ["fr"] = "Modifications non enregistrées", ["it"] = "Modifiche non salvate", ["en"] = "Unsaved Changes" },
        ["DiscardChanges"] = new() { ["de"] = "Änderungen verwerfen?", ["fr"] = "Annuler les modifications?", ["it"] = "Annullare le modifiche?", ["en"] = "Discard Changes?" },
        ["DiscardChangesPrompt"] = new() { ["de"] = "Sie haben ungespeicherte Änderungen. Möchten Sie diese verwerfen?", ["fr"] = "Vous avez des modifications non enregistrées. Voulez-vous les annuler?", ["it"] = "Hai modifiche non salvate. Vuoi annullarle?", ["en"] = "You have unsaved changes. Do you want to discard them?" },
        ["ChangesDiscarded"] = new() { ["de"] = "Änderungen verworfen.", ["fr"] = "Modifications annulées.", ["it"] = "Modifiche annullate.", ["en"] = "Changes discarded." },
    };
}
