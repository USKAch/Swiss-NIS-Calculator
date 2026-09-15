using System;
using System.Collections.Generic;
using NIS.Desktop.Calculations;
using NIS.Desktop.Models;
using NIS.Desktop.Services;
using NIS.Desktop.Services.Repositories;
using Xunit;

namespace NIS.Desktop.Tests;

public class BandAndTimestampTests
{
    [Fact]
    public void FourMeterBand_IsAvailableWithCorrectLimit()
    {
        Assert.Contains(70.0, SwissNisLimits.StandardFrequencies);
        Assert.Equal(28, SwissNisLimits.StandardFrequencyLimits[70.0]);
        Assert.Equal(28, SwissNisLimits.GetLimitVm(70.2));
    }

    [Fact]
    public void DefaultMasterData_ContainsFourMeterBand()
    {
        var bands = MasterDataStore.CreateDefaultMasterData().Bands;
        Assert.Contains(bands, b => b.Name == "4m" && Math.Abs(b.FrequencyMHz - 70.0) < 0.01);
    }

    [Theory]
    [InlineData(3.5, "80m")]
    [InlineData(10.0, "30m")]   // previously rendered as "10MHz"
    [InlineData(10.1, "30m")]
    [InlineData(24.0, "12m")]   // previously rendered as "24MHz"
    [InlineData(24.9, "12m")]
    [InlineData(50.0, "6m")]
    [InlineData(70.0, "4m")]
    [InlineData(144.0, "2m")]
    [InlineData(145.5, "2m")]
    [InlineData(432.0, "70cm")]
    public void GetBandName_ResolvesNominalAndNearbyFrequencies(double mhz, string expected)
    {
        var bands = MasterDataStore.CreateDefaultMasterData().Bands;
        Assert.Equal(expected, MasterDataStore.GetBandName(bands, mhz));
    }

    [Fact]
    public void GetBandName_FallsBackToMhzForUnknownFrequency()
    {
        var bands = MasterDataStore.CreateDefaultMasterData().Bands;
        Assert.Equal("5.7 MHz", MasterDataStore.GetBandName(bands, 5.7));
    }

    [Fact]
    public void ParseUtcTimestamp_ReturnsUtcKind()
    {
        var dt = ProjectRepository.ParseUtcTimestamp("2026-09-15T08:30:00.0000000Z");
        Assert.Equal(DateTimeKind.Utc, dt.Kind);
        Assert.Equal(new DateTime(2026, 9, 15, 8, 30, 0, DateTimeKind.Utc), dt);
    }

    [Fact]
    public void FormatLocalTimestamp_ConvertsToLocalTime()
    {
        var iso = "2026-09-15T08:30:00.0000000Z";
        var expected = new DateTime(2026, 9, 15, 8, 30, 0, DateTimeKind.Utc).ToLocalTime()
            .ToString("dd.MM.yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
        Assert.Equal(expected, ProjectRepository.FormatLocalTimestamp(iso));
    }

    [Fact]
    public void FormatLocalTimestamp_EmptyOrGarbage_DoesNotThrow()
    {
        Assert.Equal("", ProjectRepository.FormatLocalTimestamp(""));
        Assert.Equal("not a date", ProjectRepository.FormatLocalTimestamp("not a date"));
    }
}
