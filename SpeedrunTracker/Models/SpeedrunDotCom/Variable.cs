using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Variable : BaseSpeedrunObject
{
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required VariableValueContainer Values { get; set; }
    public required VariableScope Scope { get; set; }

    [JsonPropertyName("is-subcategory")]
    public bool IsSubcategory { get; set; }
}
