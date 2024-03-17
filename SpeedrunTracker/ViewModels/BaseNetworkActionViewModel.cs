using CommunityToolkit.Maui.Core;
using Refit;
using System.Net;

namespace SpeedrunTracker.ViewModels;

public abstract class BaseNetworkActionViewModel : BaseViewModel
{
    protected readonly IToastService _toastService;
    private NetworkAccess? _currentNetworkAccess;

    public BaseNetworkActionViewModel(IToastService toastService)
    {
        _toastService = toastService;
        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
    }

    private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        _currentNetworkAccess = e.NetworkAccess;
    }

    protected async Task<T> ExecuteNetworkTask<T>(Task<T> task) where T : class
    {
        if (_currentNetworkAccess == null || _currentNetworkAccess != NetworkAccess.Internet)
            _currentNetworkAccess = Connectivity.Current.NetworkAccess;

        if (_currentNetworkAccess != NetworkAccess.Internet)
        {
            await _toastService.ShowToastAsync("No internet access");
            return null;
        }

        try
        {
            return await task;
        }
        catch (Exception ex)
        {
            if (ex is ApiException apiEx)
                switch (apiEx.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        await _toastService.ShowToastAsync("You must be logged in to view this content.", ToastDuration.Long);
                        break;
                    case HttpStatusCode.NotFound:
                        // We're letting each specific case handle itself. One case in which this happens is a run that was verified by a deleted user.
                        break;
                    default:
                        await _toastService.ShowToastAsync(ex.Message);
                        break;
                }
            else
                await _toastService.ShowToastAsync(ex.Message);

            _currentNetworkAccess = Connectivity.Current.NetworkAccess;
            return null;
        }
    }
}
