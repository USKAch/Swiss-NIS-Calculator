using NIS.Core.Models;

namespace NIS.Core.Tests;

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
