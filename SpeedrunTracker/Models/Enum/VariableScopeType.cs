using System.Runtime.Serialization;

namespace SpeedrunTracker.Models.Enum;

public enum VariableScopeType
{
    Global,

    [EnumMember(Value = "full-game")]
    FullGame,

    [EnumMember(Value = "all-levels")]
    AllLevels,

    [EnumMember(Value = "single-level")]
    SingleLevel
}
