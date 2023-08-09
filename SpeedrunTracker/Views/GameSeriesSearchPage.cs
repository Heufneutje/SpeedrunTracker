using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class GameSeriesSearchPage : BaseSearchContentPage
{
    public GameSeriesSearchPage(GameSeriesSearchViewModel viewModel)
    {
        BindingContext = viewModel;
    }
}
