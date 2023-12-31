﻿namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Notification : BaseSpeedrunObject
{
    public DateTime Created { get; set; }
    public NotificationStatusType Status { get; set; }
    public string Text { get; set; }
    public NotificationLink Item { get; set; }
}
