using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model.SpeedrunDotCom;

public class GamePlatform : BaseSpeedrunObject
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
