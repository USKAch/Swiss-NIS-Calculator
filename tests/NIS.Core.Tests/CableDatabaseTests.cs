using System.Globalization;
using NIS.Core.Data;
using NIS.Core.Models;

namespace NIS.Core.Tests;

public class CableDatabaseTests
{
    [Fact]
    public void LoadDefaults_LoadsCables()
    {
        // Arrange
        var database = new CableDatabase();

        // Act
        database.LoadDefaults();

        // Assert
        Assert.NotEmpty(database.Cables);
        Assert.Contains(database.Cables, c => c.Name == "RG-213");
        Assert.Contains(database.Cables, c => c.Name == "Aircom-plus");
    }

    [Fact]
    public void GetByName_ReturnsCorrectCable()
    {
        // Arrange
        var database = new CableDatabase();
        database.LoadDefaults();

        // Act
        var cable = database.GetByName("RG-213");

        // Assert
        Assert.NotNull(cable);
        Assert.Equal("RG-213", cable.Name);
    }

    [Fact]
    public void GetByName_CaseInsensitive()
    {
        // Arrange
        var database = new CableDatabase();
        database.LoadDefaults();

        // Act
        var cable = database.GetByName("rg-213");

        // Assert
        Assert.NotNull(cable);
    }

    [Fact]
    public void Cable_GetAttenuationAt_ReturnsValue()
    {
        // Arrange
        var database = new CableDatabase();
        database.LoadDefaults();
        var cable = database.GetByName("RG-213");

        // Act
        var attenuation = cable!.GetAttenuationAt(144.0);

        // Assert
        Assert.True(attenuation > 0);
    }

    [Fact]
    public void Cable_CalculateLoss_ScalesWithLength()
    {
        // Arrange
        var database = new CableDatabase();
        database.LoadDefaults();
        var cable = database.GetByName("RG-213");

        // Act
        var loss50m = cable!.CalculateLoss(50, 144.0);
        var loss100m = cable.CalculateLoss(100, 144.0);

        // Assert - 100m should have exactly 2x the loss of 50m
        Assert.Equal(loss50m * 2, loss100m, 0.001);
    }

    [Fact]
    public void Cable_InterpolatesFrequency()
    {
        // Arrange
        var database = new CableDatabase();
        database.LoadDefaults();
        var cable = database.GetByName("RG-213");

        // Act
        var att7 = cable!.GetAttenuationAt(7.0);
        var att10 = cable.GetAttenuationAt(10.0);
        var att8 = cable.GetAttenuationAt(8.0); // Should be interpolated

        // Assert - 8 MHz should be between 7 and 10 MHz values
        Assert.True(att8 > att7);
        Assert.True(att8 < att10);
    }

    [Fact]
    public void Cable_GetAttenuationAt_WorksWithGermanLocale()
    {
        // Arrange - simulate German locale where decimal separator is comma
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");

            var cable = new Cable
            {
                Name = "TestCable",
                AttenuationPer100m = new Dictionary<string, double>
                {
                    { "14.5", 1.5 },  // Key uses period (JSON format)
                    { "28", 2.0 }
                }
            };

            // Act - should parse "14.5" correctly even in German locale
            var att = cable.GetAttenuationAt(14.5);

            // Assert
            Assert.Equal(1.5, att);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public void Cable_GetAttenuationAt_WorksWithFrenchSwissLocale()
    {
        // Arrange - simulate French Swiss locale
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("fr-CH");

            var cable = new Cable
            {
                Name = "TestCable",
                AttenuationPer100m = new Dictionary<string, double>
                {
                    { "7.0", 1.0 },
                    { "14.0", 1.5 }
                }
            };

            // Act - should interpolate correctly
            var att = cable.GetAttenuationAt(10.5);

            // Assert - should be between 1.0 and 1.5
            Assert.True(att > 1.0);
            Assert.True(att < 1.5);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }
}
