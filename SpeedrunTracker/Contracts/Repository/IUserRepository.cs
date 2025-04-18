﻿using Refit;

namespace SpeedrunTracker.Contracts.Repository;

public interface IUserRepository
{
    [Get("/users/{userId}")]
    Task<BaseData<User>> GetUserAsync(string userId);

    [Get("/users?name={name}")]
    Task<PagedData<List<User>>> SearchUsersAsync(string name);

    [Get("/users/{userId}/personal-bests?embed=game,category,category.variables,level,platform")]
    Task<BaseData<List<LeaderboardEntry>>> GetUserPersonalBestsAsync(string userId);

    [Get("/profile")]
    Task<BaseData<User>> GetUserProfileAsync([Header(Constants.ApiKeyHeader)] string? header);
}
