using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class UserSearchPage : BaseSearchContentPage
{
    public UserSearchPage(UserSearchViewModel searchViewModel)
    {
        BindingContext = searchViewModel;
    }
}
