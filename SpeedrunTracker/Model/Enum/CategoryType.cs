using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model.Enum;

public enum CategoryType
{
    [JsonPropertyName("per-game")]
    PerGame,

    [JsonPropertyName("per-level")]
    PerLevel
}
