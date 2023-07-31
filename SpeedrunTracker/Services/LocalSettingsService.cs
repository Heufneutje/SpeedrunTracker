﻿using Microsoft.Extensions.Configuration;
using SpeedrunTracker.Interfaces;

namespace SpeedrunTracker.Services;

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
                EnableGameSearch = Convert.ToBoolean(_configuration["defaults:enable-game-search"]),
                EnableUserSearch = Convert.ToBoolean(_configuration["defaults:enable-user-search"]),
                MaxLeaderboardResults = Convert.ToInt32(_configuration["defaults:max-leaderboard-results"])
            };

            await _databaseService.Connection.InsertAsync(UserSettings);
        }
    }

    public async Task SaveSettingsAsync()
    {
        await _databaseService.Connection.UpdateAsync(UserSettings);
    }
}