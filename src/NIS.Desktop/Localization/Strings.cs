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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OpenProject)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MasterData)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NisvCompliance)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Home)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Project)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Settings)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImportExport)));

        // Project Info
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProjectInfo)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProjectName)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Operator)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Location)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreateProject)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Callsign)));

        // Project Overview
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Configurations)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Configuration)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddConfiguration)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalculateAll)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExportReport)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExportPdf)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoConfigurations)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchProjects)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SortByName)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SortByModified)));

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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Linear)));
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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Modulations)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Constants)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroundReflectionFactor)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DefaultActivityFactor)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Factor)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddModulation)));

        // Results
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DistanceAntennaOka)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalculationResults)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Limit)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExportMarkdown)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FactorySettings)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusHeader)));

        // Short labels for two-line headers
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CableLbl)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GainLbl)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VertLbl)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotGainLbl)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EirpLbl)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BldLbl)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LimitLbl)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SafeDistLbl)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OkaDistLbl)));

        // Calculation Report Labels
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcTitlePrefix)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcOperator)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcAddress)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcDate)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcTransmitter)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcCable)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcAntenna)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcPolarization)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcRotation)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcLinear)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcOka)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcModulation)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcBuildingDamping)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcStatusCompliant)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcStatusNonCompliant)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcHorizontal)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcVertical)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcFixed)));

        // Calculation Table Row Labels
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcFrequency)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcDistanceToAntenna)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcTxPower)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcActivityFactor)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcModulationFactor)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcMeanPower)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcCableAttenuation)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcAdditionalLosses)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcTotalAttenuation)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcAttenuationFactor)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcAntennaGain)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcVerticalAttenuation)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcTotalAntennaGain)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcGainFactor)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcEirp)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcErp)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcBuildingDampingRow)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcBuildingDampingFactor)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcGroundReflection)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcFieldStrength)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcLimit)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcMinSafetyDistance)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcOkaDistance)));

        // Column Explanations
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcColumnExplanations)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainF)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainD)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainP)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainAF)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainMF)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainPm)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainA1)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainA2)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainA)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainAFactor)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainG1)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainG2)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainG)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainGFactor)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainPs)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainPsPrime)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainAg)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainAG)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainKr)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainE)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainEigw)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainDs)));
    }

    private string Get(string key) =>
        TranslationData.TryGetValue(key, out var t) && t.TryGetValue(_language, out var v) ? v :
        TranslationData.TryGetValue(key, out var t2) && t2.TryGetValue("de", out var fallback) ? fallback : $"[{key}]";

    // ============================================================
    // COMMON / BUTTONS
    // ============================================================
    public string Save => Get("Save");
    public string Cancel => Get("Cancel");
    public string Edit => Get("Edit");
    public string View => Get("View");
    public string ProjectSpecific => Get("ProjectSpecific");
    public string DuplicateNameError => Get("DuplicateNameError");
    public string New => Get("New");
    public string Delete => Get("Delete");
    public string Actions => Get("Actions");
    public string Modified => Get("Modified");
    public string Remove => Get("Remove");
    public string Back => Get("Back");
    public string Close => Get("Close");

    // ============================================================
    // WELCOME SCREEN
    // ============================================================
    public string AppTitle => Get("AppTitle");
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
    public string OpenProject => Get("OpenProject");
    public string MasterData => Get("MasterData");
    public string NisvCompliance => Get("NisvCompliance");
    public string Home => Get("Home");
    public string Projects => Get("Projects");
    public string Project => Get("Project");
    public string Settings => Get("Settings");
    public string ImportExport => Get("ImportExport");

    // ============================================================
    // PROJECT INFO
    // ============================================================
    public string ProjectInfo => Get("ProjectInfo");
    public string ProjectName => Get("ProjectName");
    public string Operator => Get("Operator");
    public string Callsign => Get("Callsign");
    public string Address => Get("Address");
    public string Location => Get("Location");
    public string CreateProject => Get("CreateProject");

    // ============================================================
    // PROJECT OVERVIEW
    // ============================================================
    public string Configurations => Get("Configurations");
    public string Configuration => Get("Configuration");
    public string AddConfiguration => Get("AddConfiguration");
    public string CalculateAll => Get("CalculateAll");
    public string ExportReport => Get("ExportReport");
    public string ExportPdf => Get("ExportPdf");
    public string NoConfigurations => Get("NoConfigurations");
    public string SearchProjects => Get("SearchProjects");
    public string NoSearchResults => Get("NoSearchResults");
    public string SortByName => Get("SortByName");
    public string SortByModified => Get("SortByModified");

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

    // Transmitter section
    public string SelectRadio => Get("SelectRadio");
    public string Linear => Get("Linear");
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

    // OKA Master Editor
    public string OkaDetails => Get("OkaDetails");
    public string OkaNameLabel => Get("OkaNameLabel");
    public string OkaNameRequired => Get("OkaNameRequired");
    public string OkaNameDuplicate => Get("OkaNameDuplicate");
    public string OkaDistanceRequired => Get("OkaDistanceRequired");
    public string OkaDampingNonNegative => Get("OkaDampingNonNegative");
    public string EditOka => Get("EditOka");
    public string OkaDampingHint => Get("OkaDampingHint");
    public string HeightHint => Get("HeightHint");
    public string OkaDistanceHint => Get("OkaDistanceHint");
    public string AddOka => Get("AddOka");
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
    public string Frequency => Get("Frequency");
    public string Gain => Get("Gain");
    public string AddBand => Get("AddBand");
    public string VerticalRadiationPattern => Get("VerticalRadiationPattern");
    public string PatternExplanation => Get("PatternExplanation");
    public string AddAtLeastOneBand => Get("AddAtLeastOneBand");
    public string AddNewAntenna => Get("AddNewAntenna");
    public string SelectAntennaPrompt => Get("SelectAntennaPrompt");
    public string TypeToSearch => Get("TypeToSearch");
    public string TypeToSearchDescription => Get("TypeToSearchDescription");
    public string AntennaTypeLabel => Get("AntennaTypeLabel");
    public string GenerateFromGain => Get("GenerateFromGain");

    // ============================================================
    // CABLE EDITOR
    // ============================================================
    public string CableName => Get("CableName");
    public string AttenuationData => Get("AttenuationData");
    public string AttenuationHint => Get("AttenuationHint");
    public string HfBands => Get("HfBands");
    public string VhfUhfShfBands => Get("VhfUhfShfBands");

    // ============================================================
    // RADIO EDITOR
    // ============================================================
    public string RadioDetails => Get("RadioDetails");
    public string MaxPower => Get("MaxPower");
    public string MaxPowerHint => Get("MaxPowerHint");

    // ============================================================
    // MASTER DATA MANAGER
    // ============================================================
    public string Antennas => Get("Antennas");
    public string Cables => Get("Cables");
    public string Radios => Get("Radios");
    public string Modulations => Get("Modulations");
    public string Constants => Get("Constants");
    public string GroundReflectionFactor => Get("GroundReflectionFactor");
    public string DefaultActivityFactor => Get("DefaultActivityFactor");
    public string Factor => Get("Factor");
    public string Name => Get("Name");
    public string AddModulation => Get("AddModulation");
    public string Translations => Get("Translations");
    public string Clear => Get("Clear");
    public string SearchAntennas => Get("SearchAntennas");
    public string SearchCables => Get("SearchCables");
    public string SearchRadios => Get("SearchRadios");
    public string SearchOkas => Get("SearchOkas");
    public string SearchTranslations => Get("SearchTranslations");
    public string AddAntennaButton => Get("AddAntennaButton");
    public string AddCableButton => Get("AddCableButton");
    public string AddRadioButton => Get("AddRadioButton");
    public string Database => Get("Database");
    public string DatabaseTabInfo => Get("DatabaseTabInfo");
    public string FactorySettings => Get("FactorySettings");

    // ============================================================
    // RESULTS
    // ============================================================
    public string None => Get("None");
    public string DistanceAntennaOka => Get("DistanceAntennaOka");
    public string OkaFullName => Get("OkaFullName");
    public string AboveOka => Get("AboveOka");
    public string HorizDistToMast => Get("HorizDistToMast");
    public string CalculationResults => Get("CalculationResults");
    public string Limit => Get("Limit");
    public string ExportMarkdown => Get("ExportMarkdown");
    public string StatusHeader => Get("StatusHeader");

    // Short labels for two-line headers
    public string CableLbl => Get("CableLbl");
    public string PmittelLbl => Get("PmittelLbl");
    public string GainLbl => Get("GainLbl");
    public string VertLbl => Get("VertLbl");
    public string TotGainLbl => Get("TotGainLbl");
    public string EirpLbl => Get("EirpLbl");
    public string BldLbl => Get("BldLbl");
    public string LimitLbl => Get("LimitLbl");
    public string SafeDistLbl => Get("SafeDistLbl");
    public string OkaDistLbl => Get("OkaDistLbl");

    // Calculation Report Labels
    public string CalcTitlePrefix => Get("CalcTitlePrefix");
    public string CalcOperator => Get("CalcOperator");
    public string CalcAddress => Get("CalcAddress");
    public string CalcDate => Get("CalcDate");
    public string CalcTransmitter => Get("CalcTransmitter");
    public string CalcCable => Get("CalcCable");
    public string CalcAntenna => Get("CalcAntenna");
    public string CalcPolarization => Get("CalcPolarization");
    public string CalcRotation => Get("CalcRotation");
    public string CalcLinear => Get("CalcLinear");
    public string CalcOka => Get("CalcOka");
    public string CalcModulation => Get("CalcModulation");
    public string CalcBuildingDamping => Get("CalcBuildingDamping");
    public string CalcStatusCompliant => Get("CalcStatusCompliant");
    public string CalcStatusNonCompliant => Get("CalcStatusNonCompliant");
    public string CalcHorizontal => Get("CalcHorizontal");
    public string CalcVertical => Get("CalcVertical");
    public string CalcFixed => Get("CalcFixed");

    // Calculation Table Row Labels
    public string CalcFrequency => Get("CalcFrequency");
    public string CalcDistanceToAntenna => Get("CalcDistanceToAntenna");
    public string CalcTxPower => Get("CalcTxPower");
    public string CalcActivityFactor => Get("CalcActivityFactor");
    public string CalcModulationFactor => Get("CalcModulationFactor");
    public string CalcMeanPower => Get("CalcMeanPower");
    public string CalcCableAttenuation => Get("CalcCableAttenuation");
    public string CalcAdditionalLosses => Get("CalcAdditionalLosses");
    public string CalcTotalAttenuation => Get("CalcTotalAttenuation");
    public string CalcAttenuationFactor => Get("CalcAttenuationFactor");
    public string CalcAntennaGain => Get("CalcAntennaGain");
    public string CalcVerticalAttenuation => Get("CalcVerticalAttenuation");
    public string CalcTotalAntennaGain => Get("CalcTotalAntennaGain");
    public string CalcGainFactor => Get("CalcGainFactor");
    public string CalcEirp => Get("CalcEirp");
    public string CalcErp => Get("CalcErp");
    public string CalcBuildingDampingRow => Get("CalcBuildingDampingRow");
    public string CalcBuildingDampingFactor => Get("CalcBuildingDampingFactor");
    public string CalcGroundReflection => Get("CalcGroundReflection");
    public string CalcFieldStrength => Get("CalcFieldStrength");
    public string CalcLimit => Get("CalcLimit");
    public string CalcMinSafetyDistance => Get("CalcMinSafetyDistance");
    public string CalcOkaDistance => Get("CalcOkaDistance");

    // Column Explanations
    public string CalcColumnExplanations => Get("CalcColumnExplanations");
    public string CalcExplainF => Get("CalcExplainF");
    public string CalcExplainD => Get("CalcExplainD");
    public string CalcExplainP => Get("CalcExplainP");
    public string CalcExplainAF => Get("CalcExplainAF");
    public string CalcExplainMF => Get("CalcExplainMF");
    public string CalcExplainPm => Get("CalcExplainPm");
    public string CalcExplainA1 => Get("CalcExplainA1");
    public string CalcExplainA2 => Get("CalcExplainA2");
    public string CalcExplainA => Get("CalcExplainA");
    public string CalcExplainAFactor => Get("CalcExplainAFactor");
    public string CalcExplainG1 => Get("CalcExplainG1");
    public string CalcExplainG2 => Get("CalcExplainG2");
    public string CalcExplainG => Get("CalcExplainG");
    public string CalcExplainGFactor => Get("CalcExplainGFactor");
    public string CalcExplainPs => Get("CalcExplainPs");
    public string CalcExplainPsPrime => Get("CalcExplainPsPrime");
    public string CalcExplainAg => Get("CalcExplainAg");
    public string CalcExplainAG => Get("CalcExplainAG");
    public string CalcExplainKr => Get("CalcExplainKr");
    public string CalcExplainE => Get("CalcExplainE");
    public string CalcExplainEigw => Get("CalcExplainEigw");
    public string CalcExplainDs => Get("CalcExplainDs");

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
    // PROJECT MANAGEMENT
    // ============================================================
    public string NoProjects => Get("NoProjects");
    public string DeleteProjectConfirm => Get("DeleteProjectConfirm");
    public string DeleteProjectMessage => Get("DeleteProjectMessage");
    public string ProjectDeleted => Get("ProjectDeleted");

    // ============================================================
    // IMPORT/EXPORT
    // ============================================================
    public string ExportFactoryData => Get("ExportFactoryData");
    public string ImportFactoryData => Get("ImportFactoryData");
    public string ExportFactoryDataDesc => Get("ExportFactoryDataDesc");
    public string ImportDatabaseDesc => Get("ImportDatabaseDesc");
    public string ImportFactoryConfirmMessage => Get("ImportFactoryConfirmMessage");
    public string OpenDataFolder => Get("OpenDataFolder");
    public string DataFolder => Get("DataFolder");
    public string DataFolderDesc => Get("DataFolderDesc");
    public string Factory => Get("Factory");
    public string FactoryMode => Get("FactoryMode");
    public string EnterFactoryPassword => Get("EnterFactoryPassword");
    public string WrongPassword => Get("WrongPassword");
    public string ImportProject => Get("ImportProject");
    public string ExportProject => Get("ExportProject");
    public string ImportProjectConfirmMessage => Get("ImportProjectConfirmMessage");
    public string ProjectImportExportDesc => Get("ProjectImportExportDesc");
    public string ExportSuccess => Get("ExportSuccess");
    public string ExportFailed => Get("ExportFailed");
    public string ImportSuccess => Get("ImportSuccess");
    public string ImportFailed => Get("ImportFailed");
    public string ImportUserData => Get("ImportUserData");
    public string ExportUserData => Get("ExportUserData");
    public string ImportConfirmMessage => Get("ImportConfirmMessage");
    public string SelectProjectToExport => Get("SelectProjectToExport");

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
        ["ImportExport"] = "Welcome",
        ["NisvCompliance"] = "Welcome",
        // ProjectInfo
        ["ProjectInfo"] = "ProjectInfo",
        ["ProjectName"] = "ProjectInfo",
        ["StationInfo"] = "ProjectInfo",
        ["Operator"] = "ProjectInfo",
        ["Callsign"] = "ProjectInfo",
        ["Address"] = "ProjectInfo",
        ["Location"] = "ProjectInfo",
        ["CreateProject"] = "ProjectInfo",
        ["EditStationInfo"] = "ProjectInfo",
        // ProjectOverview
        ["Configurations"] = "ProjectOverview",
        ["Configuration"] = "ProjectOverview",
        ["AddConfiguration"] = "ProjectOverview",
        ["CalculateAll"] = "ProjectOverview",
        ["ExportReport"] = "ProjectOverview",
        ["ExportPdf"] = "ProjectOverview",
        ["NoConfigurations"] = "ProjectOverview",
        ["SearchProjects"] = "ProjectOverview",
        ["NoSearchResults"] = "ProjectOverview",
        ["SortBy"] = "ProjectOverview",
        ["SortByName"] = "ProjectOverview",
        ["SortByModified"] = "ProjectOverview",
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
        ["Linear"] = "ConfigEditor",
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
        // AntennaEditor
        ["AntennaDetails"] = "AntennaEditor",
        ["Manufacturer"] = "AntennaEditor",
        ["Model"] = "AntennaEditor",
        ["Polarization"] = "AntennaEditor",
        ["Horizontal"] = "AntennaEditor",
        ["Vertical"] = "AntennaEditor",
        ["Rotatable"] = "AntennaEditor",
        ["Frequency"] = "AntennaEditor",
        ["Gain"] = "AntennaEditor",
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
        ["AntennaTypeLabel"] = "AntennaEditor",
        ["GenerateFromGain"] = "AntennaEditor",
        ["GenerateAllPatterns"] = "AntennaEditor",
        // CableEditor
        ["CableName"] = "CableEditor",
        ["AttenuationData"] = "CableEditor",
        ["AttenuationHint"] = "CableEditor",
        ["HfBands"] = "CableEditor",
        ["VhfUhfShfBands"] = "CableEditor",
        // RadioEditor
        ["RadioDetails"] = "RadioEditor",
        ["MaxPower"] = "RadioEditor",
        ["MaxPowerHint"] = "RadioEditor",
        // MasterData
        ["Antennas"] = "MasterData",
        ["Cables"] = "MasterData",
        ["Radios"] = "MasterData",
        ["Modulations"] = "MasterData",
        ["Constants"] = "MasterData",
        ["GroundReflectionFactor"] = "MasterData",
        ["DefaultActivityFactor"] = "MasterData",
        ["Factor"] = "MasterData",
        ["Name"] = "MasterData",
        ["AddModulation"] = "MasterData",
        ["Translations"] = "MasterData",
        ["MasterDataManager"] = "MasterData",
        ["Clear"] = "MasterData",
        ["SearchAntennas"] = "MasterData",
        ["SearchCables"] = "MasterData",
        ["SearchRadios"] = "MasterData",
        ["SearchOkas"] = "MasterData",
        ["SearchTranslations"] = "MasterData",
        ["AddAntennaButton"] = "MasterData",
        ["AddCableButton"] = "MasterData",
        ["AddRadioButton"] = "MasterData",
        ["Database"] = "MasterData",
        ["DatabaseTabInfo"] = "MasterData",
        ["FactorySettings"] = "MasterData",
        ["ExportDatabase"] = "MasterData",
        ["ExportDatabaseDesc"] = "MasterData",
        ["ImportDatabase"] = "MasterData",
        ["ImportDatabaseDesc"] = "MasterData",
        // Results
        ["DistanceAntennaOka"] = "Results",
        ["CalculationResults"] = "Results",
        ["Limit"] = "Results",
        ["ExportMarkdown"] = "Results",
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
        ["Modified"] = new() { ["de"] = "Geändert", ["fr"] = "Modifié", ["it"] = "Modificato", ["en"] = "Modified" },
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
        ["Projects"] = new() { ["de"] = "Projekte", ["fr"] = "Projets", ["it"] = "Progetti", ["en"] = "Projects" },
        ["File"] = new() { ["de"] = "Datei", ["fr"] = "Fichier", ["it"] = "File", ["en"] = "File" },
        ["Project"] = new() { ["de"] = "Projekt", ["fr"] = "Projet", ["it"] = "Progetto", ["en"] = "Project" },
        ["Settings"] = new() { ["de"] = "Einstellungen", ["fr"] = "Paramètres", ["it"] = "Impostazioni", ["en"] = "Settings" },
        ["ImportExport"] = new() { ["de"] = "Import/Export", ["fr"] = "Import/Export", ["it"] = "Import/Export", ["en"] = "Import/Export" },

        // Project Info
        ["ProjectInfo"] = new() { ["de"] = "Projektinformationen", ["fr"] = "Informations projet", ["it"] = "Informazioni progetto", ["en"] = "Project Information" },
        ["ProjectName"] = new() { ["de"] = "Projektname", ["fr"] = "Nom du projet", ["it"] = "Nome progetto", ["en"] = "Project Name" },
        ["StationInfo"] = new() { ["de"] = "Stationsinformationen", ["fr"] = "Informations station", ["it"] = "Informazioni stazione", ["en"] = "Station Information" },
        ["Operator"] = new() { ["de"] = "Betreiber", ["fr"] = "Opérateur", ["it"] = "Operatore", ["en"] = "Operator" },
        ["Address"] = new() { ["de"] = "Adresse", ["fr"] = "Adresse", ["it"] = "Indirizzo", ["en"] = "Address" },
        ["Location"] = new() { ["de"] = "Ort", ["fr"] = "Localité", ["it"] = "Località", ["en"] = "Location" },
        ["CreateProject"] = new() { ["de"] = "Projekt erstellen", ["fr"] = "Créer projet", ["it"] = "Crea progetto", ["en"] = "Create Project" },
        ["EditStationInfo"] = new() { ["de"] = "Stationsinfo bearbeiten", ["fr"] = "Modifier info station", ["it"] = "Modifica info stazione", ["en"] = "Edit Station Info" },

        // Project Overview
        ["Configurations"] = new() { ["de"] = "Konfigurationen", ["fr"] = "Configurations", ["it"] = "Configurazioni", ["en"] = "Configurations" },
        ["Configuration"] = new() { ["de"] = "Konfiguration", ["fr"] = "Configuration", ["it"] = "Configurazione", ["en"] = "Configuration" },
        ["AddConfiguration"] = new() { ["de"] = "+ Konfiguration hinzufügen", ["fr"] = "+ Ajouter configuration", ["it"] = "+ Aggiungi configurazione", ["en"] = "+ Add Configuration" },
        ["CalculateAll"] = new() { ["de"] = "Alle berechnen", ["fr"] = "Tout calculer", ["it"] = "Calcola tutto", ["en"] = "Calculate All" },
        ["ExportReport"] = new() { ["de"] = "Bericht exportieren", ["fr"] = "Exporter rapport", ["it"] = "Esporta rapporto", ["en"] = "Export Report" },
        ["ExportPdf"] = new() { ["de"] = "PDF exportieren", ["fr"] = "Exporter PDF", ["it"] = "Esporta PDF", ["en"] = "Export PDF" },
        ["NoConfigurations"] = new() { ["de"] = "Keine Konfigurationen. Klicken Sie auf '+ Konfiguration hinzufügen'.", ["fr"] = "Aucune configuration. Cliquez sur '+ Ajouter configuration'.", ["it"] = "Nessuna configurazione. Clicca su '+ Aggiungi configurazione'.", ["en"] = "No configurations. Click '+ Add Configuration' to get started." },
        ["SearchProjects"] = new() { ["de"] = "Projekte suchen...", ["fr"] = "Rechercher projets...", ["it"] = "Cerca progetti...", ["en"] = "Search projects..." },
        ["NoSearchResults"] = new() { ["de"] = "Keine Projekte gefunden.", ["fr"] = "Aucun projet trouvé.", ["it"] = "Nessun progetto trovato.", ["en"] = "No projects found." },
        ["SortBy"] = new() { ["de"] = "Sortieren nach", ["fr"] = "Trier par", ["it"] = "Ordina per", ["en"] = "Sort by" },
        ["SortByName"] = new() { ["de"] = "Name", ["fr"] = "Nom", ["it"] = "Nome", ["en"] = "Name" },
        ["SortByModified"] = new() { ["de"] = "Zuletzt geändert", ["fr"] = "Dernière modification", ["it"] = "Ultima modifica", ["en"] = "Last modified" },

        // Configuration Editor
        ["Antenna"] = new() { ["de"] = "Antenne", ["fr"] = "Antenne", ["it"] = "Antenna", ["en"] = "Antenna" },
        ["Transmitter"] = new() { ["de"] = "Sender", ["fr"] = "Émetteur", ["it"] = "Trasmettitore", ["en"] = "Transmitter" },
        ["FeedLine"] = new() { ["de"] = "Speiseleitung", ["fr"] = "Ligne d'alimentation", ["it"] = "Linea di alimentazione", ["en"] = "Feed Line" },
        ["OperatingParameters"] = new() { ["de"] = "Betriebsparameter", ["fr"] = "Paramètres d'exploitation", ["it"] = "Parametri operativi", ["en"] = "Operating Parameters" },
        ["EvaluationPoint"] = new() { ["de"] = "Beurteilungspunkt (OKA)", ["fr"] = "Point d'évaluation (LSM)", ["it"] = "Punto di valutazione (LST)", ["en"] = "Evaluation Point (PSS)" },

        ["SelectAntenna"] = new() { ["de"] = "-- Antenne wählen --", ["fr"] = "-- Sélectionner antenne --", ["it"] = "-- Seleziona antenna --", ["en"] = "-- Select Antenna --" },
        ["Height"] = new() { ["de"] = "Höhe", ["fr"] = "Hauteur", ["it"] = "Altezza", ["en"] = "Height" },
        ["FrequencyBands"] = new() { ["de"] = "Frequenzbänder", ["fr"] = "Bandes de fréquence", ["it"] = "Bande di frequenza", ["en"] = "Frequency Bands" },
        ["Bands"] = new() { ["de"] = "Bänder", ["fr"] = "Bandes", ["it"] = "Bande", ["en"] = "Bands" },

        ["SelectRadio"] = new() { ["de"] = "-- Transceiver wählen --", ["fr"] = "-- Sélectionner émetteur --", ["it"] = "-- Seleziona ricetrasmettitore --", ["en"] = "-- Select Radio --" },
        ["Linear"] = new() { ["de"] = "Endstufe", ["fr"] = "Amplificateur", ["it"] = "Amplificatore", ["en"] = "Linear" },
        ["Power"] = new() { ["de"] = "Leistung", ["fr"] = "Puissance", ["it"] = "Potenza", ["en"] = "Power" },

        ["Cable"] = new() { ["de"] = "Kabel", ["fr"] = "Câble", ["it"] = "Cavo", ["en"] = "Cable" },
        ["SelectCable"] = new() { ["de"] = "-- Kabel wählen --", ["fr"] = "-- Sélectionner câble --", ["it"] = "-- Seleziona cavo --", ["en"] = "-- Select Cable --" },
        ["CableLength"] = new() { ["de"] = "Kabellänge", ["fr"] = "Longueur câble", ["it"] = "Lunghezza cavo", ["en"] = "Cable Length" },
        ["AdditionalLoss"] = new() { ["de"] = "Zusätzliche Dämpfung", ["fr"] = "Pertes supplémentaires", ["it"] = "Perdite aggiuntive", ["en"] = "Additional Loss" },

        ["Modulation"] = new() { ["de"] = "Modulation", ["fr"] = "Modulation", ["it"] = "Modulazione", ["en"] = "Modulation" },
        ["ActivityFactor"] = new() { ["de"] = "Aktivitätsfaktor", ["fr"] = "Facteur d'activité", ["it"] = "Fattore di attività", ["en"] = "Activity Factor" },
        ["ParametersApplyToAllBands"] = new() { ["de"] = "Diese Parameter gelten für alle Bänder der gewählten Antenne.", ["fr"] = "Ces paramètres s'appliquent à toutes les bandes de l'antenne sélectionnée.", ["it"] = "Questi parametri si applicano a tutte le bande dell'antenna selezionata.", ["en"] = "These parameters apply to all bands of the selected antenna." },

        ["OkaDistance"] = new() { ["de"] = "Distanz (m)", ["fr"] = "Distance (m)", ["it"] = "Distanza (m)", ["en"] = "Distance (m)" },
        ["OkaDamping"] = new() { ["de"] = "Gebäudedämpfung (dB)", ["fr"] = "Atténuation bâtiment (dB)", ["it"] = "Attenuazione edificio (dB)", ["en"] = "Building Damping (dB)" },
        ["OkaExplanation"] = new() { ["de"] = "OKA = Ort für kurzfristigen Aufenthalt", ["fr"] = "LSM = Lieu de séjour momentané", ["it"] = "LST = Luogo di soggiorno temporaneo", ["en"] = "PSS = Place of Short-Term Stay" },

        // OKA Master Editor (OKA=DE, LSM=FR, LST=IT, PSS=EN)
        ["OkaDetails"] = new() { ["de"] = "OKA Details", ["fr"] = "Détails LSM", ["it"] = "Dettagli LST", ["en"] = "PSS Details" },
        ["OkaNameLabel"] = new() { ["de"] = "Bezeichnung", ["fr"] = "Désignation", ["it"] = "Designazione", ["en"] = "Name" },
        ["OkaNameRequired"] = new() { ["de"] = "Bitte eine Bezeichnung eingeben.", ["fr"] = "Veuillez entrer une désignation.", ["it"] = "Inserire una designazione.", ["en"] = "Please enter a name." },
        ["OkaNameDuplicate"] = new() { ["de"] = "Diese Bezeichnung existiert bereits.", ["fr"] = "Cette désignation existe déjà.", ["it"] = "Questa designazione esiste già.", ["en"] = "This name already exists." },
        ["OkaDistanceRequired"] = new() { ["de"] = "Die Distanz muss grösser als 0 sein.", ["fr"] = "La distance doit être supérieure à 0.", ["it"] = "La distanza deve essere maggiore di 0.", ["en"] = "Distance must be greater than 0." },
        ["OkaDampingNonNegative"] = new() { ["de"] = "Die Dämpfung darf nicht negativ sein.", ["fr"] = "L'atténuation ne peut pas être négative.", ["it"] = "L'attenuazione non può essere negativa.", ["en"] = "Damping cannot be negative." },
        ["OkaDampingHint"] = new() { ["de"] = "0 dB für Aussenbereich, typisch 6-12 dB für Innenräume", ["fr"] = "0 dB pour l'extérieur, typiquement 6-12 dB pour l'intérieur", ["it"] = "0 dB per l'esterno, tipicamente 6-12 dB per interni", ["en"] = "0 dB for outdoor, typically 6-12 dB for indoor" },
        ["HeightHint"] = new() { ["de"] = "Antennenhöhe über OKA", ["fr"] = "Hauteur de l'antenne au-dessus du LSM", ["it"] = "Altezza dell'antenna sopra LST", ["en"] = "Antenna height above PSS" },
        ["OkaDistanceHint"] = new() { ["de"] = "Horizontale Distanz vom OKA zum Antennenmast", ["fr"] = "Distance horizontale du LSM au mât d'antenne", ["it"] = "Distanza orizzontale dal LST al palo dell'antenna", ["en"] = "Horizontal distance from PSS to antenna mast" },
        ["AddOka"] = new() { ["de"] = "OKA hinzufügen", ["fr"] = "Ajouter LSM", ["it"] = "Aggiungi LST", ["en"] = "Add PSS" },
        ["EditOka"] = new() { ["de"] = "OKA bearbeiten", ["fr"] = "Modifier LSM", ["it"] = "Modifica LST", ["en"] = "Edit PSS" },
        ["SelectOka"] = new() { ["de"] = "-- OKA wählen --", ["fr"] = "-- Sélectionner LSM --", ["it"] = "-- Seleziona LST --", ["en"] = "-- Select PSS --" },

        // Antenna Editor
        ["AntennaDetails"] = new() { ["de"] = "Antennendetails", ["fr"] = "Détails antenne", ["it"] = "Dettagli antenna", ["en"] = "Antenna Details" },
        ["Manufacturer"] = new() { ["de"] = "Hersteller", ["fr"] = "Fabricant", ["it"] = "Produttore", ["en"] = "Manufacturer" },
        ["Model"] = new() { ["de"] = "Modell", ["fr"] = "Modèle", ["it"] = "Modello", ["en"] = "Model" },
        ["Polarization"] = new() { ["de"] = "Polarisation", ["fr"] = "Polarisation", ["it"] = "Polarizzazione", ["en"] = "Polarization" },
        ["Horizontal"] = new() { ["de"] = "Horizontal", ["fr"] = "Horizontal", ["it"] = "Orizzontale", ["en"] = "Horizontal" },
        ["Vertical"] = new() { ["de"] = "Vertikal", ["fr"] = "Vertical", ["it"] = "Verticale", ["en"] = "Vertical" },
        ["Rotatable"] = new() { ["de"] = "Drehbar", ["fr"] = "Rotatif", ["it"] = "Rotabile", ["en"] = "Rotatable" },
        ["Frequency"] = new() { ["de"] = "Frequenz", ["fr"] = "Fréquence", ["it"] = "Frequenza", ["en"] = "Frequency" },
        ["Gain"] = new() { ["de"] = "Gewinn", ["fr"] = "Gain", ["it"] = "Guadagno", ["en"] = "Gain" },
        ["AddBand"] = new() { ["de"] = "+ Band hinzufügen", ["fr"] = "+ Ajouter bande", ["it"] = "+ Aggiungi banda", ["en"] = "+ Add Band" },
        ["VerticalRadiationPattern"] = new() { ["de"] = "Vertikales Strahlungsdiagramm (Dämpfung in dB je Winkel vom Horizont)", ["fr"] = "Diagramme de rayonnement vertical (atténuation en dB par angle depuis l'horizon)", ["it"] = "Diagramma di radiazione verticale (attenuazione in dB per angolo dall'orizzonte)", ["en"] = "Vertical Radiation Pattern (attenuation in dB at each angle from horizon)" },
        ["PatternExplanation"] = new() { ["de"] = "0° = Horizont (max. Abstrahlung), 90° = Senkrecht nach unten (Richtung OKA). Werte: 0-60 dB Dämpfung.", ["fr"] = "0° = Horizon (rayonnement max), 90° = Verticalement vers le bas (direction LSM). Valeurs: 0-60 dB d'atténuation.", ["it"] = "0° = Orizzonte (radiazione max), 90° = Verticalmente verso il basso (direzione LSBD). Valori: 0-60 dB attenuazione.", ["en"] = "0° = Horizon (max radiation), 90° = Straight down (toward OKA). Values: 0-60 dB attenuation." },
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
        ["AntennaTypeLabel"] = new() { ["de"] = "Antennentyp", ["fr"] = "Type d'antenne", ["it"] = "Tipo di antenna", ["en"] = "Antenna Type" },
        ["GenerateFromGain"] = new() { ["de"] = "Automatisch berechnen", ["fr"] = "Calculer automatiquement", ["it"] = "Calcola automaticamente", ["en"] = "Auto-calculate" },
        ["GenerateAllPatterns"] = new() { ["de"] = "Strahlungsdiagramme aus Gewinn berechnen", ["fr"] = "Calculer diagrammes depuis gain", ["it"] = "Calcola diagrammi da guadagno", ["en"] = "Generate Patterns from Gain" },

        // Cable Editor
        ["CableName"] = new() { ["de"] = "Kabelname", ["fr"] = "Nom du câble", ["it"] = "Nome cavo", ["en"] = "Cable Name" },
        ["AttenuationData"] = new() { ["de"] = "Dämpfungsdaten (dB pro 100m)", ["fr"] = "Données d'atténuation (dB par 100m)", ["it"] = "Dati attenuazione (dB per 100m)", ["en"] = "Attenuation Data (dB per 100m)" },
        ["AttenuationHint"] = new() { ["de"] = "Geben Sie die Dämpfungswerte für die verfügbaren Frequenzen ein. Leer lassen wenn unbekannt.", ["fr"] = "Entrez les valeurs d'atténuation pour les fréquences disponibles. Laissez vide si inconnu.", ["it"] = "Inserisci i valori di attenuazione per le frequenze disponibili. Lascia vuoto se sconosciuto.", ["en"] = "Enter attenuation values for the frequencies you have data for. Leave empty if unknown." },
        ["HfBands"] = new() { ["de"] = "KW-Bänder", ["fr"] = "Bandes HF", ["it"] = "Bande HF", ["en"] = "HF Bands" },
        ["VhfUhfShfBands"] = new() { ["de"] = "VHF / UHF / SHF Bänder", ["fr"] = "Bandes VHF / UHF / SHF", ["it"] = "Bande VHF / UHF / SHF", ["en"] = "VHF / UHF / SHF Bands" },

        // Radio Editor
        ["RadioDetails"] = new() { ["de"] = "Transceiver-Details", ["fr"] = "Détails émetteur-récepteur", ["it"] = "Dettagli ricetrasmettitore", ["en"] = "Radio / Transceiver Details" },
        ["MaxPower"] = new() { ["de"] = "Max. Leistung", ["fr"] = "Puissance max.", ["it"] = "Potenza max.", ["en"] = "Max Power" },
        ["MaxPowerHint"] = new() { ["de"] = "W (maximale Ausgangsleistung)", ["fr"] = "W (puissance de sortie maximale)", ["it"] = "W (potenza di uscita massima)", ["en"] = "W (maximum output power)" },

        // Master Data Manager
        ["Antennas"] = new() { ["de"] = "Antennen", ["fr"] = "Antennes", ["it"] = "Antenne", ["en"] = "Antennas" },
        ["Cables"] = new() { ["de"] = "Kabel", ["fr"] = "Câbles", ["it"] = "Cavi", ["en"] = "Cables" },
        ["Radios"] = new() { ["de"] = "Transceiver", ["fr"] = "Émetteurs-récepteurs", ["it"] = "Ricetrasmettitori", ["en"] = "Radios" },
        ["Modulations"] = new() { ["de"] = "Modulationen", ["fr"] = "Modulations", ["it"] = "Modulazioni", ["en"] = "Modulations" },
        ["Constants"] = new() { ["de"] = "Konstanten", ["fr"] = "Constantes", ["it"] = "Costanti", ["en"] = "Constants" },
        ["GroundReflectionFactor"] = new() { ["de"] = "Bodenreflexionsfaktor", ["fr"] = "Facteur de réflexion au sol", ["it"] = "Fattore di riflessione a terra", ["en"] = "Ground Reflection Factor" },
        ["DefaultActivityFactor"] = new() { ["de"] = "Standard-Aktivitätsfaktor", ["fr"] = "Facteur d'activité par défaut", ["it"] = "Fattore di attività predefinito", ["en"] = "Default Activity Factor" },
        ["Factor"] = new() { ["de"] = "Faktor", ["fr"] = "Facteur", ["it"] = "Fattore", ["en"] = "Factor" },
        ["Name"] = new() { ["de"] = "Name", ["fr"] = "Nom", ["it"] = "Nome", ["en"] = "Name" },
        ["AddModulation"] = new() { ["de"] = "+ Modulation hinzufügen", ["fr"] = "+ Ajouter modulation", ["it"] = "+ Aggiungi modulazione", ["en"] = "+ Add Modulation" },
        ["Translations"] = new() { ["de"] = "Übersetzungen", ["fr"] = "Traductions", ["it"] = "Traduzioni", ["en"] = "Translations" },
        ["MasterDataManager"] = new() { ["de"] = "Stammdaten-Verwaltung", ["fr"] = "Gestion des données de base", ["it"] = "Gestione dati master", ["en"] = "Master Data Manager" },
        ["Clear"] = new() { ["de"] = "Löschen", ["fr"] = "Effacer", ["it"] = "Cancella", ["en"] = "Clear" },
        ["SearchAntennas"] = new() { ["de"] = "Hersteller oder Modell suchen...", ["fr"] = "Rechercher fabricant ou modèle...", ["it"] = "Cerca produttore o modello...", ["en"] = "Search by manufacturer or model..." },
        ["SearchCables"] = new() { ["de"] = "Kabelname suchen...", ["fr"] = "Rechercher nom du câble...", ["it"] = "Cerca nome cavo...", ["en"] = "Search by cable name..." },
        ["SearchRadios"] = new() { ["de"] = "Hersteller oder Modell suchen...", ["fr"] = "Rechercher fabricant ou modèle...", ["it"] = "Cerca produttore o modello...", ["en"] = "Search by manufacturer or model..." },
        ["SearchOkas"] = new() { ["de"] = "OKA suchen...", ["fr"] = "Rechercher LSM...", ["it"] = "Cerca LST...", ["en"] = "Search PSS..." },
        ["SearchTranslations"] = new() { ["de"] = "Übersetzungen suchen...", ["fr"] = "Rechercher traductions...", ["it"] = "Cerca traduzioni...", ["en"] = "Search translations..." },
        ["AddAntennaButton"] = new() { ["de"] = "+ Antenne hinzufügen", ["fr"] = "+ Ajouter antenne", ["it"] = "+ Aggiungi antenna", ["en"] = "+ Add Antenna" },
        ["AddCableButton"] = new() { ["de"] = "+ Kabel hinzufügen", ["fr"] = "+ Ajouter câble", ["it"] = "+ Aggiungi cavo", ["en"] = "+ Add Cable" },
        ["AddRadioButton"] = new() { ["de"] = "+ Transceiver hinzufügen", ["fr"] = "+ Ajouter émetteur", ["it"] = "+ Aggiungi ricetrasmettitore", ["en"] = "+ Add Radio" },

        // Database (Factory Mode)
        ["Database"] = new() { ["de"] = "Datenbank", ["fr"] = "Base de données", ["it"] = "Database", ["en"] = "Database" },
        ["DatabaseTabInfo"] = new() { ["de"] = "Änderungen an der Datenbank betreffen alle Benutzer. Nur für Wartung und Updates verwenden.", ["fr"] = "Les modifications de la base de données affectent tous les utilisateurs. Utiliser uniquement pour la maintenance.", ["it"] = "Le modifiche al database influenzano tutti gli utenti. Usare solo per manutenzione.", ["en"] = "Database changes affect all users. Use only for maintenance and updates." },
        ["FactorySettings"] = new() { ["de"] = "Werkseinstellungen", ["fr"] = "Paramètres usine", ["it"] = "Impostazioni di fabbrica", ["en"] = "Factory Settings" },
        ["ExportDatabase"] = new() { ["de"] = "Datenbank exportieren", ["fr"] = "Exporter la base de données", ["it"] = "Esporta database", ["en"] = "Export Database" },
        ["ExportDatabaseDesc"] = new() { ["de"] = "Exportiert die Werksdaten in eine JSON-Datei", ["fr"] = "Exporte les données usine dans un fichier JSON", ["it"] = "Esporta i dati di fabbrica in un file JSON", ["en"] = "Export factory data to a JSON file" },
        ["ImportDatabase"] = new() { ["de"] = "Datenbank importieren", ["fr"] = "Importer la base de données", ["it"] = "Importa database", ["en"] = "Import Database" },
        ["ImportDatabaseDesc"] = new() { ["de"] = "Importiert Werksdaten aus einer JSON-Datei. ACHTUNG: Alle Daten werden gelöscht!", ["fr"] = "Importe les données usine depuis un fichier JSON. ATTENTION: Toutes les données seront supprimées!", ["it"] = "Importa i dati di fabbrica da un file JSON. ATTENZIONE: Tutti i dati saranno eliminati!", ["en"] = "Import factory data from a JSON file. WARNING: All data will be deleted!" },

        // Results
        ["None"] = new() { ["de"] = "Keine", ["fr"] = "Aucun", ["it"] = "Nessuno", ["en"] = "None" },
        ["DistanceAntennaOka"] = new() { ["de"] = "Distanz Antenne-OKA", ["fr"] = "Distance Antenne-LSM", ["it"] = "Distanza Antenna-LST", ["en"] = "Distance Antenna-PSS" },
        ["OkaFullName"] = new() { ["de"] = "Ort für kurzfristigen Aufenthalt", ["fr"] = "Lieu de séjour momentané", ["it"] = "Luogo di soggiorno temporaneo", ["en"] = "Place of Short-Term Stay" },
        ["AboveOka"] = new() { ["de"] = "über OKA", ["fr"] = "au-dessus du LSM", ["it"] = "sopra LST", ["en"] = "above PSS" },
        ["HorizDistToMast"] = new() { ["de"] = "horizontale Distanz zum Antennenmast", ["fr"] = "distance horizontale au mât", ["it"] = "distanza orizzontale al palo", ["en"] = "horizontal distance to antenna mast" },
        ["CalculationResults"] = new() { ["de"] = "Berechnungsergebnisse", ["fr"] = "Résultats du calcul", ["it"] = "Risultati del calcolo", ["en"] = "Calculation Results" },
        ["Limit"] = new() { ["de"] = "Grenzwert", ["fr"] = "Limite", ["it"] = "Limite", ["en"] = "Limit" },
        ["ExportMarkdown"] = new() { ["de"] = "Markdown exportieren", ["fr"] = "Exporter Markdown", ["it"] = "Esporta Markdown", ["en"] = "Export Markdown" },
        ["StatusHeader"] = new() { ["de"] = "Status", ["fr"] = "Statut", ["it"] = "Stato", ["en"] = "Status" },

        // Short labels for two-line headers
        ["CableLbl"] = new() { ["de"] = "Kabel", ["fr"] = "Câble", ["it"] = "Cavo", ["en"] = "Cable" },
        ["PmittelLbl"] = new() { ["de"] = "Pmittel", ["fr"] = "Pmoy", ["it"] = "Pmedia", ["en"] = "Pmean" },
        ["GainLbl"] = new() { ["de"] = "Gew.", ["fr"] = "Gain", ["it"] = "Guad.", ["en"] = "Gain" },
        ["VertLbl"] = new() { ["de"] = "Vert.", ["fr"] = "Vert.", ["it"] = "Vert.", ["en"] = "Vert." },
        ["TotGainLbl"] = new() { ["de"] = "Tot.Gew.", ["fr"] = "Gain tot.", ["it"] = "Guad.tot.", ["en"] = "Tot.Gain" },
        ["EirpLbl"] = new() { ["de"] = "EIRP", ["fr"] = "PIRE", ["it"] = "EIRP", ["en"] = "EIRP" },
        ["BldLbl"] = new() { ["de"] = "Geb.", ["fr"] = "Bât.", ["it"] = "Ed.", ["en"] = "Bld." },
        ["LimitLbl"] = new() { ["de"] = "Grenzw.", ["fr"] = "Limite", ["it"] = "Limite", ["en"] = "Limit" },
        ["SafeDistLbl"] = new() { ["de"] = "Sich.D.", ["fr"] = "Dist.S.", ["it"] = "Dist.S.", ["en"] = "Safe D." },
        ["OkaDistLbl"] = new() { ["de"] = "OKA D.", ["fr"] = "LSM D.", ["it"] = "LSBD D.", ["en"] = "OKA D." },

        // Validation

        // Dialogs
        ["UnsavedChanges"] = new() { ["de"] = "Ungespeicherte Änderungen", ["fr"] = "Modifications non enregistrées", ["it"] = "Modifiche non salvate", ["en"] = "Unsaved Changes" },
        ["DiscardChanges"] = new() { ["de"] = "Änderungen verwerfen?", ["fr"] = "Annuler les modifications?", ["it"] = "Annullare le modifiche?", ["en"] = "Discard Changes?" },
        ["DiscardChangesPrompt"] = new() { ["de"] = "Sie haben ungespeicherte Änderungen. Möchten Sie diese verwerfen?", ["fr"] = "Vous avez des modifications non enregistrées. Voulez-vous les annuler?", ["it"] = "Hai modifiche non salvate. Vuoi annullarle?", ["en"] = "You have unsaved changes. Do you want to discard them?" },
        ["ChangesDiscarded"] = new() { ["de"] = "Änderungen verworfen.", ["fr"] = "Modifications annulées.", ["it"] = "Modifiche annullate.", ["en"] = "Changes discarded." },

        // Project Management
        ["LoadDemoProject"] = new() { ["de"] = "Demo-Projekt laden", ["fr"] = "Charger projet démo", ["it"] = "Carica progetto demo", ["en"] = "Load Demo Project" },
        ["LoadDemoProjectDesc"] = new() { ["de"] = "Lädt ein Beispielprojekt zum Ausprobieren", ["fr"] = "Charge un projet exemple pour essayer", ["it"] = "Carica un progetto di esempio per provare", ["en"] = "Load a sample project to try out" },
        ["NoProjects"] = new() { ["de"] = "Keine Projekte vorhanden", ["fr"] = "Aucun projet disponible", ["it"] = "Nessun progetto disponibile", ["en"] = "No projects available" },
        ["DeleteProject"] = new() { ["de"] = "Projekt löschen", ["fr"] = "Supprimer projet", ["it"] = "Elimina progetto", ["en"] = "Delete Project" },
        ["DeleteProjectConfirm"] = new() { ["de"] = "Projekt wirklich löschen?", ["fr"] = "Vraiment supprimer le projet?", ["it"] = "Eliminare davvero il progetto?", ["en"] = "Really delete project?" },
        ["DeleteProjectMessage"] = new() { ["de"] = "Das Projekt '{0}' wird unwiderruflich gelöscht.", ["fr"] = "Le projet '{0}' sera supprimé définitivement.", ["it"] = "Il progetto '{0}' verrà eliminato definitivamente.", ["en"] = "The project '{0}' will be permanently deleted." },
        ["ProjectDeleted"] = new() { ["de"] = "Projekt gelöscht", ["fr"] = "Projet supprimé", ["it"] = "Progetto eliminato", ["en"] = "Project deleted" },
        ["DemoProjectLoaded"] = new() { ["de"] = "Demo-Projekt geladen", ["fr"] = "Projet démo chargé", ["it"] = "Progetto demo caricato", ["en"] = "Demo project loaded" },
        ["Callsign"] = new() { ["de"] = "Rufzeichen", ["fr"] = "Indicatif", ["it"] = "Nominativo", ["en"] = "Callsign" },

        // User Data Export/Import
        ["ExportUserData"] = new() { ["de"] = "Benutzerdaten exportieren", ["fr"] = "Exporter données utilisateur", ["it"] = "Esporta dati utente", ["en"] = "Export User Data" },
        ["ImportUserData"] = new() { ["de"] = "Benutzerdaten importieren", ["fr"] = "Importer données utilisateur", ["it"] = "Importa dati utente", ["en"] = "Import User Data" },
        ["ExportUserDataDesc"] = new() { ["de"] = "Exportiert alle Projekte, OKAs und benutzerdefinierte Stammdaten", ["fr"] = "Exporte tous les projets, LSMs et données de base personnalisées", ["it"] = "Esporta tutti i progetti, LST e dati master personalizzati", ["en"] = "Export all projects, PSSs, and custom master data" },
        ["ImportUserDataDesc"] = new() { ["de"] = "Importiert Benutzerdaten aus einer Sicherungsdatei", ["fr"] = "Importe les données utilisateur depuis un fichier de sauvegarde", ["it"] = "Importa i dati utente da un file di backup", ["en"] = "Import user data from a backup file" },
        ["ImportConfirmMessage"] = new() { ["de"] = "Dies löscht alle bestehenden Daten und ersetzt sie durch das Backup. Fortfahren?", ["fr"] = "Cela supprime toutes les données existantes et les remplace par la sauvegarde. Continuer?", ["it"] = "Questo elimina tutti i dati esistenti e li sostituisce con il backup. Continuare?", ["en"] = "This deletes all existing data and replaces it with the backup. Continue?" },
        ["ExportSuccess"] = new() { ["de"] = "Export erfolgreich", ["fr"] = "Export réussi", ["it"] = "Esportazione riuscita", ["en"] = "Export successful" },
        ["ExportFailed"] = new() { ["de"] = "Export fehlgeschlagen", ["fr"] = "Échec de l'export", ["it"] = "Esportazione fallita", ["en"] = "Export failed" },
        ["ImportSuccess"] = new() { ["de"] = "Import erfolgreich", ["fr"] = "Import réussi", ["it"] = "Importazione riuscita", ["en"] = "Import successful" },
        ["ImportFailed"] = new() { ["de"] = "Import fehlgeschlagen", ["fr"] = "Échec de l'import", ["it"] = "Importazione fallita", ["en"] = "Import failed" },
        ["ImportProject"] = new() { ["de"] = "Projekt importieren", ["fr"] = "Importer projet", ["it"] = "Importa progetto", ["en"] = "Import Project" },
        ["ExportProject"] = new() { ["de"] = "Projekt exportieren", ["fr"] = "Exporter projet", ["it"] = "Esporta progetto", ["en"] = "Export Project" },
        ["SelectProjectToExport"] = new() { ["de"] = "Bitte wählen Sie ein Projekt zum Exportieren", ["fr"] = "Veuillez sélectionner un projet à exporter", ["it"] = "Seleziona un progetto da esportare", ["en"] = "Please select a project to export" },
        ["ProjectImportExportDesc"] = new() { ["de"] = "Projekte als .nisproj-Dateien importieren oder exportieren", ["fr"] = "Importer ou exporter des projets sous forme de fichiers .nisproj", ["it"] = "Importa o esporta progetti come file .nisproj", ["en"] = "Import or export individual projects as .nisproj files" },
        ["UserData"] = new() { ["de"] = "Benutzerdaten", ["fr"] = "Données utilisateur", ["it"] = "Dati utente", ["en"] = "User Data" },
        ["FactoryData"] = new() { ["de"] = "Werksdaten", ["fr"] = "Données usine", ["it"] = "Dati di fabbrica", ["en"] = "Factory Data" },
        ["ExportFactoryData"] = new() { ["de"] = "Werksdaten exportieren", ["fr"] = "Exporter données usine", ["it"] = "Esporta dati di fabbrica", ["en"] = "Export Factory Data" },
        ["ImportFactoryData"] = new() { ["de"] = "Werksdaten importieren", ["fr"] = "Importer données usine", ["it"] = "Importa dati di fabbrica", ["en"] = "Import Factory Data" },
        ["ExportFactoryDataDesc"] = new() { ["de"] = "Exportiert alle Projekte, OKAs und Stammdaten (Factory)", ["fr"] = "Exporte tous les projets, LSMs et données de base (usine)", ["it"] = "Esporta tutti i progetti, LST e dati master (fabbrica)", ["en"] = "Export all projects, PSSs, and master data (factory)" },
        ["ImportFactoryConfirmMessage"] = new() { ["de"] = "Dies löscht alle bestehenden Daten und ersetzt sie durch die Werksdaten. Fortfahren?", ["fr"] = "Cela supprime toutes les données existantes et les remplace par les données usine. Continuer?", ["it"] = "Questo elimina tutti i dati esistenti e li sostituisce con i dati di fabbrica. Continuare?", ["en"] = "This deletes all existing data and replaces it with factory data. Continue?" },
        ["ImportProjectConfirmMessage"] = new() { ["de"] = "Ein neues Projekt wird importiert. Fortfahren?", ["fr"] = "Un nouveau projet sera importé. Continuer?", ["it"] = "Verrà importato un nuovo progetto. Continuare?", ["en"] = "A new project will be imported. Continue?" },
        ["FactoryDevelopment"] = new() { ["de"] = "Werksentwicklung", ["fr"] = "Développement usine", ["it"] = "Sviluppo fabbrica", ["en"] = "Factory Development" },
        ["FactoryDevelopmentDesc"] = new() { ["de"] = "Tools zum Erstellen und Bearbeiten der mitgelieferten Datenbank", ["fr"] = "Outils pour créer et modifier la base de données livrée", ["it"] = "Strumenti per creare e modificare il database fornito", ["en"] = "Tools for creating and modifying the shipped database" },
        ["ExportAsDemo"] = new() { ["de"] = "Als Demo exportieren", ["fr"] = "Exporter comme démo", ["it"] = "Esporta come demo", ["en"] = "Export as Demo" },
        ["OpenDataFolder"] = new() { ["de"] = "Datenordner öffnen", ["fr"] = "Ouvrir dossier données", ["it"] = "Apri cartella dati", ["en"] = "Open Data Folder" },
        ["DemoProjectFailed"] = new() { ["de"] = "Demo-Projekt konnte nicht geladen werden", ["fr"] = "Impossible de charger le projet démo", ["it"] = "Impossibile caricare il progetto demo", ["en"] = "Failed to load demo project" },
        ["DemoProjectExported"] = new() { ["de"] = "Demo-Projekt exportiert", ["fr"] = "Projet démo exporté", ["it"] = "Progetto demo esportato", ["en"] = "Demo project exported" },
        ["DemoProject"] = new() { ["de"] = "Demo-Projekt", ["fr"] = "Projet démo", ["it"] = "Progetto demo", ["en"] = "Demo Project" },
        ["DemoProjectDesc"] = new() { ["de"] = "Demo-Projekt für neue Benutzer erstellen und verwalten", ["fr"] = "Créer et gérer le projet démo pour les nouveaux utilisateurs", ["it"] = "Crea e gestisci il progetto demo per i nuovi utenti", ["en"] = "Create and manage the demo project for new users" },
        ["DataFolder"] = new() { ["de"] = "Datenordner", ["fr"] = "Dossier données", ["it"] = "Cartella dati", ["en"] = "Data Folder" },
        ["DataFolderDesc"] = new() { ["de"] = "Öffnet den Datenordner mit der Datenbank (nisdata.db) für Git-Commits", ["fr"] = "Ouvre le dossier de données avec la base de données (nisdata.db) pour les commits Git", ["it"] = "Apre la cartella dati con il database (nisdata.db) per i commit Git", ["en"] = "Opens the data folder containing the database (nisdata.db) for Git commits" },
        ["DataFolderOpened"] = new() { ["de"] = "Datenordner geöffnet", ["fr"] = "Dossier données ouvert", ["it"] = "Cartella dati aperta", ["en"] = "Data folder opened" },
        ["Factory"] = new() { ["de"] = "Werksmodus", ["fr"] = "Mode usine", ["it"] = "Modalità fabbrica", ["en"] = "Factory Mode" },
        ["FactoryMode"] = new() { ["de"] = "WERKSMODUS", ["fr"] = "MODE USINE", ["it"] = "MODALITÀ FABBRICA", ["en"] = "FACTORY MODE" },
        ["EnterFactoryPassword"] = new() { ["de"] = "Bitte Werkspasswort eingeben:", ["fr"] = "Veuillez entrer le mot de passe usine:", ["it"] = "Inserisci la password di fabbrica:", ["en"] = "Please enter factory password:" },
        ["WrongPassword"] = new() { ["de"] = "Falsches Passwort", ["fr"] = "Mot de passe incorrect", ["it"] = "Password errata", ["en"] = "Wrong password" },

        // Calculation Report Labels
        ["CalcTitlePrefix"] = new() { ["de"] = "Immissionsberechnung für", ["fr"] = "Calcul d'immission pour", ["it"] = "Calcolo immissione per", ["en"] = "Emission Calculation for" },
        ["CalcOperator"] = new() { ["de"] = "Betreiber", ["fr"] = "Opérateur", ["it"] = "Operatore", ["en"] = "Operator" },
        ["CalcAddress"] = new() { ["de"] = "Adresse", ["fr"] = "Adresse", ["it"] = "Indirizzo", ["en"] = "Address" },
        ["CalcDate"] = new() { ["de"] = "Datum", ["fr"] = "Date", ["it"] = "Data", ["en"] = "Date" },
        ["CalcTransmitter"] = new() { ["de"] = "Sender", ["fr"] = "Émetteur", ["it"] = "Trasmettitore", ["en"] = "Transmitter" },
        ["CalcCable"] = new() { ["de"] = "Kabel", ["fr"] = "Câble", ["it"] = "Cavo", ["en"] = "Cable" },
        ["CalcAntenna"] = new() { ["de"] = "Antenne", ["fr"] = "Antenne", ["it"] = "Antenna", ["en"] = "Antenna" },
        ["CalcPolarization"] = new() { ["de"] = "Polarisation", ["fr"] = "Polarisation", ["it"] = "Polarizzazione", ["en"] = "Polarization" },
        ["CalcRotation"] = new() { ["de"] = "Rotation", ["fr"] = "Rotation", ["it"] = "Rotazione", ["en"] = "Rotation" },
        ["CalcLinear"] = new() { ["de"] = "Linear", ["fr"] = "Linéaire", ["it"] = "Lineare", ["en"] = "Linear" },
        ["CalcOka"] = new() { ["de"] = "OKA", ["fr"] = "LSM", ["it"] = "LSBD", ["en"] = "OKA" },
        ["CalcModulation"] = new() { ["de"] = "Modulation", ["fr"] = "Modulation", ["it"] = "Modulazione", ["en"] = "Modulation" },
        ["CalcBuildingDamping"] = new() { ["de"] = "Gebäudedämpfung", ["fr"] = "Atténuation bâtiment", ["it"] = "Attenuazione edificio", ["en"] = "Building Damping" },
        ["CalcStatusCompliant"] = new() { ["de"] = "**Status: KONFORM** - Alle Frequenzen innerhalb der Grenzwerte", ["fr"] = "**Statut: CONFORME** - Toutes les fréquences dans les limites", ["it"] = "**Stato: CONFORME** - Tutte le frequenze entro i limiti", ["en"] = "**Status: COMPLIANT** - All frequencies within limits" },
        ["CalcStatusNonCompliant"] = new() { ["de"] = "**Status: NICHT KONFORM** - Grenzwerte überschritten!", ["fr"] = "**Statut: NON CONFORME** - Limites dépassées!", ["it"] = "**Stato: NON CONFORME** - Limiti superati!", ["en"] = "**Status: NON-COMPLIANT** - Limits exceeded!" },
        ["CalcHorizontal"] = new() { ["de"] = "Horizontal", ["fr"] = "Horizontal", ["it"] = "Orizzontale", ["en"] = "Horizontal" },
        ["CalcVertical"] = new() { ["de"] = "Vertikal", ["fr"] = "Vertical", ["it"] = "Verticale", ["en"] = "Vertical" },
        ["CalcFixed"] = new() { ["de"] = "Fest", ["fr"] = "Fixe", ["it"] = "Fisso", ["en"] = "Fixed" },

        // Calculation Table Row Labels
        ["CalcFrequency"] = new() { ["de"] = "Frequenz", ["fr"] = "Fréquence", ["it"] = "Frequenza", ["en"] = "Frequency" },
        ["CalcDistanceToAntenna"] = new() { ["de"] = "Abstand zur Antenne", ["fr"] = "Distance à l'antenne", ["it"] = "Distanza dall'antenna", ["en"] = "Distance to antenna" },
        ["CalcTxPower"] = new() { ["de"] = "Senderleistung", ["fr"] = "Puissance TX", ["it"] = "Potenza TX", ["en"] = "TX Power" },
        ["CalcActivityFactor"] = new() { ["de"] = "Aktivitätsfaktor", ["fr"] = "Facteur d'activité", ["it"] = "Fattore di attività", ["en"] = "Activity factor" },
        ["CalcModulationFactor"] = new() { ["de"] = "Modulationsfaktor", ["fr"] = "Facteur de modulation", ["it"] = "Fattore di modulazione", ["en"] = "Modulation factor" },
        ["CalcMeanPower"] = new() { ["de"] = "Mittlere Leistung", ["fr"] = "Puissance moyenne", ["it"] = "Potenza media", ["en"] = "Mean power" },
        ["CalcCableAttenuation"] = new() { ["de"] = "Kabeldämpfung", ["fr"] = "Atténuation câble", ["it"] = "Attenuazione cavo", ["en"] = "Cable attenuation" },
        ["CalcAdditionalLosses"] = new() { ["de"] = "Übrige Dämpfung", ["fr"] = "Pertes supplémentaires", ["it"] = "Perdite aggiuntive", ["en"] = "Additional losses" },
        ["CalcTotalAttenuation"] = new() { ["de"] = "Gesamtdämpfung", ["fr"] = "Atténuation totale", ["it"] = "Attenuazione totale", ["en"] = "Total attenuation" },
        ["CalcAttenuationFactor"] = new() { ["de"] = "Dämpfungsfaktor", ["fr"] = "Facteur d'atténuation", ["it"] = "Fattore di attenuazione", ["en"] = "Attenuation factor" },
        ["CalcAntennaGain"] = new() { ["de"] = "Antennengewinn", ["fr"] = "Gain d'antenne", ["it"] = "Guadagno antenna", ["en"] = "Antenna gain" },
        ["CalcVerticalAttenuation"] = new() { ["de"] = "Vertikale Winkeldämpfung", ["fr"] = "Atténuation angle vertical", ["it"] = "Attenuazione angolo verticale", ["en"] = "Vertical angle attenuation" },
        ["CalcTotalAntennaGain"] = new() { ["de"] = "Totaler Antennengewinn", ["fr"] = "Gain d'antenne total", ["it"] = "Guadagno antenna totale", ["en"] = "Total antenna gain" },
        ["CalcGainFactor"] = new() { ["de"] = "Gewinnfaktor", ["fr"] = "Facteur de gain", ["it"] = "Fattore di guadagno", ["en"] = "Gain factor" },
        ["CalcEirp"] = new() { ["de"] = "EIRP in Richtung OKA", ["fr"] = "PIRE en direction du LSM", ["it"] = "EIRP in direzione del LST", ["en"] = "EIRP in direction of OKA" },
        ["CalcErp"] = new() { ["de"] = "ERP in Richtung OKA", ["fr"] = "PAR en direction du LSM", ["it"] = "ERP in direzione del LST", ["en"] = "ERP in direction of OKA" },
        ["CalcBuildingDampingRow"] = new() { ["de"] = "Gebäudedämpfung", ["fr"] = "Atténuation bâtiment", ["it"] = "Attenuazione edificio", ["en"] = "Building damping" },
        ["CalcBuildingDampingFactor"] = new() { ["de"] = "Gebäudedämpfungsfaktor", ["fr"] = "Facteur atténuation bât.", ["it"] = "Fattore attenuazione ed.", ["en"] = "Building damping factor" },
        ["CalcGroundReflection"] = new() { ["de"] = "Bodenreflexionsfaktor", ["fr"] = "Facteur réflexion sol", ["it"] = "Fattore riflessione suolo", ["en"] = "Ground reflection factor" },
        ["CalcFieldStrength"] = new() { ["de"] = "Feldstärke am OKA", ["fr"] = "Champ au LSM", ["it"] = "Campo al LSBD", ["en"] = "Field strength at OKA" },
        ["CalcLimit"] = new() { ["de"] = "Grenzwert", ["fr"] = "Limite", ["it"] = "Limite", ["en"] = "Limit" },
        ["CalcMinSafetyDistance"] = new() { ["de"] = "Min. Sicherheitsabstand", ["fr"] = "Dist. sécurité min.", ["it"] = "Dist. sicurezza min.", ["en"] = "Min. safety distance" },
        ["CalcOkaDistance"] = new() { ["de"] = "OKA Abstand", ["fr"] = "Distance LSM", ["it"] = "Distanza LSBD", ["en"] = "OKA distance" },

        // Column Explanations
        ["CalcColumnExplanations"] = new() { ["de"] = "## Spaltenlegende", ["fr"] = "## Légende des colonnes", ["it"] = "## Legenda delle colonne", ["en"] = "## Column Explanations" },
        ["CalcExplainF"] = new() { ["de"] = "Frequenz in MHz", ["fr"] = "Fréquence en MHz", ["it"] = "Frequenza in MHz", ["en"] = "Frequency in MHz" },
        ["CalcExplainD"] = new() { ["de"] = "Horizontaler Abstand vom OKA zur Antenne in Metern", ["fr"] = "Distance horizontale du LSM à l'antenne en mètres", ["it"] = "Distanza orizzontale dal LSBD all'antenna in metri", ["en"] = "Horizontal distance from OKA to antenna in meters" },
        ["CalcExplainP"] = new() { ["de"] = "Senderausgangsleistung in Watt", ["fr"] = "Puissance de sortie de l'émetteur en Watts", ["it"] = "Potenza di uscita del trasmettitore in Watt", ["en"] = "Transmitter output power in Watts" },
        ["CalcExplainAF"] = new() { ["de"] = "Aktivitätsfaktor (typisch 0.5 = 50% Sendezeit)", ["fr"] = "Facteur d'activité (typique 0.5 = 50% du temps d'émission)", ["it"] = "Fattore di attività (tipico 0.5 = 50% del tempo di trasmissione)", ["en"] = "Activity factor (typical 0.5 = 50% transmit time)" },
        ["CalcExplainMF"] = new() { ["de"] = "Modulationsfaktor (SSB=0.2, CW=0.4, FM/Digital=1.0)", ["fr"] = "Facteur de modulation (SSB=0.2, CW=0.4, FM/Digital=1.0)", ["it"] = "Fattore di modulazione (SSB=0.2, CW=0.4, FM/Digital=1.0)", ["en"] = "Modulation factor (SSB=0.2, CW=0.4, FM/Digital=1.0)" },
        ["CalcExplainPm"] = new() { ["de"] = "Pmittel = Mittlere Leistung = P × AF × MF", ["fr"] = "Pmoy = Puissance moyenne = P × AF × MF", ["it"] = "Pmedia = Potenza media = P × AF × MF", ["en"] = "Pmean = Mean power = P × AF × MF" },
        ["CalcExplainA1"] = new() { ["de"] = "Kabeldämpfung in dB", ["fr"] = "Atténuation du câble en dB", ["it"] = "Attenuazione del cavo in dB", ["en"] = "Cable attenuation in dB" },
        ["CalcExplainA2"] = new() { ["de"] = "Zusätzliche Dämpfung (Stecker, Schalter) in dB", ["fr"] = "Pertes supplémentaires (connecteurs, commutateurs) en dB", ["it"] = "Perdite aggiuntive (connettori, interruttori) in dB", ["en"] = "Additional losses (connectors, switches) in dB" },
        ["CalcExplainA"] = new() { ["de"] = "Gesamtdämpfung = a1 + a2", ["fr"] = "Atténuation totale = a1 + a2", ["it"] = "Attenuazione totale = a1 + a2", ["en"] = "Total attenuation = a1 + a2" },
        ["CalcExplainAFactor"] = new() { ["de"] = "Dämpfungsfaktor = 10^(-a/10)", ["fr"] = "Facteur d'atténuation = 10^(-a/10)", ["it"] = "Fattore di attenuazione = 10^(-a/10)", ["en"] = "Attenuation factor = 10^(-a/10)" },
        ["CalcExplainG1"] = new() { ["de"] = "Antennengewinn in dBi", ["fr"] = "Gain d'antenne en dBi", ["it"] = "Guadagno dell'antenna in dBi", ["en"] = "Antenna gain in dBi" },
        ["CalcExplainG2"] = new() { ["de"] = "Vertikale Winkeldämpfung basierend auf Antennendiagramm in dB", ["fr"] = "Atténuation angle vertical basée sur le diagramme d'antenne en dB", ["it"] = "Attenuazione angolo verticale basata sul diagramma dell'antenna in dB", ["en"] = "Vertical angle attenuation based on antenna pattern in dB" },
        ["CalcExplainG"] = new() { ["de"] = "Totaler Antennengewinn = g1 - g2", ["fr"] = "Gain d'antenne total = g1 - g2", ["it"] = "Guadagno totale dell'antenna = g1 - g2", ["en"] = "Total antenna gain = g1 - g2" },
        ["CalcExplainGFactor"] = new() { ["de"] = "Gewinnfaktor = 10^(g/10)", ["fr"] = "Facteur de gain = 10^(g/10)", ["it"] = "Fattore di guadagno = 10^(g/10)", ["en"] = "Gain factor = 10^(g/10)" },
        ["CalcExplainPs"] = new() { ["de"] = "EIRP in Richtung OKA (Equivalent Isotropic Radiated Power) = Pmittel × A × G", ["fr"] = "PIRE en direction du LSM (Puissance Isotrope Rayonnée Équivalente) = Pmoy × A × G", ["it"] = "EIRP in direzione del LST (Potenza Isotropa Irradiata Equivalente) = Pmedia × A × G", ["en"] = "EIRP in direction of OKA (Equivalent Isotropic Radiated Power) = Pmean × A × G" },
        ["CalcExplainPsPrime"] = new() { ["de"] = "ERP in Richtung OKA (Effective Radiated Power) = Ps / 1.64", ["fr"] = "PAR en direction du LSM (Puissance Apparente Rayonnée) = Ps / 1.64", ["it"] = "ERP in direzione del LST (Potenza Effettiva Irradiata) = Ps / 1.64", ["en"] = "ERP in direction of OKA (Effective Radiated Power) = Ps / 1.64" },
        ["CalcExplainAg"] = new() { ["de"] = "Gebäudedämpfung in dB (0 für Aussenbereich)", ["fr"] = "Atténuation du bâtiment en dB (0 pour l'extérieur)", ["it"] = "Attenuazione dell'edificio in dB (0 per esterni)", ["en"] = "Building damping in dB (0 for outdoor)" },
        ["CalcExplainAG"] = new() { ["de"] = "Gebäudedämpfungsfaktor = 10^(-ag/10)", ["fr"] = "Facteur d'atténuation du bâtiment = 10^(-ag/10)", ["it"] = "Fattore di attenuazione dell'edificio = 10^(-ag/10)", ["en"] = "Building damping factor = 10^(-ag/10)" },
        ["CalcExplainKr"] = new() { ["de"] = "Bodenreflexionsfaktor (1.6 gemäss NISV Anhang 2)", ["fr"] = "Facteur de réflexion au sol (1.6 selon ORNI Annexe 2)", ["it"] = "Fattore di riflessione al suolo (1.6 secondo ORNI Allegato 2)", ["en"] = "Ground reflection factor (1.6 per NISV Annex 2)" },
        ["CalcExplainE"] = new() { ["de"] = "Berechnete Feldstärke am OKA in V/m", ["fr"] = "Intensité de champ calculée au LSM en V/m", ["it"] = "Intensità di campo calcolata al LSBD in V/m", ["en"] = "Calculated field strength at OKA in V/m" },
        ["CalcExplainEigw"] = new() { ["de"] = "Immissions-Grenzwert gemäss NISV in V/m", ["fr"] = "Valeur limite d'immission selon ORNI en V/m", ["it"] = "Valore limite di immissione secondo ORNI in V/m", ["en"] = "Emission limit per NISV in V/m" },
        ["CalcExplainDs"] = new() { ["de"] = "Minimaler Sicherheitsabstand (Konformität wenn d > ds)", ["fr"] = "Distance de sécurité minimale (conforme si d > ds)", ["it"] = "Distanza di sicurezza minima (conforme se d > ds)", ["en"] = "Minimum safety distance (compliant if d > ds)" },
    };
}
