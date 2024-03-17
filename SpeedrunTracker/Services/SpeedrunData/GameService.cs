using SpeedrunTracker.Services.LocalStorage;

namespace SpeedrunTracker.Services.SpeedrunData;

public class GameService : BaseCachableService, IGameService
{
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository, ICacheService cacheService) : base(cacheService)
    {
        _gameRepository = gameRepository;
    }

    public Task<Game> GetGameAsync(string gameId)
    {
        return GetCachedResourceAsync(gameId, CacheItemType.Games, _gameRepository.GetGameAsync(gameId));
    }

    public Task<List<Category>> GetGameCategoriesAsync(string gameId)
    {
        return GetCachedResourceAsync(gameId, CacheItemType.Categories, _gameRepository.GetGameCategoriesAsync(gameId));
    }

    public Task<List<Level>> GetGameLevelsAsync(string gameId)
    {
        return GetCachedResourceAsync(gameId, CacheItemType.Levels, _gameRepository.GetGameLevelsAsync(gameId));
    }

    public Task<List<Variable>> GetGameVariablesAsync(string gameId)
    {
        return GetCachedResourceAsync(gameId, CacheItemType.Variables, _gameRepository.GetGameVariablesAsync(gameId));
    }

    public Task<PagedData<List<Game>>> SearchGamesAsync(string name)
    {
        return _gameRepository.SearchGamesAsync(name);
    }
}
