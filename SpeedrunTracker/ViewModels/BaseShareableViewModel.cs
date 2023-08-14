using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Services;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public abstract class BaseShareableViewModel : BaseNetworkActionViewModel
{
    private readonly IShareService _shareService;

    public abstract ShareDetails ShareDetails { get; }

    public ICommand ShareCommand => new AsyncRelayCommand(ShareAsync);

    protected BaseShareableViewModel(IShareService shareService, IToastService toastService) : base(toastService)
    {
        _shareService = shareService;
    }

    private async Task ShareAsync()
    {
        await _shareService.ShareUriAsync(ShareDetails);
    }
}
