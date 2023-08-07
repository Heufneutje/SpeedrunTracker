using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public class UserNameStyleColor
{
    [JsonIgnore]
    public string Light { get; set; }

    public string Dark { get; set; }

    public UserNameStyleColor()
    {
        Light = "#000000";
    }
}
