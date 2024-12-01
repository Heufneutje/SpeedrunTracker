namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record GamePlatform : BaseSpeedrunModel, INamedSpeedrunModel
{
    public required string Name { get; set; }
}
