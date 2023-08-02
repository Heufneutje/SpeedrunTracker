using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using Refit;
using SpeedrunTracker.Interfaces;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class NotificationListViewModel : BaseViewModel
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IToastService _toastService;
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

    public ICommand LoadMoreCommand => new AsyncRelayCommand(LoadNotificationsAsync);

    public ICommand RefreshCommand => new AsyncRelayCommand(RefreshNotificationsAsync);

    public NotificationListViewModel(INotificationRepository notificationRepository, IToastService toastService)
    {
        _notifications = new ObservableCollection<Notification>();
        _notificationRepository = notificationRepository;
        _toastService = toastService;
    }

    private async Task LoadNotificationsAsync()
    {
        if (_hasReachedEnd)
            return;

        try
        {
            PagedData<List<Notification>> notifications = await _notificationRepository.GetNotificationsAsync(_offset);
            foreach (Notification notification in notifications.Data)
                Notifications.Add(notification);

            if (notifications.Pagination.Size == 0)
                _hasReachedEnd = true;
            else
                _offset += notifications.Pagination.Size;
        }
        catch (Exception ex)
        {
            if (ex is ApiException apiEx && apiEx.StatusCode == HttpStatusCode.Forbidden)
                await _toastService.ShowToastAsync("You must be logged in to view notifications.", ToastDuration.Long);
            else
                await _toastService.ShowToastAsync($"An error occurred while getting notification data: {ex.Message}");
        }
    }

    public async Task RefreshNotificationsAsync()
    {
        _offset = 0;
        _hasReachedEnd = false;
        Notifications.Clear();
        await LoadNotificationsAsync();
        IsRunningBackgroundTask = false;
    }
}
