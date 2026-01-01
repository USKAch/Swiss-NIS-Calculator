using System;
using System.Collections.Generic;
using System.IO;
using NIS.Desktop.Localization;
using NIS.Desktop.Models;
using NIS.Desktop.ViewModels;
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

    public void GenerateReport(Project project, List<ConfigurationResult> results, string filePath)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(c => ComposeHeader(c, project));
                page.Content().Element(c => ComposeContent(c, project, results));
                page.Footer().Element(ComposeFooter);
            });
        });

        document.GeneratePdf(filePath);
    }

    public byte[] GenerateReportBytes(Project project, List<ConfigurationResult> results)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(c => ComposeHeader(c, project));
                page.Content().Element(c => ComposeContent(c, project, results));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private void ComposeHeader(IContainer container, Project project)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                var title = !string.IsNullOrWhiteSpace(project.Name) ? project.Name : "NIS Calculation Report";
                column.Item().Text(title)
                    .FontSize(18).Bold().FontColor(Colors.Blue.Medium);
                column.Item().Text("NISV Field Strength Calculation")
                    .FontSize(11).FontColor(Colors.Grey.Medium);
            });

            row.ConstantItem(100).AlignRight().Text(DateTime.Now.ToString("dd.MM.yyyy"))
                .FontSize(10).FontColor(Colors.Grey.Medium);
        });
    }

    private void ComposeContent(IContainer container, Project project, List<ConfigurationResult> results)
    {
        container.PaddingVertical(10).Column(column =>
        {
            column.Spacing(12);

            // Project Information
            column.Item().Element(c => ComposeSection(c, "Project Information", section =>
            {
                section.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(2);
                    });

                    if (!string.IsNullOrEmpty(project.Operator))
                        AddInfoRow(table, "Operator", project.Operator);
                    if (!string.IsNullOrEmpty(project.Callsign))
                        AddInfoRow(table, "Callsign", project.Callsign);
                    if (!string.IsNullOrEmpty(project.Address))
                        AddInfoRow(table, "Address", project.Address);
                    if (!string.IsNullOrEmpty(project.Location))
                        AddInfoRow(table, "Location", project.Location);
                });
            }));

            // Overall Compliance Summary
            var allCompliant = results.TrueForAll(r => r.IsCompliant);
            column.Item().Element(c => ComposeComplianceSummary(c, allCompliant, results.Count));

            // Configuration Results
            foreach (var result in results)
            {
                column.Item().Element(c => ComposeConfigurationResult(c, result));
            }

            // Disclaimer
            column.Item().PaddingTop(15).Text(
                "This calculation is based on the Swiss NISV (Verordnung über den Schutz vor nichtionisierender Strahlung) " +
                "regulations and assumes free-space propagation with ground reflection factor of 1.6. " +
                "Actual field strength may vary due to environmental factors.")
                .FontSize(8).FontColor(Colors.Grey.Medium).Italic();
        });
    }

    private void ComposeComplianceSummary(IContainer container, bool allCompliant, int configCount)
    {
        var bgColor = allCompliant ? Colors.Green.Lighten4 : Colors.Red.Lighten4;
        var textColor = allCompliant ? Colors.Green.Darken2 : Colors.Red.Darken2;

        container.Background(bgColor).Padding(12).Column(column =>
        {
            column.Item().AlignCenter().Text("COMPLIANCE SUMMARY")
                .FontSize(11).Bold().FontColor(textColor);

            column.Item().AlignCenter().PaddingVertical(8)
                .Text(allCompliant ? "ALL CONFIGURATIONS COMPLIANT" : "NON-COMPLIANT CONFIGURATIONS DETECTED")
                .FontSize(14).Bold().FontColor(textColor);

            column.Item().AlignCenter().Text($"{configCount} configuration(s) analyzed")
                .FontSize(10).FontColor(textColor);
        });
    }

    private void ComposeConfigurationResult(IContainer container, ConfigurationResult result)
    {
        var borderColor = result.IsCompliant ? Colors.Green.Medium : Colors.Red.Medium;

        container.Border(1).BorderColor(borderColor).Padding(10).Column(column =>
        {
            column.Spacing(8);

            // Configuration Header
            column.Item().Row(row =>
            {
                row.RelativeItem().Text(result.ConfigurationName).FontSize(12).Bold();

                var statusBg = result.IsCompliant ? Colors.Green.Lighten3 : Colors.Red.Lighten3;
                var statusText = result.IsCompliant ? "COMPLIANT" : "NON-COMPLIANT";
                row.ConstantItem(100).Background(statusBg).Padding(4).AlignCenter()
                    .Text(statusText).FontSize(9).Bold();
            });

            // Configuration Summary Table
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                });

                table.Cell().Text("Antenna:").FontSize(9).Bold();
                table.Cell().Text(result.AntennaName).FontSize(9);
                table.Cell().Text("Power:").FontSize(9).Bold();
                table.Cell().Text($"{result.PowerWatts} W").FontSize(9);

                table.Cell().Text("Modulation:").FontSize(9).Bold();
                table.Cell().Text(result.Modulation).FontSize(9);
                table.Cell().Text("OKA Distance:").FontSize(9).Bold();
                table.Cell().Text($"{result.OkaDistance:F1} m").FontSize(9);

                table.Cell().Text("Polarization:").FontSize(9).Bold();
                table.Cell().Text(result.IsHorizontallyPolarized ? "Horizontal" : "Vertical").FontSize(9);
                table.Cell().Text("Building Damping:").FontSize(9).Bold();
                table.Cell().Text($"{result.BuildingDampingDb:F2} dB").FontSize(9);
            });

            // Band Results Table
            if (result.BandResults.Count > 0)
            {
                column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);   // Freq
                        columns.RelativeColumn(1);   // Gain
                        columns.RelativeColumn(1);   // Pm
                        columns.RelativeColumn(1);   // EIRP
                        columns.RelativeColumn(1.2f); // Field
                        columns.RelativeColumn(1);   // Limit
                        columns.RelativeColumn(1);   // Safety
                        columns.RelativeColumn(1);   // Status
                    });

                    // Header
                    table.Header(header =>
                    {
                        var headerStyle = TextStyle.Default.FontSize(8).Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(3).Text("Freq MHz").Style(headerStyle);
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(3).Text("Gain dBi").Style(headerStyle);
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(3).Text($"{Strings.Instance.PmittelLbl} W").Style(headerStyle);
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(3).Text("EIRP W").Style(headerStyle);
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(3).Text("E' V/m").Style(headerStyle);
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(3).Text("Limit V/m").Style(headerStyle);
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(3).Text("ds m").Style(headerStyle);
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(3).Text("Status").Style(headerStyle);
                    });

                    // Data rows
                    foreach (var band in result.BandResults)
                    {
                        var rowStyle = TextStyle.Default.FontSize(8);
                        var statusColor = band.IsCompliant ? Colors.Green.Darken1 : Colors.Red.Darken1;

                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2)
                            .Text($"{band.FrequencyMHz:F0}").Style(rowStyle);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2)
                            .Text($"{band.GainDbi:F2}").Style(rowStyle);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2)
                            .Text($"{band.MeanPowerW:F2}").Style(rowStyle);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2)
                            .Text($"{band.Eirp:F2}").Style(rowStyle);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2)
                            .Text($"{band.FieldStrength:F2}").Style(rowStyle).Bold();
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2)
                            .Text($"{band.Limit:F1}").Style(rowStyle);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2)
                            .Text($"{band.SafetyDistance:F2}").Style(rowStyle);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2)
                            .Text(band.IsCompliant ? "OK" : "FAIL").Style(rowStyle).FontColor(statusColor).Bold();
                    }
                });
            }
        });
    }

    private void ComposeSection(IContainer container, string title, Action<ColumnDescriptor> content)
    {
        container.Column(column =>
        {
            column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                .PaddingBottom(4).Text(title).FontSize(12).Bold();
            column.Item().PaddingTop(8).Column(content);
        });
    }

    private void AddInfoRow(TableDescriptor table, string label, string value)
    {
        table.Cell().Padding(2).Text(label + ":").FontSize(9).Bold();
        table.Cell().Padding(2).Text(value).FontSize(9);
    }

    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(text =>
        {
            text.DefaultTextStyle(x => x.FontSize(8).FontColor(Colors.Grey.Medium));
            text.Span("Generated by Swiss NIS Calculator | ");
            text.Span("Page ");
            text.CurrentPageNumber();
            text.Span(" of ");
            text.TotalPages();
        });
    }
}
