using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage(AboutViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
