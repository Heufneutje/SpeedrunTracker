namespace SpeedrunTracker.ViewModels;

public class NotificationViewModel
{
    public Notification Notification { get; set; }
    public string? DateFormat { get; set; }
    public string? TimeFormat { get; set; }
    public string FormattedCreationDate => $"{Notification.Created.ToString(DateFormat)} {Notification.Created.ToString(TimeFormat)}";

    public NotificationViewModel(Notification notification, string? dateFormat, string? timeFormat)
    {
        Notification = notification;
        DateFormat = dateFormat;
        TimeFormat = timeFormat;
    }
}
