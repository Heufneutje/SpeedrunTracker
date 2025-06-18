using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class FollowingPage : ContentPage
{
    public FollowingPage(FollowedEntityViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
