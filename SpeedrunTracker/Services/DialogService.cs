using Microsoft.Maui.Controls.PlatformConfiguration;

namespace SpeedrunTracker.Services;

public class DialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        return GetMainPage()?.DisplayAlert(title, message, cancel) ?? Task.CompletedTask;
    }

    public async Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
    {
        Page? mainPage = GetMainPage();
        return mainPage != null && await mainPage.DisplayAlert(title, message, accept, cancel);
    }

    private static Page? GetMainPage() => Application.Current?.Windows[0].Page;
}
