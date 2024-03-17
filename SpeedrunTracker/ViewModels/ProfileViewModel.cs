using CommunityToolkit.Mvvm.Input;
using Refit;
using SpeedrunTracker.Extensions;
using SpeedrunTracker.Navigation;
using System.Net;
using System.Windows.Input;

namespace SpeedrunTracker.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    private readonly IUserService _userService;
    private readonly IDialogService _dialogService;
    private readonly IToastService _toastService;

    private User _user;

    public User User
    {
        get => _user;
        set
        {
            if (_user != value)
            {
                _user = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Name));
                NotifyPropertyChanged(nameof(ImageUri));
            }
        }
    }

    private bool _isLoggedIn;

    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        set
        {
            if (_isLoggedIn != value)
            {
                _isLoggedIn = value;
                NotifyPropertyChanged();
            }
        }
    }

    private string _apiKey;

    public string ApiKey
    {
        get => _apiKey;
        set
        {
            if (_apiKey != value)
            {
                _apiKey = value;
                NotifyPropertyChanged();
            }
        }
    }

    public string Name => _user?.DisplayName ?? "Guest";

    public string ImageUri => _user?.Assets?.Image?.Uri;

    public ICommand LoginCommand => new AsyncRelayCommand(LoginAsync);

    public ICommand LogoutCommand => new AsyncRelayCommand(LogoutAsync);

    public ICommand NavigateToUserCommand => new AsyncRelayCommand(NavigateToUserAsync);

    public ProfileViewModel(IUserService userService, IDialogService dialogService, IToastService toastService)
    {
        _userService = userService;
        _dialogService = dialogService;
        _toastService = toastService;
    }

    public async Task LoadProfileAsync()
    {
        IsRunningBackgroundTask = true;

        try
        {
            IsLoggedIn = !string.IsNullOrEmpty(await SecureStorage.GetAsync(Constants.ApiKey));
            if (!_isLoggedIn)
            {
                User = null;
                return;
            }

            User = await _userService.GetUserProfileAsync();
        }
        catch (Exception ex)
        {
            if (ex is ApiException apiEx && apiEx.StatusCode == HttpStatusCode.Forbidden)
            {
                await _toastService.ShowToastAsync("The provided API key is invalid.");
                await LogoutAsync(false);
            }
            else
                await _toastService.ShowToastAsync($"An error occurred while getting profile data: {ex.Message}");
        }
        finally
        {
            IsRunningBackgroundTask = false;
        }
    }

    private async Task LoginAsync()
    {
        if (string.IsNullOrEmpty(ApiKey))
        {
            await _toastService.ShowToastAsync("No API key provided.");
            return;
        }

        await SecureStorage.SetAsync(Constants.ApiKey, ApiKey);
        ApiKey = null;
        await LoadProfileAsync();
    }

    private async Task LogoutAsync()
    {
        await LogoutAsync(true);
    }

    private async Task LogoutAsync(bool confirm)
    {
        if (!confirm || await _dialogService.ShowConfirmationAsync("Log out", "Are you sure you want to log out?"))
        {
            SecureStorage.Remove(Constants.ApiKey);
            await LoadProfileAsync();
        }
    }

    private async Task NavigateToUserAsync()
    {
        if (IsLoggedIn)
            await Shell.Current.GoToAsync(Routes.UserDetailPageRoute, "User", User);
    }
}
