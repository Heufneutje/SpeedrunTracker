using SQLite;

namespace SpeedrunTracker.Services.LocalStorage;

public class CacheDatabaseService : ICacheDatabaseService
{
    public SQLiteAsyncConnection? Connection { get; private set; }

    public async Task InitAsync(string databaseName, SQLiteOpenFlags flags)
    {
        if (Connection is not null)
            return;

        Connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.CacheDirectory, databaseName), flags);
        await Connection.CreateTableAsync<CacheItem>();
    }
}
