using NIS.Desktop.Models;

namespace NIS.Desktop.Tests;

public class AntennaTests
{
    [Fact]
    public void AntennaBand_GetAttenuationAtAngle_ExactMatch_ReturnsPatternValue()
    {
        // Arrange
        var band = new AntennaBand
        {
            FrequencyMHz = 14,
            GainDbi = 6.0,
            Pattern = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] // 0°-90° in 10° steps
        };

        // Act & Assert - exact matches at 10° intervals
        Assert.Equal(0, band.GetAttenuationAtAngle(0));
        Assert.Equal(3, band.GetAttenuationAtAngle(30));
        Assert.Equal(9, band.GetAttenuationAtAngle(90));
    }

    [Fact]
    public void AntennaBand_GetAttenuationAtAngle_Interpolates_BetweenPatternPoints()
    {
        // Arrange
        var band = new AntennaBand
        {
            FrequencyMHz = 14,
            GainDbi = 6.0,
            Pattern = [0, 2, 4, 6, 8, 10, 12, 14, 16, 18] // Linear increase
        };

        // Act - 25° should interpolate between index 2 (20°=4) and index 3 (30°=6)
        var att25 = band.GetAttenuationAtAngle(25);

        // Assert - should be exactly 5 (halfway between 4 and 6)
        Assert.Equal(5, att25);
    }

    [Fact]
    public void AntennaBand_GetAttenuationAtAngle_Interpolates_AtMidpoints()
    {
        // Arrange
        var band = new AntennaBand
        {
            FrequencyMHz = 14,
            GainDbi = 6.0,
            Pattern = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
        };

        // Act & Assert - midpoints should interpolate
        Assert.Equal(0.5, band.GetAttenuationAtAngle(5));   // Between 0 and 1
        Assert.Equal(2.5, band.GetAttenuationAtAngle(25));  // Between 2 and 3
        Assert.Equal(6.5, band.GetAttenuationAtAngle(65));  // Between 6 and 7
    }

    [Fact]
    public void AntennaBand_GetAttenuationAtAngle_HandlesNegativeAngles()
    {
        // Arrange
        var band = new AntennaBand
        {
            FrequencyMHz = 14,
            GainDbi = 6.0,
            Pattern = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
        };

        // Act & Assert - negative angles should use absolute value
        Assert.Equal(3, band.GetAttenuationAtAngle(-30));
    }

    [Fact]
    public void AntennaBand_GetAttenuationAtAngle_HandlesAnglesOver90()
    {
        // Arrange
        var band = new AntennaBand
        {
            FrequencyMHz = 14,
            GainDbi = 6.0,
            Pattern = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
        };

        // Act & Assert - angles > 90 should mirror (180 - angle)
        Assert.Equal(3, band.GetAttenuationAtAngle(150)); // 180-150 = 30°
    }

    [Fact]
    public void AntennaBand_GetAttenuationAtAngle_EmptyPattern_ReturnsZero()
    {
        // Arrange
        var band = new AntennaBand
        {
            FrequencyMHz = 14,
            GainDbi = 6.0,
            Pattern = []
        };

        // Act & Assert
        Assert.Equal(0, band.GetAttenuationAtAngle(45));
    }
}

public class AntennaConfigurationCloneTests
{
    [Fact]
    public void Clone_IsDeepCopyWithAllFields()
    {
        var original = new AntennaConfiguration
        {
            Name = "HF Station",
            RadioId = 3,
            Radio = new RadioConfig { Manufacturer = "Icom", Model = "IC-7300" },
            Linear = new LinearConfig { Name = "Expert 1.3K", PowerWatts = 1000 },
            PowerWatts = 100,
            CableId = 5,
            Cable = new CableConfig { Type = "EcoFlex10", LengthMeters = 12.5, AdditionalLossDb = 0.3, AdditionalLossDescription = "Connectors" },
            AntennaId = 7,
            Antenna = new AntennaPlacement { Manufacturer = "Fritzel", Model = "FB-33", HeightMeters = 11, IsRotatable = true, HorizontalAngleDegrees = 90 },
            ModulationId = 2,
            Modulation = "SSB",
            ActivityFactor = 0.4,
            OkaId = 9,
            OkaName = "Balkon"
        };

        var clone = original.Clone();

        Assert.NotSame(original, clone);
        Assert.NotSame(original.Radio, clone.Radio);
        Assert.NotSame(original.Cable, clone.Cable);
        Assert.NotSame(original.Antenna, clone.Antenna);
        Assert.NotSame(original.Linear, clone.Linear);

        Assert.Equal(original.Name, clone.Name);
        Assert.Equal(original.RadioId, clone.RadioId);
        Assert.Equal(original.Linear!.PowerWatts, clone.Linear!.PowerWatts);
        Assert.Equal(original.PowerWatts, clone.PowerWatts);
        Assert.Equal(original.Cable.LengthMeters, clone.Cable.LengthMeters);
        Assert.Equal(original.Antenna.HorizontalAngleDegrees, clone.Antenna.HorizontalAngleDegrees);
        Assert.Equal(original.ActivityFactor, clone.ActivityFactor);
        Assert.Equal(original.OkaId, clone.OkaId);

        // Changing the clone must not affect the original
        clone.PowerWatts = 500;
        clone.Cable.LengthMeters = 1;
        Assert.Equal(100, original.PowerWatts);
        Assert.Equal(12.5, original.Cable.LengthMeters);
    }

    [Fact]
    public void Clone_WithoutLinear_KeepsLinearNull()
    {
        var clone = new AntennaConfiguration { Name = "X", Linear = null }.Clone();
        Assert.Null(clone.Linear);
    }
}
