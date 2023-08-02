namespace SpeedrunTracker.Interfaces;

public interface INotificationRepository
{
    Task<PagedData<List<Notification>>> GetNotificationsAsync(int offset);
}
