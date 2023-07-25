using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Model;
using SpeedrunTracker.Model.Enum;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels
{
    public class RunDetailsViewModel : BaseViewModel
    {
        private readonly IBrowserService _browserService;
        private readonly IUserRepository _userRepository;

        private RunDetails _runDetails;

        public RunDetails RunDetails
        {
            get => _runDetails;
            set
            {
                _runDetails = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Title));
                NotifyPropertyChanged(nameof(HasComment));
                NotifyPropertyChanged(nameof(HasVideo));
                NotifyPropertyChanged(nameof(HasInGameTime));
                NotifyPropertyChanged(nameof(HasRealtime));
                NotifyPropertyChanged(nameof(HasRealtimeNoLoads));
                NotifyPropertyChanged(nameof(StatusImage));
                NotifyPropertyChanged(nameof(StatusDescription));
                NotifyPropertyChanged(nameof(HasExaminer));
            }
        }

        public bool HasComment => !string.IsNullOrEmpty(_runDetails?.Run?.Comment);

        public bool HasVideo => _runDetails?.Run?.Videos?.Links?.Any() == true;

        public ICommand ShowVideoCommand => new AsyncRelayCommand(ShowVideo);

        public string Title => RunDetails == null ? "RunDetails" : $"{_runDetails.Category.Name} in {_runDetails.Run.Times.PrimaryTimeSpan} by {_runDetails.Run.Players.First().DisplayName}";

        public bool HasInGameTime => ShouldShowTimingType(TimingType.InGame);

        public bool HasRealtime => ShouldShowTimingType(TimingType.Realtime);

        public bool HasRealtimeNoLoads => ShouldShowTimingType(TimingType.RealtimeNoLoads);

        public bool HasExaminer => !string.IsNullOrEmpty(RunDetails?.Run?.Status?.ExaminerId);

        public string StatusDescription
        {
            get
            {
                if (RunDetails == null)
                    return null;

                return RunDetails.Run.Status.StatusType switch
                {
                    SpeedrunStatusType.New => "Verification pending",
                    SpeedrunStatusType.Verified => $"Verified on {RunDetails.Run.Status.VerifyDate:yyyy-MM-dd HH:mm}",
                    SpeedrunStatusType.Rejected => $"Rejected ({RunDetails.Run.Status.Reason ?? string.Empty})",
                    _ => string.Empty,
                };
            }
        }

        public string StatusImage => (RunDetails?.Run?.Status?.StatusType) switch
        {
            SpeedrunStatusType.New => "hourglass",
            SpeedrunStatusType.Verified => "verified",
            SpeedrunStatusType.Rejected => "rejected",
            _ => string.Empty,
        };

        public RunDetailsViewModel(IBrowserService browserService, IUserRepository userRepository)
        {
            _browserService = browserService;
            _userRepository = userRepository;
        }

        private async Task ShowVideo() => await _browserService.OpenAsync(_runDetails.Run.Videos.Links.First().Uri);

        private bool ShouldShowTimingType(TimingType timingType) => RunDetails?.Ruleset?.TimingTypes?.Contains(timingType) == true && RunDetails?.Ruleset?.DefaultTimingType != timingType;

        public async Task LoadData()
        {
            if (RunDetails.Examiner == null && RunDetails.Run.Status.ExaminerId != null)
                RunDetails.Examiner = (await _userRepository.GetUserAsync(RunDetails.Run.Status.ExaminerId))?.Data;
        }
    }
}
