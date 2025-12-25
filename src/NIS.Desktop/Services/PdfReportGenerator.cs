using System;
using System.IO;
using NIS.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NIS.Desktop.Services;

public class PdfReportGenerator
{
    static PdfReportGenerator()
    {
        // QuestPDF Community License
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public void GenerateReport(CalculationResult result, string filePath)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Element(ComposeHeader);
                page.Content().Element(c => ComposeContent(c, result));
                page.Footer().Element(ComposeFooter);
            });
        });

        document.GeneratePdf(filePath);
    }

    public byte[] GenerateReportBytes(CalculationResult result)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Element(ComposeHeader);
                page.Content().Element(c => ComposeContent(c, result));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text("Swiss NIS Calculator")
                    .FontSize(20).Bold().FontColor(Colors.Blue.Medium);
                column.Item().Text("RF Field Strength Calculation Report")
                    .FontSize(12).FontColor(Colors.Grey.Medium);
            });

            row.ConstantItem(100).AlignRight().Text(DateTime.Now.ToString("dd.MM.yyyy"))
                .FontSize(10).FontColor(Colors.Grey.Medium);
        });
    }

    private void ComposeContent(IContainer container, CalculationResult result)
    {
        var input = result.Input;
        if (input == null) return;

        container.PaddingVertical(10).Column(column =>
        {
            column.Spacing(15);

            // Operator Information
            if (!string.IsNullOrEmpty(input.Callsign))
            {
                column.Item().Element(c => ComposeSection(c, "Operator", section =>
                {
                    section.Item().Text($"Callsign: {input.Callsign}").FontSize(12);
                }));
            }

            // Main Result
            column.Item().Element(c => ComposeResultBox(c, result));

            // Input Parameters
            column.Item().Element(c => ComposeSection(c, "Input Parameters", section =>
            {
                section.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Parameter").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Value").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Unit").Bold();
                    });

                    // Data rows
                    AddTableRow(table, "Frequency", input.FrequencyMHz.ToString("F1"), "MHz");
                    AddTableRow(table, "TX Power", input.TxPowerWatts.ToString("F1"), "W");
                    AddTableRow(table, "Distance", input.DistanceMeters.ToString("F1"), "m");
                    AddTableRow(table, "Evaluation Height", input.EvaluationHeightMeters.ToString("F1"), "m");
                    AddTableRow(table, "Antenna Gain", input.AntennaGainDbi.ToString("F1"), "dBi");
                    AddTableRow(table, "Angle Attenuation", input.AngleAttenuationDb.ToString("F1"), "dB");
                    AddTableRow(table, "Total Cable Loss", input.TotalCableLossDb.ToString("F2"), "dB");
                    AddTableRow(table, "Building Damping", input.BuildingDampingDb.ToString("F1"), "dB");
                    AddTableRow(table, "Modulation Factor", input.ModulationFactor.ToString("F2"), "");
                });
            }));

            // Calculated Values
            column.Item().Element(c => ComposeSection(c, "Calculation Results", section =>
            {
                section.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Result").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Value").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Unit").Bold();
                    });

                    AddTableRow(table, "Mean Power (Pm)", result.MeanPowerWatts.ToString("F2"), "W");
                    AddTableRow(table, "Total Attenuation (a)", result.TotalAttenuationDb.ToString("F2"), "dB");
                    AddTableRow(table, "Attenuation Factor (A)", result.AttenuationFactor.ToString("F4"), "");
                    AddTableRow(table, "Total Antenna Gain (g)", result.TotalAntennaGainDb.ToString("F2"), "dB");
                    AddTableRow(table, "Antenna Gain Factor (G)", result.AntennaGainFactor.ToString("F4"), "");
                    AddTableRow(table, "EIRP (Ps)", result.EirpWatts.ToString("F2"), "W");
                    AddTableRow(table, "ERP (P's)", result.ErpWatts.ToString("F2"), "W");
                    AddTableRow(table, "Building Damping Factor (AG)", result.BuildingDampingFactor.ToString("F4"), "");
                    AddTableRow(table, "Ground Reflection Factor (kr)", result.GroundReflectionFactor.ToString("F1"), "");
                    AddTableRow(table, "Field Strength (E')", result.FieldStrengthVm.ToString("F3"), "V/m");
                    AddTableRow(table, "Swiss NIS Limit (EIGW)", result.NisLimitVm.ToString("F1"), "V/m");
                    AddTableRow(table, "Limit Usage", result.NisLimitPercentage.ToString("F1"), "%");
                    AddTableRow(table, "Safety Distance (ds)", result.SafetyDistanceMeters.ToString("F2"), "m");
                    AddTableRow(table, "Power Density", result.PowerDensityWm2.ToString("E3"), "W/m²");
                });
            }));

            // Compliance Statement
            column.Item().Element(c => ComposeComplianceBox(c, result));

            // Disclaimer
            column.Item().PaddingTop(20).Text(
                "This calculation is based on the Swiss NISV (Verordnung über den Schutz vor nichtionisierender Strahlung) " +
                "regulations and assumes free-space propagation with ground reflection factor of 1.6. " +
                "Actual field strength may vary due to environmental factors.")
                .FontSize(9).FontColor(Colors.Grey.Medium).Italic();
        });
    }

    private void ComposeResultBox(IContainer container, CalculationResult result)
    {
        var bgColor = result.IsWithinLimits ? Colors.Green.Lighten4 : Colors.Red.Lighten4;
        var textColor = result.IsWithinLimits ? Colors.Green.Darken2 : Colors.Red.Darken2;

        container.Background(bgColor).Padding(15).Column(column =>
        {
            column.Item().AlignCenter().Text("FIELD STRENGTH RESULT")
                .FontSize(12).Bold().FontColor(textColor);

            column.Item().AlignCenter().PaddingVertical(10)
                .Text($"{result.FieldStrengthVm:F3} V/m")
                .FontSize(28).Bold().FontColor(textColor);

            column.Item().AlignCenter().Text(
                result.IsWithinLimits
                    ? $"WITHIN SWISS NIS LIMITS ({result.NisLimitPercentage:F1}% of {result.NisLimitVm} V/m)"
                    : $"EXCEEDS SWISS NIS LIMITS ({result.NisLimitPercentage:F1}% of {result.NisLimitVm} V/m)")
                .FontSize(11).Bold().FontColor(textColor);
        });
    }

    private void ComposeComplianceBox(IContainer container, CalculationResult result)
    {
        var bgColor = result.IsWithinLimits ? Colors.Green.Lighten5 : Colors.Red.Lighten5;
        var borderColor = result.IsWithinLimits ? Colors.Green.Medium : Colors.Red.Medium;

        container.Border(1).BorderColor(borderColor).Background(bgColor).Padding(10).Column(column =>
        {
            column.Item().Text("Compliance Statement").Bold();
            column.Item().PaddingTop(5).Text(
                result.IsWithinLimits
                    ? $"The calculated field strength of {result.FieldStrengthVm:F3} V/m is {result.NisLimitPercentage:F1}% of the " +
                      $"Swiss NIS limit of {result.NisLimitVm} V/m for {result.Input?.FrequencyMHz:F1} MHz. " +
                      "The installation COMPLIES with Swiss regulations."
                    : $"The calculated field strength of {result.FieldStrengthVm:F3} V/m EXCEEDS the " +
                      $"Swiss NIS limit of {result.NisLimitVm} V/m for {result.Input?.FrequencyMHz:F1} MHz by " +
                      $"{result.NisLimitPercentage - 100:F1}%. " +
                      "The installation DOES NOT COMPLY with Swiss regulations.")
                .FontSize(10);
        });
    }

    private void ComposeSection(IContainer container, string title, Action<ColumnDescriptor> content)
    {
        container.Column(column =>
        {
            column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                .PaddingBottom(5).Text(title).FontSize(14).Bold();
            column.Item().PaddingTop(10).Column(content);
        });
    }

    private void AddTableRow(TableDescriptor table, string parameter, string value, string unit)
    {
        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(parameter);
        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).AlignRight().Text(value);
        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(unit);
    }

    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(text =>
        {
            text.DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.Grey.Medium));
            text.Span("Generated by Swiss NIS Calculator • ");
            text.Span("Page ");
            text.CurrentPageNumber();
            text.Span(" of ");
            text.TotalPages();
        });
    }
}
