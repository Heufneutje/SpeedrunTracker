namespace SpeedrunTracker.Model;

public class Level : BaseSpeedrunObject
{
    public string Name { get; set; }

    public override string ToString() => Name;
}
