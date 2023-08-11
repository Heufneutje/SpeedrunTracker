using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record VariableValue
{
    [JsonPropertyName("label")]
    public string Name { get; set; }

    public string Rules { get; set; }
}
