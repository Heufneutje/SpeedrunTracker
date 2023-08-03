using System.Runtime.Serialization;

namespace SpeedrunTracker.Models.Enum;

public enum CategoryType
{
    [EnumMember(Value = "per-game")]
    PerGame,

    [EnumMember(Value = "per-level")]
    PerLevel
}
