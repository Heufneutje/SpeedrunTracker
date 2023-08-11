namespace SpeedrunTracker.Services;

public class CacheService : ICacheService
{
    private readonly ICacheDatabaseService _databaseService;
    private readonly IJsonSerializationService _jsonSerializationService;

    public CacheService(ICacheDatabaseService databaseService, IJsonSerializationService jsonSerializationService)
    {
        _databaseService = databaseService;
        _jsonSerializationService = jsonSerializationService;
    }

    public Task<CacheItem> GetCacheItemAsync(string id, CacheItemType type)
    {
        return _databaseService.Connection.Table<CacheItem>().FirstOrDefaultAsync(x => x.SpeedrunObjectId == id && x.Type == type);
    }

    public async Task SaveCacheItemAsync(string id, CacheItemType type, object cacheObj)
    {
        CacheItem item = await GetCacheItemAsync(id, type);
        if (item == null)
            item = new CacheItem() { SpeedrunObjectId = id, Type = type };

        item.CachedJson = _jsonSerializationService.Serialize(cacheObj);
        item.LastUpdated = DateTime.Now;

        if (item.Id == 0)
            await _databaseService.Connection.InsertAsync(item);
        else
            await _databaseService.Connection.UpdateAsync(item);
    }

    public T DeserializeCacheItem<T>(CacheItem cacheItem)
    {
        return _jsonSerializationService.Deserialize<T>(cacheItem.CachedJson);
    }
}