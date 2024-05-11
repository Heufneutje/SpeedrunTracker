using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class NotificationListViewModel : BaseNetworkActionViewModel
{
    private readonly INotificationService _notificationService;
    private readonly IBrowserService _browserService;
    private readonly ILocalSettingsService _settingsService;
    private int _offset;
    private bool _hasReachedEnd;

    public RangedObservableCollection<NotificationViewModel> Notifications { get; set; }

    private NotificationViewModel _selectedNotification;

    public NotificationViewModel SelectedNotification
    {
        get => _selectedNotification;
        set
        {
            _selectedNotification = value;
            NotifyPropertyChanged();
        }
    }

    public ICommand LoadMoreCommand => new AsyncRelayCommand(LoadNotificationsAsync);

    public ICommand RefreshCommand => new AsyncRelayCommand(RefreshNotificationsAsync);

    public ICommand OpenLinkCommand => new AsyncRelayCommand(OpenNotificationLinkAsync);

    public NotificationListViewModel(INotificationService notificationService, IToastService toastService, IBrowserService browserService, ILocalSettingsService settingsService) : base(toastService)
    {
        _notificationService = notificationService;
        _browserService = browserService;
        _settingsService = settingsService;
        Notifications = [];
    }

    private async Task LoadNotificationsAsync()
    {
        if (_hasReachedEnd)
            return;

        PagedData<List<Notification>> notifications = await ExecuteNetworkTask(_notificationService.GetNotificationsAsync(_offset));
        if (notifications == null)
            return;

        Notifications.AddRange(notifications.Data.Select(x => new NotificationViewModel(x, _settingsService.UserSettings.DateFormat, _settingsService.UserSettings.TimeFormat)));

        if (notifications.Pagination.Size == 0)
            _hasReachedEnd = true;
        else
            _offset += notifications.Pagination.Size;
    }

    private async Task RefreshNotificationsAsync()
    {
        _offset = 0;
        _hasReachedEnd = false;
        Notifications.Clear();
        await LoadNotificationsAsync();
        IsRunningBackgroundTask = false;
    }

    private async Task OpenNotificationLinkAsync()
    {
        NotificationLink link = SelectedNotification?.Notification?.Item;

        if (link != null)
            await _browserService.OpenAsync(link.Uri);

        SelectedNotification = null;
    }
}
