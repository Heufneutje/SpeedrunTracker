using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model;

public class GameAssets
{
    public Asset Logo { get; set; }

    [JsonPropertyName("cover-tiny")]
    public Asset CoverTiny { get; set; }

    [JsonPropertyName("cover-small")]
    public Asset CoverSmall { get; set; }
}
