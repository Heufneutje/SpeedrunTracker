using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Game : BaseGame
{
    public BaseData<List<GamePlatform>> Platforms { get; set; }
    public Ruleset? Ruleset { get; set; }
    public BaseData<List<User>> Moderators { get; set; }

    [JsonPropertyName("romhack")]
    public bool IsRomhack { get; set; }

    public Game()
    {
        Platforms = new BaseData<List<GamePlatform>>() { Data = [] };
        Moderators = new BaseData<List<User>>() { Data = [] };
    }
}
