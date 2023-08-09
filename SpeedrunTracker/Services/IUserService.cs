﻿namespace SpeedrunTracker.Services;

public interface IUserService
{
    Task<BaseData<User>> GetUserAsync(string userId);

    Task<PagedData<List<User>>> SearchUsersAsync(string name);

    Task<BaseData<List<LeaderboardEntry>>> GetUserPersonalBestsAsync(string userId);

    Task<BaseData<User>> GetUserProfileAsync();
}
