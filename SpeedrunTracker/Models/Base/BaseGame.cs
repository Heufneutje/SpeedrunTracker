namespace SpeedrunTracker.Models.Base;

public record BaseGame : BaseSpeedrunModel
{
    public required Names Names { get; set; }
    public required GameAssets Assets { get; set; }
    public int Released { get; set; }
}
