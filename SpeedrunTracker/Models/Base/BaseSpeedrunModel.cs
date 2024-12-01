namespace SpeedrunTracker.Models.Base;

public record BaseSpeedrunModel
{
    public string Id { get; set; }

    public string? Weblink { get; set; }

    public List<Link> Links { get; set; }

    public BaseSpeedrunModel()
    {
        Id = string.Empty;
        Links = [];
    }
}
