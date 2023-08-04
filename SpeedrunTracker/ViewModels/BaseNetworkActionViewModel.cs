using CommunityToolkit.Maui.Core;
using Refit;
using SpeedrunTracker.Interfaces;
using System.Net;

namespace SpeedrunTracker.ViewModels
{
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
                if (ex is ApiException apiEx && apiEx.StatusCode == HttpStatusCode.Forbidden)
                    await _toastService.ShowToastAsync("You must be logged in to view this content.", ToastDuration.Long);
                else
                    await _toastService.ShowToastAsync(ex.Message);

                _currentNetworkAccess = Connectivity.Current.NetworkAccess;
                return null;
            }
        }
    }
}
