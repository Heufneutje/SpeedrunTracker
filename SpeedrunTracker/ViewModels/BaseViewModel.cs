using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SpeedrunTracker.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    private readonly IPopupService? _popupService;
    public bool IsRunningBackgroundTask { get; private set; }

    protected BaseViewModel(IPopupService? popupService = null)
    {
        _popupService = popupService;
    }

    public async Task ShowActivityIndicatorAsync(string loadingText = "Loading...")
    {
        IsRunningBackgroundTask = true;

        if (_popupService is null)
            return;

        Dictionary<string, object> queryAttributes = new()
        {
            [nameof(SpinnerPopupViewModel.LoadingText)] = loadingText
        };
        IPopupOptions popupOptions = new PopupOptions()
        {
            CanBeDismissedByTappingOutsideOfPopup = false
        };

        await _popupService.ShowPopupAsync<SpinnerPopupViewModel>(Shell.Current, popupOptions, queryAttributes);
    }

    public async Task CloseActivityIndicatorAsync()
    {
        IsRunningBackgroundTask = false;

        if (_popupService is not null)
            await _popupService.ClosePopupAsync(Shell.Current);
    }

    public async Task ShowPopupAsync<T>(Dictionary<string, object> queryAttributes) where T : BaseViewModel
    {
        IPopupOptions popupOptions = new PopupOptions()
        {
            CanBeDismissedByTappingOutsideOfPopup = true
        };

        if (_popupService is not null)
            await _popupService.ShowPopupAsync<T>(Shell.Current, popupOptions, queryAttributes);
    }

    public async Task ClosePopupAsync()
    {
        if (_popupService is not null)
            await _popupService.ClosePopupAsync(Shell.Current);
    }
}
