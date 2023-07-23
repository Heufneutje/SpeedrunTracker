using System.Runtime.Serialization;

namespace SpeedrunTracker.Model.Enum;

public enum CategoryType
{
    [EnumMember(Value = "per-game")]
    PerGame,

    [EnumMember(Value = "per-level")]
    PerLevel
}
