using SQLite;

namespace SpeedrunTracker.Services;

public interface ILocalDatabaseService
{
    SQLiteAsyncConnection Connection { get; }

    Task InitAsync(string databaseName, SQLiteOpenFlags flags);
}
