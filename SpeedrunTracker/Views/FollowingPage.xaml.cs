using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class FollowingPage : ContentPage
{
    private readonly FollowedEntityViewModel _viewModel;

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
