using SpeedrunTracker.Model.Enum;
using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model;

public class Player : BaseSpeedrunObject
{
    [JsonPropertyName("rel")]
    public PlayerType PlayerType { get; set; }
    public string Name { get; set; }
    public Names Names { get; set; }
    public string Weblink { get; set; }

    [JsonPropertyName("name-style")]
    public PlayerNameStyle NameStyle { get; set; }
    public DateTime? Signup { get; set; }
    public PlayerLocation Location { get; set; }
    public Link Twitch { get; set; }
    public Link Hitbox { get; set; }
    public Link YouTube { get; set; }
    public Link Twitter { get; set; }
    public Link SpeedrunsLive { get; set; }

    [JsonIgnore]
    public string DisplayName => PlayerType == PlayerType.Guest ? Name : string.IsNullOrEmpty(Names.International) ? Names.Japanese : Names.International;

    [JsonIgnore]
    public PlayerNameStyle DisplayNameStyle
    {
        get
        {
            if (PlayerType == PlayerType.User)
                return NameStyle;

            return new()
            {
                ColorFrom = new()
                {
                    Dark = "#ffffff",
                    Light = "#000000"
                }
            };
        }
    }
}
