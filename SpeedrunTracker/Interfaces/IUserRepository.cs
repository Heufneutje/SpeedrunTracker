using SpeedrunTracker.Model;

namespace SpeedrunTracker.Interfaces;

public interface IUserRepository
{
    Task<BaseData<User>> GetUserAsync(string userId);
}
