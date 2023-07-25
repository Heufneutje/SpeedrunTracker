using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model;

public class Speedrun : BaseSpeedrunObject
{
    public string Weblink { get; set; }
    public string Comment { get; set; }
    public SpeedrunVideos Videos { get; set; }
    public SpeedrunStatus Status { get; set; }
    public List<User> Players { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? Submitted { get; set; }
    public SpeedrunTime Times { get; set; }
    public GameSystem System { get; set; }
    public Dictionary<string, string> Values { get; set; }

    [JsonPropertyName("category")]
    public string CategoryId { get; set; }

    [JsonPropertyName("level")]
    public string LevelId { get; set; }
}
