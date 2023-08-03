using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public class UserNameStyleGradient
{
    public string Light { get; set; }
    public string Dark { get; set; }

    [JsonIgnore]
    public Color LightColor => Color.FromArgb(Light);

    [JsonIgnore]
    public Color DarkColor => Color.FromArgb(Dark);
}
