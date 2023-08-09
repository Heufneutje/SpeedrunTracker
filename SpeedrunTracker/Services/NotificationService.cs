namespace SpeedrunTracker.Services;

public class NotificationService : INotificationService
{
    private INotificationRepository _notificationService;

    public NotificationService(INotificationRepository notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<PagedData<List<Notification>>> GetNotificationsAsync(int offset)
    {
        return await _notificationService.GetNotificationsAsync(await SecureStorage.GetAsync(Constants.ApiKey), offset);
    }
}
