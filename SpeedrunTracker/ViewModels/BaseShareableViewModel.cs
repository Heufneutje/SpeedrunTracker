using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Input;

namespace SpeedrunTracker.ViewModels;

public abstract partial class BaseShareableViewModel : BaseNetworkActionViewModel
{
    private readonly IShareService _shareService;

    public abstract ShareDetails ShareDetails { get; }

    protected BaseShareableViewModel(IShareService shareService, IToastService toastService, IPopupService popupService)
        : base(toastService, popupService)
    {
        _shareService = shareService;
    }

    [RelayCommand]
    private async Task ShareAsync()
    {
        await _shareService.ShareUriAsync(ShareDetails);
    }
}
