namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record GameSeries : BaseSpeedrunObject
{
    public required Names Names { get; set; }
    public required GameAssets Assets { get; set; }
    public DateTime? Created { get; set; }
}
