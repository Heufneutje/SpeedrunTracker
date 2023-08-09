using SpeedrunTracker.Interfaces;

namespace SpeedrunTracker.Services;

public class LocalFollowService : ILocalFollowService
{
    private readonly ILocalDatabaseService _databaseService;

    public LocalFollowService(ILocalDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task FollowGameAsync(BaseGame game)
    {
        FollowedEntity item = new()
        {
            Id = game.Id,
            ImageUrl = game.Assets?.CoverSmall?.FixedGameAssetUri,
            Title = game.Names.International,
            Subtitle = $"Released: {game.Released}",
            Type = EntityType.Games
        };
        await _databaseService.Connection.InsertAsync(item);
    }

    public async Task FollowSeriesAsync(GameSeries series)
    {
        FollowedEntity item = new()
        {
            Id = series.Id,
            ImageUrl = series.Assets?.CoverSmall?.FixedGameAssetUri,
            Title = series.Names.International,
            Subtitle = $"Created: {series.Created?.ToString("yyyy-MM-dd") ?? "Unknown"}",
            Type = EntityType.Series
        };
        await _databaseService.Connection.InsertAsync(item);
    }

    public async Task FollowUserAsync(User user)
    {
        FollowedEntity item = new()
        {
            Id = user.Id,
            ImageUrl = user.Assets?.Image?.FixedUserAssetUri,
            Title = user.Names.International,
            Subtitle = $"Registered: {user.Signup:yyyy-MM-dd}",
            Type = EntityType.Users
        };
        await _databaseService.Connection.InsertAsync(item);
    }

    public async Task<List<FollowedEntity>> GetFollowedEntitiesAsync()
    {
        return await _databaseService.Connection.Table<FollowedEntity>().OrderBy(x => x.Title).ToListAsync();
    }

    public async Task<bool> IsFollowingAsync(string id)
    {
        return await _databaseService.Connection.Table<FollowedEntity>().Where(x => x.Id == id).CountAsync() > 0;
    }

    public async Task UnfollowAsync(string id)
    {
        await _databaseService.Connection.Table<FollowedEntity>().DeleteAsync(x => x.Id == id);
    }
}
