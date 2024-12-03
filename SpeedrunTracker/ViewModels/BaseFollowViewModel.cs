using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public abstract class BaseFollowViewModel : BaseShareableViewModel
{
    private bool? _isFollowing;

    public bool? IsFollowing
    {
        get => _isFollowing;
        set
        {
            _isFollowing = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(FollowButtonText));
            NotifyPropertyChanged(nameof(FollowButtonIconSource));
            NotifyPropertyChanged(nameof(IsFollowingEnabled));
        }
    }

    public bool IsFollowingEnabled => _isFollowing.HasValue;

    public string FollowButtonText => IsFollowing == true ? "Unfavorite" : "Favorite";

    public string FollowButtonIconSource => IsFollowing == true ? "favorite_enabled" : "favorite_disabled";

    protected BaseFollowViewModel(IShareService shareService, IToastService toastService, IPopupService popupService) : base(shareService, toastService, popupService)
    {
    }

    public abstract ICommand FollowCommand { get; }
}

public abstract class BaseFollowViewModel<T> : BaseFollowViewModel where T : BaseSpeedrunModel
{
    protected readonly ILocalFollowService _followService;

    protected T? _followEntity;

    public override ICommand FollowCommand => new AsyncRelayCommand(ToggleFollowAsync);

    protected BaseFollowViewModel(ILocalFollowService followService, IShareService shareService, IToastService toastService, IPopupService popupService) : base(shareService, toastService, popupService)
    {
        _followService = followService;
    }

    public async Task LoadFollowingStatusAsync()
    {
        if (_followEntity != null)
            IsFollowing = await _followService.IsFollowingAsync(_followEntity.Id);

        CloseActivityIndicator();
    }

    protected async Task ToggleFollowAsync()
    {
        if (_followEntity == null)
            return;

        if (IsFollowing == true)
            await _followService.UnfollowAsync(_followEntity.Id);
        else if (IsFollowing == false)
            await FollowAsync(_followEntity);

        IsFollowing = !(IsFollowing ?? false);
    }

    protected abstract Task FollowAsync(T entity);
}
