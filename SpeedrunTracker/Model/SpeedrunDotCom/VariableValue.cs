using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model.SpeedrunDotCom;

public class VariableValue
{
    [JsonPropertyName("label")]
    public string Name { get; set; }

    public string Rules { get; set; }
}
