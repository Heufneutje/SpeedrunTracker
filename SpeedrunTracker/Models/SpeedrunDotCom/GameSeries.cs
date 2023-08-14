namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record GameSeries : BaseSpeedrunObject
{
    public Names Names { get; set; }
    public GameAssets Assets { get; set; }
    public DateTime? Created { get; set; }
}
