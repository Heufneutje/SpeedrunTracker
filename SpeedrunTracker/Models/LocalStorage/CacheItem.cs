using SQLite;

namespace SpeedrunTracker.Models.LocalStorage;

public class CacheItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string? SpeedrunObjectId { get; set; }

    public CacheItemType Type { get; set; }

    public string? CachedJson { get; set; }

    public DateTime LastUpdated { get; set; }

    [Ignore]
    public bool IsExpired => (DateTime.Now - LastUpdated).TotalDays > 7;
}
