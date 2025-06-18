using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

[QueryProperty(nameof(RunDetails), "RunDetails")]
public partial class RunDetailsPage : ContentPage
{
    private readonly RunDetailsViewModel _viewModel;
    private bool _isLoaded;

    public RunDetails? RunDetails { get; set; }

    public RunDetailsPage(RunDetailsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }

    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (!_isLoaded)
        {
            _viewModel.RunDetails = RunDetails;
            await _viewModel.LoadDataAsync();
            BindingContext = _viewModel;
            _isLoaded = true;
        }
    }

    private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        await _viewModel.CloseActivityIndicatorAsync();
    }
}
