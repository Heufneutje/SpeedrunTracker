using CommunityToolkit.Maui.Core;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;

namespace SpeedrunTracker.ViewModels;

public class UserSearchViewModel : BaseSearchEntityViewModel
{
    private readonly IUserService _userService;

    public override string SearchTextPlaceholder => "Search for users...";

    public UserSearchViewModel(IToastService toastService, IUserService userService, IPopupService popupService)
        : base(toastService, popupService)
    {
        _userService = userService;
    }

    protected override async Task NavigateToAsync()
    {
        if (SelectedEntity?.SearchObject is User user)
        {
            ShowActivityIndicator();
            await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", user);
            SelectedEntity = null;
        }
    }

    protected override async Task<List<Entity>> SearchEntitiesAsync()
    {
        PagedData<List<User>>? apiData = await ExecuteNetworkTask(_userService.SearchUsersAsync(Query?.Trim() ?? string.Empty));
        if (apiData is null)
            return [];

        return apiData
            .Data.Select(x => new Entity()
            {
                Title = x.Names?.International,
                Subtitle = $"Registered: {x.Signup:yyyy-MM-dd}",
                ImageUrl = x.Assets?.Image?.Uri ?? "user",
                SearchObject = x,
            })
            .ToList();
    }
}
