using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class GameSearchPage : BaseSearchContentPage
{
    public GameSearchPage(GameSearchViewModel searchViewModel)
    {
        BindingContext = searchViewModel;
    }
}
