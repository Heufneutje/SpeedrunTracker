using SQLite;

namespace SpeedrunTracker.Interfaces;

public interface ILocalDatabaseService
{
    SQLiteAsyncConnection Connection { get; }

    Task InitAsync(string databaseName, SQLiteOpenFlags flags);
}
