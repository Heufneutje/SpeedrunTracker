using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record VariableValue : INamedSpeedrunModel
{
    [JsonPropertyName("label")]
    public required string Name { get; set; }

    public string? Rules { get; set; }
}
