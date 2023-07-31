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
        ILocalDatabaseService databaseService = services.GetRequiredService<ILocalDatabaseService>();
        ILocalSettingsService settingsService = services.GetRequiredService<ILocalSettingsService>();

        Task.Run(async () =>
        {
            await databaseService.InitAsync(_configuration["storage:db-name"], SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            await settingsService.LoadSettingsAsync();
        }).GetAwaiter().GetResult();
    }
}
