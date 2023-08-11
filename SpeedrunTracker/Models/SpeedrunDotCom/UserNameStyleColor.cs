using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record UserNameStyleColor
{
    // We're ignoring the API value since speedrun.com doesn't allow you to set it (anymore) and a lot of colors that are returned are very hard to read when using the light theme.
    [JsonIgnore]
    public string Light { get; set; }

    public string Dark { get; set; }

    public UserNameStyleColor()
    {
        Light = "#000000";
    }
}
