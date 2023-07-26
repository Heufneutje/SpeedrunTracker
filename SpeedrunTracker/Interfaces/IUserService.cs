using Refit;
using SpeedrunTracker.Model;

namespace SpeedrunTracker.Interfaces;

public interface IUserService
{
    [Get("/users/{userId}")]
    Task<BaseData<User>> GetUserAsync(string userId);

    [Get("/users?name={name}")]
    Task<PagedData<List<User>>> SearchUsersAsync(string name);
}
