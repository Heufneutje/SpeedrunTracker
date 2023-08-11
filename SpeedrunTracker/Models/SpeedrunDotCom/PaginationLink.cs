namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record PaginationLink
{
    public string Rel { get; set; }
    public string Uri { get; set; }
}
