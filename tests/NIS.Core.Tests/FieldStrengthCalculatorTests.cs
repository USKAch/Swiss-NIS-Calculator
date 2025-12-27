using NIS.Core.Models;
using NIS.Core.Services;

namespace NIS.Core.Tests;

public class FieldStrengthCalculatorTests
{
    private readonly FieldStrengthCalculator _calculator = new();

    [Fact]
    public void Calculate_WithBasicInput_ReturnsValidResult()
    {
        // Arrange
        var input = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 100,
            DistanceMeters = 10,
            AntennaGainDbi = 6,
            TotalCableLossDb = 2
        };

        // Act
        var result = _calculator.Calculate(input);

        // Assert
        Assert.True(result.FieldStrengthVm > 0);
        Assert.True(result.PowerDensityWm2 > 0);
        Assert.Equal(28, result.NisLimitVm); // 144 MHz limit
    }

    [Fact]
    public void Calculate_FieldStrengthDecreasesWithDistance()
    {
        // Arrange
        var input1 = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 100,
            DistanceMeters = 10,
            AntennaGainDbi = 6
        };

        var input2 = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 100,
            DistanceMeters = 20,
            AntennaGainDbi = 6
        };

        // Act
        var result1 = _calculator.Calculate(input1);
        var result2 = _calculator.Calculate(input2);

        // Assert - doubling distance should halve field strength
        Assert.True(result2.FieldStrengthVm < result1.FieldStrengthVm);
        Assert.InRange(result2.FieldStrengthVm, result1.FieldStrengthVm * 0.4, result1.FieldStrengthVm * 0.6);
    }

    [Fact]
    public void Calculate_FieldStrengthIncreasesWithPower()
    {
        // Arrange
        var input1 = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 100,
            DistanceMeters = 10,
            AntennaGainDbi = 0
        };

        var input2 = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 400, // 4x power
            DistanceMeters = 10,
            AntennaGainDbi = 0
        };

        // Act
        var result1 = _calculator.Calculate(input1);
        var result2 = _calculator.Calculate(input2);

        // Assert - 4x power should give 2x field strength (sqrt relationship)
        Assert.True(result2.FieldStrengthVm > result1.FieldStrengthVm);
        Assert.InRange(result2.FieldStrengthVm, result1.FieldStrengthVm * 1.8, result1.FieldStrengthVm * 2.2);
    }

    [Fact]
    public void Calculate_CableLossReducesFieldStrength()
    {
        // Arrange
        var input1 = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 100,
            DistanceMeters = 10,
            AntennaGainDbi = 0,
            TotalCableLossDb = 0
        };

        var input2 = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 100,
            DistanceMeters = 10,
            AntennaGainDbi = 0,
            TotalCableLossDb = 3 // 3 dB loss = half power
        };

        // Act
        var result1 = _calculator.Calculate(input1);
        var result2 = _calculator.Calculate(input2);

        // Assert - 3dB loss should reduce power by half, field strength by sqrt(0.5)
        Assert.True(result2.FieldStrengthVm < result1.FieldStrengthVm);
        double expectedRatio = Math.Sqrt(Math.Pow(10, -3.0 / 10));
        Assert.InRange(result2.FieldStrengthVm / result1.FieldStrengthVm, expectedRatio * 0.95, expectedRatio * 1.05);
    }

    [Fact]
    public void Calculate_AntennaGainIncreasesFieldStrength()
    {
        // Arrange
        var input1 = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 100,
            DistanceMeters = 10,
            AntennaGainDbi = 0
        };

        var input2 = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 100,
            DistanceMeters = 10,
            AntennaGainDbi = 10 // +10 dBi
        };

        // Act
        var result1 = _calculator.Calculate(input1);
        var result2 = _calculator.Calculate(input2);

        // Assert - 10dB gain should increase field strength
        Assert.True(result2.FieldStrengthVm > result1.FieldStrengthVm);
        double expectedRatio = Math.Sqrt(Math.Pow(10, 10.0 / 10));
        Assert.InRange(result2.FieldStrengthVm / result1.FieldStrengthVm, expectedRatio * 0.95, expectedRatio * 1.05);
    }

    [Fact]
    public void Calculate_ZeroDistance_ThrowsException()
    {
        // Arrange
        var input = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 100,
            DistanceMeters = 0 // Invalid
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _calculator.Calculate(input));
    }

    [Fact]
    public void Calculate_BuildingDampingReducesFieldStrength()
    {
        // Arrange
        var input1 = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 100,
            DistanceMeters = 10,
            BuildingDampingDb = 0
        };

        var input2 = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 100,
            DistanceMeters = 10,
            BuildingDampingDb = 10 // 10 dB building damping
        };

        // Act
        var result1 = _calculator.Calculate(input1);
        var result2 = _calculator.Calculate(input2);

        // Assert
        Assert.True(result2.FieldStrengthVm < result1.FieldStrengthVm);
    }

    [Fact]
    public void FindMinimumSafeDistance_ReturnsPositiveDistance()
    {
        // Arrange
        var input = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 1000,
            AntennaGainDbi = 12
        };

        // Act
        var minDistance = _calculator.FindMinimumSafeDistance(input);

        // Assert
        Assert.True(minDistance > 0);
    }

    [Fact]
    public void Calculate_AtMinimumSafeDistance_EqualsNisLimit()
    {
        // Arrange
        var input = new CalculationInput
        {
            FrequencyMHz = 144.0,
            TxPowerWatts = 1000,
            AntennaGainDbi = 12
        };

        // Act
        var minDistance = _calculator.FindMinimumSafeDistance(input);
        input.DistanceMeters = minDistance;
        var result = _calculator.Calculate(input);

        // Assert - field strength at minimum distance should equal the limit
        Assert.InRange(result.FieldStrengthVm, result.NisLimitVm * 0.99, result.NisLimitVm * 1.01);
    }

    [Theory]
    [InlineData(1.8, 64.7)]    // 160m band
    [InlineData(3.5, 46.5)]    // 80m band
    [InlineData(7.0, 32.4)]    // 40m band
    [InlineData(14.0, 28)]     // 20m band
    [InlineData(144.0, 28)]    // 2m band
    [InlineData(432.0, 28.6)]  // 70cm band
    [InlineData(1296.0, 48.5)] // 23cm band
    [InlineData(2400.0, 61)]   // 13cm band
    public void Calculate_ReturnsCorrectNisLimit(double frequencyMHz, double expectedLimit)
    {
        // Arrange
        var input = new CalculationInput
        {
            FrequencyMHz = frequencyMHz,
            TxPowerWatts = 100,
            DistanceMeters = 10
        };

        // Act
        var result = _calculator.Calculate(input);

        // Assert
        Assert.Equal(expectedLimit, result.NisLimitVm);
    }

    /// <summary>
    /// Golden test based on FT-1000.ber example from VB6 application at 14 MHz.
    /// VB6 Output: 29.52 V/m at 3m distance
    /// </summary>
    [Fact]
    public void Calculate_GoldenTest_FT1000_14MHz_MatchesVB6Output()
    {
        // Arrange - values from Example/berechnungen/FT-1000.ber at 14 MHz
        var input = new CalculationInput
        {
            FrequencyMHz = 14.0,
            TxPowerWatts = 200,
            ActivityFactor = 0.5,
            ModulationFactor = 0.4, // CW
            DistanceMeters = 3.0,
            AntennaGainDbi = 6.29,
            AngleAttenuationDb = 0.50,
            TotalCableLossDb = 1.72,
            BuildingDampingDb = 0
        };

        // Act
        var result = _calculator.Calculate(input);

        // Assert - VB6 calculated 29.52 V/m, allow 1% tolerance
        Assert.InRange(result.FieldStrengthVm, 29.52 * 0.99, 29.52 * 1.01);
        Assert.Equal(28, result.NisLimitVm); // 14 MHz limit (10-400 MHz: 28 V/m)
    }

    /// <summary>
    /// Golden test based on FT-1000.ber example from VB6 application at 7 MHz.
    /// VB6 Output: 28.45 V/m at 3m distance
    /// </summary>
    [Fact]
    public void Calculate_GoldenTest_FT1000_7MHz_MatchesVB6Output()
    {
        // Arrange - values from Example/berechnungen/FT-1000.ber at 7 MHz
        var input = new CalculationInput
        {
            FrequencyMHz = 7.0,
            TxPowerWatts = 200,
            ActivityFactor = 0.5,
            ModulationFactor = 0.4, // CW
            DistanceMeters = 3.0,
            AntennaGainDbi = 5.61,
            AngleAttenuationDb = 0.39,
            TotalCableLossDb = 1.47,
            BuildingDampingDb = 0
        };

        // Act
        var result = _calculator.Calculate(input);

        // Assert - VB6 calculated 28.45 V/m, allow 1% tolerance
        Assert.InRange(result.FieldStrengthVm, 28.45 * 0.99, 28.45 * 1.01);
        Assert.Equal(32.4, result.NisLimitVm); // 7 MHz limit (40m band)
    }

    /// <summary>
    /// Golden test based on FT-1000.ber example from VB6 application at 28 MHz.
    /// VB6 Output: 33.70 V/m at 3m distance
    /// </summary>
    [Fact]
    public void Calculate_GoldenTest_FT1000_28MHz_MatchesVB6Output()
    {
        // Arrange - values from Example/berechnungen/FT-1000.ber at 28 MHz
        var input = new CalculationInput
        {
            FrequencyMHz = 28.0,
            TxPowerWatts = 200,
            ActivityFactor = 0.5,
            ModulationFactor = 0.4, // CW
            DistanceMeters = 3.0,
            AntennaGainDbi = 8.04,
            AngleAttenuationDb = 0.75,
            TotalCableLossDb = 2.07,
            BuildingDampingDb = 0
        };

        // Act
        var result = _calculator.Calculate(input);

        // Assert - VB6 calculated 33.70 V/m, allow 1% tolerance
        Assert.InRange(result.FieldStrengthVm, 33.70 * 0.99, 33.70 * 1.01);
        Assert.Equal(28, result.NisLimitVm); // 28 MHz limit (10-400 MHz: 28 V/m)
    }
}
