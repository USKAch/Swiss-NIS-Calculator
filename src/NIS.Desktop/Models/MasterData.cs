using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIS.Desktop.Models;

public class BandDefinition
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("frequencyMHz")]
    public double FrequencyMHz { get; set; }
}

public class MasterConstants
{
    [JsonPropertyName("groundReflectionFactor")]
    public double GroundReflectionFactor { get; set; } = 1.6;

    [JsonPropertyName("defaultActivityFactor")]
    public double DefaultActivityFactor { get; set; } = 0.5;
}

public class MasterDataFile
{
    [JsonPropertyName("bands")]
    public List<BandDefinition> Bands { get; set; } = new();

    [JsonPropertyName("constants")]
    public MasterConstants Constants { get; set; } = new();
}
