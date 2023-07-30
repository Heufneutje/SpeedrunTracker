using SpeedrunTracker.Model;

namespace SpeedrunTracker.Interfaces;

public interface ILocalFollowService
{
    Task FollowGameAsync(BaseGame game);

    Task UnfollowAsync(string id);

    Task<bool> IsFollowingAsync(string id);

    Task<List<FollowedEntity>> GetFollowedEntitiesAsync();
}
