﻿using Microsoft.Extensions.Configuration;

namespace SpeedrunTracker.Services.LocalStorage;

public class LocalSettingsService : ILocalSettingsService
{
    private readonly IConfiguration _configuration;
    private readonly ILocalDatabaseService _databaseService;

    public LocalSettingsService(IConfiguration configuration, ILocalDatabaseService databaseService)
    {
        _configuration = configuration;
        _databaseService = databaseService;
    }

    public UserSettings UserSettings { get; set; }

    public async Task LoadSettingsAsync()
    {
        UserSettings = await _databaseService.Connection.Table<UserSettings>().FirstOrDefaultAsync();
        if (UserSettings == null)
        {
            UserSettings = new()
            {
                MaxLeaderboardResults = Convert.ToInt32(_configuration["defaults:max-leaderboard-results"])
            };

            await _databaseService.Connection.InsertAsync(UserSettings);
        }

        if (UserSettings.DateFormat == null)
            UserSettings.DateFormat = _configuration["defaults:date-format"];

        if (UserSettings.TimeFormat == null)
            UserSettings.TimeFormat = _configuration["defaults:time-format"];
    }

    public async Task SaveSettingsAsync()
    {
        await _databaseService.Connection.UpdateAsync(UserSettings);
    }
}
