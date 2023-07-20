using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model.Enum;

public enum VariableScopeType
{
    Global,

    [JsonPropertyName("full-game")]
    FullGame,

    [JsonPropertyName("all-levels")]
    AllLevels
}
