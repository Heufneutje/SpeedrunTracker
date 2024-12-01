using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Ruleset
{
    [JsonPropertyName("default-time")]
    public TimingType DefaultTimingType { get; set; }

    [JsonPropertyName("run-times")]
    public List<TimingType> TimingTypes { get; set; }

    public Ruleset()
    {
        TimingTypes = [];
    }
}
