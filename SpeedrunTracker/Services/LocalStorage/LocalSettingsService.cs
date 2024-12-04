using Microsoft.Extensions.Configuration;

namespace SpeedrunTracker.Services.LocalStorage;

public class LocalSettingsService : BaseDatabaseService, ILocalSettingsService
{
    private readonly IConfiguration _configuration;

    public LocalSettingsService(IConfiguration configuration, ILocalDatabaseService databaseService)
        : base(databaseService)
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
                MaxLeaderboardResults = _configuration.GetValue<int>("defaults:max-leaderboard-results"),
                DisplayBackgrounds = _configuration.GetValue<bool>("defaults:display-backgrounds"),
            };

            await GetConnection().InsertAsync(UserSettings);
        }

        if (UserSettings.DateFormat == null)
            UserSettings.DateFormat = _configuration["defaults:date-format"];

        if (UserSettings.TimeFormat == null)
            UserSettings.TimeFormat = _configuration["defaults:time-format"];

        if (UserSettings.DisplayBackgrounds == null)
            UserSettings.DisplayBackgrounds = _configuration.GetValue<bool>("defaults:display-backgrounds");
    }

    public async Task SaveSettingsAsync()
    {
        await GetConnection().UpdateAsync(UserSettings);
    }
}
