﻿using Microsoft.Extensions.Configuration;
using SpeedrunTracker.Services.LocalStorage;

namespace SpeedrunTracker.Services.SpeedrunData;

public class GameService : BaseCachableService, IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly ExclusionCollection _archivedCategories;
    private readonly ExclusionCollection _archivedVariables;
    private readonly ExclusionCollection _archivedLevels;

    public GameService(IGameRepository gameRepository, ICacheService cacheService, IConfiguration configuration) : base(cacheService)
    {
        _gameRepository = gameRepository;
        _archivedCategories = ExclusionCollection.ParseExclusions(configuration["speedrun-dot-com:archived-categories"]);
        _archivedVariables = ExclusionCollection.ParseExclusions(configuration["speedrun-dot-com:archived-variables"]);
        _archivedLevels = ExclusionCollection.ParseExclusions(configuration["speedrun-dot-com:archived-levels"]);
    }

    public Task<Game> GetGameAsync(string gameId)
    {
        return GetCachedResourceAsync(gameId, CacheItemType.Games, _gameRepository.GetGameAsync(gameId));
    }

    public async Task<List<Category>> GetGameCategoriesAsync(string gameId)
    {
        return (await GetCachedResourceAsync(gameId, CacheItemType.Categories, _gameRepository.GetGameCategoriesAsync(gameId))).Where(x => !_archivedCategories.IsExcluded(gameId, x.Id)).ToList();
    }

    public async Task<List<Level>> GetGameLevelsAsync(string gameId)
    {
        return (await GetCachedResourceAsync(gameId, CacheItemType.Levels, _gameRepository.GetGameLevelsAsync(gameId))).Where(x => !_archivedLevels.IsExcluded(gameId, x.Id)).ToList();
    }

    public async Task<List<Variable>> GetGameVariablesAsync(string gameId)
    {
        return (await GetCachedResourceAsync(gameId, CacheItemType.Variables, _gameRepository.GetGameVariablesAsync(gameId))).Where(x => !_archivedVariables.IsExcluded(gameId, x.Id)).ToList();
    }

    public Task<PagedData<List<Game>>> SearchGamesAsync(string name)
    {
        return _gameRepository.SearchGamesAsync(name);
    }
}
