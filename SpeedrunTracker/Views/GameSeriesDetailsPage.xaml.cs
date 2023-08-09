using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

[QueryProperty(nameof(Series), "Series")]
public partial class GameSeriesDetailsPage : ContentPage
{
    private readonly GameSeriesDetailViewModel _viewModel;
    private bool _isLoaded;

    public GameSeries Series
    {
        get => _viewModel.Series;
        set => _viewModel.Series = value;
    }

    public GameSeriesDetailsPage(GameSeriesDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (!_isLoaded)
        {
            await _viewModel.LoadGamesAsync();
            _isLoaded = true;
        }
    }
}
