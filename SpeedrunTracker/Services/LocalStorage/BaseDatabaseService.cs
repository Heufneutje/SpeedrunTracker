using SQLite;

namespace SpeedrunTracker.Services.LocalStorage;

public class BaseDatabaseService
{
    private readonly IDatabaseService _databaseService;

    public BaseDatabaseService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    protected SQLiteAsyncConnection GetConnection()
    {
        if (_databaseService.Connection is null)
            throw new InvalidOperationException("Local database was not initialized");

        return _databaseService.Connection;
    }
}
