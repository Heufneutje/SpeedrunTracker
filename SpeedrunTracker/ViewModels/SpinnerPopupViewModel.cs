using CommunityToolkit.Mvvm.ComponentModel;
using SpeedrunTracker.Resources.Localization;

namespace SpeedrunTracker.ViewModels;

public partial class SpinnerPopupViewModel : BasePopupViewModel
{
    [ObservableProperty]
    private string? _loadingText;

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(LoadingText), out object? loadingTextObj) &&
            loadingTextObj is string loadingText)
            LoadingText = loadingText;
        else
            LoadingText = Translate(nameof(AppStrings.SpinnerLoadingText));
    }
}
