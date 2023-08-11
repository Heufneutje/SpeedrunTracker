namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record NotificationLink
{
    public NotificationLinkType? Rel { get; set; }
    public string Uri { get; set; }
}
