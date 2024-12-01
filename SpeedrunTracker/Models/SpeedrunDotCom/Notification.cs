namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Notification : BaseSpeedrunModel
{
    public DateTime Created { get; set; }
    public NotificationStatusType Status { get; set; }
    public required string Text { get; set; }
    public NotificationLink? Item { get; set; }
}
