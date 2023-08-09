namespace SpeedrunTracker.Services;

public interface INotificationService
{
    Task<PagedData<List<Notification>>> GetNotificationsAsync(int offset);
}
