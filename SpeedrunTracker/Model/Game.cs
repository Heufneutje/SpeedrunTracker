using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model;

public class Game : BaseGame
{
    public int Released { get; set; }
    public BaseData<List<GamePlatform>> Platforms { get; set; }
    public Ruleset Ruleset { get; set; }
    public BaseData<List<User>> Moderators { get; set; }

    [JsonPropertyName("romhack")]
    public bool IsRomhack { get; set; }
}
