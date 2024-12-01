using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record GamePlatform : BaseSpeedrunModel, INamedSpeedrunModel
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}
