namespace SpeedrunTracker.Services;

public interface ILocalFollowService
{
    Task FollowGameAsync(BaseGame game);

    Task FollowUserAsync(User user);

    Task FollowSeriesAsync(GameSeries series);

    Task UnfollowAsync(string id);

    Task<bool> IsFollowingAsync(string id);

    Task<List<FollowedEntity>> GetFollowedEntitiesAsync();
}
