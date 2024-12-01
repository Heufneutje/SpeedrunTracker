namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record SpeedrunVideos
{
    public List<Link> Links { get; set; }

    public SpeedrunVideos()
    {
        Links = [];
    }
}
