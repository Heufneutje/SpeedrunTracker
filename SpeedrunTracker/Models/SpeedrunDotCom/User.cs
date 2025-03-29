using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record User : BaseSpeedrunModel
{
    [JsonPropertyName("rel")]
    public PlayerType PlayerType { get; set; }

    public string? Name { get; set; }
    public Names? Names { get; set; }
    public string? Pronouns { get; set; }

    [JsonPropertyName("name-style")]
    public UserNameStyle? NameStyle { get; set; }

    public DateTime? Signup { get; set; }
    public UserLocation? Location { get; set; }
    public Link? Twitch { get; set; }
    public Link? Hitbox { get; set; }

    [JsonPropertyName("youtube")]
    public Link? YouTube { get; set; }

    public Link? Twitter { get; set; }

    [JsonPropertyName("speedrunslive")]
    public Link? SpeedRunsLive { get; set; }

    public UserAssets? Assets { get; set; }

    [JsonIgnore]
    public override string DisplayName
    {
        get
        {
            if (PlayerType == PlayerType.Guest)
                return Name ?? string.Empty;

            return (string.IsNullOrEmpty(Names?.International) ? Names?.Japanese : Names.International) ?? string.Empty;
        }
    }

    public static User GetUserNotFoundPlaceholder()
    {
        return new User() { Names = new Names() { International = "User Not Found" } };
    }
}
