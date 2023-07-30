using Microsoft.Extensions.Configuration;
using SpeedrunTracker.Interfaces;
using SQLite;

namespace SpeedrunTracker.Services;

public class LocalDatabaseInitializer : IMauiInitializeService
{
    private readonly IConfiguration _configuration;

    public LocalDatabaseInitializer(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Initialize(IServiceProvider services)
    {
        ILocalDatabaseService service = services.GetRequiredService<ILocalDatabaseService>();
        Task.Run(async () =>
        {
            await service.InitAsync(_configuration["storage:db-name"], SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
        }).GetAwaiter().GetResult();
    }
}
