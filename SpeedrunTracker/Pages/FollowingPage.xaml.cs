using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Pages;

public partial class FollowingPage : ContentPage
{
    private FollowedEntityViewModel _viewModel;

    public FollowingPage(FollowedEntityViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        await _viewModel.LoadFollowedEntities();
    }
}
