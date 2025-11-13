using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using SpeedrunTracker.Localization;
using SpeedrunTracker.Resources.Localization;

namespace SpeedrunTracker.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    private bool _isDisplayingActivityIndicator;
    private readonly IPopupService? _popupService;
    private readonly Rectangle _popupShape;
    public bool IsRunningBackgroundTask { get; private set; }

    protected BaseViewModel(IPopupService? popupService = null)
    {
        _popupService = popupService;
        _popupShape = new Rectangle()
        {
            StrokeThickness = 0,
            RadiusX = 10,
            RadiusY = 10
        };
    }

    public void ShowActivityIndicator(string? loadingText = null)
    {
        loadingText ??= Translate(nameof(AppStrings.SpinnerLoadingText));
        IsRunningBackgroundTask = true;

        if (_popupService is null || _isDisplayingActivityIndicator)
            return;

        Dictionary<string, object> queryAttributes = new()
        {
            [nameof(SpinnerPopupViewModel.LoadingText)] = loadingText
        };
        IPopupOptions popupOptions = new PopupOptions()
        {
            CanBeDismissedByTappingOutsideOfPopup = false,
            Shape = _popupShape
        };

        _isDisplayingActivityIndicator = true;
#pragma warning disable S6966 // Awaitable method should be used
        _popupService.ShowPopup<SpinnerPopupViewModel>(Shell.Current, popupOptions, queryAttributes);
#pragma warning restore S6966 // Awaitable method should be used
    }

    public async Task CloseActivityIndicatorAsync()
    {
        IsRunningBackgroundTask = false;

        if (_popupService is not null && _isDisplayingActivityIndicator)
            await _popupService.ClosePopupAsync(Shell.Current);

        _isDisplayingActivityIndicator = false;
    }

    public async Task ShowPopupAsync<T>(Dictionary<string, object> queryAttributes) where T : BaseViewModel
    {
        IPopupOptions popupOptions = new PopupOptions()
        {
            CanBeDismissedByTappingOutsideOfPopup = true,
            Shape = _popupShape
        };

        if (_popupService is not null)
            await _popupService.ShowPopupAsync<T>(Shell.Current, popupOptions, queryAttributes);
    }

    public async Task ClosePopupAsync()
    {
        if (_popupService is not null)
            await _popupService.ClosePopupAsync(Shell.Current);
    }

    protected static string Translate(string resourceKey)
    {
        return LocalizationResourceManager.Instance[resourceKey]?.ToString() ?? string.Empty;
    }
}
