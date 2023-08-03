using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public class UserNameStyle
{
    [JsonPropertyName("color-from")]
    public UserNameStyleGradient ColorFrom { get; set; }

    [JsonPropertyName("color-to")]
    public UserNameStyleGradient ColorTo { get; set; }
}
