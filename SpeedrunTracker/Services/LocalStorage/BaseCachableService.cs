namespace SpeedrunTracker.Services.LocalStorage;

public abstract class BaseCachableService
{
    private readonly ICacheService _cacheService;

    protected BaseCachableService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    protected async Task<T> GetCachedResourceAsync<T>(string gameId, CacheItemType itemType, Task<BaseData<T>> repositoryAction) where T : class
    {
        CacheItem cacheItem = await _cacheService.GetCacheItemAsync(gameId, itemType);
        if (cacheItem != null && !cacheItem.IsExpired)
            return _cacheService.DeserializeCacheItem<T>(cacheItem);

        T resource = (await repositoryAction)?.Data;
        if (resource != null)
            await _cacheService.SaveCacheItemAsync(gameId, itemType, resource);
        return resource;
    }
}
