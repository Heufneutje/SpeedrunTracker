using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record GameAssets
{
    public Asset Logo { get; set; }

    [JsonPropertyName("cover-tiny")]
    public Asset CoverTiny { get; set; }

    [JsonPropertyName("cover-small")]
    public Asset CoverSmall { get; set; }

    public Asset Background { get; set; }

    [JsonPropertyName("trophy-1st")]
    public Asset TrophyFirstPlace { get; set; }

    [JsonPropertyName("trophy-2nd")]
    public Asset TrophySecondPlace { get; set; }

    [JsonPropertyName("trophy-3rd")]
    public Asset TrophyThirdPlace { get; set; }

    [JsonPropertyName("trophy-4th")]
    public Asset TrophyFouthPlace { get; set; }

    public Asset GetTrophyAsset(int place)
    {
        return place switch
        {
            1 => TrophyFirstPlace,
            2 => TrophySecondPlace,
            3 => TrophyThirdPlace,
            4 => TrophyFouthPlace,
            _ => null
        };
    }
}
