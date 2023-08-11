using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record GamePlatform : BaseSpeedrunObject
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
