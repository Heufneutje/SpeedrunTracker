using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model;

public class PlayerNameStyle
{
    [JsonPropertyName("color-from")]
    public PlayerNameStyleGradient ColorFrom { get; set; }

    [JsonPropertyName("color-to")]
    public PlayerNameStyleGradient ColorTo { get; set; }
}
