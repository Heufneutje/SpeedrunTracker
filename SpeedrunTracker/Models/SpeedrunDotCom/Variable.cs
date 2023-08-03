using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public class Variable : BaseSpeedrunObject
{
    public string Name { get; set; }
    public string Category { get; set; }
    public VariableValueContainer Values { get; set; }
    public VariableScope Scope { get; set; }

    [JsonPropertyName("is-subcategory")]
    public bool IsSubcategory { get; set; }
}
