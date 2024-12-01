using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Level : BaseSpeedrunObject
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    public override string ToString() => Name;
}
