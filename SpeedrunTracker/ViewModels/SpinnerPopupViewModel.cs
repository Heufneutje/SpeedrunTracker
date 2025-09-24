using CommunityToolkit.Mvvm.ComponentModel;
using SpeedrunTracker.Resources.Localization;

namespace SpeedrunTracker.ViewModels;

public partial class SpinnerPopupViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _loadingText = AppStrings.SpinnerLoadingText;
}
