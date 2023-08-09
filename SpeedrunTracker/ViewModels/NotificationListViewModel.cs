using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class NotificationListViewModel : BaseNetworkActionViewModel
{
    private readonly INotificationService _notificationService;
    private readonly IBrowserService _browserService;
    private int _offset;
    private bool _hasReachedEnd;

    private ObservableCollection<Notification> _notifications;

    public ObservableCollection<Notification> Notifications
    {
        get => _notifications;
        set
        {
            _notifications = value;
            NotifyPropertyChanged();
        }
    }

    private Notification _selectedNotification;

    public Notification SelectedNotification
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

    public NotificationListViewModel(INotificationService notificationService, IToastService toastService, IBrowserService browserService) : base(toastService)
    {
        _notifications = new ObservableCollection<Notification>();
        _notificationService = notificationService;
        _browserService = browserService;
    }

    private async Task LoadNotificationsAsync()
    {
        if (_hasReachedEnd)
            return;

        PagedData<List<Notification>> notifications = await ExecuteNetworkTask(_notificationService.GetNotificationsAsync(_offset));
        if (notifications == null)
            return;

        foreach (Notification notification in notifications.Data)
            Notifications.Add(notification);

        if (notifications.Pagination.Size == 0)
            _hasReachedEnd = true;
        else
            _offset += notifications.Pagination.Size;
    }

    public async Task RefreshNotificationsAsync()
    {
        _offset = 0;
        _hasReachedEnd = false;
        Notifications.Clear();
        await LoadNotificationsAsync();
        IsRunningBackgroundTask = false;
    }

    private async Task OpenNotificationLinkAsync()
    {
        NotificationLink link = SelectedNotification?.Item;

        if (link != null)
            await _browserService.OpenAsync(link.Uri);

        SelectedNotification = null;
    }
}
