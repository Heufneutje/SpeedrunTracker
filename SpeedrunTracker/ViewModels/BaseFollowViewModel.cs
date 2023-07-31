using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Model;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels
{
    public abstract class BaseFollowViewModel<T> : BaseViewModel where T : BaseSpeedrunObject
    {
        protected readonly ILocalFollowService _followService;
        protected T _followEntity;

        private bool _isFollowing;

        public bool IsFollowing
        {
            get => _isFollowing;
            set
            {
                _isFollowing = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(FollowButtonText));
            }
        }

        public string FollowButtonText => IsFollowing ? "Unfollow" : "Follow";

        public ICommand FollowCommand => new AsyncRelayCommand(ToggleFollowAsync);

        public BaseFollowViewModel(ILocalFollowService followService)
        {
            _followService = followService;
        }

        public async Task LoadFollowingStatusAsync()
        {
            IsFollowing = await _followService.IsFollowingAsync(_followEntity.Id);
        }

        protected async Task ToggleFollowAsync()
        {
            if (IsFollowing)
                await _followService.UnfollowAsync(_followEntity.Id);
            else
                await FollowAsync(_followEntity);

            IsFollowing = !IsFollowing;
        }

        protected abstract Task FollowAsync(T entity);
    }
}
