using System;
using System.Collections.Generic;
using NIS.Desktop.Localization;
using NIS.Desktop.ViewModels;

namespace NIS.Desktop.Services;

/// <summary>
/// Defines a row in the calculation results table.
/// </summary>
public class CalculationRowDefinition
{
    public required string LabelKey { get; init; }
    public required string Symbol { get; init; }
    public string? SymbolKey { get; init; }  // If set, use Strings lookup instead of Symbol
    public required string Unit { get; init; }
    public required string Format { get; init; }
    public required bool IsBold { get; init; }

    // Value getters - exactly one should be set
    public Func<BandResult, double>? BandValueGetter { get; init; }
    public Func<ConfigurationResult, double>? ConfigValueGetter { get; init; }
    public Func<ConfigurationResult, string>? StringValueGetter { get; init; }

    public bool IsConstantNumeric => ConfigValueGetter != null;
    public bool IsConstantString => StringValueGetter != null;

    public string GetSymbol(Strings s) => SymbolKey != null ? s.Get(SymbolKey) : Symbol;
    public string GetLabel(Strings s) => s.Get(LabelKey);
}

/// <summary>
/// Shared calculation table structure for Markdown and PDF reports.
/// Single source of truth for FSD compliance.
/// </summary>
public static class CalculationTableDefinition
{
    /// <summary>
    /// The 22 calculation rows (OKA number moved to header).
    /// Bold rows: f, d, P, Pm, P's, E', ds
    /// </summary>
    public static IReadOnlyList<CalculationRowDefinition> Rows { get; } = new CalculationRowDefinition[]
    {
        new() { LabelKey = "CalcFrequency", Symbol = "f", Unit = "MHz", Format = "F0", IsBold = true, BandValueGetter = b => b.FrequencyMHz },
        new() { LabelKey = "CalcDistanceToAntenna", Symbol = "d", Unit = "m", Format = "F1", IsBold = true, ConfigValueGetter = c => c.OkaDistance },
        new() { LabelKey = "CalcTxPower", Symbol = "P", Unit = "W", Format = "F2", IsBold = true, BandValueGetter = b => b.TxPowerW },
        new() { LabelKey = "CalcActivityFactor", Symbol = "AF", Unit = "-", Format = "F2", IsBold = false, BandValueGetter = b => b.ActivityFactor },
        new() { LabelKey = "CalcModulationFactor", Symbol = "MF", Unit = "-", Format = "F2", IsBold = false, BandValueGetter = b => b.ModulationFactor },
        new() { LabelKey = "CalcMeanPower", Symbol = "Pm", SymbolKey = "PmittelLbl", Unit = "W", Format = "F2", IsBold = true, BandValueGetter = b => b.MeanPowerW },
        new() { LabelKey = "CalcCableAttenuation", Symbol = "a1", Unit = "dB", Format = "F2", IsBold = false, BandValueGetter = b => b.CableLossDb },
        new() { LabelKey = "CalcAdditionalLosses", Symbol = "a2", Unit = "dB", Format = "F2", IsBold = false, BandValueGetter = b => b.AdditionalLossDb },
        new() { LabelKey = "CalcTotalAttenuation", Symbol = "a", Unit = "dB", Format = "F2", IsBold = false, BandValueGetter = b => b.TotalLossDb },
        new() { LabelKey = "CalcAttenuationFactor", Symbol = "A", Unit = "-", Format = "F2", IsBold = false, BandValueGetter = b => b.AttenuationFactor },
        new() { LabelKey = "CalcAntennaGain", Symbol = "g1", Unit = "dBi", Format = "F2", IsBold = false, BandValueGetter = b => b.GainDbi },
        new() { LabelKey = "CalcVerticalAttenuation", Symbol = "g2", Unit = "dB", Format = "F2", IsBold = false, BandValueGetter = b => b.VerticalAttenuation },
        new() { LabelKey = "CalcTotalAntennaGain", Symbol = "g", Unit = "dB", Format = "F2", IsBold = false, BandValueGetter = b => b.TotalGainDbi },
        new() { LabelKey = "CalcGainFactor", Symbol = "G", Unit = "-", Format = "F2", IsBold = false, BandValueGetter = b => b.GainFactor },
        new() { LabelKey = "CalcEirp", Symbol = "Ps", Unit = "W", Format = "F2", IsBold = false, BandValueGetter = b => b.Eirp },
        new() { LabelKey = "CalcErp", Symbol = "P's", Unit = "W", Format = "F2", IsBold = true, BandValueGetter = b => b.Erp },
        new() { LabelKey = "CalcBuildingDampingRow", Symbol = "ag", Unit = "dB", Format = "F2", IsBold = false, BandValueGetter = b => b.BuildingDampingDb },
        new() { LabelKey = "CalcBuildingDampingFactor", Symbol = "AG", Unit = "-", Format = "F2", IsBold = false, BandValueGetter = b => b.BuildingDampingFactor },
        new() { LabelKey = "CalcGroundReflection", Symbol = "kr", Unit = "-", Format = "F2", IsBold = false, BandValueGetter = b => b.GroundReflectionFactor },
        new() { LabelKey = "CalcFieldStrength", Symbol = "E'", Unit = "V/m", Format = "F2", IsBold = true, BandValueGetter = b => b.FieldStrength },
        new() { LabelKey = "CalcLimit", Symbol = "EIGW", Unit = "V/m", Format = "F1", IsBold = false, BandValueGetter = b => b.Limit },
        new() { LabelKey = "CalcMinSafeDistance", Symbol = "ds", Unit = "m", Format = "F2", IsBold = true, BandValueGetter = b => b.SafetyDistance },
    };

    /// <summary>
    /// Column explanations for the report footer.
    /// Tuple: (Symbol or SymbolKey, ExplanationKey)
    /// </summary>
    public static IReadOnlyList<(string Symbol, string? SymbolKey, string ExplanationKey)> Explanations { get; } = new[]
    {
        ("f", null, "CalcExplainF"),
        ("d", null, "CalcExplainD"),
        ("P", null, "CalcExplainP"),
        ("AF", null, "CalcExplainAF"),
        ("MF", null, "CalcExplainMF"),
        ("Pm", "PmittelLbl", "CalcExplainPm"),
        ("a1", null, "CalcExplainA1"),
        ("a2", null, "CalcExplainA2"),
        ("a", null, "CalcExplainA"),
        ("A", null, "CalcExplainAFactor"),
        ("g1", null, "CalcExplainG1"),
        ("g2", null, "CalcExplainG2"),
        ("g", null, "CalcExplainG"),
        ("G", null, "CalcExplainGFactor"),
        ("Ps", null, "CalcExplainPs"),
        ("P's", null, "CalcExplainPsPrime"),
        ("ag", null, "CalcExplainAg"),
        ("AG", null, "CalcExplainAG"),
        ("kr", null, "CalcExplainKr"),
        ("E'", null, "CalcExplainE"),
        ("EIGW", null, "CalcExplainEigw"),
        ("ds", null, "CalcExplainDs"),
    };

    /// <summary>
    /// Get the display symbol, using localization if SymbolKey is set.
    /// </summary>
    public static string GetDisplaySymbol(string symbol, string? symbolKey, Strings s)
        => symbolKey != null ? s.Get(symbolKey) : symbol;
}
