using CommunityToolkit.Maui.Views;
using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class SpinnerPopup : Popup
{
    public SpinnerPopup(SpinnerPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
