using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpeedrunTracker.Model;

public class LeaderboardEntry
{
    public int Place { get; set; }
    public Speedrun Run { get; set; }
    public BaseData<BaseGame> Game { get; set; }

    // For some ungodly reason it's a single object when there is a level and an empty list if there's not.
    [JsonPropertyName("level")]
    public BaseData<object> ThisShouldBeALevel { get; set; }

    public BaseData<Category> Category { get; set; }

    public BaseData<GamePlatform> Platform { get; set; }

    private Level _level;

    [JsonIgnore]
    public Level Level
    {
        get
        {
            if (_level == null)
                _level = Run.LevelId == null ? null : JsonSerializer.Deserialize<Level>(ThisShouldBeALevel.Data.ToString());
            return _level;
        }
    }

    private string _ordinalPlace;

    [JsonIgnore]
    public string OrdinalPlace
    {
        get
        {
            if (_ordinalPlace == null)
                _ordinalPlace = Place.AsOrdinal();
            return _ordinalPlace;
        }
    }

    private Asset _trophyAsset;

    [JsonIgnore]
    public Asset TrophyAsset
    {
        get
        {
            if (_trophyAsset == null && Place < 5)
            {
                _trophyAsset = Place switch
                {
                    1 => Game?.Data?.Assets?.TrophyFirstPlace,
                    2 => Game?.Data?.Assets?.TrophySecondPlace,
                    3 => Game?.Data?.Assets?.TrophyThirdPlace,
                    4 => Game?.Data?.Assets?.TrophyFouthPlace,
                    _ => null
                };
            }
            return _trophyAsset;
        }
    }
}
