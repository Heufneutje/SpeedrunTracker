using CommunityToolkit.Maui.Views;
using SpeedrunTracker.ViewModels;

namespace SpeedrunTracker.Views;

public partial class ImagePopup : Popup
{
	public ImagePopup(ImagePopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}
