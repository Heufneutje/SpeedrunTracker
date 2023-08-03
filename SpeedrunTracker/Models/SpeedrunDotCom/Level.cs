using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public class Level : BaseSpeedrunObject
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    public override string ToString() => Name;
}
