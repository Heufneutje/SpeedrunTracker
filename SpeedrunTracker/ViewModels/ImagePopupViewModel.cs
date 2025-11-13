using CommunityToolkit.Mvvm.ComponentModel;

namespace SpeedrunTracker.ViewModels;

public partial class ImagePopupViewModel : BasePopupViewModel
{
    [ObservableProperty]
    private string? _imageSource;

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(ImageSource), out object? imageSourceObj) &&
            imageSourceObj is string imageSource)
            ImageSource = imageSource;
    }
}
