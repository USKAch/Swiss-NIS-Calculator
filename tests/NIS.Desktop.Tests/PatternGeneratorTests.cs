using System.Linq;
using NIS.Desktop.Calculations;
using NIS.Desktop.Models;
using Xunit;

namespace NIS.Desktop.Tests;

public class PatternGeneratorTests
{
    [Theory]
    [InlineData(6.0)]
    [InlineData(10.0)]
    [InlineData(15.0)]
    public void DirectionalPattern_StartsAtZeroAndRisesMonotonically(double gainDbi)
    {
        var pattern = PatternGenerator.GenerateDirectionalPattern(gainDbi);

        Assert.Equal(10, pattern.Length);
        Assert.Equal(0, pattern[0]);
        for (int i = 1; i < pattern.Length; i++)
        {
            Assert.True(pattern[i] >= pattern[i - 1], $"Attenuation must not decrease at {i * 10}°");
        }
    }

    [Theory]
    [InlineData(6.0, 20.0)]   // A_zenith floor: max(20, 20 + (6-6)) = 20
    [InlineData(21.0, 35.0)]  // A_zenith cap:   min(35, 20 + (21-6)) = 35
    [InlineData(12.0, 26.0)]  // In between:     20 + (12-6) = 26
    public void DirectionalPattern_ZenithAttenuationFollowsFsdFormula(double gainDbi, double expectedZenith)
    {
        var pattern = PatternGenerator.GenerateDirectionalPattern(gainDbi);
        Assert.Equal(expectedZenith, pattern[9], 1);
    }

    [Fact]
    public void DirectionalPattern_HigherGainIsNarrower()
    {
        var wide = PatternGenerator.GenerateDirectionalPattern(6);
        var narrow = PatternGenerator.GenerateDirectionalPattern(15);
        // At 30° the higher-gain antenna must already attenuate more
        Assert.True(narrow[3] > wide[3]);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(2.15)]
    [InlineData(6.0)]
    public void VerticalPattern_IsWithinValidRangeAndMonotonic(double gainDbi)
    {
        var pattern = PatternGenerator.GenerateVerticalPattern(gainDbi);

        Assert.Equal(10, pattern.Length);
        Assert.All(pattern, v => Assert.InRange(v, 0, 60));
        for (int i = 1; i < pattern.Length; i++)
        {
            Assert.True(pattern[i] >= pattern[i - 1]);
        }
    }

    [Theory]
    [InlineData(AntennaTypes.Yagi, true)]
    [InlineData(AntennaTypes.Quad, true)]
    [InlineData(AntennaTypes.LogPeriodic, true)]
    [InlineData(AntennaTypes.Vertical, false)]
    [InlineData(AntennaTypes.Wire, false)]
    [InlineData(AntennaTypes.Loop, false)]
    [InlineData(AntennaTypes.Other, false)]
    public void IsDirectional_MatchesAntennaType(string type, bool expected)
    {
        Assert.Equal(expected, PatternGenerator.IsDirectional(type));
    }

    [Fact]
    public void GeneratePattern_DispatchesByAntennaType()
    {
        const double gain = 10;
        Assert.Equal(PatternGenerator.GenerateDirectionalPattern(gain), PatternGenerator.GeneratePattern(AntennaTypes.Yagi, gain));
        Assert.Equal(PatternGenerator.GenerateVerticalPattern(gain), PatternGenerator.GeneratePattern(AntennaTypes.Vertical, gain));
        // Unknown / wire types fall back to the (safer) omnidirectional model
        Assert.Equal(PatternGenerator.GenerateVerticalPattern(gain), PatternGenerator.GeneratePattern(AntennaTypes.Wire, gain));
        Assert.Equal(PatternGenerator.GenerateVerticalPattern(gain), PatternGenerator.GeneratePattern("SomethingElse", gain));
    }

    [Fact]
    public void GeneratedPatterns_PassAntennaEditorValidation()
    {
        // The antenna editor rejects pattern values outside 0..60 dB
        foreach (var type in AntennaTypes.All)
        {
            foreach (var gain in new[] { 0.0, 2.15, 6.0, 12.0, 20.0, 30.0 })
            {
                var pattern = PatternGenerator.GeneratePattern(type, gain);
                Assert.All(pattern, v => Assert.InRange(v, 0, 60));
            }
        }
    }
}
