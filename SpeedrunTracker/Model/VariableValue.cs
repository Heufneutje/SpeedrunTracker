using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model;

public class VariableValue
{
    [JsonPropertyName("label")]
    public string Name { get; set; }

    public string Rules { get; set; }
}
