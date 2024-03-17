namespace SpeedrunTracker.Contracts.SpeedrunData;

public interface INotificationService
{
    Task<PagedData<List<Notification>>> GetNotificationsAsync(int offset);
}
