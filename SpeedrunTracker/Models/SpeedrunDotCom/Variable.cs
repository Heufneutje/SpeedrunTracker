using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Variable : BaseNamedSpeedrunModel
{
    public required string Category { get; set; }
    public required VariableValueContainer Values { get; set; }
    public required VariableScope Scope { get; set; }

    [JsonPropertyName("is-subcategory")]
    public bool IsSubcategory { get; set; }
}
