using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model.SpeedrunDotCom;

public class UserNameStyle
{
    [JsonPropertyName("color-from")]
    public UserNameStyleGradient ColorFrom { get; set; }

    [JsonPropertyName("color-to")]
    public UserNameStyleGradient ColorTo { get; set; }
}
