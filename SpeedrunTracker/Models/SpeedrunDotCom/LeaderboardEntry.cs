using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record LeaderboardEntry
{
    public int Place { get; set; }
    public required Speedrun Run { get; set; }
    public BaseData<BaseGame>? Game { get; set; }

    // For some ungodly reason it's a single object when there is a level and an empty list if there's not.
    [JsonPropertyName("level")]
    public BaseData<object>? LevelJson { get; set; }

    public BaseData<Category>? Category { get; set; }

    // For some ungodly reason it's a single object when there is a platform and an empty list if there's not.
    [JsonPropertyName("platform")]
    public BaseData<object>? PlatformJson { get; set; }

    private Level? _level;

    [JsonIgnore]
    public Level? Level
    {
        get
        {
            _level ??=
                Run.LevelId is null || LevelJson?.Data is null
                    ? null
                    : JsonSerializer.Deserialize<Level>(LevelJson.Data.ToString()!);
            return _level;
        }
    }

    private GamePlatform? _platform;

    [JsonIgnore]
    public GamePlatform? Platform
    {
        get
        {
            _platform ??=
                Run?.System?.PlatformId is null || PlatformJson?.Data is null
                    ? null
                    : JsonSerializer.Deserialize<GamePlatform>(PlatformJson.Data.ToString()!);
            return _platform;
        }
    }

    private string? _ordinalPlace;

    [JsonIgnore]
    public string OrdinalPlace
    {
        get
        {
            _ordinalPlace ??= Place.AsOrdinal();
            return _ordinalPlace;
        }
    }

    private Asset? _trophyAsset;

    [JsonIgnore]
    public Asset? TrophyAsset
    {
        get
        {
            if (_trophyAsset is null && Place < 5)
                _trophyAsset = Game?.Data?.Assets?.GetTrophyAsset(Place);

            return _trophyAsset;
        }
        set
        {
            if (_trophyAsset != value)
                _trophyAsset = value;
        }
    }
}
