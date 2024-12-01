namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Country
{
    public required string Code { get; set; }
    public required Names Names { get; set; }
}
