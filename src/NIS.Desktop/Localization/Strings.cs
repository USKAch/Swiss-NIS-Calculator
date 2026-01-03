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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CannotDelete)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemInUse)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConfigurationIncomplete)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FixErrorsBeforeCalculating)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoConfigurationsToCalculate)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoAntennaSelected)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoCableSelected)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoModulationSelected)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoOkaSelected)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OkaNotFound)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OkaDistanceInvalid)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AntennaNotFound)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CableNotFound)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModulationNotFound)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AntennaNoBands)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Calculating)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalculationComplete)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConfigurationsAnalyzed)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));
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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MasterData)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NisvCompliance)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Home)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Project)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Settings)));

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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeightShort)));
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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OkaDistanceShort)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OkaShort)));
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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcSubtitle)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcOperator)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcCallsign)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcAddress)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcLocation)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcAllCompliant)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcNonCompliantDetected)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcDisclaimer)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Compliant)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NonCompliant)));
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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcOkaNumber)));
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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcMinSafeDistance)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcOkaDistance)));

        // Column Explanations
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcColumnExplanations)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainF)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainOkaNumber)));
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
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CalcExplainOkaDistance)));
    }

    /// <summary>
    /// Get a localized string by key. Falls back to German, then shows [key] if not found.
    /// </summary>
    public string Get(string key) =>
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
    public string CannotDelete => Get("CannotDelete");
    public string ItemInUse => Get("ItemInUse");
    public string ConfigurationIncomplete => Get("ConfigurationIncomplete");
    public string FixErrorsBeforeCalculating => Get("FixErrorsBeforeCalculating");
    public string NoConfigurationsToCalculate => Get("NoConfigurationsToCalculate");
    public string NoAntennaSelected => Get("NoAntennaSelected");
    public string NoCableSelected => Get("NoCableSelected");
    public string NoModulationSelected => Get("NoModulationSelected");
    public string NoOkaSelected => Get("NoOkaSelected");
    public string OkaNotFound => Get("OkaNotFound");
    public string OkaDistanceInvalid => Get("OkaDistanceInvalid");
    public string AntennaNotFound => Get("AntennaNotFound");
    public string CableNotFound => Get("CableNotFound");
    public string ModulationNotFound => Get("ModulationNotFound");
    public string AntennaNoBands => Get("AntennaNoBands");
    public string Calculating => Get("Calculating");
    public string CalculationComplete => Get("CalculationComplete");
    public string ConfigurationsAnalyzed => Get("ConfigurationsAnalyzed");
    public string Error => Get("Error");
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
    public string MasterData => Get("MasterData");
    public string NisvCompliance => Get("NisvCompliance");
    public string Home => Get("Home");
    public string Projects => Get("Projects");
    public string Project => Get("Project");
    public string Settings => Get("Settings");

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
    public string HeightShort => Get("HeightShort");
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
    public string OkaDistanceShort => Get("OkaDistanceShort");
    public string OkaShort => Get("OkaShort");
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
    public string CalcSubtitle => Get("CalcSubtitle");
    public string CalcOperator => Get("CalcOperator");
    public string CalcCallsign => Get("CalcCallsign");
    public string CalcAddress => Get("CalcAddress");
    public string CalcLocation => Get("CalcLocation");
    public string CalcDate => Get("CalcDate");
    public string CalcAllCompliant => Get("CalcAllCompliant");
    public string CalcNonCompliantDetected => Get("CalcNonCompliantDetected");
    public string CalcDisclaimer => Get("CalcDisclaimer");
    public string Compliant => Get("Compliant");
    public string NonCompliant => Get("NonCompliant");
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
    public string CalcOkaNumber => Get("CalcOkaNumber");
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
    public string CalcMinSafeDistance => Get("CalcMinSafeDistance");
    public string CalcOkaDistance => Get("CalcOkaDistance");

    // Column Explanations
    public string CalcColumnExplanations => Get("CalcColumnExplanations");
    public string CalcExplainF => Get("CalcExplainF");
    public string CalcExplainOkaNumber => Get("CalcExplainOkaNumber");
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
    public string CalcExplainOkaDistance => Get("CalcExplainOkaDistance");

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
    public string ExportFailed => Get("ExportFailed");
    public string StatusReady => Get("StatusReady");
    public string CannotExportNoProject => Get("CannotExportNoProject");
    public string ExportedToFile => Get("ExportedToFile");
    public string PdfExportError => Get("PdfExportError");
    public string ExportMarkdownTitle => Get("ExportMarkdownTitle");
    public string ExportPdfTitle => Get("ExportPdfTitle");
    public string ImportFailed => Get("ImportFailed");
    public string ImportWarning => Get("ImportWarning");
    public string MissingMasterData => Get("MissingMasterData");
    public string ConfirmImport => Get("ConfirmImport");
    public string ReplaceMasterDataConfirm => Get("ReplaceMasterDataConfirm");
    public string ExportComplete => Get("ExportComplete");
    public string ImportComplete => Get("ImportComplete");
    public string FactoryDataExportedTo => Get("FactoryDataExportedTo");
    public string FactoryDataImportedFrom => Get("FactoryDataImportedFrom");
    public string OperationFailed => Get("OperationFailed");
    public string Export => Get("Export");
    public string Import => Get("Import");
    public string FailedToOpenDataFolder => Get("FailedToOpenDataFolder");
    public string DatabaseResetRequired => Get("DatabaseResetRequired");
    public string DatabaseResetMessage => Get("DatabaseResetMessage");
    public string PatternPrefix => Get("PatternPrefix");
    public string NoPattern => Get("NoPattern");
    public string ConfigurationNumber => Get("ConfigurationNumber");

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
        ["MasterData"] = "Welcome",
        ["NisvCompliance"] = "Welcome",
        // ProjectInfo
        ["ProjectInfo"] = "ProjectInfo",
        ["ProjectName"] = "ProjectInfo",
        ["Operator"] = "ProjectInfo",
        ["Callsign"] = "ProjectInfo",
        ["Address"] = "ProjectInfo",
        ["Location"] = "ProjectInfo",
        ["CreateProject"] = "ProjectInfo",
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
        ["SelectAntennaPrompt"] = "AntennaEditor",
        ["TypeToSearch"] = "AntennaEditor",
        ["TypeToSearchDescription"] = "AntennaEditor",
        ["AntennaTypeLabel"] = "AntennaEditor",
        ["GenerateFromGain"] = "AntennaEditor",
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
        ["ImportDatabaseDesc"] = "MasterData",
        // Results
        ["DistanceAntennaOka"] = "Results",
        ["CalculationResults"] = "Results",
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
        ["Cancel"] = new() { ["de"] = "Abbrechen", ["fr"] = "Annuler", ["it"] = "Annulla", ["en"] = "Cancel" },
        ["Edit"] = new() { ["de"] = "Bearbeiten", ["fr"] = "Modifier", ["it"] = "Modifica", ["en"] = "Edit" },
        ["View"] = new() { ["de"] = "Anzeigen", ["fr"] = "Afficher", ["it"] = "Visualizza", ["en"] = "View" },
        ["ProjectSpecific"] = new() { ["de"] = "[Projekt]", ["fr"] = "[Projet]", ["it"] = "[Progetto]", ["en"] = "[Project]" },
        ["DuplicateNameError"] = new() { ["de"] = "Ein Eintrag mit diesem Namen existiert bereits", ["fr"] = "Une entrée avec ce nom existe déjà", ["it"] = "Esiste già una voce con questo nome", ["en"] = "An item with this name already exists" },
        ["New"] = new() { ["de"] = "Neu", ["fr"] = "Nouveau", ["it"] = "Nuovo", ["en"] = "New" },
        ["Delete"] = new() { ["de"] = "Löschen", ["fr"] = "Supprimer", ["it"] = "Elimina", ["en"] = "Delete" },
        ["CannotDelete"] = new() { ["de"] = "Löschen nicht möglich", ["fr"] = "Suppression impossible", ["it"] = "Impossibile eliminare", ["en"] = "Cannot Delete" },
        ["ItemInUse"] = new() { ["de"] = "Dieses Element wird in folgenden Konfigurationen verwendet:", ["fr"] = "Cet élément est utilisé dans les configurations suivantes:", ["it"] = "Questo elemento è utilizzato nelle seguenti configurazioni:", ["en"] = "This item is used in the following configurations:" },
        ["ConfigurationIncomplete"] = new() { ["de"] = "Konfiguration unvollständig", ["fr"] = "Configuration incomplète", ["it"] = "Configurazione incompleta", ["en"] = "Configuration Incomplete" },
        ["FixErrorsBeforeCalculating"] = new() { ["de"] = "Bitte beheben Sie die folgenden Fehler vor der Berechnung:", ["fr"] = "Veuillez corriger les erreurs suivantes avant le calcul:", ["it"] = "Correggi i seguenti errori prima del calcolo:", ["en"] = "Please fix the following errors before calculating:" },
        ["NoConfigurationsToCalculate"] = new() { ["de"] = "Keine Konfigurationen zum Berechnen vorhanden", ["fr"] = "Aucune configuration à calculer", ["it"] = "Nessuna configurazione da calcolare", ["en"] = "No configurations to calculate" },
        ["NoAntennaSelected"] = new() { ["de"] = "Keine Antenne ausgewählt", ["fr"] = "Aucune antenne sélectionnée", ["it"] = "Nessuna antenna selezionata", ["en"] = "No antenna selected" },
        ["NoCableSelected"] = new() { ["de"] = "Kein Kabel ausgewählt", ["fr"] = "Aucun câble sélectionné", ["it"] = "Nessun cavo selezionato", ["en"] = "No cable selected" },
        ["NoModulationSelected"] = new() { ["de"] = "Keine Modulation ausgewählt", ["fr"] = "Aucune modulation sélectionnée", ["it"] = "Nessuna modulazione selezionata", ["en"] = "No modulation selected" },
        ["NoOkaSelected"] = new() { ["de"] = "Kein OKA ausgewählt", ["fr"] = "Aucun LSM sélectionné", ["it"] = "Nessun LST selezionato", ["en"] = "No PSS selected" },
        ["OkaNotFound"] = new() { ["de"] = "OKA nicht in Datenbank gefunden", ["fr"] = "LSM introuvable dans la base de données", ["it"] = "LST non trovato nel database", ["en"] = "PSS not found in database" },
        ["OkaDistanceInvalid"] = new() { ["de"] = "OKA-Distanz muss grösser als 0 sein", ["fr"] = "La distance LSM doit être supérieure à 0", ["it"] = "La distanza LST deve essere maggiore di 0", ["en"] = "PSS distance must be greater than 0" },
        ["AntennaNotFound"] = new() { ["de"] = "Antenne nicht in Datenbank gefunden", ["fr"] = "Antenne introuvable dans la base de données", ["it"] = "Antenna non trovata nel database", ["en"] = "Antenna not found in database" },
        ["CableNotFound"] = new() { ["de"] = "Kabel nicht in Datenbank gefunden", ["fr"] = "Câble introuvable dans la base de données", ["it"] = "Cavo non trovato nel database", ["en"] = "Cable not found in database" },
        ["ModulationNotFound"] = new() { ["de"] = "Modulation nicht in Datenbank gefunden", ["fr"] = "Modulation introuvable dans la base de données", ["it"] = "Modulazione non trovata nel database", ["en"] = "Modulation not found in database" },
        ["AntennaNoBands"] = new() { ["de"] = "Antenne hat keine Frequenzbänder definiert", ["fr"] = "L'antenne n'a pas de bandes de fréquences définies", ["it"] = "L'antenna non ha bande di frequenza definite", ["en"] = "Antenna has no frequency bands defined" },
        ["Calculating"] = new() { ["de"] = "Berechne...", ["fr"] = "Calcul en cours...", ["it"] = "Calcolo in corso...", ["en"] = "Calculating..." },
        ["CalculationComplete"] = new() { ["de"] = "Berechnung abgeschlossen", ["fr"] = "Calcul terminé", ["it"] = "Calcolo completato", ["en"] = "Calculation complete" },
        ["ConfigurationsAnalyzed"] = new() { ["de"] = "Konfigurationen analysiert", ["fr"] = "configurations analysées", ["it"] = "configurazioni analizzate", ["en"] = "configurations analyzed" },
        ["Error"] = new() { ["de"] = "Fehler", ["fr"] = "Erreur", ["it"] = "Errore", ["en"] = "Error" },
        ["Actions"] = new() { ["de"] = "Aktionen", ["fr"] = "Actions", ["it"] = "Azioni", ["en"] = "Actions" },
        ["Modified"] = new() { ["de"] = "Geändert", ["fr"] = "Modifié", ["it"] = "Modificato", ["en"] = "Modified" },
        ["Remove"] = new() { ["de"] = "Entfernen", ["fr"] = "Retirer", ["it"] = "Rimuovi", ["en"] = "Remove" },
        ["Back"] = new() { ["de"] = "Zurück", ["fr"] = "Retour", ["it"] = "Indietro", ["en"] = "Back" },
        ["Close"] = new() { ["de"] = "Schliessen", ["fr"] = "Fermer", ["it"] = "Chiudi", ["en"] = "Close" },

        // Welcome
        ["AppTitle"] = new() { ["de"] = "Swiss NIS Calculator", ["fr"] = "Swiss NIS Calculator", ["it"] = "Swiss NIS Calculator", ["en"] = "Swiss NIS Calculator" },
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
        ["MasterData"] = new() { ["de"] = "Stammdaten", ["fr"] = "Données de base", ["it"] = "Dati master", ["en"] = "Master Data" },
        ["NisvCompliance"] = new() { ["de"] = "NISV-Konformitätsrechner für Schweizer Amateurfunkstationen", ["fr"] = "Calculateur de conformité ORNI pour stations radioamateurs suisses", ["it"] = "Calcolatore conformità ORNI per stazioni radioamatoriali svizzere", ["en"] = "NISV Compliance Calculator for Swiss Amateur Radio Stations" },
        ["Home"] = new() { ["de"] = "Start", ["fr"] = "Accueil", ["it"] = "Home", ["en"] = "Home" },
        ["Projects"] = new() { ["de"] = "Projekte", ["fr"] = "Projets", ["it"] = "Progetti", ["en"] = "Projects" },
        ["Project"] = new() { ["de"] = "Projekt", ["fr"] = "Projet", ["it"] = "Progetto", ["en"] = "Project" },
        ["Settings"] = new() { ["de"] = "Einstellungen", ["fr"] = "Paramètres", ["it"] = "Impostazioni", ["en"] = "Settings" },

        // Project Info
        ["ProjectInfo"] = new() { ["de"] = "Projektinformationen", ["fr"] = "Informations projet", ["it"] = "Informazioni progetto", ["en"] = "Project Information" },
        ["ProjectName"] = new() { ["de"] = "Projektname", ["fr"] = "Nom du projet", ["it"] = "Nome progetto", ["en"] = "Project Name" },
        ["Operator"] = new() { ["de"] = "Betreiber", ["fr"] = "Opérateur", ["it"] = "Operatore", ["en"] = "Operator" },
        ["Address"] = new() { ["de"] = "Adresse", ["fr"] = "Adresse", ["it"] = "Indirizzo", ["en"] = "Address" },
        ["Location"] = new() { ["de"] = "Ort", ["fr"] = "Localité", ["it"] = "Località", ["en"] = "Location" },
        ["CreateProject"] = new() { ["de"] = "Projekt erstellen", ["fr"] = "Créer projet", ["it"] = "Crea progetto", ["en"] = "Create Project" },

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
        ["HeightShort"] = new() { ["de"] = "Höhe", ["fr"] = "Haut.", ["it"] = "Alt.", ["en"] = "Height" },
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
        ["OkaDistanceShort"] = new() { ["de"] = "Dist.", ["fr"] = "Dist.", ["it"] = "Dist.", ["en"] = "Dist." },
        ["OkaShort"] = new() { ["de"] = "OKA", ["fr"] = "LSM", ["it"] = "LST", ["en"] = "PSS" },
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
        ["PatternExplanation"] = new() { ["de"] = "0° = Horizont (max. Abstrahlung), 90° = Senkrecht nach unten (Richtung OKA). Werte: 0-60 dB Dämpfung.", ["fr"] = "0° = Horizon (rayonnement max), 90° = Verticalement vers le bas (direction LSM). Valeurs: 0-60 dB d'atténuation.", ["it"] = "0° = Orizzonte (radiazione max), 90° = Verticalmente verso il basso (direzione LST). Valori: 0-60 dB attenuazione.", ["en"] = "0° = Horizon (max radiation), 90° = Straight down (toward PSS). Values: 0-60 dB attenuation." },
        ["AddAtLeastOneBand"] = new() { ["de"] = "Fügen Sie mindestens ein Frequenzband mit Gewinn und vertikalem Strahlungsdiagramm hinzu.", ["fr"] = "Ajoutez au moins une bande de fréquence avec gain et diagramme de rayonnement vertical.", ["it"] = "Aggiungi almeno una banda di frequenza con guadagno e diagramma di radiazione verticale.", ["en"] = "Add at least one frequency band with gain and vertical radiation pattern." },
        ["AddNewAntenna"] = new() { ["de"] = "Neue Antenne hinzufügen", ["fr"] = "Ajouter nouvelle antenne", ["it"] = "Aggiungi nuova antenna", ["en"] = "Add New Antenna" },
        ["SelectAntennaPrompt"] = new() { ["de"] = "Antenne auswählen", ["fr"] = "Sélectionner antenne", ["it"] = "Seleziona antenna", ["en"] = "Select Antenna" },
        ["TypeToSearch"] = new() { ["de"] = "Eingabe zum Suchen...", ["fr"] = "Tapez pour rechercher...", ["it"] = "Digita per cercare...", ["en"] = "Type to search..." },
        ["TypeToSearchDescription"] = new() { ["de"] = "Suche nach Hersteller oder Modell:", ["fr"] = "Rechercher par fabricant ou modèle:", ["it"] = "Cerca per produttore o modello:", ["en"] = "Type to search by manufacturer or model:" },
        ["AntennaTypeLabel"] = new() { ["de"] = "Antennentyp", ["fr"] = "Type d'antenne", ["it"] = "Tipo di antenna", ["en"] = "Antenna Type" },
        ["GenerateFromGain"] = new() { ["de"] = "Automatisch berechnen", ["fr"] = "Calculer automatiquement", ["it"] = "Calcola automaticamente", ["en"] = "Auto-calculate" },

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
        ["ImportDatabaseDesc"] = new() { ["de"] = "Importiert Werksdaten aus einer JSON-Datei. ACHTUNG: Alle Daten werden gelöscht!", ["fr"] = "Importe les données usine depuis un fichier JSON. ATTENTION: Toutes les données seront supprimées!", ["it"] = "Importa i dati di fabbrica da un file JSON. ATTENZIONE: Tutti i dati saranno eliminati!", ["en"] = "Import factory data from a JSON file. WARNING: All data will be deleted!" },

        // Results
        ["None"] = new() { ["de"] = "Keine", ["fr"] = "Aucun", ["it"] = "Nessuno", ["en"] = "None" },
        ["DistanceAntennaOka"] = new() { ["de"] = "Distanz Antenne-OKA", ["fr"] = "Distance Antenne-LSM", ["it"] = "Distanza Antenna-LST", ["en"] = "Distance Antenna-PSS" },
        ["OkaFullName"] = new() { ["de"] = "Ort für kurzfristigen Aufenthalt", ["fr"] = "Lieu de séjour momentané", ["it"] = "Luogo di soggiorno temporaneo", ["en"] = "Place of Short-Term Stay" },
        ["AboveOka"] = new() { ["de"] = "über OKA", ["fr"] = "au-dessus du LSM", ["it"] = "sopra LST", ["en"] = "above PSS" },
        ["HorizDistToMast"] = new() { ["de"] = "horizontale Distanz zum Antennenmast", ["fr"] = "distance horizontale au mât", ["it"] = "distanza orizzontale al palo", ["en"] = "horizontal distance to antenna mast" },
        ["CalculationResults"] = new() { ["de"] = "Berechnungsergebnisse", ["fr"] = "Résultats du calcul", ["it"] = "Risultati del calcolo", ["en"] = "Calculation Results" },
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
        ["OkaDistLbl"] = new() { ["de"] = "OKA D.", ["fr"] = "LSM D.", ["it"] = "LST D.", ["en"] = "PSS D." },

        // Validation

        // Dialogs
        ["UnsavedChanges"] = new() { ["de"] = "Ungespeicherte Änderungen", ["fr"] = "Modifications non enregistrées", ["it"] = "Modifiche non salvate", ["en"] = "Unsaved Changes" },
        ["DiscardChanges"] = new() { ["de"] = "Änderungen verwerfen?", ["fr"] = "Annuler les modifications?", ["it"] = "Annullare le modifiche?", ["en"] = "Discard Changes?" },
        ["DiscardChangesPrompt"] = new() { ["de"] = "Sie haben ungespeicherte Änderungen. Möchten Sie diese verwerfen?", ["fr"] = "Vous avez des modifications non enregistrées. Voulez-vous les annuler?", ["it"] = "Hai modifiche non salvate. Vuoi annullarle?", ["en"] = "You have unsaved changes. Do you want to discard them?" },
        ["ChangesDiscarded"] = new() { ["de"] = "Änderungen verworfen.", ["fr"] = "Modifications annulées.", ["it"] = "Modifiche annullate.", ["en"] = "Changes discarded." },

        // Project Management
        ["NoProjects"] = new() { ["de"] = "Keine Projekte vorhanden", ["fr"] = "Aucun projet disponible", ["it"] = "Nessun progetto disponibile", ["en"] = "No projects available" },
        ["DeleteProjectConfirm"] = new() { ["de"] = "Projekt wirklich löschen?", ["fr"] = "Vraiment supprimer le projet?", ["it"] = "Eliminare davvero il progetto?", ["en"] = "Really delete project?" },
        ["DeleteProjectMessage"] = new() { ["de"] = "Das Projekt '{0}' wird unwiderruflich gelöscht.", ["fr"] = "Le projet '{0}' sera supprimé définitivement.", ["it"] = "Il progetto '{0}' verrà eliminato definitivamente.", ["en"] = "The project '{0}' will be permanently deleted." },
        ["ProjectDeleted"] = new() { ["de"] = "Projekt gelöscht", ["fr"] = "Projet supprimé", ["it"] = "Progetto eliminato", ["en"] = "Project deleted" },
        ["Callsign"] = new() { ["de"] = "Rufzeichen", ["fr"] = "Indicatif", ["it"] = "Nominativo", ["en"] = "Callsign" },

        // Export/Import
        ["ExportFailed"] = new() { ["de"] = "Export fehlgeschlagen", ["fr"] = "Échec de l'export", ["it"] = "Esportazione fallita", ["en"] = "Export failed" },
        ["StatusReady"] = new() { ["de"] = "Bereit", ["fr"] = "Prêt", ["it"] = "Pronto", ["en"] = "Ready" },
        ["CannotExportNoProject"] = new() { ["de"] = "Export nicht möglich. Bitte speichern Sie zuerst das Projekt.", ["fr"] = "Export impossible. Veuillez d'abord enregistrer le projet.", ["it"] = "Impossibile esportare. Salvare prima il progetto.", ["en"] = "Cannot export. Please save the project first." },
        ["ExportedToFile"] = new() { ["de"] = "Exportiert nach {0}", ["fr"] = "Exporté vers {0}", ["it"] = "Esportato in {0}", ["en"] = "Exported to {0}" },
        ["PdfExportError"] = new() { ["de"] = "PDF-Export fehlgeschlagen: {0}", ["fr"] = "Échec de l'export PDF: {0}", ["it"] = "Esportazione PDF fallita: {0}", ["en"] = "PDF export failed: {0}" },
        ["ExportMarkdownTitle"] = new() { ["de"] = "Ergebnisse als Markdown exportieren", ["fr"] = "Exporter les résultats en Markdown", ["it"] = "Esporta risultati come Markdown", ["en"] = "Export Results as Markdown" },
        ["ExportPdfTitle"] = new() { ["de"] = "Ergebnisse als PDF exportieren", ["fr"] = "Exporter les résultats en PDF", ["it"] = "Esporta risultati come PDF", ["en"] = "Export Results as PDF" },
        ["ImportFailed"] = new() { ["de"] = "Import fehlgeschlagen", ["fr"] = "Échec de l'import", ["it"] = "Importazione fallita", ["en"] = "Import failed" },
        ["ImportWarning"] = new() { ["de"] = "Import-Warnung", ["fr"] = "Avertissement d'import", ["it"] = "Avviso di importazione", ["en"] = "Import Warning" },
        ["MissingMasterData"] = new() { ["de"] = "Einige Stammdaten-Referenzen konnten nicht aufgelöst werden:", ["fr"] = "Certaines références de données de base n'ont pas pu être résolues:", ["it"] = "Alcuni riferimenti ai dati master non sono stati risolti:", ["en"] = "Some master data references could not be resolved:" },
        ["ImportProject"] = new() { ["de"] = "Projekt importieren", ["fr"] = "Importer projet", ["it"] = "Importa progetto", ["en"] = "Import Project" },
        ["ExportProject"] = new() { ["de"] = "Projekt exportieren", ["fr"] = "Exporter projet", ["it"] = "Esporta progetto", ["en"] = "Export Project" },
        ["ExportFactoryData"] = new() { ["de"] = "Werksdaten exportieren", ["fr"] = "Exporter données usine", ["it"] = "Esporta dati di fabbrica", ["en"] = "Export Factory Data" },
        ["ImportFactoryData"] = new() { ["de"] = "Werksdaten importieren", ["fr"] = "Importer données usine", ["it"] = "Importa dati di fabbrica", ["en"] = "Import Factory Data" },
        ["ExportFactoryDataDesc"] = new() { ["de"] = "Exportiert alle Projekte, OKAs und Stammdaten (Factory)", ["fr"] = "Exporte tous les projets, LSMs et données de base (usine)", ["it"] = "Esporta tutti i progetti, LST e dati master (fabbrica)", ["en"] = "Export all projects, PSSs, and master data (factory)" },
        ["ImportProjectConfirmMessage"] = new() { ["de"] = "Ein neues Projekt wird importiert. Fortfahren?", ["fr"] = "Un nouveau projet sera importé. Continuer?", ["it"] = "Verrà importato un nuovo progetto. Continuare?", ["en"] = "A new project will be imported. Continue?" },
        ["ConfirmImport"] = new() { ["de"] = "Import bestätigen", ["fr"] = "Confirmer l'import", ["it"] = "Conferma importazione", ["en"] = "Confirm Import" },
        ["ReplaceMasterDataConfirm"] = new() { ["de"] = "Dies ersetzt ALLE bestehenden Stammdaten. Fortfahren?", ["fr"] = "Cela remplacera TOUTES les données de base existantes. Continuer?", ["it"] = "Questo sostituirà TUTTI i dati master esistenti. Continuare?", ["en"] = "This will replace ALL existing master data. Continue?" },
        ["ExportComplete"] = new() { ["de"] = "Export abgeschlossen", ["fr"] = "Export terminé", ["it"] = "Esportazione completata", ["en"] = "Export Complete" },
        ["ImportComplete"] = new() { ["de"] = "Import abgeschlossen", ["fr"] = "Import terminé", ["it"] = "Importazione completata", ["en"] = "Import Complete" },
        ["FactoryDataExportedTo"] = new() { ["de"] = "Werksdaten exportiert nach:\n{0}", ["fr"] = "Données usine exportées vers:\n{0}", ["it"] = "Dati di fabbrica esportati in:\n{0}", ["en"] = "Factory data exported to:\n{0}" },
        ["FactoryDataImportedFrom"] = new() { ["de"] = "Werksdaten importiert von:\n{0}", ["fr"] = "Données usine importées depuis:\n{0}", ["it"] = "Dati di fabbrica importati da:\n{0}", ["en"] = "Factory data imported from:\n{0}" },
        ["OperationFailed"] = new() { ["de"] = "{0} fehlgeschlagen: {1}", ["fr"] = "{0} a échoué: {1}", ["it"] = "{0} fallito: {1}", ["en"] = "{0} failed: {1}" },
        ["Error"] = new() { ["de"] = "Fehler", ["fr"] = "Erreur", ["it"] = "Errore", ["en"] = "Error" },
        ["Export"] = new() { ["de"] = "Export", ["fr"] = "Export", ["it"] = "Esportazione", ["en"] = "Export" },
        ["Import"] = new() { ["de"] = "Import", ["fr"] = "Import", ["it"] = "Importazione", ["en"] = "Import" },
        ["FailedToOpenDataFolder"] = new() { ["de"] = "Datenordner konnte nicht geöffnet werden: {0}", ["fr"] = "Impossible d'ouvrir le dossier de données: {0}", ["it"] = "Impossibile aprire la cartella dati: {0}", ["en"] = "Failed to open data folder: {0}" },
        ["DatabaseResetRequired"] = new() { ["de"] = "Datenbank-Reset erforderlich", ["fr"] = "Réinitialisation de la base de données requise", ["it"] = "Reset del database richiesto", ["en"] = "Database Reset Required" },
        ["DatabaseResetMessage"] = new() { ["de"] = "Das Datenbankschema ist inkompatibel und muss zurückgesetzt werden.\n\nACHTUNG: Alle Projekte und Konfigurationen werden gelöscht!\n\nKlicken Sie Ja zum Zurücksetzen oder Nein zum Beenden.", ["fr"] = "Le schéma de la base de données est incompatible et doit être réinitialisé.\n\nATTENTION: Tous les projets et configurations seront supprimés!\n\nCliquez Oui pour réinitialiser ou Non pour quitter.", ["it"] = "Lo schema del database è incompatibile e deve essere reimpostato.\n\nATTENZIONE: Tutti i progetti e le configurazioni verranno eliminati!\n\nClicca Sì per reimpostare o No per uscire.", ["en"] = "The database schema is incompatible and needs to be reset.\n\nWARNING: All projects and configurations will be deleted!\n\nClick Yes to reset the database, or No to exit the application." },
        ["PatternPrefix"] = new() { ["de"] = "Muster:", ["fr"] = "Diagramme:", ["it"] = "Pattern:", ["en"] = "Pattern:" },
        ["NoPattern"] = new() { ["de"] = "Kein Muster", ["fr"] = "Pas de diagramme", ["it"] = "Nessun pattern", ["en"] = "No pattern" },
        ["ConfigurationNumber"] = new() { ["de"] = "Konfiguration {0}", ["fr"] = "Configuration {0}", ["it"] = "Configurazione {0}", ["en"] = "Configuration {0}" },
        ["OpenDataFolder"] = new() { ["de"] = "Datenordner öffnen", ["fr"] = "Ouvrir dossier données", ["it"] = "Apri cartella dati", ["en"] = "Open Data Folder" },
        ["DataFolder"] = new() { ["de"] = "Datenordner", ["fr"] = "Dossier données", ["it"] = "Cartella dati", ["en"] = "Data Folder" },
        ["DataFolderDesc"] = new() { ["de"] = "Öffnet den Datenordner mit der Datenbank (nisdata.db) für Git-Commits", ["fr"] = "Ouvre le dossier de données avec la base de données (nisdata.db) pour les commits Git", ["it"] = "Apre la cartella dati con il database (nisdata.db) per i commit Git", ["en"] = "Opens the data folder containing the database (nisdata.db) for Git commits" },
        ["Factory"] = new() { ["de"] = "Werksmodus", ["fr"] = "Mode usine", ["it"] = "Modalità fabbrica", ["en"] = "Factory Mode" },
        ["FactoryMode"] = new() { ["de"] = "WERKSMODUS", ["fr"] = "MODE USINE", ["it"] = "MODALITÀ FABBRICA", ["en"] = "FACTORY MODE" },
        ["EnterFactoryPassword"] = new() { ["de"] = "Bitte Werkspasswort eingeben:", ["fr"] = "Veuillez entrer le mot de passe usine:", ["it"] = "Inserisci la password di fabbrica:", ["en"] = "Please enter factory password:" },
        ["WrongPassword"] = new() { ["de"] = "Falsches Passwort", ["fr"] = "Mot de passe incorrect", ["it"] = "Password errata", ["en"] = "Wrong password" },

        // Calculation Report Labels
        ["CalcTitlePrefix"] = new() { ["de"] = "Immissionsberechnung für", ["fr"] = "Calcul d'immission pour", ["it"] = "Calcolo immissione per", ["en"] = "Emission Calculation for" },
        ["CalcSubtitle"] = new() { ["de"] = "NISV Feldstärkeberechnung", ["fr"] = "Calcul de l'intensité du champ ORNI", ["it"] = "Calcolo intensità campo ORNI", ["en"] = "NISV Field Strength Calculation" },
        ["CalcOperator"] = new() { ["de"] = "Betreiber", ["fr"] = "Opérateur", ["it"] = "Operatore", ["en"] = "Operator" },
        ["CalcCallsign"] = new() { ["de"] = "Rufzeichen", ["fr"] = "Indicatif", ["it"] = "Nominativo", ["en"] = "Callsign" },
        ["CalcAddress"] = new() { ["de"] = "Adresse", ["fr"] = "Adresse", ["it"] = "Indirizzo", ["en"] = "Address" },
        ["CalcLocation"] = new() { ["de"] = "Standort", ["fr"] = "Emplacement", ["it"] = "Posizione", ["en"] = "Location" },
        ["CalcDate"] = new() { ["de"] = "Datum", ["fr"] = "Date", ["it"] = "Data", ["en"] = "Date" },
        ["CalcAllCompliant"] = new() { ["de"] = "ALLE KONFIGURATIONEN KONFORM", ["fr"] = "TOUTES LES CONFIGURATIONS CONFORMES", ["it"] = "TUTTE LE CONFIGURAZIONI CONFORMI", ["en"] = "ALL CONFIGURATIONS COMPLIANT" },
        ["CalcNonCompliantDetected"] = new() { ["de"] = "NICHT KONFORME KONFIGURATIONEN ERKANNT", ["fr"] = "CONFIGURATIONS NON CONFORMES DÉTECTÉES", ["it"] = "CONFIGURAZIONI NON CONFORMI RILEVATE", ["en"] = "NON-COMPLIANT CONFIGURATIONS DETECTED" },
        ["CalcDisclaimer"] = new() { ["de"] = "Diese Berechnung basiert auf der Schweizer NISV (Verordnung über den Schutz vor nichtionisierender Strahlung) und geht von einer Freiraumausbreitung mit einem Bodenreflexionsfaktor von 1.6 aus. Die tatsächliche Feldstärke kann aufgrund von Umweltfaktoren variieren.", ["fr"] = "Ce calcul est basé sur l'ORNI suisse (Ordonnance sur la protection contre le rayonnement non ionisant) et suppose une propagation en espace libre avec un facteur de réflexion au sol de 1.6. L'intensité réelle du champ peut varier en raison de facteurs environnementaux.", ["it"] = "Questo calcolo si basa sull'ORNI svizzera (Ordinanza sulla protezione dalle radiazioni non ionizzanti) e presuppone una propagazione in spazio libero con un fattore di riflessione al suolo di 1.6. L'intensità effettiva del campo può variare a causa di fattori ambientali.", ["en"] = "This calculation is based on the Swiss NISV (Verordnung über den Schutz vor nichtionisierender Strahlung) regulations and assumes free-space propagation with ground reflection factor of 1.6. Actual field strength may vary due to environmental factors." },
        ["Compliant"] = new() { ["de"] = "KONFORM", ["fr"] = "CONFORME", ["it"] = "CONFORME", ["en"] = "COMPLIANT" },
        ["NonCompliant"] = new() { ["de"] = "NICHT KONFORM", ["fr"] = "NON CONFORME", ["it"] = "NON CONFORME", ["en"] = "NON-COMPLIANT" },
        ["CalcTransmitter"] = new() { ["de"] = "Sender", ["fr"] = "Émetteur", ["it"] = "Trasmettitore", ["en"] = "Transmitter" },
        ["CalcCable"] = new() { ["de"] = "Kabel", ["fr"] = "Câble", ["it"] = "Cavo", ["en"] = "Cable" },
        ["CalcAntenna"] = new() { ["de"] = "Antenne", ["fr"] = "Antenne", ["it"] = "Antenna", ["en"] = "Antenna" },
        ["CalcPolarization"] = new() { ["de"] = "Polarisation", ["fr"] = "Polarisation", ["it"] = "Polarizzazione", ["en"] = "Polarization" },
        ["CalcRotation"] = new() { ["de"] = "Rotation", ["fr"] = "Rotation", ["it"] = "Rotazione", ["en"] = "Rotation" },
        ["CalcLinear"] = new() { ["de"] = "Linear", ["fr"] = "Linéaire", ["it"] = "Lineare", ["en"] = "Linear" },
        ["CalcOka"] = new() { ["de"] = "OKA", ["fr"] = "LSM", ["it"] = "LST", ["en"] = "PSS" },
        ["CalcModulation"] = new() { ["de"] = "Modulation", ["fr"] = "Modulation", ["it"] = "Modulazione", ["en"] = "Modulation" },
        ["CalcBuildingDamping"] = new() { ["de"] = "Gebäudedämpfung", ["fr"] = "Atténuation bâtiment", ["it"] = "Attenuazione edificio", ["en"] = "Building Damping" },
        ["CalcStatusCompliant"] = new() { ["de"] = "**Status: KONFORM** - Alle Frequenzen innerhalb der Grenzwerte", ["fr"] = "**Statut: CONFORME** - Toutes les fréquences dans les limites", ["it"] = "**Stato: CONFORME** - Tutte le frequenze entro i limiti", ["en"] = "**Status: COMPLIANT** - All frequencies within limits" },
        ["CalcStatusNonCompliant"] = new() { ["de"] = "**Status: NICHT KONFORM** - Grenzwerte überschritten!", ["fr"] = "**Statut: NON CONFORME** - Limites dépassées!", ["it"] = "**Stato: NON CONFORME** - Limiti superati!", ["en"] = "**Status: NON-COMPLIANT** - Limits exceeded!" },
        ["CalcHorizontal"] = new() { ["de"] = "Horizontal", ["fr"] = "Horizontal", ["it"] = "Orizzontale", ["en"] = "Horizontal" },
        ["CalcVertical"] = new() { ["de"] = "Vertikal", ["fr"] = "Vertical", ["it"] = "Verticale", ["en"] = "Vertical" },
        ["CalcFixed"] = new() { ["de"] = "Fest", ["fr"] = "Fixe", ["it"] = "Fisso", ["en"] = "Fixed" },

        // Calculation Table Row Labels (VB6 translations for fr/it)
        ["CalcFrequency"] = new() { ["de"] = "Frequenz", ["fr"] = "Fréquence", ["it"] = "Frequenza", ["en"] = "Frequency" },
        ["CalcOkaNumber"] = new() { ["de"] = "Nr. des OKA auf dem Situationsplan", ["fr"] = "No du LSM sur le plan de situation", ["it"] = "Nr. del LST sulla planimetria", ["en"] = "PSS number on site plan" },
        ["CalcDistanceToAntenna"] = new() { ["de"] = "Abstand OKA zur Antenne", ["fr"] = "Distance entre le LSM et l'antenne", ["it"] = "Distanza dal LST all'antenna", ["en"] = "Distance PSS to antenna" },
        ["CalcTxPower"] = new() { ["de"] = "Leistung am Senderausgang", ["fr"] = "Puissance à la sortie de l'émetteur", ["it"] = "Potenza d'emissione del trasmettitore", ["en"] = "TX output power" },
        ["CalcActivityFactor"] = new() { ["de"] = "Aktivitätsfaktor", ["fr"] = "Facteur d'activité", ["it"] = "Fattore d'attività", ["en"] = "Activity factor" },
        ["CalcModulationFactor"] = new() { ["de"] = "Modulationsfaktor", ["fr"] = "Facteur de modulation", ["it"] = "Fattore di modulazione", ["en"] = "Modulation factor" },
        ["CalcMeanPower"] = new() { ["de"] = "Mittl. Leistung am Senderausgang", ["fr"] = "Puissance moyenne à la sortie de l'émetteur", ["it"] = "Potenza media d'emissione del trasmettitore", ["en"] = "Mean TX output power" },
        ["CalcCableAttenuation"] = new() { ["de"] = "Kabeldämpfung", ["fr"] = "Atténuation des câbles", ["it"] = "Attenuazione del cavo", ["en"] = "Cable attenuation" },
        ["CalcAdditionalLosses"] = new() { ["de"] = "Übrige Dämpfung", ["fr"] = "Autres atténuations", ["it"] = "Attenuazioni diverse", ["en"] = "Other attenuation" },
        ["CalcTotalAttenuation"] = new() { ["de"] = "Summe der Dämpfung", ["fr"] = "Total des atténuations", ["it"] = "Somma delle attenuazioni", ["en"] = "Sum of attenuation" },
        ["CalcAttenuationFactor"] = new() { ["de"] = "Dämpfungsfaktor", ["fr"] = "Facteur d'atténuation", ["it"] = "Fattore d'attenuazione", ["en"] = "Attenuation factor" },
        ["CalcAntennaGain"] = new() { ["de"] = "Antennengewinn", ["fr"] = "Gain d'antenne", ["it"] = "Guadagno d'antenna", ["en"] = "Antenna gain" },
        ["CalcVerticalAttenuation"] = new() { ["de"] = "Vertikale Winkeldämpfung", ["fr"] = "Atténuation angulaire verticale", ["it"] = "Attenuazione d'angolo verticale", ["en"] = "Vertical angle attenuation" },
        ["CalcTotalAntennaGain"] = new() { ["de"] = "Totaler Antennengewinn", ["fr"] = "Gain total d'antenne", ["it"] = "Guadagno totale dell'antenna", ["en"] = "Total antenna gain" },
        ["CalcGainFactor"] = new() { ["de"] = "Antennengewinnfaktor", ["fr"] = "Facteur du gain total d'antenne", ["it"] = "Fattore di guadagno dell'antenna", ["en"] = "Antenna gain factor" },
        ["CalcEirp"] = new() { ["de"] = "Massgebende Sendeleistung (EIRP)", ["fr"] = "Puissance déterminante (PIRE)", ["it"] = "Potenza di trasmissione determinante (EIRP)", ["en"] = "Relevant TX power (EIRP)" },
        ["CalcErp"] = new() { ["de"] = "Massgebende Sendeleistung (ERP)", ["fr"] = "Puissance déterminante (PAR)", ["it"] = "Potenza di trasmissione determinante (ERP)", ["en"] = "Relevant TX power (ERP)" },
        ["CalcBuildingDampingRow"] = new() { ["de"] = "Gebäudedämpfung", ["fr"] = "Atténuation due au bâtiment", ["it"] = "Attenuazione dello stabile", ["en"] = "Building attenuation" },
        ["CalcBuildingDampingFactor"] = new() { ["de"] = "Gebäudedämpfungsfaktor", ["fr"] = "Facteur d'atténuation due au bâtiment", ["it"] = "Fattore d'attenuazione dello stabile", ["en"] = "Building attenuation factor" },
        ["CalcGroundReflection"] = new() { ["de"] = "Bodenreflexionsfaktor", ["fr"] = "Facteur réflexion du sol", ["it"] = "Fattore di riflessione del suolo", ["en"] = "Ground reflection factor" },
        ["CalcFieldStrength"] = new() { ["de"] = "Massgebende Feldstärke am OKA", ["fr"] = "Intensité de champ électrique déterminante au LSM", ["it"] = "Intensità di campo determinante nel LST", ["en"] = "Relevant field strength at PSS" },
        ["CalcLimit"] = new() { ["de"] = "Immissions-Grenzwert", ["fr"] = "Valeur limite des immissions", ["it"] = "Valore limite delle immissioni", ["en"] = "Immission limit" },
        ["CalcMinSafeDistance"] = new() { ["de"] = "Sicherheitsabstand", ["fr"] = "Distance de sécurité", ["it"] = "Distanza di sicurezza", ["en"] = "Safety distance" },
        ["CalcOkaDistance"] = new() { ["de"] = "OKA Abstand", ["fr"] = "Distance LSM", ["it"] = "Distanza LST", ["en"] = "PSS distance" },

        // Column Explanations (VB6 translations)
        ["CalcColumnExplanations"] = new() { ["de"] = "Erläuterungen zu den verschiedenen Tabellenspalten", ["fr"] = "Explications des différents champs de la tabelle", ["it"] = "Definizione dei termini utilizzati per il calcolo", ["en"] = "Explanation of the table columns" },
        ["CalcExplainF"] = new() { ["de"] = "Sendefrequenz der Amateurfunkstation", ["fr"] = "Fréquence d'émission de la station de radioamateur", ["it"] = "Frequenza di trasmissione della stazione radioamatore", ["en"] = "Transmit frequency of the amateur radio station" },
        ["CalcExplainOkaNumber"] = new() { ["de"] = "Im Situationsplan eingezeichneter Ort für den kurzfristigen Aufenthalt", ["fr"] = "Emplacement du séjour momentané marqué sur le plan de situation", ["it"] = "Spazio/locale di soggiorno di breve durata disegnato sulla planimetria", ["en"] = "Location for short-term stay marked on site plan" },
        ["CalcExplainD"] = new() { ["de"] = "Antenne – Ort für den kurzfristigen Aufenthalt", ["fr"] = "Antenne - lieu de séjour momentané", ["it"] = "Antenna – Spazio/locale di soggiorno di breve durata", ["en"] = "Antenna – location for short-term stay" },
        ["CalcExplainP"] = new() { ["de"] = "Ausgangsleistung des Senders oder Linears", ["fr"] = "Puissance de sortie de l'émetteur ou de l'ampli linéaire", ["it"] = "Potenza d'uscita del trasmettitore o dell'amplificatore lineare", ["en"] = "Output power of transmitter or linear amplifier" },
        ["CalcExplainAF"] = new() { ["de"] = "In der Regel AF = 0.5", ["fr"] = "Normalement AF = 0.5", ["it"] = "Di regola AF = 0.5", ["en"] = "Normally AF = 0.5" },
        ["CalcExplainMF"] = new() { ["de"] = "bei SSB: MF=0.2, bei CW: MF=0.4, bei FM/RTTY/PSK31: MF=1.0", ["fr"] = "en SSB: MF=0.2, en CW: MF=0.4, en FM/RTTY/PSK31: MF=1.0", ["it"] = "in SSB: MF=0.2, in CW: MF=0.4, in FM/RTTY/PSK31: MF=1.0", ["en"] = "SSB: MF=0.2, CW: MF=0.4, FM/RTTY/PSK31: MF=1.0" },
        ["CalcExplainPm"] = new() { ["de"] = "Ausgangsleistung reduziert um Aktivitäts- und Modulationsfaktor", ["fr"] = "Puissance de sortie diminuée par le facteur d'activité et de modulation", ["it"] = "Potenza d'uscita ridotta secondo il fattore d'attività e di modulazione", ["en"] = "Output power reduced by activity and modulation factor" },
        ["CalcExplainA1"] = new() { ["de"] = "Kabeldämpfung bezogen auf Kabellänge", ["fr"] = "Atténuation du câble par rapport à sa longueur", ["it"] = "Attenuazione del cavo in rapporto alla sua lunghezza", ["en"] = "Cable attenuation relative to cable length" },
        ["CalcExplainA2"] = new() { ["de"] = "Stecker, SWR-Brücke, Antennenschalter", ["fr"] = "Prises et fiches, coupleur et commutateurs d'antenne", ["it"] = "Prese e PL, accoppiatori e commutatori d'antenna", ["en"] = "Connectors, SWR bridge, antenna switch" },
        ["CalcExplainA"] = new() { ["de"] = "Kabeldämpfung + übrige Dämpfung", ["fr"] = "Atténuations des câbles et autres atténuations", ["it"] = "Cavo e attenuazioni diverse", ["en"] = "Cable attenuation + other attenuation" },
        ["CalcExplainAFactor"] = new() { ["de"] = "In absolute Zahl umgerechnete «Summe der Dämpfungen»", ["fr"] = "Total des atténuations converties en valeur absolue", ["it"] = "Somma delle attenuazioni convertite in valore assoluto", ["en"] = "Sum of attenuation converted to absolute value" },
        ["CalcExplainG1"] = new() { ["de"] = "Maximaler Gewinn der Antenne gemäss Hersteller", ["fr"] = "Gain maximal d'antenne selon le fabricant", ["it"] = "Guadagno massimo secondo il costruttore", ["en"] = "Maximum antenna gain per manufacturer" },
        ["CalcExplainG2"] = new() { ["de"] = "Gewinnverminderung, wegen vertikalem Strahlungsdiagramm der Antenne", ["fr"] = "Diminution du gain selon diagramme du lobe de rayonnement vertical", ["it"] = "Diminuzione del guadagno dovuto all'angolo d'irradiazione verticale", ["en"] = "Gain reduction due to vertical radiation pattern of antenna" },
        ["CalcExplainG"] = new() { ["de"] = "Antennengewinn − vertikale Winkeldämpfung", ["fr"] = "Gain d'antenne moins l'atténuation verticale d'angle", ["it"] = "Guadagno dell'antenna meno l'attenuazione verticale", ["en"] = "Antenna gain − vertical angle attenuation" },
        ["CalcExplainGFactor"] = new() { ["de"] = "In absolute Zahl umgerechneter «Antennengewinn»", ["fr"] = "Gain total d'antenne converti en valeur absolue", ["it"] = "Somma del guadagno dell'antenna convertito in valore", ["en"] = "Antenna gain converted to absolute value" },
        ["CalcExplainPs"] = new() { ["de"] = "Äquivalente abgestrahlte Leistung bezogen auf einen isotropen Strahler", ["fr"] = "Puissance équivalente rayonnée comparée à une antenne isotropique", ["it"] = "Potenza irradiata equivalente ad una irradiazione isotropa", ["en"] = "Equivalent radiated power relative to isotropic radiator" },
        ["CalcExplainPsPrime"] = new() { ["de"] = "Äquivalente abgestrahlte Leistung bezogen auf einen Dipol", ["fr"] = "Puissance équivalente rayonnée comparée à un dipôle", ["it"] = "Potenza irradiata equivalente riferita ad un dipolo", ["en"] = "Equivalent radiated power relative to dipole" },
        ["CalcExplainAg"] = new() { ["de"] = "Dämpfung durch Gebäudemauern und Decken", ["fr"] = "Atténuation dues aux murs et couverture du bâtiment", ["it"] = "Attenuazione dei muri e del tetto", ["en"] = "Attenuation by building walls and ceilings" },
        ["CalcExplainAG"] = new() { ["de"] = "In absolute Zahlen umgerechnete «Gebäudedämpfung»", ["fr"] = "Atténuation du bâtiment converti en valeur absolue", ["it"] = "Attenuazione dello stabile convertito in valore assoluto", ["en"] = "Building attenuation converted to absolute value" },
        ["CalcExplainKr"] = new() { ["de"] = "Faktor welcher zu einer Zunahme der Feldstärke führt", ["fr"] = "Facteur amenant une augmentation de la force du champ", ["it"] = "Fattore che determina un aumento dell'intensità di campo", ["en"] = "Factor leading to an increase in field strength" },
        ["CalcExplainE"] = new() { ["de"] = "6-Minuten-Mittelwert der Feldstärke am Ort für den kurzfristigen Aufenthalt", ["fr"] = "Valeur moyenne sur 6 minutes de l'intensité de champ électrique au lieu de séjour momentané", ["it"] = "Valore medio durante 6 minuti dell'intensità di campo nello spazio/locale di soggiorno di breve durata", ["en"] = "6-minute average field strength at location for short-term stay" },
        ["CalcExplainEigw"] = new() { ["de"] = "Immissions-Grenzwert für die elektrische Feldstärke gemäss NISV", ["fr"] = "Valeur limite d'immissions de l'intensité de champ électrique selon ORNI", ["it"] = "Valore limite d'immissione per l'intensità di campo elettrico ai sensi dell'ORNI", ["en"] = "Immission limit for electric field strength per NISV" },
        ["CalcExplainDs"] = new() { ["de"] = "Distanz von der Antenne, wo der Immissions-Grenzwert erreicht wird", ["fr"] = "Distance jusqu'à l'antenne à partir de laquelle la valeur limite des immissions est atteinte", ["it"] = "Distanza tra l'antenna ed il luogo dove è raggiunto il valore limite", ["en"] = "Distance from antenna where immission limit is reached" },
    };
}
