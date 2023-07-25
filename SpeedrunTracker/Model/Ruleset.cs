using SpeedrunTracker.Model.Enum;
using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model;

public class Ruleset
{
    [JsonPropertyName("default-time")]
    public TimingType DefaultTimingType { get; set; }

    [JsonPropertyName("run-times")]
    public List<TimingType> TimingTypes { get; set; }
}
