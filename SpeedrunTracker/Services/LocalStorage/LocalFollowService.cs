 namespace SpeedrunTracker.Services.LocalStorage;

public class LocalFollowService : BaseDatabaseService, ILocalFollowService
{
    public LocalFollowService(ILocalDatabaseService databaseService) : base(databaseService)
    {
    }

    public async Task FollowGameAsync(BaseGame game)
    {
        FollowedEntity item = new()
        {
            Id = game.Id,
            ImageUrl = game.Assets?.CoverSmall?.Uri,
            Title = game.Names.International,
            Subtitle = $"Released: {game.Released}",
            Type = EntityType.Games
        };
        await GetConnection().InsertAsync(item);
    }

    public async Task FollowSeriesAsync(GameSeries series)
    {
        FollowedEntity item = new()
        {
            Id = series.Id,
            ImageUrl = series.Assets?.CoverSmall?.Uri,
            Title = series.Names.International,
            Subtitle = $"Created: {series.Created?.ToString("yyyy-MM-dd") ?? "Unknown"}",
            Type = EntityType.Series
        };
        await GetConnection().InsertAsync(item);
    }

    public async Task FollowUserAsync(User user)
    {
        FollowedEntity item = new()
        {
            Id = user.Id,
            ImageUrl = user.Assets?.Image?.Uri ?? "user",
            Title = user.Names?.International,
            Subtitle = $"Registered: {user.Signup:yyyy-MM-dd}",
            Type = EntityType.Users
        };
        await GetConnection().InsertAsync(item);
    }

    public async Task<List<FollowedEntity>> GetFollowedEntitiesAsync()
    {
        return await GetConnection().Table<FollowedEntity>().OrderBy(x => x.Title).ToListAsync();
    }

    public async Task<bool> IsFollowingAsync(string id)
    {
        return await GetConnection().Table<FollowedEntity>().Where(x => x.Id == id).CountAsync() > 0;
    }

    public async Task UnfollowAsync(string id)
    {
        await GetConnection().Table<FollowedEntity>().DeleteAsync(x => x.Id == id);
    }
}
