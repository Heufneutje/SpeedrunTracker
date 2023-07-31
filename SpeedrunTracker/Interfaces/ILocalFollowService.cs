namespace SpeedrunTracker.Interfaces;

public interface ILocalFollowService
{
    Task FollowGameAsync(BaseGame game);

    Task FollowUserAsync(User user);

    Task UnfollowAsync(string id);

    Task<bool> IsFollowingAsync(string id);

    Task<List<FollowedEntity>> GetFollowedEntitiesAsync();
}
