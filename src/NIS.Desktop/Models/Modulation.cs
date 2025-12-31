using System.Text.Json.Serialization;

namespace NIS.Desktop.Models;

public class Modulation
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("factor")]
    public double Factor { get; set; }

    [JsonPropertyName("isUserData")]
    public bool IsUserData { get; set; }

    public override string ToString() => $"{Name} ({Factor:0.0})";
}
