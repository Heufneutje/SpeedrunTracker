using System.Text.Json.Serialization;
using static Java.Util.Jar.Attributes;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public class User : BaseSpeedrunObject
{
    [JsonPropertyName("rel")]
    public PlayerType PlayerType { get; set; }

    public string Name { get; set; }
    public Names Names { get; set; }
    public string Weblink { get; set; }
    public string Pronouns { get; set; }

    [JsonPropertyName("name-style")]
    public UserNameStyle NameStyle { get; set; }

    public DateTime? Signup { get; set; }
    public UserLocation Location { get; set; }
    public Link Twitch { get; set; }
    public Link Hitbox { get; set; }

    [JsonPropertyName("youtube")]
    public Link YouTube { get; set; }

    public Link Twitter { get; set; }

    [JsonPropertyName("speedrunslive")]
    public Link SpeedRunsLive { get; set; }

    public UserAssets Assets { get; set; }

    [JsonIgnore]
    public string DisplayName => PlayerType == PlayerType.Guest ? Name : string.IsNullOrEmpty(Names.International) ? Names.Japanese : Names.International;

    public User()
    {
    }

    public static User GetUserNotFoundPlaceholder()
    {
        return new User() { Names = new Names() { International = "User Not Found" } };
    }
}
