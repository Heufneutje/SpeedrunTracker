using System.Net;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using SpeedrunTracker.Resources.Localization;

namespace SpeedrunTracker.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly IUserService _userService;
    private readonly IDialogService _dialogService;
    private readonly IToastService _toastService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Name))]
    [NotifyPropertyChangedFor(nameof(ImageUri))]
    private User? _user;

    [ObservableProperty]
    private bool _isLoggedIn;

    [ObservableProperty]
    private string? _apiKey;

    public string Name => User?.DisplayName ?? AppStrings.ProfilePageGuestLabel;

    public string? ImageUri => User?.Assets?.Image?.SecureUri;

    public ProfileViewModel(
        IUserService userService,
        IDialogService dialogService,
        IToastService toastService,
        IPopupService popupService
    )
        : base(popupService)
    {
        _userService = userService;
        _dialogService = dialogService;
        _toastService = toastService;
    }

    public async Task LoadProfileAsync()
    {
        ShowActivityIndicator();

        try
        {
            IsLoggedIn = !string.IsNullOrEmpty(await SecureStorage.GetAsync(Constants.ApiKey));
            if (!IsLoggedIn)
            {
                User = null;
                return;
            }

            User = await _userService.GetUserProfileAsync();
        }
        catch (HttpRequestException httpEx)
        {
            await HandleUnknownError(httpEx);
        }
        catch (Exception ex)
        {
            if (ex is ApiException apiEx && apiEx.StatusCode == HttpStatusCode.Forbidden)
                await _toastService.ShowToastAsync(AppStrings.ProfilePageApiKeyErrorToast);
            else
                await HandleUnknownError(ex);

            await LogoutAsync(false);
        }
        finally
        {
            CloseActivityIndicator();
        }
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrEmpty(ApiKey))
        {
            await _toastService.ShowToastAsync(AppStrings.ProfilePageNoApiKeyErrorToast);
            return;
        }

        await SecureStorage.SetAsync(Constants.ApiKey, ApiKey);
        ApiKey = null;
        await LoadProfileAsync();
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await LogoutAsync(true);
    }

    private async Task LogoutAsync(bool confirm)
    {
        try
        {
            if (!confirm || await _dialogService.ShowConfirmationAsync(AppStrings.ProfilePageLogoutTitle, AppStrings.ProfilePageLogoutMessage))
            {
                SecureStorage.Remove(Constants.ApiKey);
                await LoadProfileAsync();
            }
        }
        catch (Exception)
        {
            // Prevent crash.
        }
    }

    [RelayCommand]
    private async Task NavigateToUserAsync()
    {
        if (IsLoggedIn && User is not null)
            await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", User);
    }

    private async Task HandleUnknownError(Exception ex)
    {
        await _toastService.ShowToastAsync($"{AppStrings.ProfilePageUnknownErrorToast}: {ex.Message}");
    }
}
