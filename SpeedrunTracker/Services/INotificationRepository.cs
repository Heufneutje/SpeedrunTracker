using Refit;

namespace SpeedrunTracker.Services;

public interface INotificationRepository
{
    [Get("/notifications?offset={offset}")]
    Task<PagedData<List<Notification>>> GetNotificationsAsync([Header(Constants.ApiKeyHeader)] string header, int offset);
}
