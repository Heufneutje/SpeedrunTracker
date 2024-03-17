using SQLite;

namespace SpeedrunTracker.Contracts.LocalStorage;

public interface IDatabaseService
{
    SQLiteAsyncConnection Connection { get; }

    Task InitAsync(string databaseName, SQLiteOpenFlags flags);
}
