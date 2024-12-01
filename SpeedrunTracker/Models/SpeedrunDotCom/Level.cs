namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Level : BaseNamedSpeedrunModel
{
    public override string ToString() => Name;
}
