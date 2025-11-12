using CommunityToolkit.Mvvm.ComponentModel;
using SpeedrunTracker.Resources.Localization;

namespace SpeedrunTracker.ViewModels;

public partial class SpinnerPopupViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _loadingText = Translate(nameof(AppStrings.SpinnerLoadingText));
}
