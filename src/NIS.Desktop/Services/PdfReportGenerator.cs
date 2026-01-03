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
        var document = CreateDocument(project, results);
        document.GeneratePdf(filePath);
    }

    public byte[] GenerateReportBytes(Project project, List<ConfigurationResult> results)
    {
        var document = CreateDocument(project, results);
        return document.GeneratePdf();
    }

    private Document CreateDocument(Project project, List<ConfigurationResult> results)
    {
        return Document.Create(container =>
        {
            // One page per configuration
            for (int i = 0; i < results.Count; i++)
            {
                var result = results[i];
                var configNumber = i + 1;
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(c => ComposeHeader(c, project));
                    page.Content().Element(c => ComposeConfigurationPage(c, project, result, configNumber));
                    page.Footer().Element(ComposeFooter);
                });
            }

            // Last page: Explanations
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(c => ComposeHeader(c, project));
                page.Content().Element(ComposeExplanationsPage);
                page.Footer().Element(ComposeFooter);
            });
        });
    }

    private void ComposeHeader(IContainer container, Project project)
    {
        var s = Strings.Instance;
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                var title = !string.IsNullOrWhiteSpace(project.Name) ? project.Name : s.CalcTitlePrefix;
                column.Item().Text(title)
                    .FontSize(18).Bold().FontColor(Colors.Blue.Medium);
                column.Item().Text(s.CalcSubtitle)
                    .FontSize(11).FontColor(Colors.Grey.Medium);
            });

            row.ConstantItem(100).AlignRight().Text(DateTime.Now.ToString("dd.MM.yyyy"))
                .FontSize(10).FontColor(Colors.Grey.Medium);
        });
    }

    private void ComposeConfigurationPage(IContainer container, Project project, ConfigurationResult result, int configNumber)
    {
        var s = Strings.Instance;
        container.PaddingVertical(5).Column(column =>
        {
            column.Spacing(8);

            // Project Information (compact)
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                });

                // Row 1: Operator + Callsign
                if (!string.IsNullOrEmpty(project.Operator))
                {
                    table.Cell().Padding(2).Text(s.CalcOperator + ":").FontSize(9).Bold();
                    table.Cell().Padding(2).Text(project.Operator).FontSize(9);
                }
                else
                {
                    table.Cell(); table.Cell();
                }
                if (!string.IsNullOrEmpty(project.Callsign))
                {
                    table.Cell().Padding(2).Text(s.CalcCallsign + ":").FontSize(9).Bold();
                    table.Cell().Padding(2).Text(project.Callsign).FontSize(9);
                }
                else
                {
                    table.Cell(); table.Cell();
                }

                // Row 2: Address + Location
                if (!string.IsNullOrEmpty(project.Address))
                {
                    table.Cell().Padding(2).Text(s.CalcAddress + ":").FontSize(9).Bold();
                    table.Cell().Padding(2).Text(project.Address).FontSize(9);
                }
                else
                {
                    table.Cell(); table.Cell();
                }
                if (!string.IsNullOrEmpty(project.Location))
                {
                    table.Cell().Padding(2).Text(s.CalcLocation + ":").FontSize(9).Bold();
                    table.Cell().Padding(2).Text(project.Location).FontSize(9);
                }
                else
                {
                    table.Cell(); table.Cell();
                }
            });

            // Configuration content
            column.Item().Element(c => ComposeConfigurationResult(c, result, configNumber));

            // Disclaimer at bottom
            column.Item().PaddingTop(10).Text(s.CalcDisclaimer)
                .FontSize(7).FontColor(Colors.Grey.Medium).Italic();
        });
    }

    private void ComposeConfigurationResult(IContainer container, ConfigurationResult result, int configNumber)
    {
        var s = Strings.Instance;
        var borderColor = result.IsCompliant ? Colors.Green.Medium : Colors.Red.Medium;

        container.Border(1).BorderColor(borderColor).Padding(10).Column(column =>
        {
            column.Spacing(8);

            // Configuration Header
            var configName = !string.IsNullOrWhiteSpace(result.ConfigurationName)
                ? result.ConfigurationName
                : $"{s.Configuration} #{configNumber}";
            column.Item().Row(row =>
            {
                row.RelativeItem().Text(configName).FontSize(12).Bold();

                var statusBg = result.IsCompliant ? Colors.Green.Lighten3 : Colors.Red.Lighten3;
                var statusText = result.IsCompliant ? s.Compliant : s.NonCompliant;
                row.ConstantItem(120).Background(statusBg).Padding(4).AlignCenter()
                    .Text(statusText).FontSize(9).Bold();
            });

            // Configuration Summary Table (matching markdown)
            column.Item().Background(Colors.Grey.Lighten4).Padding(8).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                });

                // Radio/Transmitter
                if (result.HasLinear)
                {
                    AddInfoRow(table, s.CalcTransmitter, result.RadioName);
                    AddInfoRow(table, s.CalcLinear, $"{result.LinearName}, {result.LinearPowerWatts:F0}W");
                }
                else
                {
                    AddInfoRow(table, s.CalcTransmitter, $"{result.RadioName}, {result.RadioPowerWatts:F0}W");
                }

                AddInfoRow(table, s.CalcCable, result.CableDescription);
                AddInfoRow(table, s.CalcAntenna, result.AntennaName);
                AddInfoRow(table, s.CalcPolarization, result.IsHorizontallyPolarized ? s.CalcHorizontal : s.CalcVertical);
                AddInfoRow(table, s.CalcRotation, result.IsRotatable ? $"{result.HorizontalAngleDegrees}°" : s.CalcFixed);
                AddInfoRow(table, $"{s.OkaFullName} ({s.CalcOka})", $"Nr. {result.OkaNumber}: {result.OkaName} @ {result.OkaDistance:F1}m");
                AddInfoRow(table, s.CalcModulation, result.Modulation);
                AddInfoRow(table, s.CalcBuildingDamping, $"{result.BuildingDampingDb:F2} dB");
            });

            // Band Results Table - detailed vertical format matching markdown
            if (result.BandResults.Count > 0)
            {
                column.Item().PaddingTop(8).Table(table =>
                {
                    // Column definitions: Parameter | Symbol | Unit | Band1 | Band2 | ...
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);    // Parameter name
                        columns.RelativeColumn(0.8f); // Symbol
                        columns.RelativeColumn(0.8f); // Unit
                        foreach (var _ in result.BandResults)
                        {
                            columns.RelativeColumn(1); // One column per band
                        }
                    });

                    // Header row with frequency values
                    table.Header(header =>
                    {
                        var headerStyle = TextStyle.Default.FontSize(8).Bold();
                        var headerBg = Colors.Blue.Lighten4;
                        header.Cell().Background(headerBg).Padding(3).Text("Parameter").Style(headerStyle);
                        header.Cell().Background(headerBg).Padding(3).Text("Sym").Style(headerStyle);
                        header.Cell().Background(headerBg).Padding(3).Text("Unit").Style(headerStyle);
                        foreach (var band in result.BandResults)
                        {
                            header.Cell().Background(headerBg).Padding(3).AlignRight()
                                .Text($"{band.FrequencyMHz:F0}").Style(headerStyle);
                        }
                    });

                    // Data rows - matching FSD Section 5 format (23 rows matching VB6)
                    foreach (var row in CalculationTableDefinition.Rows)
                    {
                        var label = row.GetLabel(s);
                        var symbol = row.GetSymbol(s);

                        if (row.IsConstantString)
                        {
                            AddDataRowConstant(table, label, symbol, row.Unit, result.BandResults, row.StringValueGetter!(result), row.IsBold);
                        }
                        else if (row.IsConstantNumeric)
                        {
                            AddDataRowConstant(table, label, symbol, row.Unit, result.BandResults, row.ConfigValueGetter!(result).ToString(row.Format), row.IsBold);
                        }
                        else
                        {
                            AddDataRow(table, label, symbol, row.Unit, result.BandResults, row.BandValueGetter!, row.Format, row.IsBold);
                        }
                    }
                });

                // Compliance status
                var statusBg = result.IsCompliant ? Colors.Green.Lighten4 : Colors.Red.Lighten4;
                var statusColor = result.IsCompliant ? Colors.Green.Darken2 : Colors.Red.Darken2;
                column.Item().PaddingTop(8).Background(statusBg).Padding(8)
                    .Text(result.IsCompliant ? s.CalcStatusCompliant : s.CalcStatusNonCompliant)
                    .FontSize(9).FontColor(statusColor).Bold();
            }
        });
    }

    private void AddDataRow(TableDescriptor table, string param, string symbol, string unit,
        IEnumerable<BandResult> bands, Func<BandResult, double> getValue, string format, bool highlight)
    {
        var style = TextStyle.Default.FontSize(8);
        var bgColor = highlight ? Colors.Yellow.Lighten4 : Colors.White;

        table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(2)
            .Text(param).Style(highlight ? style.Bold() : style);
        table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(2)
            .Text(symbol).Style(highlight ? style.Bold() : style);
        table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(2)
            .Text(unit).Style(style);

        foreach (var band in bands)
        {
            table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(2)
                .AlignRight().Text(getValue(band).ToString(format)).Style(highlight ? style.Bold() : style);
        }
    }

    private void AddDataRowConstant(TableDescriptor table, string param, string symbol, string unit,
        IEnumerable<BandResult> bands, string value, bool highlight)
    {
        var style = TextStyle.Default.FontSize(8);
        var bgColor = highlight ? Colors.Yellow.Lighten4 : Colors.White;

        table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(2)
            .Text(param).Style(highlight ? style.Bold() : style);
        table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(2)
            .Text(symbol).Style(highlight ? style.Bold() : style);
        table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(2)
            .Text(unit).Style(style);

        foreach (var _ in bands)
        {
            table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(2)
                .AlignRight().Text(value).Style(highlight ? style.Bold() : style);
        }
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

    private void ComposeExplanationsPage(IContainer container)
    {
        var s = Strings.Instance;
        container.PaddingVertical(5).Column(column =>
        {
            column.Spacing(4);

            // Title
            column.Item().Text(s.CalcColumnExplanations.Replace("## ", ""))
                .FontSize(14).Bold().FontColor(Colors.Blue.Medium);

            column.Item().PaddingTop(8).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);   // Parameter label
                    columns.RelativeColumn(2);   // Explanation
                });

                var rows = CalculationTableDefinition.Rows;
                var explanations = CalculationTableDefinition.Explanations;
                for (int i = 0; i < rows.Count && i < explanations.Count; i++)
                {
                    var label = rows[i].GetLabel(s);
                    var explanation = s.Get(explanations[i].ExplanationKey);

                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(3)
                        .Text(label).FontSize(9).Bold();
                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(3)
                        .Text(explanation).FontSize(9);
                }
            });

            // Disclaimer at bottom
            column.Item().PaddingTop(15).Text(s.CalcDisclaimer)
                .FontSize(7).FontColor(Colors.Grey.Medium).Italic();
        });
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
