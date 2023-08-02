using Refit;

namespace SpeedrunTracker.Interfaces;

public interface INotificationService
{
    [Get("/notifications?offset={offset}")]
    Task<PagedData<List<Notification>>> GetNotificationsAsync([Header(Constants.ApiKeyHeader)] string header, int offset);
}
