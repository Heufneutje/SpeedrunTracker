namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Asset
{
    public string Uri { get; set; }

    // TODO: Remove properties below when speedrun.com actually fixes their API. https://github.com/speedruncomorg/api/issues/169
    public string FixedGameAssetUri => Uri?.Replace("gameasset", "static/game");

    public string FixedThemeAssetUri => Uri?.Replace("themeasset", "static/theme");

    public string FixedUserAssetUri => Uri?.Replace("userasset", "static/user");
}
