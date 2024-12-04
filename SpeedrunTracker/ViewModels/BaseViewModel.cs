using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Maui.Core;

namespace SpeedrunTracker.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    private readonly IPopupService? _popupService;
    public bool IsRunningBackgroundTask { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected BaseViewModel(IPopupService? popupService = null)
    {
        _popupService = popupService;
    }

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void ShowActivityIndicator(string loadingText = "Loading...")
    {
        IsRunningBackgroundTask = true;
        _popupService?.ShowPopup<SpinnerPopupViewModel>(onPresenting => onPresenting.LoadingText = loadingText);
    }

    public void CloseActivityIndicator()
    {
        IsRunningBackgroundTask = false;
        _popupService?.ClosePopup();
    }
}
