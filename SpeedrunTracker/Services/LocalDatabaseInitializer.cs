using SpeedrunTracker.Interfaces;
using SQLite;

namespace SpeedrunTracker.Services;

public class LocalDatabaseInitializer : IMauiInitializeService
{
    public void Initialize(IServiceProvider services)
    {
        ILocalDatabaseService service = services.GetRequiredService<ILocalDatabaseService>();
        Task.Run(async () =>
        {
            await service.InitAsync("speedruntracker.db3", SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
        }).GetAwaiter().GetResult();
    }
}
