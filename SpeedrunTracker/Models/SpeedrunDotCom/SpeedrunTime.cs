using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public class SpeedrunTime
{
    [JsonPropertyName("primary_t")]
    public double PrimarySeconds { get; set; }

    [JsonPropertyName("realtime_t")]
    public double RealtimeSeconds { get; set; }

    [JsonPropertyName("realtime_noloads_t")]
    public double RealtimeNoLoadsSeconds { get; set; }

    [JsonPropertyName("ingame_t")]
    public double IngameSeconds { get; set; }

    private string _primaryTimeSpan;

    [JsonIgnore]
    public string PrimaryTimeSpan
    {
        get
        {
            _primaryTimeSpan ??= TimeSpan.FromSeconds(PrimarySeconds).ToShortForm();
            return _primaryTimeSpan;
        }
    }

    private string _realtimeTimeSpan;

    [JsonIgnore]
    public string RealtimeTimeSpan
    {
        get
        {
            _realtimeTimeSpan ??= TimeSpan.FromSeconds(RealtimeSeconds).ToShortForm();
            return _realtimeTimeSpan;
        }
    }

    private string _realtimeNoLoadsTimeSpan;

    [JsonIgnore]
    public string RealtimeNoLoadsTimeSpan
    {
        get
        {
            _realtimeNoLoadsTimeSpan ??= TimeSpan.FromSeconds(RealtimeNoLoadsSeconds).ToShortForm();
            return _realtimeNoLoadsTimeSpan;
        }
    }

    private string _ingameTimeSpan;

    [JsonIgnore]
    public string IngameTimeSpan
    {
        get
        {
            _ingameTimeSpan ??= TimeSpan.FromSeconds(IngameSeconds).ToShortForm();
            return _ingameTimeSpan;
        }
    }
}
