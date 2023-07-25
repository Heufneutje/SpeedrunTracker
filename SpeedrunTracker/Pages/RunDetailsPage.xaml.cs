using SpeedrunTracker.Model;
using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Pages;

[QueryProperty(nameof(RunDetails), "RunDetails")]
public partial class RunDetailsPage : ContentPage
{
    private readonly RunDetailsViewModel _viewModel;
    private bool _isLoaded;

    public RunDetails RunDetails { get; set; }

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
            await _viewModel.LoadData();
            BindingContext = _viewModel;
            _isLoaded = true;
        }
    }
}
