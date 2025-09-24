using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using SpeedrunTracker.Resources.Localization;

namespace SpeedrunTracker.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    private readonly IPopupService? _popupService;
    public bool IsRunningBackgroundTask { get; private set; }

    protected BaseViewModel(IPopupService? popupService = null)
    {
        _popupService = popupService;
    }

    public void ShowActivityIndicator(string? loadingText = null)
    {
        loadingText ??= AppStrings.SpinnerLoadingText;
        IsRunningBackgroundTask = true;
        _popupService?.ShowPopup<SpinnerPopupViewModel>(onPresenting => onPresenting.LoadingText = loadingText);
    }

    public void CloseActivityIndicator()
    {
        IsRunningBackgroundTask = false;
        _popupService?.ClosePopup();
    }

    public void ShowPopup<T>(Action<T> onPresenting) where T : BaseViewModel
    {
        _popupService?.ShowPopup(onPresenting);
    }

    public void ClosePopup()
    {
        _popupService?.ClosePopup();
    }
}
