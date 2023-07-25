using System.Runtime.Serialization;

namespace SpeedrunTracker.Model.Enum;

public enum TimingType
{
    [EnumMember(Value = "realtime")]
    Realtime,

    [EnumMember(Value = "realtime_noloads")]
    RealtimeNoLoads,

    [EnumMember(Value = "ingame")]
    InGame
}
