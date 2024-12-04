using System.Windows.Input;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;

namespace SpeedrunTracker.ViewModels;

public abstract class BaseShareableViewModel : BaseNetworkActionViewModel
{
    private readonly IShareService _shareService;

    public abstract ShareDetails ShareDetails { get; }

    public ICommand ShareCommand => new AsyncRelayCommand(ShareAsync);

    protected BaseShareableViewModel(IShareService shareService, IToastService toastService, IPopupService popupService)
        : base(toastService, popupService)
    {
        _shareService = shareService;
    }

    private async Task ShareAsync()
    {
        await _shareService.ShareUriAsync(ShareDetails);
    }
}
