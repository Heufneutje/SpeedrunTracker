namespace SpeedrunTracker.Services;

public interface INotificationRepository
{
    Task<PagedData<List<Notification>>> GetNotificationsAsync(int offset);
}
