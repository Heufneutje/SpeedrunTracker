namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Level : BaseSpeedrunModel, INamedSpeedrunModel
{
    public required string Name { get; set; }

    public override string ToString() => Name;
}
