namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record NotificationLink : BaseLink
{
    public NotificationLinkType? Rel { get; set; }
}
