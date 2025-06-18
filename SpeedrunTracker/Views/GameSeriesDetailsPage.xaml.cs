using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

[QueryProperty(nameof(Series), "Series")]
public partial class GameSeriesDetailsPage : BaseDetailPage
{
    private readonly GameSeriesDetailViewModel _viewModel;

    public GameSeries? Series
    {
        get => _viewModel?.Series;
        set => _viewModel.Series = value;
    }

    public GameSeriesDetailsPage(GameSeriesDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
}
