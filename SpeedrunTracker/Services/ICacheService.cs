namespace SpeedrunTracker.Services;

public interface ICacheService
{
    Task<CacheItem> GetCacheItemAsync(string id, CacheItemType type);

    Task SaveCacheItemAsync(string id, CacheItemType type, object cacheObj);

    T DeserializeCacheItem<T>(CacheItem cacheItem);
}
