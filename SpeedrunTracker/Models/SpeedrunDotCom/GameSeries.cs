namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record GameSeries : BaseSpeedrunModel
{
    public required Names Names { get; set; }
    public required GameAssets Assets { get; set; }
    public DateTime? Created { get; set; }
}
