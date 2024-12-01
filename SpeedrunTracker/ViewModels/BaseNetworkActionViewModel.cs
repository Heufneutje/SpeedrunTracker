using CommunityToolkit.Maui.Core;
using Refit;
using System.Net;

namespace SpeedrunTracker.ViewModels;

public abstract class BaseNetworkActionViewModel : BaseViewModel
{
    protected readonly IToastService _toastService;
    private NetworkAccess? _currentNetworkAccess;

    protected BaseNetworkActionViewModel(IToastService toastService)
    {
        _toastService = toastService;
        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
    }

    private void Connectivity_ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
    {
        _currentNetworkAccess = e.NetworkAccess;
    }

    protected async Task<T?> ExecuteNetworkTask<T>(Task<T> task) where T : class?
    {
        if (_currentNetworkAccess == null || _currentNetworkAccess != NetworkAccess.Internet)
            _currentNetworkAccess = Connectivity.Current.NetworkAccess;

        if (_currentNetworkAccess != NetworkAccess.Internet)
        {
            await _toastService.ShowToastAsync("No internet access.");
            return null;
        }

        try
        {
            return await task;
        }
        catch (ApiException apiEx)
        {
            switch (apiEx.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    await _toastService.ShowToastAsync("You must be logged in to view this content.", ToastDuration.Long);
                    break;
                case HttpStatusCode.NotFound:
                    // We're letting each specific case handle itself. One case in which this happens is a run that was verified by a deleted user.
                    break;
                case HttpStatusCode.InternalServerError:
                    await _toastService.ShowToastAsync("Received an unknown error from the speedrun.com API.", ToastDuration.Long);
                    break;
                default:
                    await _toastService.ShowToastAsync(apiEx.Message);
                    break;
            }
        }
        catch (Exception ex)
        {
            await _toastService.ShowToastAsync(ex.Message);
        }

        _currentNetworkAccess = Connectivity.Current.NetworkAccess;
        return null;
    }
}
