namespace SpeedrunTracker.Services;

public class NotificationRepository : INotificationRepository
{
    private INotificationService _notificationService;

    public NotificationRepository(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<PagedData<List<Notification>>> GetNotificationsAsync(int offset)
    {
        return await _notificationService.GetNotificationsAsync(await SecureStorage.GetAsync(Constants.ApiKey), offset);
    }
}
