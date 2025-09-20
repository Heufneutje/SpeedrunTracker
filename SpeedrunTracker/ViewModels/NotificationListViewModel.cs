using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SpeedrunTracker.ViewModels;

public partial class NotificationListViewModel : BaseNetworkActionViewModel
{
    private readonly INotificationService _notificationService;
    private readonly IBrowserService _browserService;
    private readonly ILocalSettingsService _settingsService;
    private int _offset;
    private bool _hasReachedEnd;

    [ObservableProperty]
    private bool _isRefreshing;

    public RangedObservableCollection<NotificationViewModel> Notifications { get; set; }

    [ObservableProperty]
    private NotificationViewModel? _selectedNotification;

    public NotificationListViewModel(
        INotificationService notificationService,
        IToastService toastService,
        IBrowserService browserService,
        ILocalSettingsService settingsService,
        IPopupService popupService
    )
        : base(toastService, popupService)
    {
        _notificationService = notificationService;
        _browserService = browserService;
        _settingsService = settingsService;
        Notifications = [];
    }

    [RelayCommand]
    private async Task LoadNotificationsAsync()
    {
        if (_hasReachedEnd)
            return;

        PagedData<List<Notification>>? notifications = await ExecuteNetworkTask(
            _notificationService.GetNotificationsAsync(_offset)
        );
        if (notifications is null)
            return;

        Notifications.AddRange(
            notifications.Data.Select(x => new NotificationViewModel(
                x,
                _settingsService.UserSettings.DateFormat,
                _settingsService.UserSettings.TimeFormat
            ))
        );

        if (notifications.Pagination.Size == 0)
            _hasReachedEnd = true;
        else
            _offset += notifications.Pagination.Size;
    }

    [RelayCommand]
    private async Task RefreshNotificationsAsync()
    {
        _offset = 0;
        _hasReachedEnd = false;
        Notifications.Clear();
        await LoadNotificationsAsync();
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task OpenNotificationLinkAsync()
    {
        NotificationLink? link = SelectedNotification?.Notification?.Item;

        if (link is not null)
            await _browserService.OpenAsync(link.SecureUri);

        SelectedNotification = null;
    }
}
