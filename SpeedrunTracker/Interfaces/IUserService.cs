using Refit;
using SpeedrunTracker.Model;

namespace SpeedrunTracker.Interfaces;

public interface IUserService
{
    [Get("/users/{userId}")]
    Task<BaseData<User>> GetUserAsync(string userId);
}
