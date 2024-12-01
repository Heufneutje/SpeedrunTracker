namespace SpeedrunTracker.Models.Base;

public record BaseGame : BaseSpeedrunObject
{
    public required Names Names { get; set; }
    public required GameAssets Assets { get; set; }
    public int Released { get; set; }
}
