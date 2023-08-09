using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using SpeedrunTracker.Services;

namespace SpeedrunTracker.ViewModels;

public class UserSearchViewModel : BaseSearchEntityViewModel
{
    private IUserRepository _userRepository;

    public override string SearchTextPlaceholder => "Search for users...";

    public UserSearchViewModel(IToastService toastService, IUserRepository userRepository) : base(toastService)
    {
        _userRepository = userRepository;
    }

    protected override async Task NavigateToAsync(Entity entity)
    {
        if (entity.SearchObject is User user)
            await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", user);
    }

    protected override async Task<List<Entity>> SearchEntitiesAsync()
    {
        PagedData<List<User>> apiData = await ExecuteNetworkTask(_userRepository.SearchUsersAsync(Query.Trim()));
        if (apiData == null)
            return null;

        return apiData.Data.Select(x => new Entity()
        {
            Title = x.Names.International,
            Subtitle = $"Registered: {x.Signup:yyyy-MM-dd}",
            ImageUrl = x.Assets.Image.FixedUserAssetUri,
            SearchObject = x
        }).ToList();
    }
}
