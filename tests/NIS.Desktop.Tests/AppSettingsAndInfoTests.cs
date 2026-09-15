using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NIS.Desktop.Calculations;
using NIS.Desktop.Models;
using NIS.Desktop.Services;
using Xunit;

namespace NIS.Desktop.Tests;

public class AppSettingsAndInfoTests
{
    [Theory]
    [InlineData("de", "de")]
    [InlineData("fr", "fr")]
    [InlineData("it", "it")]
    [InlineData("en", "en")]
    [InlineData("", "de")]
    [InlineData(null, "de")]
    public void ResolveLanguage_ExplicitCodesPassThrough(string? stored, string expected)
    {
        Assert.Equal(expected, AppSettings.ResolveLanguage(stored));
    }

    [Theory]
    [InlineData("de-CH", "de")]
    [InlineData("fr-CH", "fr")]
    [InlineData("it-CH", "it")]
    [InlineData("en-GB", "en")]
    [InlineData("rm-CH", "de")] // Romansh: unsupported → German fallback
    [InlineData("ja-JP", "de")]
    public void ResolveLanguage_SystemFollowsUiCulture(string culture, string expected)
    {
        var original = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
            Assert.Equal(expected, AppSettings.ResolveLanguage(AppSettings.SystemLanguage));
            Assert.Equal(expected, AppSettings.ResolveLanguage("SYSTEM"));
        }
        finally
        {
            CultureInfo.CurrentUICulture = original;
        }
    }

    [Theory]
    [InlineData(new string[0], false)]
    [InlineData(new[] { "--factory" }, true)]
    [InlineData(new[] { "/factory" }, true)]
    [InlineData(new[] { "--FACTORY" }, true)]
    [InlineData(new[] { "project.nisproj", "--factory" }, true)]
    [InlineData(new[] { "--factor" }, false)]
    [InlineData(new[] { "factory" }, false)]
    public void ParseCommandLine_DetectsFactorySwitch(string[] args, bool expected)
    {
        AppInfo.ParseCommandLine(args);
        Assert.Equal(expected, AppInfo.FactoryModeEnabled);
        AppInfo.ParseCommandLine(Array.Empty<string>());
    }

    [Fact]
    public void Version_IsSemanticVersionWithoutBuildMetadata()
    {
        Assert.Matches(@"^\d+\.\d+\.\d+", AppInfo.Version);
        Assert.DoesNotContain("+", AppInfo.Version);
    }

    [Fact]
    public void MergeMissingDefaultBands_AddsNewDefaultsKeepsCustomAndSorts()
    {
        // Simulates an older masterdata.json: no 4m band, plus a user-defined band
        var data = new MasterDataFile
        {
            Bands = new List<BandDefinition>
            {
                new() { Name = "80m", FrequencyMHz = 3.5 },
                new() { Name = "Custom", FrequencyMHz = 27.0 },
                new() { Name = "2m", FrequencyMHz = 144.0 },
            }
        };

        var changed = MasterDataStore.MergeMissingDefaultBands(data);

        Assert.True(changed);
        Assert.Contains(data.Bands, b => b.Name == "4m" && Math.Abs(b.FrequencyMHz - 70.0) < 0.01);
        Assert.Contains(data.Bands, b => b.Name == "Custom");
        Assert.Equal(data.Bands.OrderBy(b => b.FrequencyMHz).Select(b => b.FrequencyMHz), data.Bands.Select(b => b.FrequencyMHz));
        var expectedCount = MasterDataStore.CreateDefaultMasterData().Bands.Count + 1;
        Assert.Equal(expectedCount, data.Bands.Count);
    }

    [Fact]
    public void MergeMissingDefaultBands_NoChangeWhenComplete()
    {
        var data = MasterDataStore.CreateDefaultMasterData();
        var before = data.Bands.Select(b => b.FrequencyMHz).ToList();

        Assert.False(MasterDataStore.MergeMissingDefaultBands(data));
        Assert.Equal(before, data.Bands.Select(b => b.FrequencyMHz));
    }

    [Fact]
    public void DefaultBands_MatchStandardFrequencyTable_UpTo70cm()
    {
        // Every default band must have a limit; every limit up to 70cm must have a band name.
        var bands = MasterDataStore.CreateDefaultMasterData().Bands;
        foreach (var band in bands)
        {
            Assert.True(SwissNisLimits.StandardFrequencyLimits.ContainsKey(band.FrequencyMHz),
                $"No NISV limit for default band {band.Name} ({band.FrequencyMHz} MHz)");
        }
        foreach (var f in SwissNisLimits.StandardFrequencies.Where(f => f <= 430))
        {
            Assert.Contains(bands, b => Math.Abs(b.FrequencyMHz - f) < 0.01);
        }
    }

    [Theory]
    [InlineData(1.8, 64.7)]
    [InlineData(3.5, 46.5)]
    [InlineData(7.0, 32.9)]
    [InlineData(10.0, 28)]
    [InlineData(28.0, 28)]
    [InlineData(50.0, 28)]
    [InlineData(70.0, 28)]
    [InlineData(144.0, 28)]
    [InlineData(430.0, 28.6)]
    [InlineData(1240.0, 48.5)]
    [InlineData(1296.0, 48.5)]
    [InlineData(2400.0, 61)]
    [InlineData(10000.0, 61)]
    public void GetLimitVm_MatchesStandardTable(double mhz, double expected)
    {
        Assert.Equal(expected, SwissNisLimits.GetLimitVm(mhz), 1);
    }
}
