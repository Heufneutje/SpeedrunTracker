namespace SpeedrunTracker.Services;

public class DialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        return Application.Current.MainPage.DisplayAlert(title, message, cancel);
    }

    public Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
    {
        return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
    }
}
