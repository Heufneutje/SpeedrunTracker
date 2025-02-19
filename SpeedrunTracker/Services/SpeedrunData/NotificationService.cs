﻿namespace SpeedrunTracker.Services.SpeedrunData;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<PagedData<List<Notification>>> GetNotificationsAsync(int offset)
    {
        return await _notificationRepository.GetNotificationsAsync(
            await SecureStorage.GetAsync(Constants.ApiKey),
            offset
        );
    }
}
