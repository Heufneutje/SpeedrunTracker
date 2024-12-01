using Microsoft.Extensions.Configuration;

namespace SpeedrunTracker.Services.LocalStorage;

public class LocalSettingsService : BaseDatabaseService, ILocalSettingsService
{
    private readonly IConfiguration _configuration;

    public LocalSettingsService(IConfiguration configuration, ILocalDatabaseService databaseService) : base(databaseService)
    {
        _configuration = configuration;
        UserSettings = new();
    }

    public UserSettings UserSettings { get; set; }

    public async Task LoadSettingsAsync()
    {
        UserSettings = await GetConnection().Table<UserSettings>().FirstOrDefaultAsync();
        if (UserSettings == null)
        {
            UserSettings = new()
            {
                MaxLeaderboardResults = Convert.ToInt32(_configuration["defaults:max-leaderboard-results"])
            };

            await GetConnection().InsertAsync(UserSettings);
        }

        if (UserSettings.DateFormat == null)
            UserSettings.DateFormat = _configuration["defaults:date-format"];

        if (UserSettings.TimeFormat == null)
            UserSettings.TimeFormat = _configuration["defaults:time-format"];
    }

    public async Task SaveSettingsAsync()
    {
        await GetConnection().UpdateAsync(UserSettings);
    }
}
