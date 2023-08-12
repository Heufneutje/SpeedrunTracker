using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Services;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public abstract class BaseFollowViewModel<T> : BaseNetworkActionViewModel where T : BaseSpeedrunObject
{
    protected readonly ILocalFollowService _followService;
    protected T _followEntity;

    private bool? _isFollowing;

    public bool? IsFollowing
    {
        get => _isFollowing;
        set
        {
            _isFollowing = value;
            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(FollowButtonText));
            NotifyPropertyChanged(nameof(IsFollowingEnabled));
        }
    }

    public bool IsFollowingEnabled => _isFollowing.HasValue;

    public string FollowButtonText => IsFollowing == true ? "Unfavorite" : "Favorite";

    public ICommand FollowCommand => new AsyncRelayCommand(ToggleFollowAsync);

    public BaseFollowViewModel(ILocalFollowService followService, IToastService toastService) : base(toastService)
    {
        _followService = followService;
    }

    public async Task LoadFollowingStatusAsync()
    {
        IsFollowing = await _followService.IsFollowingAsync(_followEntity.Id);
    }

    protected async Task ToggleFollowAsync()
    {
        if (IsFollowing == true)
            await _followService.UnfollowAsync(_followEntity.Id);
        else if (IsFollowing == false)
            await FollowAsync(_followEntity);

        IsFollowing = !(IsFollowing ?? false);
    }

    protected abstract Task FollowAsync(T entity);
}
