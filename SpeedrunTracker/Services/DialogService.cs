using Microsoft.Maui.Controls.PlatformConfiguration;

namespace SpeedrunTracker.Services;

public class DialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        return GetMainPage()?.DisplayAlert(title, message, cancel);
    }

    public Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
    {
        return GetMainPage()?.DisplayAlert(title, message, accept, cancel);
    }

    private Page GetMainPage() => Application.Current.Windows[0].Page;
}
