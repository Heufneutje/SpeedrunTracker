using SQLite;

namespace SpeedrunTracker.Services;

public interface IDatabaseService
{
    SQLiteAsyncConnection Connection { get; }

    Task InitAsync(string databaseName, SQLiteOpenFlags flags);
}
