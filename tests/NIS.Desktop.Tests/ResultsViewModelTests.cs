using NIS.Desktop.Localization;
using NIS.Desktop.ViewModels;
using Xunit;

namespace NIS.Desktop.Tests;

/// <summary>
/// Compliance summary / status logic of the results view (regression for the
/// "0 Konfigurationen analysiert / NICHT KONFORM although all PASS" bug).
/// </summary>
public class ResultsViewModelTests
{
    private static ConfigurationResult MakeResult(string name, params (double field, double limit)[] bands)
    {
        var result = new ConfigurationResult
        {
            ConfigurationName = name,
            AntennaName = "Test antenna",
            RadioName = "Test radio",
            PowerWatts = 100,
            RadioPowerWatts = 100,
            LinearPowerWatts = 0,
            Modulation = "SSB",
            OkaDistance = 10,
            AntennaHeight = 10,
            OkaName = "Balkon",
            OkaNumber = "1",
            BuildingDampingDb = 0,
            CableDescription = "10m RG-213",
            LinearName = "",
            IsRotatable = false,
            HorizontalAngleDegrees = 360,
            IsHorizontallyPolarized = true
        };
        foreach (var (field, limit) in bands)
        {
            result.BandResults.Add(new BandResult { FrequencyMHz = 14, FieldStrength = field, Limit = limit });
        }
        return result;
    }

    [Fact]
    public void ConfigurationResult_IsCompliant_OnlyWhenAllBandsWithinLimit()
    {
        Assert.True(MakeResult("A", (20, 28), (27.9, 28)).IsCompliant);
        Assert.False(MakeResult("B", (20, 28), (28.1, 28)).IsCompliant);
        Assert.False(MakeResult("C").IsCompliant); // no bands = nothing proven
    }

    [Fact]
    public void ComplianceSummary_EmptyBeforeCalculation()
    {
        var vm = new ResultsViewModel();
        Assert.False(vm.HasResults);
        Assert.False(vm.AllCompliant);
        Assert.Equal("", vm.ComplianceSummary);
    }

    [Fact]
    public void ComplianceSummary_AllCompliant_WhenEveryResultPasses()
    {
        var vm = new ResultsViewModel();
        vm.Results.Add(MakeResult("G5RV", (20, 28)));
        vm.Results.Add(MakeResult("Yagi", (25, 28)));

        Assert.True(vm.AllCompliant);
        Assert.Equal(Strings.Instance.CalcAllCompliant, vm.ComplianceSummary);
    }

    [Fact]
    public void ComplianceSummary_NamesFailingConfigurations()
    {
        var vm = new ResultsViewModel();
        vm.Results.Add(MakeResult("G5RV", (20, 28)));
        vm.Results.Add(MakeResult("Yagi 20m", (30, 28)));
        vm.Results.Add(MakeResult("Vertical", (40, 28)));

        Assert.False(vm.AllCompliant);
        Assert.StartsWith(Strings.Instance.CalcNonCompliantDetected, vm.ComplianceSummary);
        Assert.Contains("Yagi 20m", vm.ComplianceSummary);
        Assert.Contains("Vertical", vm.ComplianceSummary);
        Assert.DoesNotContain("G5RV", vm.ComplianceSummary);
    }

    [Fact]
    public void RealDistance_IsHypotenuseOfHorizontalDistanceAndHeight()
    {
        var r = MakeResult("X");
        Assert.Equal(14.14, r.RealDistance, 2); // sqrt(10² + 10²)
    }
}
