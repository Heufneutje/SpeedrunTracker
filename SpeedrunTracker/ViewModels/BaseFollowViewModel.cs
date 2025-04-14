using System.Windows.Input;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SpeedrunTracker.ViewModels;

public abstract partial class BaseFollowViewModel : BaseShareableViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsFollowingEnabled))]
    [NotifyPropertyChangedFor(nameof(FollowButtonText))]
    [NotifyPropertyChangedFor(nameof(FollowButtonIconSource))]
    [NotifyPropertyChangedFor(nameof(IsFollowingEnabled))]
    private bool? _isFollowing;

    public bool IsFollowingEnabled => IsFollowing.HasValue;

    public string FollowButtonText => IsFollowing == true ? "Unfavorite" : "Favorite";

    public string FollowButtonIconSource => IsFollowing == true ? "favorite_enabled" : "favorite_disabled";

    protected BaseFollowViewModel(IShareService shareService, IToastService toastService, IPopupService popupService)
        : base(shareService, toastService, popupService) { }

    public abstract ICommand FollowCommand { get; }
}

public abstract partial class BaseFollowViewModel<T> : BaseFollowViewModel
    where T : BaseSpeedrunModel
{
    protected readonly ILocalFollowService _followService;

    protected T? _followEntity;

    public override ICommand FollowCommand => new AsyncRelayCommand(ToggleFollowAsync);

    protected BaseFollowViewModel(
        ILocalFollowService followService,
        IShareService shareService,
        IToastService toastService,
        IPopupService popupService
    )
        : base(shareService, toastService, popupService)
    {
        _followService = followService;
    }

    public async Task LoadFollowingStatusAsync()
    {
        if (_followEntity is not null)
            IsFollowing = await _followService.IsFollowingAsync(_followEntity.Id);

        CloseActivityIndicator();
    }

    protected async Task ToggleFollowAsync()
    {
        if (_followEntity is null)
            return;

        if (IsFollowing == true)
        {
            await _followService.UnfollowAsync(_followEntity.Id);
            await _toastService.ShowToastAsync($"Removed \"{_followEntity.DisplayName}\" from your favorites.");
        }
        else if (IsFollowing == false)
        {
            await FollowAsync(_followEntity);
            await _toastService.ShowToastAsync($"Added \"{_followEntity.DisplayName}\" to your favorites.");
        }

        IsFollowing = !(IsFollowing ?? false);
    }

    protected abstract Task FollowAsync(T entity);
}
