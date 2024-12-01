namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Link
{
    public string? Rel { get; set; }
    public required string Uri { get; set; }
}
