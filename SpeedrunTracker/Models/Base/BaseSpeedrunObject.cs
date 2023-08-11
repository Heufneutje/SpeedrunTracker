namespace SpeedrunTracker.Models.Base;

public record BaseSpeedrunObject
{
    public string Id { get; set; }

    public List<Link> Links { get; set; }
}
