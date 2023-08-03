using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public class Game : BaseGame
{
    public BaseData<List<GamePlatform>> Platforms { get; set; }
    public Ruleset Ruleset { get; set; }
    public BaseData<List<User>> Moderators { get; set; }

    [JsonPropertyName("romhack")]
    public bool IsRomhack { get; set; }
}
