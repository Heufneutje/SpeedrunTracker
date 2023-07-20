using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model;

public class SpeedrunTime
{
    [JsonPropertyName("primary")]
    public string PrimaryTimeCode { get; set; }

    [JsonPropertyName("primary_t")]
    public double PrimarySeconds { get; set; }

    [JsonPropertyName("realtime")]
    public string RealtimeTimeCode { get; set; }

    [JsonPropertyName("realtime_t")]
    public double RealtimeSeconds { get; set; }

    [JsonPropertyName("realtime_noloads")]
    public string RealtimeNoLoadsTimeCode { get; set; }

    [JsonPropertyName("realtime_noloads_t")]
    public double RealtimeNoLoadsSeconds { get; set; }

    [JsonPropertyName("ingame")]
    public string IngameTimeCode { get; set; }

    [JsonPropertyName("ingame_t")]
    public double IngameSeconds { get; set; }

    [JsonIgnore]
    public TimeSpan PrimaryTimeSpan => TimeSpan.FromSeconds(PrimarySeconds);

    [JsonIgnore]
    public TimeSpan RealtimeTimeSpan => TimeSpan.FromSeconds(RealtimeSeconds);

    [JsonIgnore]
    public TimeSpan RealtimeNoLoadsTimeSpan => TimeSpan.FromSeconds(RealtimeNoLoadsSeconds);

    [JsonIgnore]
    public TimeSpan IngameTimeSpan => TimeSpan.FromSeconds(IngameSeconds);
}
