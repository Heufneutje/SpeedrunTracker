namespace SpeedrunTracker.Models.Base;

public record BaseGame : BaseSpeedrunObject
{
    public Names Names { get; set; }
    public GameAssets Assets { get; set; }
    public int Released { get; set; }
}
