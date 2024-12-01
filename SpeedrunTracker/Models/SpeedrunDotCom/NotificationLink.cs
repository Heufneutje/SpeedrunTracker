namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record NotificationLink
{
    public NotificationLinkType? Rel { get; set; }
    public required string Uri { get; set; }
}
