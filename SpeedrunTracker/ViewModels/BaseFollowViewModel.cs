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
            NotifyPropertyChanged(nameof(IsFollowingEnabled));
        }
    }

    public bool IsFollowingEnabled => _isFollowing.HasValue;

    public string FollowButtonText => IsFollowing == true ? "Unfavorite" : "Favorite";

    protected BaseFollowViewModel(IShareService shareService, IToastService toastService) : base(shareService, toastService)
    {
    }

    public abstract ICommand FollowCommand { get; }
}

public abstract class BaseFollowViewModel<T> : BaseFollowViewModel where T : BaseSpeedrunObject
{
    protected readonly ILocalFollowService _followService;

    protected T _followEntity;

    public override ICommand FollowCommand => new AsyncRelayCommand(ToggleFollowAsync);

    public BaseFollowViewModel(ILocalFollowService followService, IShareService shareService, IToastService toastService) : base(shareService, toastService)
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
