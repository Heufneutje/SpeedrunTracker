using CommunityToolkit.Mvvm.ComponentModel;

namespace SpeedrunTracker.ViewModels;

public partial class ImagePopupViewModel : BaseViewModel
{
    [ObservableProperty]
    private string? _imageSource;
}
