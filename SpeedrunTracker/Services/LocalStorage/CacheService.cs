namespace SpeedrunTracker.Services.LocalStorage;

public class CacheService : BaseDatabaseService, ICacheService
{
    private readonly IJsonSerializationService _jsonSerializationService;

    public CacheService(ICacheDatabaseService databaseService, IJsonSerializationService jsonSerializationService) : base(databaseService)
    {
        _jsonSerializationService = jsonSerializationService;
    }

    public Task<CacheItem> GetCacheItemAsync(string id, CacheItemType type)
    {
        return GetConnection().Table<CacheItem>().FirstOrDefaultAsync(x => x.SpeedrunObjectId == id && x.Type == type);
    }

    public async Task SaveCacheItemAsync(string id, CacheItemType type, object cacheObj)
    {
        CacheItem item = await GetCacheItemAsync(id, type);
        item ??= new CacheItem() { SpeedrunObjectId = id, Type = type };

        item.CachedJson = _jsonSerializationService.Serialize(cacheObj);
        item.LastUpdated = DateTime.Now;

        if (item.Id == 0)
            await GetConnection().InsertAsync(item);
        else
            await GetConnection().UpdateAsync(item);
    }

    public T? DeserializeCacheItem<T>(CacheItem cacheItem)
    {
        if (cacheItem.CachedJson == null)
            return default;

        return _jsonSerializationService.Deserialize<T>(cacheItem.CachedJson);
    }
}
