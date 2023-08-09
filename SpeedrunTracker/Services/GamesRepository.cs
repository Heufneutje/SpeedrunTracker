using SpeedrunTracker.Services;

namespace SpeedrunTracker.Repository;

public class GamesRepository : IGamesRepository
{
    private readonly IGamesService _gamesService;

    public GamesRepository(IGamesService gamesService)
    {
        _gamesService = gamesService;
    }

    public Task<BaseData<Game>> GetGameAsync(string gameId)
    {
        return _gamesService.GetGameAsync(gameId);
    }

    public Task<BaseData<List<Category>>> GetGameCategoriesAsync(string gameId)
    {
        return _gamesService.GetGameCategoriesAsync(gameId);
    }

    public Task<BaseData<List<Level>>> GetGameLevelsAsync(string gameId)
    {
        return _gamesService.GetGameLevelsAsync(gameId);
    }

    public Task<BaseData<List<Variable>>> GetGameVariablesAsync(string gameId)
    {
        return _gamesService.GetGameVariablesAsync(gameId);
    }

    public Task<PagedData<List<Game>>> SearchGamesAsync(string name)
    {
        return _gamesService.SearchGamesAsync(name);
    }
}
