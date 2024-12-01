using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;

namespace SpeedrunTracker.ViewModels;

public class UserSearchViewModel : BaseSearchEntityViewModel
{
    private readonly IUserService _userService;

    public override string SearchTextPlaceholder => "Search for users...";

    public UserSearchViewModel(IToastService toastService, IUserService userService) : base(toastService)
    {
        _userService = userService;
    }

    protected override async Task NavigateToAsync(Entity? entity)
    {
        if (entity?.SearchObject is User user)
            await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", user);
    }

    protected override async Task<List<Entity>> SearchEntitiesAsync()
    {
        PagedData<List<User>>? apiData = await ExecuteNetworkTask(_userService.SearchUsersAsync(Query.Trim()));
        if (apiData == null)
            return [];

        return apiData.Data.Select(x => new Entity()
        {
            Title = x.Names?.International,
            Subtitle = $"Registered: {x.Signup:yyyy-MM-dd}",
            ImageUrl = x.Assets?.Image?.Uri ?? "user",
            SearchObject = x
        }).ToList();
    }
}
